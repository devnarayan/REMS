
var myApp = angular.module('PropertyApp', []);
//Defining a Controller 
myApp.controller('PropertyController', function ($scope, $http, $filter) {
    // Get all Property list for dropdownlist
    $scope.Error = "";
    var orderBy = $filter('orderBy');

    // Admin/CreateProperty/AddFlat
    $scope.AddFlatInit = function () {
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getAllTower'
        }).success(function (data) {
            $scope.TowerList = data;
            $http({
                method: 'Get',
                url: '/Master/Flat/GetFlatType'
            }).success(function (data) {
                $scope.FlatTypeList = data;
            })
        })
    }

    $scope.GenerateFaltHtml = function () {

        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                FloorNo: {
                    required: true
                },
                FlatTotal: {
                    required: true
                },
                TowerList: {
                    required: true
                },
                FlatTypeList: {
                    required: true
                },
                FlatSizeList: {
                    required: true
                }

            },

            // Messages for form validation
            messages: {
                FloorNo: {
                    required: "Please enter Total Floor in Tower."
                },
                FlatTotal: {
                    required: "Total Flat in a floor required."
                },
                TowerList: {
                    required: "Select Tower Name."
                },
                FlatTypeList: {
                    required: "Select Default Flat Type"
                },
                FlatSizeList: {
                    required: "Select Default Flat Size."
                }
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();
                        $http({
                            method: 'POST',
                            url: '/Admin/CreateProperty/FlatHmtl',
                            data: { TotalFlat: $("#FlatTotal").val() }
                            //headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            $("#FlatHtml").html(data);
                            $("#loading").hide();
                        }).error(function (error) {
                            //Showing error message
                            $("#loading").hide();
                            $scope.status = 'Unable to Generate Flat: ' + error.message;
                        });
                    }
                });
            },
            // Do not change code below.
            errorPlacement: function (error, element) {
                $("#loading").hide();
                error.insertAfter(element.parent());
            }
        });
    }
    $scope.GenerateTower = function () {
        $("#loading").show();
        var TotalFlat = $("#FlatTotal").val();
        var TotalFloor = $("#FloorNo").val();
        var TowerID = $("#TowerID").val();
        var FlatType = $("#FlatTypeList").find(":selected").text();
        var FlatTypeSize = $("#FlatSizeList").find(":selected").val();
        var FlatNo = [];
        var pclids = [];
        var PreIncrement = [];
        var valbool = true;
        for (var j = 1; j <= TotalFlat; j++) {
            // add flat
            if ($("#FlatNo" + j).val() == "") {
                valbool = false;
            }
            FlatNo.push($("#FlatNo" + j).val());
            PreIncrement.push($("#PreIncrement" + j).is(":checked"));
            var div = "#panel" + j.toString();

            $(div).find('input:checkbox')
            .each(function () {
                if ($(this).is(":checked")) {
                    pclids.push($(this).val());
                }
                else {
                    pclids.push(0);
                }
            }).promise().done(function () {
                // alert(pclids)

            });
        }
        if (valbool == true) {

            $http({
                method: 'POST',
                url: '/Admin/CreateProperty/GenerateTowerAddFlat',
                data: { TotalFloor: TotalFloor, FlatNo: FlatNo, PreIncrement: PreIncrement, PLCIDs: pclids, TowerID: TowerID, FltType: FlatType, FltTSize: FlatTypeSize }
                //headers: { 'Content-Type': 'application/JSON' }
            }).success(function (data) {
                // $("#FlatHtml").html(data);
                $("#loading").hide();
                $("#MessageArea").show();
                $scope.MessageTitle = "Success";
                $scope.MessageClass = "success";
                $scope.Message = "Tower Property Generated.";
            }).error(function (error) {
                //Showing error message
                $("#MessageArea").show();
                $scope.MessageTitle = "Error";
                $scope.MessageClass = "danger";
                $scope.Message = "Tower  can not generated.";
                $scope.status = 'Unable to Generate tower property: ' + error.message;
                $("#loading").hide();
            });
        }
        else {
            $("#MessageArea").show();
            $scope.MessageTitle = "Error";
            $scope.MessageClass = "danger";
            $scope.Message = "Floors Flat No can't be blance.";
            $("#loading").hide();
        }
    }
    $scope.ViewTowerMatrix = function () {
        $("#loading").show();
        $http({
            method: 'POST',
            url: '/Admin/PropertyProject/TowerView',
            data: { towerID: $("#TowerID").find(":selected").val() }
            //headers: { 'Content-Type': 'application/JSON' }
        }).success(function (data) {
            $("#TowerHtml").html(data);
            $("#loading").hide();
        }).error(function (error) {
            //Showing error message
            $("#loading").hide();
            $scope.status = 'Unable to Generate Flat: ' + error.message;
        });
    }

    $scope.ViewFloors = function () {
        var towerid = $("#TowerID").find(":selected").val();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getAllFloorByTowerID',
            params: { towerid: towerid }
        }).success(function (data) {
            $scope.FloorList = data;
        })
    }
    $scope.ViewAllFloorsClick = function () {
        var towerid = $("#TowerID").find(":selected").val();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getAllFloorByTowerID',
            params: { towerid: towerid }
        }).success(function (data) {
            $scope.FloorList = data;
        })
    }
    $scope.ChangeFlatType = function () {
        var flatid = $("#FlatTypeList").find(":selected").val();
        $http({
            method: 'Get',
            url: '/Master/Flat/GetFlatTypeSizeByNameList',
            params: { ftype: flatid }
        }).success(function (data) {
            $scope.FlatTypeSizeList = data;
            $("#loading").hide();
        }).error(function (error) {
            $("#loading").hide();
        })
    }
    $scope.ChangeFlatTypeByID = function () {
        var flatid = $("#FlatTypeList").find(":selected").val();
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/Flat/GetFlatTypeSizeList',
            params: { falttypeid: flatid }
        }).success(function (data) {
            $scope.FlatTypeSizeList = data;
            $("#loading").hide();
        }).error(function (error) {
            $("#loading").hide();
        })
    }
    $scope.AddFloorInit = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getAllTower'
        }).success(function (data) {
            $scope.TowerList = data;
            $http({
                method: 'Get',
                url: '/Admin/CreateProperty/getAllFloorByTowerID',
                params: { towerid: data[0].TowerID }
            }).success(function (data) {
                $scope.FloorList = data;
                $("#loading").hide();
            })
        })
        $("#btnUpdate").hide();
        $("#btnSave").show();
    }
    $scope.AddFloorSave = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                FloorName: {
                    required: true
                },
                FloorNo: {
                    required: true,
                    digits: true
                },

            },

            // Messages for form validation
            messages: {
                FloorName: {
                    required: "Please enter floor name."
                },
                FloorNo: {
                    required: "Floor no can't blank.",
                    digits: "Please enter numbers only"
                },
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();
                        $http({
                            method: 'POST',
                            url: '/Admin/CreateProperty/SaveFloor',
                            data: JSON.stringify($scope.floor),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            var status = data;
                            $http({
                                method: 'Get',
                                url: '/Admin/CreateProperty/getAllFloorByTowerID',
                                params: { towerid: $scope.floor.TowerID }
                            }).success(function (data) {
                                $scope.FloorList = data;
                                if (status == 0) {
                                    $("#MessageArea").show();
                                    $scope.MessageTitle = "Error";
                                    $scope.MessageClass = "danger";
                                    $scope.Message = "Floor Record can not inserted.";
                                }
                                else if (status == "2") {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "danger";
                                    $scope.MessageTitle = "Error";
                                    $scope.Message = "Floor No already Exists.";
                                }
                                else {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "success";
                                    $scope.MessageTitle = "Success";
                                    $scope.Message = "Floor Record inserted successfully.";
                                }
                                $("#loading").hide();
                            })
                        }).error(function (error) {
                            //Showing error message
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.Message = "Floor Record can not inserted.";
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
    $scope.UpdateFloorSave = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                FloorName: {
                    required: true
                },
                FloorNo: {
                    required: true,
                    digits: true
                },

            },

            // Messages for form validation
            messages: {
                FloorName: {
                    required: "Please enter floor name."
                },
                FloorNo: {
                    required: "Floor no can't blank.",
                    digits: "only numbers are allowed."
                },
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();
                        $http({
                            method: 'POST',
                            url: '/Admin/CreateProperty/UpdateFloor',
                            data: JSON.stringify($scope.floor),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            var status = data;
                            $http({
                                method: 'Get',
                                url: '/Admin/CreateProperty/getAllFloorByTowerID',
                                params: { towerid: $scope.floor.TowerID }
                            }).success(function (data) {
                                $scope.FloorList = data;
                                if (status == 0) {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "danger";
                                    $scope.MessageTitle = "Error";
                                    $scope.Message = "Floor Record can not updated.";
                                }
                                else {
                                    $("#MessageArea").show();
                                    $scope.MessageClass = "success";
                                    $scope.MessageTitle = "Success";
                                    $scope.Message = "Floor Record updated successfully.";
                                }
                                $("#loading").hide();
                            })
                        }).error(function (error) {
                            //Showing error message
                            $("#MessageArea").show();
                            $scope.MessageClass = "danger";
                            $scope.MessageTitle = "Error";
                            $scope.Message = "Floor Record can not updated.";
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
    $scope.CancelFloorSave = function () {
        $("#btnUpdate").hide();
        $("#btnSave").show();
    }
    $scope.EditFloorClick = function (floorid) {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getFloorByID',
            params: { floorid: floorid },
        }).success(function (data) {
            $scope.floor = data;
            $("#btnUpdate").show();
            $("#btnSave").hide();
            $("#loading").hide();
        })
    }
    $scope.DeleteFloor = function () {
        $("#loading").show();
        $http({
            method: 'Post',
            url: '/Admin/CreateProperty/DeleteFloor',
            data: { floorid: $scope.floor.FloorID },
        }).success(function (data) {
            if (data == 0) {
                $("#MessageArea").show();
                $scope.MessageTitle = "Error";
                $scope.MessageClass = "danger";
                $scope.Message = "Floor Record can not deleted.";
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = "Success";
                $scope.Message = "Floor Record deleted successfully.";
            }
            $http({
                method: 'Get',
                url: '/Admin/CreateProperty/getAllFloorByTowerID',
                params: { towerid: $scope.floor.TowerID }
            }).success(function (data) {
                $scope.FloorList = data;
            })
            $("#loading").hide();
        })
    }

    // ======== Flat Service=====================
    $scope.EditFlatInit = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getAllTower'
        }).success(function (data) {
            $scope.TowerList = data;
            $http({
                method: 'Get',
                url: '/Master/AChargesCtrl/AditionalChargeList'
            }).success(function (data) {
                $scope.AdditionalChargeList = data;
                $http({
                    method: 'Get',
                    url: '/Master/AOCharge/AddOnChargeList'
                }).success(function (data) {
                    $scope.AddOnChargeList = data;

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
                        })
                    })
                })
            })

            if ($("#hidFlatID").val() == "0") {
                $("#headdiv").show();
                $("#contentdiv").hide();
            }
            else {
                // flat Id already found.
                var flatid = $("#hidFlatID").val();
                $http({
                    method: 'Get',
                    url: '/Admin/CreateProperty/GetFlatsByFloorID',
                    params: { floorid: $("#hidFloorID").val()}
                }).success(function (data) {
                    $scope.FlatList = data;

                    // get flat details
                    $http({
                        method: 'Get',
                        url: '/Admin/CreateProperty/getFlatByID',
                        params: { flatid: flatid }
                    }).success(function (data) {
                        $scope.flat = data;
                        $("#flatplchtml").html(data);
                        $http({
                            method: 'Get',
                            url: '/Admin/CreateProperty/GetFlatChageList',
                            params: { flatid: flatid }
                        }).success(function (data) {
                            $scope.FlatChargeList = data;
                            $http({
                                method: 'Get',
                                url: '/Admin/CreateProperty/GetFlatOChargeList',
                                params: { flatid: flatid }
                            }).success(function (data) {
                                $scope.FlatOChargeList = data;
                            })
                            $("#addtionalchargediv").show();
                            $("#btnUpdate").show();
                            $("#btnSave").hide();
                            FlatplcHtml(flatid)

                            $("#loading").hide();
                        })
                    })
                })
            }
            plcHtml();
        })
        $("#addtionalchargediv").hide();
        $("#btnUpdate").hide();
        $("#btnSave").show();
    }
    $scope.CancelFlat = function () {
        $("#addtionalchargediv").hide();
        $("#btnUpdate").hide();
        $("#btnSave").show();
    }
    $scope.towerChange = function () {
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getAllFloorByTowerID',
            params: { towerid: $scope.flat.TowerID }
        }).success(function (data) {
            $scope.FloorList = data;
            $("#loading").hide();
        })
    }
    $scope.ViewAllFlatClick = function () {
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetFlatsByFloorID',
            params: { floorid: $scope.flat.FloorID }
        }).success(function (data) {
            $scope.FlatList = data;

            $("#headdiv").show();
            $("#contentdiv").show();
            $("#loading").hide();

        })
    }

    function plcHtml() {
        $("#flatplchtml").html("");
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetPCLHtml',
            params: {}
        }).success(function (data) {
            $("#flatplchtml").html(data);
        })
    }
    function FlatplcHtml(flatid) {
        $("#flatplchtml").html("");
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetFlatPCLHtml',
            params: { flatid: flatid }
        }).success(function (data) {
            $("#flatplchtml").html(data);
        })
    }
    $scope.EditFlat = function (flatid) {
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getFlatByID',
            params: { flatid: flatid }
        }).success(function (data) {
            $scope.flat = data;
            $("#flatplchtml").html(data);
            $http({
                method: 'Get',
                url: '/Admin/CreateProperty/GetFlatChageList',
                params: { flatid: flatid }
            }).success(function (data) {
                $scope.FlatChargeList = data;
                $http({
                    method: 'Get',
                    url: '/Admin/CreateProperty/GetFlatOChargeList',
                    params: { flatid: flatid }
                }).success(function (data) {
                    $scope.FlatOChargeList = data;
                })
                $("#addtionalchargediv").show();
                $("#btnUpdate").show();
                $("#btnSave").hide();
                FlatplcHtml(flatid)
            })
        })
    }
    $scope.SelectFlat = function (flatid) {
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getFlatByID',
            params: { flatid: flatid }
        }).success(function (data) {
            $scope.flat = data;
            $("#flatplchtml").html(data);
            $http({
                method: 'Get',
                url: '/Admin/CreateProperty/GetFlatChageList',
                params: { flatid: flatid }
            }).success(function (data) {
                $scope.FlatChargeList = data;

                FlatplcHtml(flatid)
            })
        })
    }

    $scope.AddFlatSave = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                FlatNo: {
                    required: true
                },
                FlatSize: {
                    required: true
                },
                SalePrice: {
                    required: true
                },
            },

            // Messages for form validation
            messages: {
                FlatNo: {
                    required: "Please enter Flat Number."
                },
                FlatSize: {
                    required: "Select Flat Size"
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

                        var pclids = [];
                        $("#flatplchtml").find('input:checkbox').each(function () {
                            if ($(this).is(":checked")) {
                                pclids.push($(this).val());
                            }
                            else {
                                pclids.push(0);
                            }
                        }).promise().done(function () {
                            $scope.flat.FlatPLCs = pclids;
                            $scope.flat.FlatType = $("#FlatTypeList").find(":selected").text();
                            $http({
                                method: 'POST',
                                url: '/Admin/CreateProperty/SaveFat',
                                data: JSON.stringify($scope.flat),
                                headers: { 'Content-Type': 'application/JSON' }
                            }).success(function (data) {
                                var status = data;
                                $http({
                                    method: 'Get',
                                    url: '/Admin/CreateProperty/GetFlatsByFloorID',
                                    params: { floorid: $scope.flat.FloorID }
                                }).success(function (data) {
                                    $scope.FlatList = data;
                                    $("#loading").hide();
                                    if (status == 0) {
                                        $("#MessageArea").show();
                                        $scope.MessageClass = "danger";
                                        $scope.MessageTitle = "Error";
                                        $scope.Message = "Flat Record can not inserted.";
                                    }
                                    else {
                                        $("#MessageArea").show();
                                        $scope.MessageClass = "success";
                                        $scope.MessageTitle = "Success";
                                        $scope.Message = "Flat Record inserted successfully.";
                                    }
                                })
                            }).error(function (error) {
                                //Showing error message
                                $("#MessageArea").show();
                                $scope.MessageClass = "danger";
                                $scope.MessageTitle = "Error";
                                $scope.Message = "Flat Record can not inserted.";
                            });
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
    $scope.UpdateFlatSave = function () {
        
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                FlatNo: {
                    required: true
                },
                FlatSize: {
                    required: true
                },
                SalePrice: {
                    required: true
                },
            },

            // Messages for form validation
            messages: {
                FlatNo: {
                    required: "Please enter floor name."
                },
                FlatSize: {
                    required: "Floor no can't blank."
                },
                SalePrice: {
                    required: "Floor no can't blank."
                },
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();
                        var pclids = [];
                        $("#flatplchtml").find('input:checkbox').each(function () {
                            if ($(this).is(":checked")) {
                                pclids.push($(this).val());
                            }
                            else {
                                pclids.push(0);
                            }
                        }).promise().done(function () {
                            $scope.flat.FlatPLCs = pclids;
                            //  $scope.flat.FlatType = $("#FlatTypeList").find(":selected").text();
                            $http({
                                method: 'POST',
                                url: '/Admin/CreateProperty/UpdateFlat',
                                data: JSON.stringify($scope.flat),
                                headers: { 'Content-Type': 'application/JSON' }
                            }).success(function (data) {
                                var status = data;
                                $http({
                                    method: 'Get',
                                    url: '/Admin/CreateProperty/GetFlatsByFloorID',
                                    params: { floorid: $scope.flat.FloorID }
                                }).success(function (data) {
                                    $scope.FlatList = data;
                                    $("#loading").hide();
                                    if (status == "0") {
                                        $("#MessageArea").show();
                                        $scope.MessageClass = "danger";
                                        $scope.MessageTitle = "Error";
                                        $scope.Message = "Flat Record can not updated.";
                                    }
                                    else {
                                        $("#MessageArea").show();
                                        $scope.MessageClass = "success";
                                        $scope.MessageTitle = "Success";
                                        $scope.Message = "Flat Record updated successfully.";
                                    }
                                })
                            }).error(function (error) {
                                //Showing error message
                                $("#MessageArea").show();
                                $scope.MessageClass = "danger";
                                $scope.MessageTitle = "Error";
                                $scope.Message = "Flat Record can not updated.";
                            });
                        })
                    }
                });
            },

            // Do not change code below.
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Flat Record can not updated.";
            }
        });
    }

    $scope.DeleteFlat = function () {
        $("#loading").show();
        $http({
            method: 'Post',
            url: '/Admin/CreateProperty/DeleteFlat',
            data: { flatid: $scope.flat.FlatID },
        }).success(function (data) {
            $http({
                method: 'Get',
                url: '/Admin/CreateProperty/GetFlatsByFloorID',
                params: { floorid: $scope.flat.FloorID }
            }).success(function (data) {
                $scope.FlatList = data;
                $("#loading").hide();
            })
            if (data == "0") {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Flat Record can not deleted.";
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = "Success";
                $scope.Message = "Flat Record deleted successfully.";
            }
            $("#myModal").model("hide");
        })
    }
    // ===================AdditionalCharge=================================
    $scope.AddAdditionalCharge = function () {
        $("#loading").show();
        $http({
            method: 'Post',
            url: "/Admin/CreateProperty/SaveFlatChage",
            data: JSON.stringify($scope.flat),
        }).success(function (data) {

            if (data == "0") {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Additional Charge can't Added.";
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = "Success";
                $scope.Message = "Additional Charge Added.";
            }
           
            $http({
                method: 'Get',
                url: '/Admin/CreateProperty/GetFlatChageList',
                params: { flatid: $scope.flat.FlatID }
            }).success(function (data) {
                $scope.FlatChargeList = data;
            })
            $("#loading").hide();
        })
    }
    $scope.DeleteFlatCharge = function (FlatChargeID) {
        $("#loading").show();
        $http({
            method: 'Post',
            url: '/Admin/CreateProperty/DeleteFlatCharge',
            data: { FlatChargeID: FlatChargeID },
        }).success(function (data) {

            if (data == "0") {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Additional Charge can't Deleted.";
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = "Success";
                $scope.Message = "Additional Charge Deleted.";
            }
            $http({
                method: 'Get',
                url: '/Admin/CreateProperty/GetFlatChageList',
                params: { flatid: $scope.flat.FlatID }
            }).success(function (data) {
                $scope.FlatChargeList = data;
            })
            $("#loading").hide();
        })
    }

    //=================== Flat Add On Charge===============================
    $scope.AddAddOnCharge = function () {
        $("#loading").show();
        $http({
            method: 'Post',
            url: "/Admin/CreateProperty/SaveFlatOChage",
            data: JSON.stringify($scope.flat),
        }).success(function (data) {

            if (data == "0") {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Optional Charge can't Added.";
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = "Success";
                $scope.Message = "Optional Charge Added.";
            }

            $http({
                method: 'Get',
                url: '/Admin/CreateProperty/GetFlatOChargeList',
                params: { flatid: $scope.flat.FlatID }
            }).success(function (data) {
                $scope.FlatOChargeList = data;
            })
            $("#loading").hide();
        })
    }
    $scope.DeleteFlatOCharge = function (FlatOChargeID) {
        $("#loading").show();
        $http({
            method: 'Post',
            url: '/Admin/CreateProperty/DeleteFlatOCharge',
            data: { FlatOChargeID: FlatOChargeID },
        }).success(function (data) {

            if (data == "0") {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Optional Charge can't Deleted.";
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = "Success";
                $scope.Message = "Optional Charge Deleted.";
            }
            $http({
                method: 'Get',
                url: '/Admin/CreateProperty/GetFlatOChargeList',
                params: { flatid: $scope.flat.FlatID }
            }).success(function (data) {
                $scope.FlatOChargeList = data;
            })
            $("#loading").hide();
        })
    }
    //==================Update Flat Type===================================
    $scope.UpdateAllFlatType = function () {
        $("#loading").show();
        $scope.flat.FlatType = $("#FlatTypeList").find(":selected").text();
        $http({
            method: 'POST',
            url: '/Admin/CreateProperty/UpdateAllFlatType',
            data: JSON.stringify($scope.flat),
            headers: { 'Content-Type': 'application/JSON' }
        }).success(function (data) {
            var status = data;
            if (status == "0") {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Flat Record can not updated.";
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = "Success";
                $scope.Message = "Flat Record updated successfully.";
            }
            $("#loading").hide();
        }).error(function (error) {

            $("#loading").hide();
        })
    }
    $('#loading').hide();
})