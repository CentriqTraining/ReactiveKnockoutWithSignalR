// Model Version 1
var EmployeeViewModel = function () {
    //  Need a way to refer to this employee view everywhere
    //  within this model 
    //  we'll use self instead of this - because this has 
    //  different meaning in all of those anonymous functions
    var self = this;

    //  This is to store our data.  We want it as an observable
    //  Array so we get notifications anytime it changes
    self.employees = ko.observableArray();

    //  We use this observable val to track when loading is done
    //  We set it to loading = true
    // All observable values are set using ()
    self.loading = ko.observable(true);

    //  Now go get our data from MVC4 Web API
    $.getJSON('/employees', function (data) {

        //  When it is done...put the data right into 
        //  our observable array
        self.employees(data);

        //  Data is here...set loading = false
        self.loading(false);
    });
}

$(function () {
    // when the DOM is ready, let's load our viewmodel
    // and apply all the bindings in our index.cshtml
    ko.applyBindings(new EmployeeViewModel());
})