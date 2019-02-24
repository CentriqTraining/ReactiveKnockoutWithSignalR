// Model Version 4
var EmployeeViewModel = function (signalR) {
    var self = this;

    self.employees = ko.observableArray();
    self.loading = ko.observable(true);

    //  add this method too allow the selection of the
    //  template to use in the view to become dynamic
    //  based on whether or not user has chosen to edit
    //  or not
    self.displayMode = function (employee) {
        return employee.IsInEditMode() ? 'edit-mode' : 'readonly-mode';
    };

    //  This method will be called to change the edit mode
    //  for this one employee
    self.edit = function (employee) {
        employee.IsInEditMode(true);
    };

    //  This method will signal editing is complete for
    //  this one employee
    self.done = function (employee) {
        employee.IsInEditMode(false);
    };

    //  Add key != 'IsInEditMode' so we don't generate 
    //  a WEbAPI call for an internal item
    self.watchModel = function (model, callback) {
        for (var key in model) {
            if (model.hasOwnProperty(key) &&
                ko.isObservable(model[key]) &&
                key != 'IsInEditMode') {
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
        //  Add an IsInEditMode item - NOT to your data
        //  Only in the model
        self.employees(ko.utils.arrayMap(data, function (employee) {
            var obsEmployee = {
                Id: employee.Id,
                FirstName: ko.observable(employee.FirstName),
                LastName: ko.observable(employee.LastName),
                Position: ko.observable(employee.Position),
                Salary: ko.observable(employee.Salary),
                TerminationDate: ko.observable(employee.TerminationDate),
                IsInEditMode: ko.observable(false)
            }
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

    $.connection.hub.start().done(function () {
        ko.applyBindings(viewModel);
    });

});
