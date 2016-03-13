
var myApp = angular.module('AdminApp', []);
//Defining a Controller 
myApp.controller('PropertyProjectController', function ($scope, $http, $filter) {
    // Get all Property list for dropdownlist
    $scope.Error = "";
    var orderBy = $filter('orderBy');

    $scope.AddProjectInit = function () {
        $http({
            method: 'Get',
            url: '/Admin/PropertyProject/GetAllProjects'
        }).success(function (data) {
            $scope.ProjectList = data;
        })
    }

    $scope.AddProjectSave = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                PropertyPrefix: {
                    required: true
                },
                Block: {
                    required: true
                },
                PropertyName: {
                    required: true,
                    minlength: 2,
                    maxlength: 100
                },
                CompanyName: {
                    required: true
                },
                Location: {
                    required: true
                },
                AuthoritySign: {
                    required: true
                },
                PropertyAddress: {
                    required: true,
                    minlength: 10,
                    maxlength: 256
                },
                OfficeAddress: {
                    required: true,
                    minlength: 10,
                    maxlength: 256
                },
                Jurisdiction: {
                    required: true
                },
                PossessionDate: {
                    required: true
                },
                Contact:{
                    required:true
                },
                AuthEmail:{
                    required:true
                }
            },

            // Messages for form validation
            messages: {
                PropertyPrefix: {
                    required: "Property prefix name is required for naming of property doc"
                },
                Block: {
                    required: "Please enter block name"
                },
                PropertyName: {
                    required: "Enter property name",
                    minlength: "Property can't not less then 2 chars",
                    maxlength: "Property can't not more then 100 chars",
                },
                CompanyName: {
                    required: "Enter Company name of this property"
                },
                Location: {
                    required: "Enter location of property tower"
                },
                AuthoritySign: {
                    required: "Enter name of authorized person of property"
                },
                PropertyAddress: {
                    required: "Enter property full address",
                    minlength: "Property can't not less then 10 chars",
                    maxlength: "Property can't not more then 256 chars",
                },
                OfficeAddress: {
                    required: "Enter Registered Office address of property",
                    minlength: "Property can't not less then 10 chars",
                    maxlength: "Property can't not more then 256 chars",
                },
                Jurisdiction: {
                    required: "Enter Jurisdiction details"
                },
                PossessionDate: {
                    required: "Enter Possession Date of property"
                },
                Contact:{
                    required:"Enter Contact No"
                },
                AuthEmail:{
                    required:"Enter Email Id"
                }
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {

                        $http({
                            method: 'POST',
                            url: '/Admin/PropertyProject/SaveProject',
                            data: JSON.stringify($scope.project),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            $scope.status = "The Property Project Saved Successfully!!!";
                            alert("Tower has been saved successfully");
                        }).error(function (error) {
                            //Showing error message
                            $scope.status = 'Unable to save property: ' + error.message;
                        });
                    }
                });
            },

            // Do not change code below.
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
            }
        });
    }

    $scope.AddProjectTypeInit = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/PropertyProject/GetAllProjects'
        }).success(function (data) {
            $scope.ProjectList = data;
            $http({
                method: 'Get',
                url: '/Admin/PropertyProject/GetProjectType',
                params: { projectID: 0 },
            }).success(function (data) {
                $scope.ProjectTypeList = data;

                $("#loading").hide();
            })
        })
    }

    $scope.AddPropertyTypeSave = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                ProjectID: {
                    required: true
                },

                ProjectType: {
                    required: true
                },
            },

            // Messages for form validation
            messages: {
                ProjectID: {
                    required: "Property select project name"
                },
                ProjectType: {
                    required: "Please enter property type"
                },
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {

                        $("#loading").show();
                        $http({
                            method: 'POST',
                            url: '/Admin/PropertyProject/SaveProjectType',
                            data: JSON.stringify($scope.project),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            $http({
                                method: 'Get',
                                url: '/Admin/PropertyProject/GetProjectType',
                                params: { projectID: $("#ProjectID").find(":selected").val() },
                            }).success(function (data) {
                                $scope.ProjectTypeList = data;
                                $("#loading").hide();

                                $scope.status = "The Property Project Saved Successfully.";
                                alert("The Property Project Saved Successfully.");
                            })
                        }).error(function (error) {
                            //Showing error message
                            $("#loading").hide();
                            $scope.status = 'Unable to save property: ' + error.message;
                        });
                    }
                });
            },

            // Do not change code below.
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
                $("#loading").hide();
            }
        });
    }

    $scope.ProjectChange = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/PropertyProject/GetProjectType',
            params: { projectID: $("#ProjectID").find(":selected").val() },
        }).success(function (data) {
            $scope.ProjectTypeList = data;

            $("#loading").hide();
        })
    }
    $('#loading').hide();
})