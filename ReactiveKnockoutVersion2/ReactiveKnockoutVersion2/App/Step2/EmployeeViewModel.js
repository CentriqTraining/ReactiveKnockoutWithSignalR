// Model Version 2 
/// <reference path="../../Scripts/bootstrap.js" />
var EmployeeViewModel = function () {
    var self = this;

    self.employees = ko.observableArray();
    self.loading = ko.observable(true);

    //  this method goes through each property in the 
    //  employee and registers a callback for property 
    //  changes if the key is an observable item.
    self.watchModel = function (model, callback) {
        for (var key in model) {
            if (model.hasOwnProperty(key) && ko.isObservable(model[key])) {
                self.subscribeToProperty(model, key, function (key, val) {
                    callback(model, key, val);
                });
            }
        }
    };
    //  part II of this same functionality
    self.subscribeToProperty = function (model, key, callback) {
        model[key].subscribe(function (val) {
            callback(key, val);
        });
    };

    $.getJSON("/employees", function (data) {
        // here we change our employee collection
        // Adds/Removes are seen by an observableArray
        // but NOT property changes.

        // So we'll add each employee as an object with 
        // individual observable properties
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
            };

            //  Tell the watch mechanism to start watching
            //  all the observable properties on this employee.
            //  When any change, it will automatically call
            //  an event handler we'll set up called modelChanged
            self.watchModel(obsEmployee, self.modelChanged);
            return obsEmployee;
        }));
        self.loading(false);
    });

    //  One of our employees has changed...just one property
    //  We'll send ID, the property that changed, and new value
    //  into a JSON object and send it to MVC4 Web Api to update
    self.modelChanged = function (model, key, val) {
        var payload = {
            Id: model.Id,
            PropertyName: key,
            PropertyValue: val
        };

        // The greatest method name EVER!
        var str = JSON.stringify(payload);

        // Do the deed
        $.ajax({
            url: '/employees/save',
            type: 'PUT',
            data: str,
            contentType: 'application/json',
            dataType: 'json'
        });
    };
};

$(function () {
    ko.applyBindings(new EmployeeViewModel);
});
