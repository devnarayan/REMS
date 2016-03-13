var myApp = angular.module('MasterApp', []);
//Defining a Controller 
myApp.controller('PLCController', function ($scope, $http, $filter) {

    $scope.PLCAddInit = function () {
        $("#MessageArea").hide();
        $http({
            method: 'Get',
            url: '/Master/PLC/PLCList',
            params: {}
        }).success(function (data) {
            $scope.PLCList = data;
        }).error(function (error) {
            $("#MessageArea").show();
            $scope.MessageTitle = "Error";
            $scope.MessageClass = "danger";
            $scope.Message = "PLC Record can't loaded.";
        })
        $("#btnEdit").hide();
    }
    $scope.AddPLCSave = function () {

        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                PLCName: {
                    required: true
                },
                AmountSqFt: {
                    required: true,
                    digits: true
                }
            },

            // Messages for form validation
            messages: {
                PLCName: {
                    required: 'Please enter PLC name'
                },
                AmountSqFt: {
                    required: 'Please enter amount',
                    digits: 'Digits only please'
                }
            },

            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        //  $scope.ads.ChargeType = $("#divChargeType input[type='radio']:checked").val();
                        $("#loading").show();
                        $http({
                            method: 'Post',
                            url: '/Master/PLC/SavePLC',
                            data: JSON.stringify($scope.plc),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            if (data == "0") {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Error";
                                $scope.MessageClass = "danger";
                                $scope.Message = "PLC Record can not inserted.";
                            }
                            else {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Success";
                                $scope.MessageClass = "success";
                                $scope.Message = "PLC Record inserted successfully.";
                                $http({
                                    method: 'Get',
                                    url: '/Master/PLC/PLCList',
                                    params: {}
                                }).success(function (data) {
                                    $scope.PLCList = data;
                                }).error(function (error) {
                                    $("#MessageArea").show();
                                    $scope.MessageTitle = "Error";
                                    $scope.MessageClass = "danger";
                                    $scope.Message = "PLC Record can't loaded.";
                                })
                            }
                            $("#loading").hide();
                        }).error(function (error) {
                            //Showing error message
                            $("#loading").hide();
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.MessageClass = "danger";
                            $scope.Message = "PLC Record can not inserted.";
                        });
                    }
                });
            },

            // Do not change code below
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
            }
        });
    }

    $scope.EditPLC = function (PLCID) {
        $scope.PLCID = PLCID;
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/PLC/GetPLC',
            params: { plcid: PLCID }
        }).success(function (data) {
            $scope.plc = data;
            $("#btnEdit").show();
            $("#btnSave").hide();
            $("#loading").hide();
        }).error(function (error) {
            $("#MessageArea").show();
            $scope.MessageTitle = "Error";
            $scope.Message = "Additional Charge Record can't loaded.";
            $("#btnEdit").hide();
            $("#btnSave").show();
            $("#loading").hide();
        })

    }
    $scope.DeletePLC = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/PLC/DeletePLC',
            params: { plcid: $scope.PLCID }
        }).success(function (data) {
            $("#myModal").modal('hide');
            $("#loading").hide();
        }).error(function (error) {
            $("#MessageArea").show();
            $scope.MessageTitle = "Error";
            $scope.Message = "Additional Charge Record can't loaded.";
            $("#btnEdit").hide();
            $("#btnSave").show();
            $("#loading").hide();
        });
    }
    $scope.CancelPLC = function () {
        $("#btnSave").show();
        $("#btnEdit").hide();
    }
    $scope.UpdatePLC = function () {

        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                PLCName: {
                    required: true
                },
                AmountSqFt: {
                    required: true,
                    digits: true
                }
            },

            // Messages for form validation
            messages: {
                PLCName: {
                    required: 'Please enter PLC name'
                },
                AmountSqFt: {
                    required: 'Please enter amount',
                    digits: 'Digits only please'
                }
            },
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();

                        $http({
                            method: 'Post',
                            url: '/Master/PLC/EditPLC',
                            data: JSON.stringify($scope.plc),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            if (data == "0") {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Error";
                                $scope.MessageClass = "danger";
                                $scope.Message = "PLC Record can not inserted.";
                            }
                            else {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Success";
                                $scope.MessageClass = "success";
                                $scope.Message = "PLC Record updated successfully.";
                                $http({
                                    method: 'Get',
                                    url: '/Master/PLC/PLCList',
                                    params: {}
                                }).success(function (data) {
                                    $scope.PLCList = data;
                                }).error(function (error) {
                                    $("#MessageArea").show();
                                    $scope.MessageTitle = "Error";
                                    $scope.MessageClass = "danger";
                                    $scope.Message = "PLC Record can't loaded.";
                                })
                            }
                            $("#loading").hide();
                        }).error(function (error) {
                            //Showing error message
                            $("#loading").hide();
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.MessageClass = "danger";
                            $scope.Message = "PLC Record can not inserted.";
                        });
                    }
                });
            },

            // Do not change code below
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
            }
        });
    }

    $scope.ServiceTaxInit = function () {
        $("#btnSave").show();
        $("#btnEdit").hide();
        $("#MessageArea").hide();
        $http({
            method: 'Get',
            url: '/Master/PLC/GetServiceTaxList',
            params: {}
        }).success(function (data) {
            $scope.ServiceTaxList = data;
        }).error(function (error) {
            $("#MessageArea").show();
            $scope.MessageTitle = "Error";
            $scope.MessageClass = "danger";
            $scope.Message = "ServiceTax Record can't loaded.";
        })
        $("#btnEdit").hide();
    }
    $scope.AddServiceTax = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                ServiceTaxName: {
                    required: true
                },
                ServiceTaxPer: {
                    required: true,
                   // digits: true
                },
                EndDate: {
                    required: true,
                },
            },

            // Messages for form validation
            messages: {
                ServiceTaxName: {
                    required: 'Please enter Service Tax name'
                },
                ServiceTaxPer: {
                    required: 'Please enter Tax Percentage',
                   // digits: 'Digits only please'
                },
                EndDate: {
                    required: 'Please enter Date'
                }
            },

            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        //  $scope.ads.ChargeType = $("#divChargeType input[type='radio']:checked").val();
                        $("#loading").show();
                        $http({
                            method: 'Post',
                            url: '/Master/PLC/AddServiceTax',
                            data: JSON.stringify($scope.tax),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            if (data == "0") {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Error";
                                $scope.MessageClass = "danger";
                                $scope.Message = "Service Tax Record can not inserted.";
                            }
                            else {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Success";
                                $scope.MessageClass = "success";
                                $scope.Message = "Service Tax Record inserted successfully.";
                                $http({
                                    method: 'Get',
                                    url: '/Master/PLC/GetServiceTaxList',
                                    params: {}
                                }).success(function (data) {
                                    $scope.ServiceTaxList = data;
                                }).error(function (error) {
                                    $("#MessageArea").show();
                                    $scope.MessageTitle = "Error";
                                    $scope.MessageClass = "danger";
                                    $scope.Message = "ServiceTax Record can't loaded.";
                                })
                            }
                            $("#loading").hide();
                        }).error(function (error) {
                            //Showing error message
                            $("#loading").hide();
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.MessageClass = "danger";
                            $scope.Message = "PLC Record can not inserted.";
                        });
                    }
                });
            },

            // Do not change code below
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
            }
        });
    }
    $scope.UpdateServiceTax = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                ServiceTaxName: {
                    required: true
                },
                ServiceTaxPer: {
                    required: true,
                    //digits: true
                },
                EndDate: {
                    required: true,
                },
            },

            // Messages for form validation
            messages: {
                ServiceTaxName: {
                    required: 'Please enter Service Tax name'
                },
                ServiceTaxPer: {
                    required: 'Please enter Tax Percentage',
                   // digits: 'Digits only please'
                },
                EndDate: {
                    required: 'Please enter Date'
                }
            },

            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        //  $scope.ads.ChargeType = $("#divChargeType input[type='radio']:checked").val();
                        $("#loading").show();
                        $http({
                            method: 'Post',
                            url: '/Master/PLC/UpdateServiceTax',
                            data: JSON.stringify($scope.tax),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            if (data == "0") {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Error";
                                $scope.MessageClass = "danger";
                                $scope.Message = "Service Tax Record can not inserted.";
                            }
                            else {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Success";
                                $scope.MessageClass = "success";
                                $scope.Message = "Service Tax Record updated successfully.";
                                $http({
                                    method: 'Get',
                                    url: '/Master/PLC/GetServiceTaxList',
                                    params: {}
                                }).success(function (data) {
                                    $scope.ServiceTaxList = data;
                                }).error(function (error) {
                                    $("#MessageArea").show();
                                    $scope.MessageTitle = "Error";
                                    $scope.MessageClass = "danger";
                                    $scope.Message = "ServiceTax Record can't loaded.";
                                })
                            }
                            $("#loading").hide();
                        }).error(function (error) {
                            //Showing error message
                            $("#loading").hide();
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.MessageClass = "danger";
                            $scope.Message = "PLC Record can not inserted.";
                        });
                    }
                });
            },

            // Do not change code below
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
            }
        });
    }
    $scope.CancelServiceTax = function () {
        $("#btnSave").show();
        $("#btnEdit").hide();
    }
    $scope.EditServiceTax = function (ServiceTaxID,isEdit) {
        if (isEdit == 1) {
            $("#btnSave").hide();
            $("#btnEdit").show();
        }
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/PLC/GetServiceTaxByID',
            params: { ServiceTaxID: ServiceTaxID }
        }).success(function (data) {
            $scope.tax = data;
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageTitle = "Success";
            $scope.MessageClass = "success";
            $scope.Message = "ServiceTax Record Edited.";
        }).error(function (error) {
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageTitle = "Error";
            $scope.MessageClass = "danger";
            $scope.Message = "ServiceTax Record can't loaded.";
        });
    }
    $scope.ConfirmDeleteServiceTax = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/PLC/DeleteServiceTax',
            params: { ServiceTaxID: $scope.tax.ServiceTaxID }
        }).success(function (data) {
            $scope.tax = data;
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageTitle = "Success";
            $scope.MessageClass = "success";
            $scope.Message = "ServiceTax Record Deleted.";
            $("#myModal").modal('hide');
            $http({
                method: 'Get',
                url: '/Master/PLC/GetServiceTaxList',
                params: {}
            }).success(function (data) {
                $scope.ServiceTaxList = data;
            }).error(function (error) {
                $("#MessageArea").show();
                $scope.MessageTitle = "Error";
                $scope.MessageClass = "danger";
                $scope.Message = "ServiceTax Record can't loaded.";
            })
        }).error(function (error) {
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageTitle = "Error";
            $scope.MessageClass = "danger";
            $scope.Message = "ServiceTax Record can't loaded.";
        });
    }
    $("#loading").hide();
});