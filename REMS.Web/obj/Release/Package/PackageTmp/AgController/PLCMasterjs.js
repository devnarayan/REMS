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
            $scope.MessageClass = "alert-danger";
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
                                $scope.MessageClass = "alert-danger";
                                $scope.Message = "PLC Record can not inserted.";
                            }
                            else {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Success";
                                $scope.MessageClass = "alert-success";
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
                                    $scope.MessageClass = "alert-danger";
                                    $scope.Message = "PLC Record can't loaded.";
                                })
                            }
                            $("#loading").hide();
                        }).error(function (error) {
                            //Showing error message
                            $("#loading").hide();
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.MessageClass = "alert-danger";
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
                                $scope.MessageClass = "alert-danger";
                                $scope.Message = "PLC Record can not inserted.";
                            }
                            else {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Success";
                                $scope.MessageClass = "alert-success";
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
                                    $scope.MessageClass = "alert-danger";
                                    $scope.Message = "PLC Record can't loaded.";
                                })
                            }
                            $("#loading").hide();
                        }).error(function (error) {
                            //Showing error message
                            $("#loading").hide();
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.MessageClass = "alert-danger";
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
    $("#loading").hide();
})