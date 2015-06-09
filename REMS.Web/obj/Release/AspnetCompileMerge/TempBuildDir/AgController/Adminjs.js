
var myApp = angular.module('AdminApp', []);
//Defining a Controller 
myApp.controller('CreatePropertyController', function ($scope, $http, $filter) {
    // Get all Property list for dropdownlist
    $scope.Error = "";
    var orderBy = $filter('orderBy');

    $scope.AddTowerInit = function () {
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
                $http({
                    method: 'Get',
                    url: '/Admin/CreateProperty/AllTowerProject'
                }).success(function (data) {
                    $scope.TowerProjectList = data;
                    $("#ProjectTypeID").find(":selected").val(0)
                    $("#ProjectTypeID").find(":selected").text("Select Property type");
                    $("#ProjectID").find(":selected").val(0)
                    $("#ProjectID").find(":selected").text("Select Project ");
                    $("#btnUpdate").hide();
                    $("#btnSave").show();

                    $("#loading").hide();
                })
            })
        })
    }
    $scope.ProjectChange = function () {
        $http({
            method: 'Get',
            url: '/Admin/PropertyProject/GetProjectType',
            params: { projectID: $("#ProjectID").find(":selected").val() },
        }).success(function (data) {
            $scope.ProjectTypeList = data;
        })
    }
    $scope.AddTowerSave = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                TowerName: {
                    required: true
                },
                //TowerNo: {
                //    required: true
                //},

                Block: {
                    required: true
                },
                ProjectID: {
                    required: true
                },
                ProjectTypeID: {
                    required: true
                },

            },

            // Messages for form validation
            messages: {
                TowerName: {
                    required: "Please enter tower name."
                },
                //TowerNo: {
                //    required: "Tower no can't blank."
                //},
                Block: {
                    required: "Please enter block name"
                },
                ProjectID: {
                    required: "select project name"
                },
                ProjectTypeID: {
                    required: "select property project type"
                },
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $('#loading').show();

                        $scope.tower.ProjectTypeID = $("#ProjectTypeID").find(":selected").val();
                        $http({
                            method: 'POST',
                            url: '/Admin/CreateProperty/SaveTower',
                            data: JSON.stringify($scope.tower),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            if (data == "2") {
                                $('#loading').hide();
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Error";
                                $scope.MessageClass = "danger";
                                $scope.Message = "Tower Record already exists.";
                            }
                            else {

                                $http({
                                    method: 'Get',
                                    url: '/Admin/CreateProperty/AllTowerProject'
                                }).success(function (data) {
                                    $scope.TowerProjectList = data;
                                    $('#loading').hide();
                                    $scope.status = "The Property Saved Successfully!!!";
                                    $("#MessageArea").show();
                                    $scope.MessageTitle = "Success";
                                    $scope.MessageClass = "success";
                                    $scope.Message = "Tower Record can not inserted.";
                                })
                            }
                        }).error(function (error) {
                            //Showing error message
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.MessageClass = "danger";
                            $scope.Message = "Tower Record can not inserted.";
                            $('#loading').hide();
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

    $scope.EditTowerClick = function (towerid, TowerName, Block, ProjectID) {
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getTowerbyID',
            params: { towerid: towerid },
        }).success(function (data) {
            $scope.tower = data;
            $("#ProjectTypeID").find(":selected").val(data.ProjectTypeID);
            $("#btnUpdate").show();
            $("#btnSave").hide();
        })
    }
    $scope.UpdateTower = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                TowerName: {
                    required: true
                },
                //TowerNo: {
                //    required: true
                //},

                Block: {
                    required: true
                },
                ProjectID: {
                    required: true
                },
                ProjectTypeID: {
                    required: true
                },

            },

            // Messages for form validation
            messages: {
                TowerName: {
                    required: "Please enter tower name."
                },
                //TowerNo: {
                //    required: "Tower no can't blank."
                //},
                Block: {
                    required: "Please enter block name"
                },
                ProjectID: {
                    required: "select project name"
                },
                ProjectTypeID: {
                    required: "select property project type"
                },
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $('#loading').show();

                        $scope.tower.ProjectTypeID = $("#ProjectTypeID").find(":selected").val();
                        $http({
                            method: 'POST',
                            url: '/Admin/CreateProperty/UpdateTower',
                            data: JSON.stringify($scope.tower),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            $http({
                                method: 'Get',
                                url: '/Admin/CreateProperty/AllTowerProject'
                            }).success(function (data) {
                                $scope.TowerProjectList = data;
                                $('#loading').hide();
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Success";
                                $scope.MessageClass = "success";
                                $scope.Message = "Tower has been updated successfully";
                                $scope.status = "Tower has been updated successfully";
                            })
                        }).error(function (error) {
                            $('#loading').hide();
                            //Showing error message
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.MessageClass = "danger";
                            $scope.Message = "Unable to save property";
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
    $('#loading').hide();
})