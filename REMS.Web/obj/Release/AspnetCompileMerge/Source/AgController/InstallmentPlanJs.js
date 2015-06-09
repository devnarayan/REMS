var myApp = angular.module('MasterApp', []);
//Defining a Controller 
myApp.controller('InstallmentPlanController', function ($scope, $http, $filter) {

    $("#loading").hide();

    $scope.InstallmentPlanInit = function () {
        $("#MessageArea").hide();
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/PlanCtrl/GetPlanList',
        }).success(function (data) {
            $scope.PlanList = data;

            $http({
                method: 'Get',
                url: '/Master/InstallmentPlan/GetPlanInstallmentByPlanID',
                params:{planID:data[0].PlanID}
            }).success(function (data) {
                $scope.InstallmentPlanList = data;
                $("#loading").hide();
            }).error(function (error) {
                $("#loading").hide();
                $("#MessageArea").show();
                $scope.MessageTitle = "Error";
                $scope.MessageClass = "alert-danger";
                $scope.Message = "Installment Plan can't loaded.";
            })
        });

        $("#btnEdit").hide();
    }

    $scope.ChangePlanInstallment = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/InstallmentPlan/GetPlanInstallmentByPlanID',
            params: { planID: $("#PlanID").find(":selected").val() }
        }).success(function (data) {
            $scope.InstallmentPlanList = data;
            $("#loading").hide();
        }).error(function (error) {
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageTitle = "Error";
            $scope.MessageClass = "alert-danger";
            $scope.Message = "Installment Plan can't loaded.";
        })
    }
    $scope.AddPlanInstallmentSave = function () {

        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                Installment: {
                    required: true
                },
                InstallmentNo:{
                    required: true,
                    digits:true
                },
                Amount: {
                    required: true,
                }
            },

            // Messages for form validation
            messages: {
                Installment: {
                    required: 'Please enter installment name'
                },
                InstallmentNo: {
                    required: 'Please enter serial no of installment',
                    digits: 'Digits only please'
                },
                Amount: {
                    required: 'Plase enter BSP percenrages',
                }
            },

            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        //  $scope.ads.ChargeType = $("#divChargeType input[type='radio']:checked").val();
                        $("#loading").show();
                        $http({
                            method: 'Post',
                            url: '/Master/InstallmentPlan/SavePlanInstallment',
                            data: JSON.stringify($scope.installment),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            if (data == "0") {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Error";
                                $scope.MessageClass = "alert-danger";
                                $scope.Message = "Installment Plan Record can not inserted.";
                            }
                            else {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Success";
                                $scope.MessageClass = "alert-success";
                                $scope.Message = "Installment Plan Record inserted successfully.";
                                $http({
                                    method: 'Get',
                                    url: '/Master/InstallmentPlan/GetPlanInstallmentByPlanID',
                                    params: { planID: $("#PlanID").find(":selected").val() }
                                }).success(function (data) {
                                    $scope.InstallmentPlanList = data;
                                }).error(function (error) {
                                    $("#MessageArea").show();
                                    $scope.MessageTitle = "Error";
                                    $scope.MessageClass = "alert-danger";
                                    $scope.Message = "Installment Plan Record can't loaded.";
                                })
                            }
                            $("#loading").hide();
                        }).error(function (error) {
                            //Showing error message
                            $("#loading").hide();
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.MessageClass = "alert-danger";
                            $scope.Message = "Installment Plan Record can not inserted.";
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

    $scope.EditPlanInstallment = function (PlanInstallmentID) {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/InstallmentPlan/GetPlanInstallment',
            params: { planInstallmentid: PlanInstallmentID }
        }).success(function (data) {
            $scope.installment = data;
            $("#btnEdit").show();
            $("#btnSave").hide();
            $("#loading").hide();
        }).error(function (error) {
            $("#MessageArea").show();
            $scope.MessageTitle = "Error";
            $scope.Message = "Plan installment Record can't loaded.";
            $("#btnEdit").hide();
            $("#btnSave").show();
            $("#loading").hide();
        })

    }
    $scope.UpdatePlanInstallment = function () {

        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                Installment: {
                    required: true
                },
                InstallmentNo: {
                    required: true,
                    digits: true
                },
                Amount: {
                    required: true,
                }
            },

            // Messages for form validation
            messages: {
                Installment: {
                    required: 'Please enter installment name'
                },
                InstallmentNo: {
                    required: 'Please enter serial no of installment',
                    digits: 'Digits only please'
                },
                Amount: {
                    required: 'Plase enter BSP percenrages',
                }
            },
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();
                        $http({
                            method: 'Post',
                            url: '/Master/InstallmentPlan/EditPlanInstallment',
                            data: JSON.stringify($scope.installment),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            if (data == "0") {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Error";
                                $scope.MessageClass = "alert-danger";
                                $scope.Message = "Installment Plan Record can not updated.";
                            }
                            else {
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Success";
                                $scope.MessageClass = "alert-success";
                                $scope.Message = "Installment Plan Record updated successfully.";
                                $http({
                                    method: 'Get',
                                    url: '/Master/InstallmentPlan/GetPlanInstallmentByPlanID',
                                    params: { planID: $("#PlanID").find(":selected").val() }
                                }).success(function (data) {
                                    $scope.InstallmentPlanList = data;
                                }).error(function (error) {
                                    $("#MessageArea").show();
                                    $scope.MessageTitle = "Error";
                                    $scope.MessageClass = "alert-danger";
                                    $scope.Message = "Installment Plan Record can't loaded.";
                                })
                            }
                            $("#loading").hide();
                        }).error(function (error) {
                            //Showing error message
                            $("#loading").hide();
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.MessageClass = "alert-danger";
                            $scope.Message = "Installment Plan Record can not updated.";
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