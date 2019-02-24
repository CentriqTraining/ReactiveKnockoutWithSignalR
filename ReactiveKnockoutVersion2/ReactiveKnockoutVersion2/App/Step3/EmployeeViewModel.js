// Model Version 3
//  Allow the signalR object to be passed into our model
//  signalR actually set up at the end of this file.
var EmployeeViewModel = function (signalR) {
    //             This changed   ^^^^^^^                   
    var self = this;

    self.employees = ko.observableArray();
    self.loading = ko.observable(true);

    self.watchModel = function (model, callback) {
        for (var key in model) {
            if (model.hasOwnProperty(key) &&
                ko.isObservable(model[key])) {
                self.subscribeToProperty(model, key, function (key, val) {
                    callback(model, key, val);
                });
            }
        }
    }
    self.subscribeToProperty = function (model, key, callback) {
        model[key].subscribe(function (val) {
            callback(key, val);
        });
    }

    $.getJSON("/employees", function (data) {
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
                TerminationDate: ko.observable(termvalue)
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

        //  Our lone little change here
        //  Some property just changed.  Send a message
        //  through the signalR Hub to every other connected
        //  client that this employee, this prop and this val
        //  just changed
        signalR.server.sendUpdate(model.Id, key, val)
    };
};

$(function () {
    // create a variable to point to our signalR Hub.
    //  The variable "reactive" after connection is
    // defined by our Hub class.  We have an attribute called
    //  [HubName("x")] that says what it should be here
    var signalR = $.connection.reactive;

    //  change your view model and pass signalR into it
    //  so we can make some calls from in there
    var viewModel = new EmployeeViewModel(signalR)

    //  we'll use this function to avoid having to 
    //  copy and paste this functionality
    var findEmployee = function (id) {
        return ko.utils.arrayFirst(viewModel.employees(), function (item) {
            if (item.Id == id) {
                return item;
            }
        });
    };

    //  This is the handler for signalR updateEmployee
    //  this is actually called by our hub class
    signalR.client.updateEmployee = function (id, key, val) {
        var employee = findEmployee(id);
        employee[key](val);
    };

    //  Now we call hub.start and when it is up and running
    //  we go ahead and apply out bindings
    $.connection.hub.start().done(function () {
        ko.applyBindings(viewModel);
    });

});
