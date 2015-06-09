
var myApp = angular.module('MasterApp', []);
//Defining a Controller 
myApp.controller('AChargesCtrlController', function ($scope, $http, $filter) {

    $scope.AdditionalChargeInit = function () {
        $("#MessageArea").hide();
        $http({
            method: 'Get',
            url: '/Master/AChargesCtrl/AditionalChargeList',
            params: {}
        }).success(function(data){
            $scope.AdditionalChargeList = data;
            $scope.ads.ChargeType = "Sq. Ft.";
        }).error(function (error) {
            $("#MessageArea").show();
            $scope.MessageTitle = "Error";
            $scope.MessageClass = "alert-danger";
            $scope.Message = "Additional Charge Record can't loaded.";
        })
        $("#btnEdit").hide();
    }
    $scope.AddAdditionalCharge = function () {

            var $checkoutForm = $('#checkout-form').validate({
                // Rules for form validation
                rules: {
                    Name: {
                        required: true
                    },
                    Amount: {
                        required: true,
                        digits: true
                    }
                },

                // Messages for form validation
                messages: {
                    Name: {
                        required: 'Please enter Additional Charge name'
                    },
                    Amount: {
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
                                url: '/Master/AChargesCtrl/SaveAdditionalCharge',
                                data: JSON.stringify($scope.ads),
                                headers: { 'Content-Type': 'application/JSON' }
                            }).success(function (data) {
                                $scope.FloorList = data;
                                if (status == "0") {
                                    $("#MessageArea").show();
                                    $scope.MessageTitle = "Error";
                                    $scope.MessageClass = "alert-danger";
                                    $scope.Message = "Additional Charge Record can not inserted.";

                                }
                                else {
                                    $("#MessageArea").show();
                                    $scope.MessageTitle = "Success";
                                    $scope.MessageClass = "alert-success";
                                    $scope.Message = "Additional Charge Record inserted successfully.";
                                    $http({
                                        method: 'Get',
                                        url: '/Master/AChargesCtrl/AditionalChargeList',
                                        params: {}
                                    }).success(function (data) {
                                        $scope.AdditionalChargeList = data;
                                    }).error(function (error) {
                                        $("#MessageArea").show();
                                        $scope.MessageTitle = "Error";
                                        $scope.MessageClass = "alert-danger";
                                        $scope.Message = "Additional Charge Record can't loaded.";
                                    })
                                }
                                $("#loading").hide();
                            }).error(function (error) {
                                //Showing error message
                                $("#loading").hide();
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Error";
                                $scope.MessageClass = "alert-danger";
                                $scope.Message = "Additional Charge Record can not inserted.";
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

    $scope.EditAdditionalCharge = function (additionalChargeid) {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/AChargesCtrl/GetAdditionalCharge',
            params: { additionalChargeid: additionalChargeid }
        }).success(function (data) {
            $scope.ads = data;
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
    $scope.UpdateAdditionalCharge = function () {

        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                Name: {
                    required: true
                },
                Amount: {
                    required: true,
                    digits: true
                }
            },

            // Messages for form validation
            messages: {
                Name: {
                    required: 'Please enter Additional Charge name'
                },
                Amount: {
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
                            url: '/Master/AChargesCtrl/EditAdditionalCharge',
                            data: JSON.stringify($scope.ads),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            $scope.FloorList = data;
                            if (status == "0") {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Error";
                                $scope.MessageClass = "alert-danger";
                                $scope.Message = "Additional Charge Record can not inserted.";
                            }
                            else {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Success";
                                $scope.MessageClass = "alert-success";
                                $scope.Message = "Additional Charge Record updated successfully.";
                                $http({
                                    method: 'Get',
                                    url: '/Master/AChargesCtrl/AditionalChargeList',
                                    params: {}
                                }).success(function (data) {
                                    $scope.AdditionalChargeList = data;
                                }).error(function (error) {
                                    $("#MessageArea").show();
                                    $scope.MessageTitle = "Error";
                                    $scope.MessageClass = "alert-danger";
                                    $scope.Message = "Additional Charge Record can't loaded.";
                                })
                            }
                            $("#loading").hide();
                        }).error(function (error) {
                            //Showing error message
                            $("#loading").hide();
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.MessageClass = "alert-danger";
                            $scope.Message = "Additional Charge Record can not inserted.";
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