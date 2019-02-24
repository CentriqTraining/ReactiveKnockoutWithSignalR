//  Model Version 5 
var EmployeeViewModel = function (signalR) {
    var self = this;

    self.employees = ko.observableArray();
    self.loading = ko.observable(true);

    //  change displaymode to include if locked
    self.displayMode = function (employee) {
        if (employee.IsLocked()) {
            return 'locked-mode'
        }
        else {
            return employee.IsInEditMode() ? 'edit-mode' : 'readonly-mode';
        }
    };

    //  Add a call to lock in signalR
    self.edit = function (employee) {
        employee.IsInEditMode(true);
        signalR.server.lock(employee.Id);
    };

    //  Add a call to unlock in signalR
    self.done = function (employee) {
        employee.IsInEditMode(false);
        signalR.server.unlock(employee.Id);
    };

    // Also add key != IsLocked here 
    // (the locking mechanism will be handled through Hub only)
    self.watchModel = function (model, callback) {
        for (var key in model) {
            if (model.hasOwnProperty(key) &&
                ko.isObservable(model[key]) &&
                key !== 'IsInEditMode' &&
                key !== 'IsLocked' &&
                key !== 'LockHolder') {
                self.subscribeToProperty(model, key, function (key, val) {
                    callback(model, key, val);
                });
            }
        }
    };

    self.subscribeToProperty = function (model, key, callback) {
        model[key].subscribe(function (val) {
            callback(key, val);
        });
    };

    $.getJSON("/employees", function (data) {
        //  Add an IsLocked item from the model
        self.employees(ko.utils.arrayMap(data, function (employee) {
            var termvalue = "";
            if (employee.TerminationDate !== null) {
                var date = new Date(employee.TerminationDate);
                termvalue = date.toLocaleDateString("en-US");
            }

            var obsEmployee = {
                Id: employee.Id,
                FirstName: ko.observable(employee.FirstName),
                LastName: ko.observable(employee.LastName),
                Position: ko.observable(employee.Position),
                Salary: ko.observable(employee.Salary),
                TerminationDate: ko.observable(termvalue),
                IsInEditMode: ko.observable(false),
                IsLocked: ko.observable(employee.IsLocked),
                LockHolder: ko.observable(employee.LockHolder)
            };
            self.watchModel(obsEmployee, self.modelChanged);
            return obsEmployee;
        }));
        self.loading(false);
    });

    self.modelChanged = function (model, key, val) {
        var payload = {
            Id: model.Id,
            PropertyName: key,
            PropertyValue: val
        };

        var str = JSON.stringify(payload);
        $.ajax({
            url: '/employees/save',
            type: 'PUT',
            data: str,
            contentType: 'application/json',
            dataType: 'json'
        });
        signalR.server.sendUpdate(model.Id, key, val)
    };
};

$(function () {
    var signalR = $.connection.reactive;
    var viewModel = new EmployeeViewModel(signalR)
    var findEmployee = function (id) {
        return ko.utils.arrayFirst(viewModel.employees(), function (item) {
            if (item.Id == id) {
                return item;
            }
        });
    };

    signalR.client.updateEmployee = function (id, key, val) {
        var employee = findEmployee(id);
        employee[key](val);
    };

    //  Add handler for lockemployee called by signalR
    signalR.client.lockEmployee = function (id, name) {
        var employee = findEmployee(id);
        employee.LockHolder = name;
        employee.IsLocked(true);
    };

    //  Add handler for unlockemployee called by signalR
    signalR.client.unlockEmployee = function (id) {
        var employee = findEmployee(id);
        employee.IsLocked(false);
    };

    $.connection.hub.start().done(function () {
        ko.applyBindings(viewModel);
    });

});
