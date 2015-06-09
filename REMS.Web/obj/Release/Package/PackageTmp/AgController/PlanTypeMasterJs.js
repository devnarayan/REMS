var myApp = angular.module('MasterApp', []);
myApp.controller('PlanCtrlController', function ($scope, $http, $filter) {
    $scope.Error = "";
    var orderBy = $filter('orderBy');
    $("#loading").show();
    $scope.FlatTypeMasterInit = function () {

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
                $http({
                    method: 'Get',
                    url: '/Master/PlanCtrl/GetPlanList',
                }).success(function (data) {
                    $scope.PlanList = data;
                });
                $http({
                    method: 'Get',
                    url: '/Master/PlanCtrl/GetPlanTypeMasterList',
                }).success(function (data) {
                    $scope.PlanTypeMasterList = data;
                })
            })
        })
        $("#btnUpdate").hide();
        $("#btnSave").show();
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
    $scope.ChangeFlatTypeByName = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/Flat/GetFlatTypeSizeByNameList',
            params: { ftype: $("#FlatTypeList").find(":selected").val() }
        }).success(function (data) {
            $scope.FlatTypeSizeList = data;
            $("#loading").hide();
        }).error(function (error) {
            $("#loading").hide();
        })
    }

    $scope.AddPlanTypeMsterSave = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                PlanType: {
                    required: true
                },
                SalePrice: {
                    required: true
                },
            },

            // Messages for form validation
            messages: {
                PlanType: {
                    required: "Please enter Pla Type Name."
                },
                SalePrice: {
                    required: "Please enter sale price of flat"
                },
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();

                            $http({
                                method: 'POST',
                                url: '/Master/PlanCtrl/SavePlanTypeMaster',
                                data: JSON.stringify($scope.plan),
                                headers: { 'Content-Type': 'application/JSON' }
                            }).success(function (data) {
                                var status = data;
                                $http({
                                    method: 'Get',
                                    url: '/Master/PlanCtrl/GetPlanTypeMasterList',
                                }).success(function (data) {
                                    $scope.PlanTypeMasterList = data;
                                    $("#loading").hide();
                                    if (status == 0) {
                                        $("#MessageArea").show();
                                        $scope.MessageTitle = "Error";
                                        $scope.MessageClass = "danger";
                                        $scope.Message = "Plan Price can not inserted.";
                                    }
                                    else {
                                        $("#MessageArea").show();
                                        $scope.MessageClass = "success";
                                        $scope.MessageTitle = "Success";
                                        $scope.Message = "Plan Price inserted successfully.";
                                    }
                                })
                            }).error(function (error) {
                                //Showing error message
                                $("#MessageArea").show();
                                $scope.MessageClass = "danger";
                                $scope.MessageTitle = "Error";
                                $scope.Message = "Plan Price can not inserted.";
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
    $scope.UpdatePlanTypeMster = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                PlanType: {
                    required: true
                },
                SalePrice: {
                    required: true
                },
            },

            // Messages for form validation
            messages: {
                PlanType: {
                    required: "Please enter Pla Type Name."
                },
                SalePrice: {
                    required: "Please enter sale price of flat"
                },
            },


            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();
                            $http({
                                method: 'POST',
                                url: '/Master/PlanCtrl/EditPlanTypeMaster',
                                data: JSON.stringify($scope.plan),
                                headers: { 'Content-Type': 'application/JSON' }
                            }).success(function (data) {
                                var status = data;
                                $http({
                                    method: 'Get',
                                    url: '/Master/PlanCtrl/GetPlanTypeMasterList',
                                }).success(function (data) {
                                    $scope.PlanTypeMasterList = data;
                                    $("#loading").hide();
                                    if (status == 0) {
                                        $("#MessageArea").show();
                                        $scope.MessageTitle = "Error";
                                        $scope.MessageClass = "danger";
                                        $scope.Message = "Plan Price can not updated.";
                                    }
                                    else {
                                        $("#MessageArea").show();
                                        $scope.MessageClass = "success";
                                        $scope.MessageTitle = "Success";
                                        $scope.Message = "Plan Price updated successfully.";
                                    }
                                })
                            }).error(function (error) {
                                //Showing error message
                                $("#loading").hide();
                                $("#MessageArea").show();
                                $scope.MessageClass = "danger";
                                $scope.MessageTitle = "Error";
                                $scope.Message = "Plan Price can not updated !";
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
    $scope.EditPlanTypeMaster = function (PlanTypeMasterID) {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/PlanCtrl/GetPlanTypeMasterByID',
            params: { planTypeMasterID: PlanTypeMasterID }
        }).success(function (data) {
            $scope.plan = data;
            $("#loading").hide();
            $("#btnSave").hide();
            $("#btnUpdate").show();
        }).error(function (error) {
            $("#loading").hide();
        })
    }

    $scope.CancelPlanTypeMaster = function () {
        $("#addtionalchargediv").hide();
        $("#btnUpdate").hide();
        $("#btnSave").show();
    }


    //======================= Plan ==============================
    $scope.PlanInit = function () {
        $http({
            method: 'Get',
            url: '/Master/PlanCtrl/GetPlanList',
        }).success(function (data) {
            $scope.PlanList = data;
        });
        $("#btnUpdate").hide();
        $("#btnSave").show();
    }

    $scope.AddPlanSave = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                PlanName: {
                    required: true
                },
            },

            // Messages for form validation
            messages: {
                PlanName: {
                    required: "Please enter Plan Name."
                },
               
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();

                            $http({
                                method: 'POST',
                                url: '/Master/PlanCtrl/SavePlan',
                                data: JSON.stringify($scope.pln),
                                headers: { 'Content-Type': 'application/JSON' }
                            }).success(function (data) {
                                var status = data;
                                $http({
                                    method: 'Get',
                                    url: '/Master/PlanCtrl/GetPlanList',
                                }).success(function (data) {
                                    $scope.PlanList = data;
                                    $("#loading").hide();
                                    if (status == 0) {
                                        $("#MessageArea").show();
                                        $scope.MessageClass = "danger";
                                        $scope.MessageTitle = "Error";
                                        $scope.Message = "Plan Record can not inserted.";
                                    }
                                    else {
                                        $("#MessageArea").show();
                                        $scope.MessageClass = "success";
                                        $scope.MessageTitle = "Success";
                                        $scope.Message = "Plan Record inserted successfully.";
                                    }
                                })
                            }).error(function (error) {
                                //Showing error message
                                $("#MessageArea").show();
                                $scope.MessageClass = "danger";
                                $scope.MessageTitle = "Error";
                                $scope.Message = "Plan Record can not inserted.";
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
    $scope.UpdatePlanSave = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                PlanName: {
                    required: true
                },
            },
            // Messages for form validation
            messages: {
                PlanName: {
                    required: "Please enter Plan Name."
                },

            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();

                        $http({
                            method: 'POST',
                            url: '/Master/PlanCtrl/EditPlan',
                            data: JSON.stringify($scope.pln),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            var status = data;
                            $http({
                                method: 'Get',
                                url: '/Master/PlanCtrl/GetPlanList',
                            }).success(function (data) {
                                $scope.PlanList = data;
                                $("#loading").hide();
                                if (status == 0) {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "danger";
                                    $scope.MessageTitle = "Error";
                                    $scope.Message = "Plan Record can not updated.";
                                }
                                else {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "success";
                                    $scope.MessageTitle = "Success";
                                    $scope.Message = "Plan Record updated successfully.";
                                }
                            })
                        }).error(function (error) {
                            //Showing error message
                            $("#MessageArea").show();
                            $scope.MessageClass = "danger";
                            $scope.MessageTitle = "Error";
                            $scope.Message = "Plan Record can not updated.";
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
    $scope.EditPlan = function (planID) {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/PlanCtrl/GetPlanByID',
            params: { planID: planID }
        }).success(function (data) {
            $scope.pln = data;
            $("#loading").hide();
            $("#btnUpdate").show();
            $("#btnSave").hide();
        }).error(function (error) {
            $("#loading").hide();
        })
    }

    $scope.CancelPlan = function () {
        $("#btnUpdate").hide();
        $("#btnSave").show();
    }

    $("#loading").hide();
})