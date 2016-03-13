var myApp = angular.module('TransferApp', []);
myApp.controller('TransferPropertyController', function ($scope, $http, $filter) {

    $scope.Error = "";
    var orderBy = $filter('orderBy');
    $("#loading").show();

    $scope.TransferProeprtyInit=function()
    {
        $("#divTrans").hide();
        var flatid = $("#hidFlatID").val();
        if (flatid == "0") {
            $("#Newsaletower").show();
            GetLoadTowerFlat();
           }
        else
        {
            $("#loading").show();        

            $http({
                method: 'Get',
                url: '/Sale/Property/GetFlatDetails',
                params: { flatid: flatid },
            }).success(function (data) {
                $scope.FlatDetails = data;
                if ($scope.FlatDetails.Status == "Available") {
                    $("#MessageArea").show();
                    $scope.MessageClass = "danger";
                    $scope.MessageTitle = "Error";
                    $scope.Message = "Flat is available to sale, can't transfer";
                }
                else {
                    $("#divTrans").show();
                    $("#MessageArea").hide();
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
                    url: '/Sale/Property/GetFlatsaleId',
                    params: { flatid: flatid },
                }).success(function (data) {
                    $scope.Cust = data;
                    $("#loading").hide();
                })
                //$('input:radio[name=SaleType]').val("Sale");
              
            })
        }


    }

    $scope.NewSaleInit = function () {
        var flatid = $("#hidFlatID").val();
        if (flatid == "0") {
            $("#Newsaletower").show();
            GetLoadTowerFlat();
            $http({
                method: 'Get',
                url: '/Master/PlanCtrl/GetPlanTypeMasterDistList',
            }).success(function (data) {
                $scope.PlanTypeMasterList = data;
            })
        }
        else {
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
                $http({
                    method: 'Get',
                    url: '/Sale/Property/GetFlatsaleId',
                    params: { flatid: flatid },
                }).success(function (data) {
                    $scope.Cust = data;
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
    $scope.SearchsaleTransferProperty = function () {
        $scope.Error = "";
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        $scope.TransferProeprtyInit();
    }
    $scope.SearchsaleProperty = function () {
        $scope.Error = "";
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        $scope.NewSaleInit();
    }
    $scope.ShowPlanPriceNewSale = function (FType, Size) {
        $("#loading").show();
        var PlanName = $("#PlanID").find(":selected").val();
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
                                        alert(data)
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
                                                alert(flatid)

                                                $("#divShowInstallment").show();
                                                $("#divAddInstallments").hide();
                                                $("#btnDeleteInstallment").show();
                                                $("#btnAddRow").hide();
                                                $("#loading").hide();
                                            })
                                            $("#loading").hide();
                                        }
                                        else {
                                            $("#MessageArea").show();
                                            $("#MessageArea").removeClass("alert-success");
                                            $("#MessageArea").addClass("alert-danger");
                                            $("#MessageTitle").html('').html("Error");
                                            $("#Message").html('').html("Flat Installment Can't Saved");
                                        }
                                    }).error(function (error) {
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
        if ($scope.flat.SaleType == null || $("#txtSaleDate").val() == null) {
            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Please provide date or sale type";
        }
        else {
            $http({
                method: 'Post',
                url: '/Sale/Property/SaleFlatSave',
                data: { flatid: flatid, salerate: $scope.GTotal, saleDate: $("#txtSaleDate").val(), saletype: $scope.flat.SaleType }
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

                    $("#MessageArea").show();
                    $scope.MessageClass = "success";
                    $scope.MessageTitle = "Success";
                    $scope.Message = "Flat Successfully " + $scope.flat.SaleType;
                }
                else {
                    $("#MessageArea").show();
                    $scope.MessageClass = "danger";
                    $scope.MessageTitle = "Error";
                    $scope.Message = "Flat Not " + $scope.flat.SaleType + " , Please try again.";
                }
                $('#myModal').modal('hide');
            })
        }
        $("#loading").hide();
    }
    //$scope.SaveTransfer = function () {
    //    $("#loading").show();
    //    var flatid = $("#hidFlatID").val();
    //    var OldCustId = $("#custId").val();
    //    var OldCustId = $("#custId").val();
    //    var Fname = $("#NewCustFname").val();
    //    var Mname = $("#NewCustMname").val();
    //    var Lname = $("#NewCustLname").val();
    //    if ($scope.flat.SaleType == null || $("#txtSaleDate").val() == null) {
    //        $("#MessageArea").show();
    //        $scope.MessageClass = "danger";
    //        $scope.MessageTitle = "Error";
    //        $scope.Message = "Please provide date or sale type";
    //    }
    //    else {
    //        $http({
    //            method: 'Post',
    //            url: '/Sale/Property/AddCustomer',
    //            data: { FName: Fname,MName:Mname,LName:Lname,Oldcustid:OldCustId, }
    //        })
    //        $http({
    //            method: 'Post',
    //            url: '/Sale/Property/SaleFlatSave',
    //            data: { flatid: flatid, salerate: $scope.GTotal, saleDate: $("#txtSaleDate").val(), saletype: $scope.flat.SaleType }
    //        }).success(function (data) {
    //            if (data == "1") {
    //                // Update Flat status and insert into customer default value with flat and saleid.
    //                $http({
    //                    method: 'Post',
    //                    url: '/Admin/CreateProperty/UpdateFlatStatus',
    //                    data: { flatid: flatid, status: $scope.flat.SaleType }
    //                }).success(function (data) {
    //                    if (data == "1") {
    //                        $("#btnSaveSale").hide();
    //                    }
    //                })

    //                $("#MessageArea").show();
    //                $scope.MessageClass = "success";
    //                $scope.MessageTitle = "Success";
    //                $scope.Message = "Flat Successfully " + $scope.flat.SaleType;
    //            }
    //            else {
    //                $("#MessageArea").show();
    //                $scope.MessageClass = "danger";
    //                $scope.MessageTitle = "Error";
    //                $scope.Message = "Flat Not " + $scope.flat.SaleType + " , Please try again.";
    //            }
    //            $('#myModal').modal('hide');
    //        })
    //    }
    //    $("#loading").hide();
    //}

    $scope.SaveTransfer = function ()
    {
        var flatid = $("#hidFlatID").val();
        var saleid = $("#custId").val();
        var OldCustId = $("#custmerid").val();
        var apptitle = $("#ddlAppTitle").find(":selected").val();
        var Fname = $("#NewCustFname").val();
        var Mname = $("#NewCustMname").val();
        var Lname = $("#NewCustLname").val();
        var Amount = $("#txtTAmount").val();
        var TDate = $("#txtTDate").val();
        $("#loading").show();
        $http({
            method: 'Post',
            url: '/Sale/Property/SaleFlatTransfer',
            data: {AppTitle:apptitle, FName: Fname, MName: Mname, LName: Lname, salecustid: saleid,OldCustId:OldCustId, TransferAmount: Amount,TDate:TDate }
        }).success(function (data) {
            var ata = data;
            ata = ata.replace('"','').replace('"','');
            if (ata == "Yes") {
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = "Success";
                $scope.Message = "Successfully Transfered";
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Successfully Not Transfer , Please try again.";
            }
            $("#loading").hide();
        })
    }

    $scope.TransferChange = function () {
       
        var trans = $("#TransferID").find(":selected").val();
        if (trans == "Transfer Flat") {
            $("#divTower").hide();
            $("#divFlat").show();
        }
        else if(trans=="Transfer Tower"){
            $("#divTower").show();
            $("#divFlat").show();
        }
        else if (trans == "Transfer Project" || trans=="Transfer Company") {
            $("#divTower").show();
            $("#divFlat").show();
        }
    }
    $scope.ChangeTower2 = function () {

        $scope.Tower2Name = $("#ddlTower").find(":selected").text();
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetFlatByTowerID',
            params: { towerid: $scope.Trans.TowerID }
        }).success(function (data) {
            $scope.FlatList2 = data;
            $("#loading").hide();
        })
    }
    $scope.ChangeFlat = function () {

        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getFlatByID',
            params: { flatid: $scope.Trans.FlatID }
        }).success(function (data) {
            $scope.FlatDetails2 = data;
            $("#loading").hide();
            if ($scope.FlatDetails2.Status != "Available") {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Flat is not available to sale";
                $("#btnTransfer").hide();
            }
            else {
                $("#MessageArea").hide();
                $("#btnTransfer").show();
            }
        });
    }

    
    $scope.ConfirmedTransferProperty = function () {
        $("#btnConfirmSave").hide();
        $("#loading").show();
        $http({
            method: 'Post',
            url: '/Customer/Transfer/TranserPropertySave',
            data: { curFlatID: $scope.Flat.FlatID, newFlatID: $scope.Trans.FlatID, saleID: $scope.FlatDetails.SaleFlatModel[0].SaleID, transferType: $scope.Flat.TransferID }
        }).success(function (data) {
            $("#loading").hide();
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = "Success";
                $scope.Message = "Property Transfered Successfully.";
        }).error(function (error) {
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Please try agian, something missing";
        });
    }

    $("#loading").hide();
})