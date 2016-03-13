
var myApp = angular.module('SaleApp', []);
//Defining a Controller 
myApp.controller('PropertyController', function ($scope, $http, $filter) {

    // Get all Property list for dropdownlist
    $scope.Error = "";
    var orderBy = $filter('orderBy');
    $("#loading").show();

    // CalculatePrice
    $scope.PropertyPricingInit = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/PlanCtrl/GetPlanTypeMasterList',
        }).success(function (data) {
            $scope.PlanTypeMasterList = data;
        })

        var flatid = $("#hidFlatID").val();
        $http({
            method: 'Get',
            url: '/Sale/Property/GetFlatDetails',
            params: { flatid: flatid },
        }).success(function (data) {
            $scope.FlatDetails = data;
            var totalPLC = 0;
            var GtotalPLC = 0;
            var FlatSize = 0;
            var TotalFloorPlc = 0;
            var GTotalFloorPlc = 0;
            var GAdditionalCharge = 0;
            var GAFlatOCharge = 0;
            var GTFlatOCharge = 0;
            $scope.FlatSize = $scope.FlatDetails.FlatSize;
            for (var i = 0; i < $scope.FlatDetails.FlatPLCList.length; i++) {
                var list = $scope.FlatDetails.FlatPLCList[i];
                totalPLC += list.AmountSqFt;
                GtotalPLC += list.TotalAmount;
            }
            $scope.TotalPLCAmount = totalPLC;
            $scope.GTotalPLCAmount = GtotalPLC;
            var totalCharge = 0;
            var GtotalCharge = 0;
            for (var i = 0; i < $scope.FlatDetails.FlatChargeList.length; i++) {
                var list = $scope.FlatDetails.FlatChargeList[i];
                totalCharge += list.Amount;
                GtotalCharge += list.TotalAmount;
            }
            debugger;

            for (var i = 0; i < $scope.FlatDetails.FlatOChargeList.length; i++) {
                var list = $scope.FlatDetails.FlatOChargeList[i];
                GAFlatOCharge = parseFloat(GAFlatOCharge) + list.Amount;
                GTFlatOCharge = parseFloat(GTFlatOCharge) + list.TotalAmount;
            }

            $scope.TotalFloorPlc = TotalFloorPlc;
            $scope.GTotalFloorPlc = GTotalFloorPlc;
            $scope.GAdditionalCharge = GAdditionalCharge;
            $("#loading").hide();
            $scope.TotalChargeAmount = totalCharge;
            $scope.GTotalChargeAmount = GtotalCharge;
            $scope.FlatOChargeTotal = GAFlatOCharge;
            $scope.GTFlatOCharge = GTFlatOCharge;
            $scope.GTotal = $scope.GTotalChargeAmount + $scope.GTotalPLCAmount + $scope.GTFlatOCharge + $scope.FlatDetails.SalePrice;
        })
    }
    //$scope.PropertyPricingInit = function () {
    //    $("#loading").show();
    //    $http({
    //        method: 'Get',
    //        url: '/Master/PlanCtrl/GetPlanTypeMasterList',
    //    }).success(function (data) {
    //        $scope.PlanTypeMasterList = data;
    //    })

    //    var flatid = $("#hidFlatID").val();
    //    $http({
    //        method: 'Get',
    //        url: '/Sale/Property/GetFlatDetails',
    //        params: { flatid: flatid },
    //    }).success(function (data) {
    //        $scope.FlatDetails = data;
    //        var totalPLC = 0;
    //        var GtotalPLC = 0;
    //        var FlatSize = 0;
    //        var TotalFloorPlc = 0;
    //        var GTotalFloorPlc = 0;
    //        var GAdditionalCharge = 0;
    //        var GAFlatOCharge = 0;
    //        var GTFlatOCharge = 0;
    //        $scope.FlatSize = $scope.FlatDetails.FlatSize;
    //        for (var i = 0; i < $scope.FlatDetails.FlatPLCList.length; i++) {
    //            var list = $scope.FlatDetails.FlatPLCList[i];
    //            totalPLC += list.AmountSqFt;
    //            GtotalPLC += list.TotalAmount;
    //        }
    //        $scope.TotalPLCAmount = totalPLC;
    //        $scope.GTotalPLCAmount = GtotalPLC;
    //        var totalCharge = 0;
    //        var GtotalCharge = 0;
    //        for (var i = 0; i < $scope.FlatDetails.FlatChargeList.length; i++) {
    //            var list = $scope.FlatDetails.FlatChargeList[i];
    //            totalCharge += list.Amount;
    //            GtotalCharge += list.TotalAmount;
    //        }
    //        debugger;

    //        for (var i = 0; i < $scope.FlatDetails.FlatOChargeList.length; i++) {
    //            var list = $scope.FlatDetails.FlatOChargeList[i];
    //            GAFlatOCharge = parseFloat(GAFlatOCharge) + list.Amount;
    //            GTFlatOCharge = parseFloat(GTFlatOCharge) + list.TotalAmount;
    //        }

    //        $scope.TotalFloorPlc = TotalFloorPlc;
    //        $scope.GTotalFloorPlc = GTotalFloorPlc;
    //        $scope.GAdditionalCharge = GAdditionalCharge;
    //        if ($scope.FlatDetails.FlatPlanCharge.length > 0)
    //            document.getElementById("planaf").innerHTML = parseFloat(parseFloat($scope.FlatDetails.FlatPlanCharge[0].AmountSqFt) * parseFloat($scope.FlatDetails.FlatPlanCharge[0].Size)).toFixed(0);
    //        if ($scope.FlatDetails.FlatPlanCharge.length > 1)
    //            document.getElementById("planauf").innerHTML = parseFloat(parseFloat($scope.FlatDetails.FlatPlanCharge[1].AmountSqFt) * parseFloat($scope.FlatDetails.FlatPlanCharge[1].Size)).toFixed(0);
    //        if ($scope.FlatDetails.FlatPlanCharge.length > 2)
    //            document.getElementById("planbf").innerHTML = parseFloat(parseFloat($scope.FlatDetails.FlatPlanCharge[2].AmountSqFt) * parseFloat($scope.FlatDetails.FlatPlanCharge[2].Size)).toFixed(0);
    //        if ($scope.FlatDetails.FlatPlanCharge.length > 3)
    //            document.getElementById("planbuf").innerHTML = parseFloat(parseFloat($scope.FlatDetails.FlatPlanCharge[3].AmountSqFt) * parseFloat($scope.FlatDetails.FlatPlanCharge[3].Size)).toFixed(0);
    //        if ($scope.FlatDetails.FlatPlanCharge.length > 4)
    //            document.getElementById("plancf").innerHTML = parseFloat(parseFloat($scope.FlatDetails.FlatPlanCharge[4].AmountSqFt) * parseFloat($scope.FlatDetails.FlatPlanCharge[4].Size)).toFixed(0);
    //        if ($scope.FlatDetails.FlatPlanCharge.length > 5)
    //            document.getElementById("plancuf").innerHTML = parseFloat(parseFloat($scope.FlatDetails.FlatPlanCharge[5].AmountSqFt) * parseFloat($scope.FlatDetails.FlatPlanCharge[5].Size)).toFixed(0);
    //        $("#loading").hide();
    //        $scope.TotalChargeAmount = totalCharge;
    //        $scope.GTotalChargeAmount = GtotalCharge;
    //        $scope.FlatOChargeTotal = GAFlatOCharge;
    //        $scope.GTFlatOCharge = GTFlatOCharge;
    //        $scope.GTotal = $scope.GTotalChargeAmount + $scope.GTotalPLCAmount + $scope.GTFlatOCharge + $scope.FlatDetails.SalePrice;
    //    })
    //}
    $scope.ShowPlanPrice = function (FType, Size) {
        $("#loading").show();
        var PlanName = $("#PlanID").find(":selected").text();
        $http({
            method: 'Get',
            url: '/Sale/Property/GetPlanTypeMasterByParams',
            params: { PlanName: PlanName, FType: FType, Size: Size }
        }).success(function (data) {
            if (data != "null") {
                $scope.GTotal = $scope.GTotalChargeAmount + $scope.GTotalPLCAmount + data.AmountSqFt * Size;
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Installment Plan details not found.";
            }

            $("#loading").hide();
        }).error(function (error) {
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Installment Plan details not found.";
        })
    }

    $scope.SearchsaleProperty = function () {
        $scope.Error = "";
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        $scope.NewSaleInit();
    }
    //NewSale
    $scope.NewSaleInit = function () {
        var flatid = $("#hidFlatID").val();
        if (flatid == "0") {
            $("#Newsaletower").show();
            $("#Salediv").hide();
            GetLoadTowerFlat();
            $http({
                method: 'Get',
                url: '/Master/PlanCtrl/GetPlanTypeMasterDistList',
            }).success(function (data) {
                $scope.PlanTypeMasterList = data;
            })
        }
        else {
            $("#Salediv").show();
            $("#loading").show();
            $http({
                method: 'Get',
                url: '/Master/PlanCtrl/GetPlanTypeMasterDistList',
            }).success(function (data) {
                $scope.PlanTypeMasterList = data;
            })

            $http({
                method: 'Get',
                url: '/Sale/Property/GetFlatDetails',
                params: { flatid: flatid },
            }).success(function (data) {
                $scope.FlatDetails = data;
                var salebl = $scope.FlatDetails.SaleFlatModel.length;
                $scope.plan = [];
                $scope.SaleStatus = "Available";
                if (salebl > 0) {
                    $scope.SaleStatus = $scope.FlatDetails.SaleFlatModel[0].Status;
                    $scope.isSaled = true;
                     //alert($scope.FlatDetails.SaleFlatModel[0].PlanName);
                    $scope.plan.PlanName = $scope.FlatDetails.SaleFlatModel[0].PlanName;
                    $("#txtSaleDate").val($scope.FlatDetails.SaleFlatModel[0].SaleDateSt);
                    $scope.ShowPlanPriceNewSale($scope.FlatDetails.FlatType, $scope.FlatDetails.FlatSize);
                    if ($scope.SaleStatus == "Regenerate") {
                        $("#MessageArea").show();
                        $scope.MessageClass = "danger";
                        $scope.MessageTitle = "Error";
                        $scope.Message = "Installments are updated, Please regenerate Installment.";
                    }
                }
                else {
                    $scope.isSaled = false;
                }

                var totalPLC = 0;
                var GtotalPLC = 0;
                for (var i = 0; i < $scope.FlatDetails.FlatPLCList.length; i++) {
                    var list = $scope.FlatDetails.FlatPLCList[i];
                    totalPLC += list.AmountSqFt;
                    GtotalPLC += list.TotalAmount;
                }
                $scope.TotalPLCAmount = totalPLC;
                $scope.GTotalPLCAmount = GtotalPLC;
                var totalCharge = 0;
                var GtotalCharge = 0;
                for (var i = 0; i < $scope.FlatDetails.FlatChargeList.length; i++) {
                    var list = $scope.FlatDetails.FlatChargeList[i];
                    totalCharge += list.Amount;
                    GtotalCharge += list.TotalAmount;
                }
                $scope.TotalChargeAmount = totalCharge;
                $scope.GTotalChargeAmount = GtotalCharge;

                var GAFlatOCharge = 0;
                var GTFlatOCharge = 0;
                for (var i = 0; i < $scope.FlatDetails.FlatOChargeList.length; i++) {
                    var list = $scope.FlatDetails.FlatOChargeList[i];
                    GAFlatOCharge = parseFloat(GAFlatOCharge) + list.Amount;
                    GTFlatOCharge = parseFloat(GTFlatOCharge) + list.TotalAmount;
                }

                $scope.FlatOChargeTotal = GAFlatOCharge;
                $scope.GTFlatOCharge = GTFlatOCharge;
                $scope.InstallmentTotal = $scope.FlatDetails.SalePrice * $scope.FlatDetails.FlatSize;
                $scope.GTotal = $scope.GTotalChargeAmount + $scope.GTotalPLCAmount + $scope.GTFlatOCharge + $scope.FlatDetails.SalePrice * $scope.FlatDetails.FlatSize;
                $http({
                    method: 'Get',
                    url: '/Sale/Property/GetFlatInstallmentWithCharges',
                    params: { flatid: flatid, flatsize: $scope.FlatDetails.FlatSize, version: 0 }
                }).success(function (data) {
                    $scope.FlatInstallmentList = data;
                    if (data == "") {
                        $("#divShowInstallment").hide();
                        $("#divAddInstallments").show();
                        $("#btnDeleteInstallment").hide();
                        $("#btnAddRow").show();
                    }
                    else {
                        $("#divShowInstallment").show();
                        $("#divAddInstallments").hide();
                        $("#btnDeleteInstallment").show();
                        $("#btnAddRow").hide();
                    }
                })
                $('input:radio[name=SaleType]').val("Sale");
                $("#loading").hide();
            })
        }
    }
    function GetLoadTowerFlat() {
        // Get All Tower
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getAllTower'
        }).success(function (data) {
            $scope.TowerList = data;

            $http({
                method: 'Get',
                url: '/Admin/CreateProperty/GetFlatByTowerID',
                params: { towerid: data[1].TowerID }
            }).success(function (data) {
                $scope.FlatList = data;
            })
        })

    }

    $scope.TowerChange = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetFlatByTowerID',
            params: { towerid: $scope.Flat.TowerID }
        }).success(function (data) {
            $scope.FlatList = data;
            $("#loading").hide();
        })
    }
    $scope.ShowPlanPriceNewSale = function (FType, Size) {

        $("#loading").show();
        var PlanName = $("#PlanID").find(":selected").val();
        if (PlanName == '? undefined:undefined ?') {
            PlanName = $scope.plan.PlanName;
        }
        $http({
            method: 'Get',
            url: '/Sale/Property/GetPlanTypeMasterByParams',
            params: { PlanName: PlanName, FType: FType, Size: Size }
        }).success(function (data) {
            if (data != "null") {

                $scope.PlanSaleRate = data.AmountSqFt;
                $scope.InstallmentTotal = data.AmountSqFt * Size;

                $scope.GTotal = $scope.GTotalChargeAmount + $scope.GTotalPLCAmount + $scope.GTFlatOCharge + data.AmountSqFt * Size;
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Installment Plan details not found.";
            }

            $("#loading").hide();
        }).error(function (error) {
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Installment Plan details not found.";
        })
    }
    $scope.ChageSaleRate = function () {
        $scope.InstallmentTotal = parseFloat($("#PlanSalePrice").val()) * $scope.FlatDetails.FlatSize;
        $scope.GTotal = $scope.GTotalChargeAmount + $scope.GTotalPLCAmount + $scope.GTFlatOCharge + parseFloat($("#PlanSalePrice").val()) * $scope.FlatDetails.FlatSize;
    }

    $scope.GenerateInstallment = function () {

        var plc = $("#chkPLC").is(":checked");
        var acharge = $("#chkACharges").is(":checked");
        var aocharge = $("#chkAOCharges").is(":checked");

        var plcvalue = $("#txtPLCAmount").val();
        var achargevalue = $("#txtAChargeAmount").val();
        var aochargevalue = $("#txtAOChargeAmount").val();

        if ($scope.FlatInstallmentList != "") {
            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Please delete generated installment to regenerate installment.";
            return;
        }

        if ($("#txtSaleDate").val() == "") {
            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Please select Flat Sale/Booking Date.";
        }
        else {
            $http({
                method: 'Get',
                url: '/Sale/Property/GetInstallmentPlan',
                params: { PlanName: $scope.plan.PlanName, FType: $scope.FlatDetails.FlatType, Size: $scope.FlatDetails.FlatSize, plc: plc, acharge: acharge, aocharge: aocharge, bookdate: $("#txtSaleDate").val() }
            }).success(function (data) {
                $("#planFoot").html(data[2]);
                $("#planHead").html(data[1]);
                $("#planbody").html(data[0]);
                BSPChange();
                PLCChange();
                AChargeChange();
                AOChargeChange();

                $("#divShowInstallment").hide();
                $("#divAddInstallments").show();
                $("#btnDeleteInstallment").hide();
                $("#btnAddRow").show();
            })
        }
    }

    $scope.AddRowClick = function () {
        $("#loading").show();
        var plc = $("#chkPLC").is(":checked");
        var acharge = $("#chkACharges").is(":checked");
        var aocharge = $("#chkAOCharges").is(":checked");

        $http({
            method: 'Get',
            url: '/Sale/Property/AddInstallmentPlanRow',
            params: { PlanName: $scope.plan.PlanName, FType: $scope.FlatDetails.FlatType, Size: $scope.FlatDetails.FlatSize, plc: plc, acharge: acharge, aocharge: aocharge }
        }).success(function (data) {
            $("#planbody").append(data);
            $("#loading").hide();
            BSPChange();
            PLCChange();
            AChargeChange();
            AOChargeChange();
        }).error(function (error) {
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Installment plan row can't added, please validate information";
        })
    }

    $scope.SaveInstallment = function () {
        var bsp = "";
        var plc = "";
        var acharge = "";
        var ocharge = "";
        var installment = "";
        var dueDate = "";
        $("#loading").show();
        $("input[name='txtBSP']").each(function (index, value) {
            bsp = bsp + ":" + parseFloat($(this).val());
        }).promise().done(function () {

            $("input[name='txtPLC']").each(function (index, value) {
                plc = plc + ":" + $(this).val();
            }).promise().done(function () {
                $("input[name='txtACharge']").each(function (index, value) {
                    acharge = acharge + ":" + $(this).val();
                }).promise().done(function () {
                    $("input[name='txtACharge']").each(function (index, value) {
                        acharge = acharge + ":" + $(this).val();
                    }).promise().done(function () {
                        $("input[name='txtAOCharge']").each(function (index, value) {
                            ocharge = ocharge + ":" + $(this).val();
                        }).promise().done(function () {
                            $("select[name='ddlInstallment']").each(function (index, value) {
                                installment = installment + ":" + $(this).find(":selected").val();
                            }).promise().done(function () {
                                $("input[name='txtDueDate']").each(function (index, value) {
                                    dueDate = dueDate + ":" + $(this).val()
                                }).promise().done(function () {

                                    var flatid = $("#hidFlatID").val();
                                    var sale = new Object();
                                    sale.FlatsID = flatid;
                                    sale.Installment = installment;
                                    sale.bsp = bsp;
                                    sale.plc = plc;
                                    sale.acharges = acharge;
                                    sale.ocharges = ocharge;
                                    sale.dueDate = dueDate;
                                    sale.InstallmentTotal = $("#InstallmentTotal").html();
                                    sale.PLCTotal = $("#txtPLCAmount").val();
                                    sale.AChargeTotal = $("#txtAChargeAmount").val();
                                    sale.OChargeTotal = $("#txtAOChargeAmount").val();

                                    $http({
                                        method: 'Post',
                                        contentType: "application/json; charset=utf-8",
                                        url: '/Sale/Property/SaveInstallment2',
                                        data: "{'sale':" + JSON.stringify(sale) + "}",
                                        dataType: "json",
                                        // data: { sale: $scope.FSale }
                                        // data: JSON.stringify({ FlatsID: flatid, Installment: installment, bsp: bsp, plc: plc, acharges: acharge, ocharges: ocharge, dueDate: dueDate, InstallmentTotal: $("#InstallmentTotal").html(), PLCTotal: $("#txtPLCAmount").val(), AChargeTotal: $("#txtAChargeAmount").val(), OChargeTotal: $("#txtAOChargeAmount").val() }),
                                    }).success(function (data) {
                                        if (data == "True") {
                                            $("#MessageArea").show();
                                            $("#MessageArea").removeClass("alert-danger");
                                            $("#MessageArea").addClass("alert-success");
                                            $("#MessageTitle").html('').html("Success");
                                            $("#Message").html('').html("Flat Installment Saved");
                                            $("#divShowInstallment").show();
                                            $("#divAddInstallments").hide();
                                            $("#btnDeleteInstallment").show();
                                            $("#btnAddRow").hide();
                                            var flatid = $("#hidFlatID").val();
                                            $("#loading").show();
                                            $http({
                                                method: 'Get',
                                                url: '/Sale/Property/GetFlatInstallmentWithCharges',
                                                params: { flatid: flatid, flatsize: $scope.FlatDetails.FlatSize, version: 0 }
                                            }).success(function (data) {
                                                $scope.FlatInstallmentList = data;

                                                $("#divShowInstallment").show();
                                                $("#divAddInstallments").hide();
                                                $("#btnDeleteInstallment").show();
                                                $("#btnAddRow").hide();
                                                $("#loading").hide();
                                            })
                                            $("#loading").hide();
                                        }
                                        else {
                                            $("#loading").hide();
                                            $("#MessageArea").show();
                                            $("#MessageArea").removeClass("alert-success");
                                            $("#MessageArea").addClass("alert-danger");
                                            $("#MessageTitle").html('').html("Error");
                                            $("#Message").html('').html("Flat Installment Can't Saved");
                                        }
                                    }).error(function (error) {
                                        $("#loading").hide();
                                        alert(error)
                                    });

                                })
                            })
                        })
                    })
                })
            })
        });

    }

    $scope.DeleteInstallmentsClick = function () {

        $("#loading").show();
        var flatid = $("#hidFlatID").val();

        $http({
            method: 'Get',
            url: '/Sale/Property/DeleteInstallment',
            params: { flatid: flatid }
        }).success(function (data) {
            //  $scope.FlatInstallmentList = data;
            if (data == "0") {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Installment Can't Deleted.";
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = "Success";
                $scope.Message = "Installment Deleted.";
                $scope.FlatInstallmentList = "";
            }
            $("#divShowInstallment").hide();
            $("#divAddInstallments").show();
            $("#btnDeleteInstallment").hide();
            $("#btnAddRow").show();
        })
        $("#loading").hide();
    }
    $scope.ShowInstallmentsClick = function () {
        var flatid = $("#hidFlatID").val();
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Sale/Property/GetFlatInstallmentWithCharges',
            params: { flatid: flatid, flatsize: $scope.FlatDetails.FlatSize, version: 0 }
        }).success(function (data) {
            $scope.FlatInstallmentList = data;

            $("#divShowInstallment").show();
            $("#divAddInstallments").hide();
            $("#btnDeleteInstallment").show();
            $("#btnAddRow").hide();
            $("#loading").hide();
        })
        $("#loading").hide();
    }
    $scope.SaveSale = function () {
        $("#loading").show();
        var flatid = $("#hidFlatID").val();
        if ($scope.flat.SaleType == null || $("#txtSaleDate").val() == null || $("#txtSaleDate").val() == "") {
            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Please provide date or sale type";
        }
        else {
            $http({
                method: 'Post',
                url: '/Sale/Property/SaleFlatSave',
                data: { flatid: flatid, salerate: $scope.GTotal, saleDate: $("#txtSaleDate").val(), saletype: $scope.flat.SaleType, planName: $scope.plan.PlanName }
            }).success(function (data) {
                if (data == "1") {
                    // Update Flat status and insert into customer default value with flat and saleid.
                    $http({
                        method: 'Post',
                        url: '/Admin/CreateProperty/UpdateFlatStatus',
                        data: { flatid: flatid, status: $scope.flat.SaleType }
                    }).success(function (data) {
                        if (data == "1") {
                            $("#btnSaveSale").hide();
                        }
                    })
                    $("#loading").hide();

                    $("#MessageArea").show();
                    $scope.MessageClass = "success";
                    $scope.MessageTitle = "Success";
                    $scope.Message = "Flat Successfully " + $scope.flat.SaleType;
                }
                else {
                    $("#loading").hide();

                    $("#MessageArea").show();
                    $scope.MessageClass = "danger";
                    $scope.MessageTitle = "Error";
                    $scope.Message = "Flat Not " + $scope.flat.SaleType + " , Please try again.";
                }
                $('#myModal').modal('hide');
            })
        }
    }

    $scope.ViewInstallmentInit = function () {
        GetLoadTowerFlat();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getFlatByID',
            params: { flatid: $("#hidFlatID").val() }
        }).success(function (data) {
            $scope.FlatDetails = data;
        });
        $("#loading").hide();
    }

    $scope.SearchFlatClick = function () {
        $("#hidFlatID").val($scope.Flat.FlatID);
        // Get Installment Version list.
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Sale/Property/GetInstallmentVersion',
            params: { flatid: $("#hidFlatID").val() }
        }).success(function (data) {
            $scope.InstallmentVersion = data;
            $("#loading").hide();
        }).error(function (error) {
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Installment version not found.";
        });
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getFlatByID',
            params: { flatid: $("#hidFlatID").val() }
        }).success(function (data) {
            $scope.FlatDetails = data;
        });
    }
    $scope.ViewInstallmentClick = function (insVer) {
        $http({
            method: 'Get',
            url: '/Sale/Property/GetFlatInstallmentWithCharges',
            params: { flatid: $("#hidFlatID").val(), flatsize: $scope.FlatDetails.FlatSize, version: insVer }
        }).success(function (data) {
            $scope.FlatInstallmentList = data;
            
            $("#loading").hide();
        }).error(function (error) {
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Installments not found.";
        });
    }

    $scope.DirectEditPLC = function (plcid) {
        $scope.EditType = "Flat PLC";
        $scope.EditTypeID = plcid;
    }
    $scope.DirectEditFlatCharge = function (flatChargeId) {
        $scope.EditType = "Flat Charge";
        $scope.EditTypeID = flatChargeId;
    }
    $scope.DirectEditFlatOCharge = function (flatOChargeId) {
        $scope.EditType = "Flat Other Charge";
        $scope.EditTypeID = flatOChargeId;
    }
     $scope.DirectDeletePLC = function (plcid) {
        $scope.EditType = "Flat PLC";
        $scope.EditTypeID = plcid;
    }
    $scope.DirectDeleteFlatCharge = function (flatChargeId) {
        $scope.EditType = "Flat Charge";
        $scope.EditTypeID = flatChargeId;
    }
    $scope.DirectDeleteFlatOCharge = function (flatOChargeId) {
        $scope.EditType = "Flat Other Charge";
        $scope.EditTypeID = flatOChargeId;
    }
    $scope.SaveNewEditAmount = function () {
        var $checkoutForm = $('#checkout-form1').validate({
            // Rules for form validation
            rules: {
                txtNewEditAmount: {
                    required: true,
                    digits: true
                },
                
            },

            // Messages for form validation
            messages: {
                txtNewEditAmount: {
                    required: 'Please enter Amount.',
                    digits: 'Please enter numeric value'
                },
            },

            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        //  $scope.ads.ChargeType = $("#divChargeType input[type='radio']:checked").val();
                        $("#loading").show();
                        $http({
                            method: "POST",
                            url: "/Sale/Property/EditFlatCharge",
                            data: { flatID: $("#hidFlatID").val(), editTypeID: $scope.EditTypeID, editType: $scope.EditType, amount: $("#txtNewEditAmount").val() }
                        }).success(function (data) {
                            $("#myModalEdit").modal("hide");
                            alert(window.location.href);
                            window.location.href=window.location.href
                            //if ($scope.EditType == "Flat PLC") {
                            //    GetFlatPLC();
                            //}
                            //else if ($scope.EditType == "Flat Charge") {
                            //    GetFlatCharge();
                            //}
                            //else if ($scope.EditType == "Flat Other Charge") {
                            //    GetFlatOCharge();
                            //}
                            //$("#loading").hide();
                            //$("#MessageArea").show();
                            //$scope.MessageClass = "success";
                            //$scope.MessageTitle = "Success";
                            //$scope.Message = "Flat " + $scope.EditType + " Updated Successfully ";

                        }).error(function (error) {
                            $("#loading").hide();

                            $("#MessageArea").show();
                            $scope.MessageClass = "danger";
                            $scope.MessageTitle = "Error";
                            $scope.Message = "Flat " + $scope.EditType + " Not Updated, Please try again.";
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
    $scope.DeleteFlatAmountSave = function () {
        $("#loading").show();
        $http({
            method: "POST",
            url: "/Sale/Property/DeleteFlatAttribute",
            data: { flatID: $("#hidFlatID").val(), editTypeID: $scope.EditTypeID, editType: $scope.EditType }
        }).success(function (data) {
            $("#myModalDelete").modal("hide");
            if ($scope.EditType == "Flat PLC") {
                GetFlatPLC();
            }
            else if ($scope.EditType == "Flat Charge") {
                GetFlatCharge();
            }
            else if ($scope.EditType == "Flat Other Charge") {
                GetFlatOCharge();
            }
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageClass = "success";
            $scope.MessageTitle = "Success";
            $scope.Message = "Flat " + $scope.EditType + " Updated Successfully ";

        }).error(function (error) {
            $("#loading").hide();

            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Flat " + $scope.EditType + " Not Updated, Please try again.";
        });
    }

    $("#loading").hide();

    function BSPChange() {
        var bspTotal = 0;
        $("input[name='txtBSP']").each(function (index, value) {
            bspTotal += parseFloat($(this).val());
        })
        $("#TotalBSP").html(bspTotal);
    }
    function PLCChange() {
        var plcTotal = 0;
        $("input[name='txtPLC']").each(function (index, value) {
            plcTotal += parseFloat($(this).val());
        })
        $("#TotalPLC").html(plcTotal);
    }
    function AChargeChange() {
        var achargeTotal = 0;
        $("input[name='txtACharge']").each(function (index, value) {
            achargeTotal += parseFloat($(this).val());
        })
        $("#TotalACharge").html(achargeTotal);
    }
    function AOChargeChange() {
        var aochargeTotal = 0;
        $("input[name='txtAOCharge']").each(function (index, value) {
            aochargeTotal += parseFloat($(this).val());
        })
        $("#TotalAOCharge").html(aochargeTotal);
    }
    function GetFlatPLC() {
        $http({
            method: 'Get',
            url: '/Sale/Property/GetFlatPLCList',
            params: { flatid: $("#hidFlatID").val() }
        }).success(function (data) {
            $scope.FlatDetails.FlatPLCList = data;
        });
    }
    function GetFlatCharge() {
        $http({
            method: 'Get',
            url: '/Sale/Property/GetFlatChargeList',
            params: { flatid: $("#hidFlatID").val() }
        }).success(function (data) {
            $scope.FlatDetails.FlatChargeList = data;
        });
    }
    function GetFlatOCharge() {
        $http({
            method: 'Get',
            url: '/Sale/Property/GetFlatoChargeList',
            params: { flatid: $("#hidFlatID").val() }
        }).success(function (data) {
            $scope.FlatDetails.FlatOChargeList = data;
        });
    }
   
});

function SaleInstallmentClick() {
    var bsp = "";
    var plc = "";
    var acharge = "";
    var ocharge = "";
    var installment = "";
    var dueDate = "";

    $("input[name='txtBSP']").each(function (index, value) {
        bsp = bsp + ":" + parseFloat($(this).val());
    }).promise().done(function () {

        $("input[name='txtPLC']").each(function (index, value) {
            plc = plc + ":" + $(this).val();
        }).promise().done(function () {
            $("input[name='txtACharge']").each(function (index, value) {
                acharge = acharge + ":" + $(this).val();
            }).promise().done(function () {
                $("input[name='txtACharge']").each(function (index, value) {
                    acharge = acharge + ":" + $(this).val();
                }).promise().done(function () {
                    $("input[name='txtAOCharge']").each(function (index, value) {
                        ocharge = ocharge + ":" + $(this).val();
                    }).promise().done(function () {
                        $("select[name='ddlInstallment']").each(function (index, value) {
                            installment = installment + ":" + $(this).find(":selected").val();
                        }).promise().done(function () {
                            $("input[name='txtDueDate']").each(function (index, value) {
                                dueDate = dueDate + ":" + $(this).val()
                            }).promise().done(function () {

                                var flatid = $("#hidFlatID").val();
                                alert(flatid)
                                var sale = new Object();
                                sale.FlatsID = flatid;
                                sale.Installment = installment;
                                sale.bsp = bsp;
                                sale.plc = plc;
                                sale.acharges = acharge;
                                sale.ocharges = ocharge;
                                sale.dueDate = dueDate;
                                sale.InstallmentTotal = $("#InstallmentTotal").html();
                                sale.PLCTotal = $("#txtPLCAmount").val();
                                sale.AChargeTotal = $("#txtAChargeAmount").val();
                                sale.OChargeTotal = $("#txtAOChargeAmount").val();

                                $.ajax({
                                    type: 'Post',
                                    contentType: "application/json; charset=utf-8",
                                    url: '/Sale/Property/SaveInstallment2',
                                    data: "{'sale':" + JSON.stringify(sale) + "}",
                                    dataType: "json",
                                    // data: { sale: $scope.FSale }
                                    // data: JSON.stringify({ FlatsID: flatid, Installment: installment, bsp: bsp, plc: plc, acharges: acharge, ocharges: ocharge, dueDate: dueDate, InstallmentTotal: $("#InstallmentTotal").html(), PLCTotal: $("#txtPLCAmount").val(), AChargeTotal: $("#txtAChargeAmount").val(), OChargeTotal: $("#txtAOChargeAmount").val() }),
                                }).success(function (data) {
                                    if (data == "True") {
                                        $("#MessageArea").show();
                                        $("#MessageArea").removeClass("alert-danger");
                                        $("#MessageArea").addClass("alert-success");
                                        $("#MessageTitle").html('').html("Success");
                                        $("#Message").html('').html("Flat Installment Saved");
                                        $("#divShowInstallment").show();
                                        $("#divAddInstallments").hide();
                                        $("#btnDeleteInstallment").show();
                                        $("#btnAddRow").hide();
                                        $.ajax({
                                            type: 'Post',
                                            url: '/Sale/Property/GetFlatInstallmentWithCharges2',
                                            data: { FlatID: flatid, flatsize: $("#FlatSize").html() }
                                        }).success(function (data) {

                                            var scope = angular.element($("#content")).scope();
                                            scope.$apply(function () {
                                                scope.FlatInstallmentList = data;
                                            })

                                        })
                                    }
                                    else {
                                        $("#MessageArea").show();
                                        $("#MessageArea").removeClass("alert-success");
                                        $("#MessageArea").addClass("alert-danger");
                                        $("#MessageTitle").html('').html("Error");
                                        $("#Message").html('').html("Flat Installment Can't Saved");
                                    }
                                });

                            })
                        })
                    })
                })
            })
        })
    });






}

function DeletePlanRow(rno) {
    $("#" + rno).remove();
    BSPChange();
    PLCChange();
    AChargeChange();
    AOChargeChange();
}

function BSPChange() {
    var bspTotal = 0;
    $("input[name='txtBSP']").each(function (index, value) {
        bspTotal += parseFloat($(this).val());
    })
    $("#TotalBSP").html(bspTotal);
}
function PLCChange() {
    var plcTotal = 0;
    $("input[name='txtPLC']").each(function (index, value) {
        plcTotal += parseFloat($(this).val());
    })
    $("#TotalPLC").html(plcTotal);
}
function AChargeChange() {
    var achargeTotal = 0;
    $("input[name='txtACharge']").each(function (index, value) {
        achargeTotal += parseFloat($(this).val());
    })
    $("#TotalACharge").html(achargeTotal);
}
function AOChargeChange() {
    var aochargeTotal = 0;
    $("input[name='txtAOCharge']").each(function (index, value) {
        aochargeTotal += parseFloat($(this).val());
    })
    $("#TotalAOCharge").html(aochargeTotal);
}