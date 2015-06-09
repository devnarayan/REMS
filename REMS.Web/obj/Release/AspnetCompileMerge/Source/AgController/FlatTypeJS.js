var myApp = angular.module('MasterApp', []);
//Defining a Controller 
myApp.controller('FlatTypeController', function ($scope, $http, $filter) {
    // Get all Property list for dropdownlist
    $scope.Error = "";
    $("#loading").show();
    $scope.FlatTypeInit = function () {

        $http({
            method: 'Get',
            url: '/Master/Flat/GetFlatType'
        }).success(function (data) {
            $scope.FlatTypeList = data;
        })

        $("#btnEdit").hide();
    }
    $scope.EditFlatType = function (FlatTypeID) {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/Flat/GetFlatTypeByID',
            params: { flattypeid: FlatTypeID }
        }).success(function (data) {
            $scope.ftp = data;
            $("#loading").hide();
            $("#btnEdit").show();
            $("#btnSave").hide();
        })
    }
    $scope.CancelClick = function () {
        $("#btnSave").show();
        $("#btnEdit").hide();
        $scope.ftp.FType = "";
        $scope.ftp.FullName = "";
    }
    $scope.AddFlatType = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                FType: {
                    required: true
                },
                FullName: {
                    required: true
                },
            },

            // Messages for form validation
            messages: {
                FType: {
                    required: "Please enter Flat Type."
                },
                FullName: {
                    required: "Enter Fullname of flat type"
                },
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();
                        $http({
                            method: 'POST',
                            url: '/Master/Flat/SaveFlatType',
                            data: JSON.stringify($scope.ftp),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            var status = data;
                            $http({
                                method: 'Get',
                                url: '/Master/Flat/GetFlatType'
                            }).success(function (data) {
                                $scope.FlatTypeList = data;
                                $("#loading").hide();
                                if (status == 0) {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "danger";
                                    $scope.MessageTitle = "Error";
                                    $scope.Message = "Flat Type Record can not inserted.";
                                }
                                else {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "success";
                                    $scope.MessageTitle = "Success";
                                    $scope.Message = "Flat Type Record inserted successfully.";
                                }
                            })
                        }).error(function (error) {
                            //Showing error message
                            $("#MessageArea").show();
                            $scope.MessageClass = "danger";
                            $scope.MessageTitle = "Error";
                            $scope.Message = "Flat Type Record can not inserted.";
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
    $scope.UpdateFlatType = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                FType: {
                    required: true
                },
                FullName: {
                    required: true
                },
            },

            // Messages for form validation
            messages: {
                FType: {
                    required: "Please enter Flat Type."
                },
                FullName: {
                    required: "Enter Fullname of flat type"
                },
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();
                        $http({
                            method: 'POST',
                            url: '/Master/Flat/UpdateFlatType',
                            data: JSON.stringify($scope.ftp),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            var status = data;
                            $http({
                                method: 'Get',
                                url: '/Master/Flat/GetFlatType'
                            }).success(function (data) {
                                $scope.FlatTypeList = data;
                                $("#loading").hide();
                                if (status == 0) {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "danger";
                                    $scope.MessageTitle = "Error";
                                    $scope.Message = "Flat Type Record can not inserted.";
                                }
                                else {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "success";
                                    $scope.MessageTitle = "Success";
                                    $scope.Message = "Flat Type Record inserted successfully.";
                                }
                            })
                        }).error(function (error) {
                            //Showing error message
                            $("#MessageArea").show();
                            $scope.MessageClass = "danger";
                            $scope.MessageTitle = "Error";
                            $scope.Message = "Flat Type Record can not inserted.";
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

    $scope.FlatTypeSizeInit = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/Flat/GetFlatType'
        }).success(function (data) {
            $scope.FlatTypeList = data;
            $http({
                method: 'Get',
                url: '/Master/Flat/GetFlatTypeSizeList',
                params: { falttypeid: data[0].FlatTypeID }
            }).success(function (data) {
                $scope.FlatTypeSizeList = data;
                $("#loading").hide();
            }).error(function (error) {
                $("#loading").hide();
            })
        }).error(function (error) {
            $("#loading").hide();
        })
        $("#btnEdit").hide();
        $("#btnSave").show();
    }

    $scope.EditFlatTypeSize = function (FlatTypeID) {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/Flat/GetFlatTypeSizeByID',
            params: { falttypesizeid: FlatTypeID }
        }).success(function (data) {
            $scope.ftps = data;
            $("#loading").hide();
            $("#btnEdit").show();
            $("#btnSave").hide();
        })
    }
    $scope.CancelSizeClick = function () {
        $("#btnSave").show();
        $("#btnEdit").hide();
        $scope.ftp.FType = "";
        $scope.ftp.FullName = "";
    }
    $scope.AddFlatTypeSize = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                FlatTypeList: {
                    required: true
                },
                Size: {
                    required: true,
                },
            },

            // Messages for form validation
            messages: {
                FlatTypeList: {
                    required: "Please enter Flat Type."
                },
                Size: {
                    required: "Enter flat size",
                },
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();
                        $http({
                            method: 'POST',
                            url: '/Master/Flat/SaveFlatTypeSize',
                            data: JSON.stringify($scope.ftps),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            var status = data;
                            $http({
                                method: 'Get',
                                url: '/Master/Flat/GetFlatTypeSizeList',
                                params: { falttypeid: $("#FlatTypeList").find(":selected").val()}
                            }).success(function (data) {
                                $scope.FlatTypeSizeList = data;
                                $("#loading").hide();
                                if (status == "0") {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "danger";
                                    $scope.MessageTitle = "Error";
                                    $scope.Message = "Flat Type Record can not inserted.";
                                }
                                else if(status=="2"){
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "danger";
                                    $scope.MessageTitle = "Error";
                                    $scope.Message = "Flat Size is not valid.";
                                }
                                else {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "success";
                                    $scope.MessageTitle = "Success";
                                    $scope.Message = "Flat Type Size Record inserted successfully.";
                                }
                            }).error(function (error) {
                                $("#loading").hide();
                            })
                        }).error(function (error) {
                            //Showing error message
                            $("#MessageArea").show();
                            $scope.MessageClass = "danger";
                            $scope.MessageTitle = "Error";
                            $scope.Message = "Flat Type Record can not inserted.";
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
    $scope.UpdateFlatTypeSize = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                FlatTypeList: {
                    required: true
                },
                Size: {
                    required: true,
                    digits:true
                },
            },

            // Messages for form validation
            messages: {
                FlatTypeList: {
                    required: "Please enter Flat Type."
                },
                Size: {
                    required: "Enter flat size",
                    digits:"Flat Size only numbers allowed"
                },
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();
                        $http({
                            method: 'POST',
                            url: '/Master/Flat/UpdateFlatTypeSize',
                            data: JSON.stringify($scope.ftps),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            var status = data;
                            $http({
                                method: 'Get',
                                url: '/Master/Flat/GetFlatTypeSizeList',
                                params: { falttypeid: $("#FlatTypeList").find(":selected").val() }
                            }).success(function (data) {
                                $scope.FlatTypeSizeList = data;
                                $("#loading").hide();
                                if (status == "0") {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "danger";
                                    $scope.MessageTitle = "Error";
                                    $scope.Message = "Flat Type Record can not updated.";
                                }
                                else if (status == "2") {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "danger";
                                    $scope.MessageTitle = "Error";
                                    $scope.Message = "Flat Size is not valid.";
                                }
                                else {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "success";
                                    $scope.MessageTitle = "Success";
                                    $scope.Message = "Flat Type Size Record updated successfully.";
                                }
                            }).error(function (error) {
                                $("#loading").hide();
                            })
                        }).error(function (error) {
                            //Showing error message
                            $("#MessageArea").show();
                            $scope.MessageClass = "danger";
                            $scope.MessageTitle = "Error";
                            $scope.Message = "Flat Type Record can not updated.";
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
    $scope.ChangeFlatType = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/Flat/GetFlatTypeSizeList',
            params: { falttypeid: $("#FlatTypeList").find(":selected").val() }
        }).success(function (data) {
            $scope.FlatTypeSizeList = data;
            $("#loading").hide();
        }).error(function (error) {
            $("#loading").hide();
        })
    }

    $("#loading").hide();
})