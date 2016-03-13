var myApp = angular.module('PaymentApp', []);
myApp.controller('PaymentDiscountController', function ($scope, $http, $filter) {

    $scope.AddDiscountInit1 = function () {
        $("#divTrans").hide();
        GetLoadTowerFlat();
    }
    $scope.AddDiscountInit = function () {
        $("#divTrans").hide();

        var flatid = $("#hidFlatID").val();
        if (flatid == "0") {
            $("#Newsaletower").show();

        }
        else {
            GetFlatInfo(flatid);
            $scope.Pay = {};
            $("#loading").show();
            GetOtherPayment(flatid);
            $http({
                method: 'Get',
                url: '/Sale/Property/GetFlatsaleId',
                params: { flatid: flatid },
            }).success(function (data) {
                $scope.Cust = data;
                $scope.Pay.SaleID = $scope.Cust.Sale[0].SaleID;
                $("#loading").hide();
            })
        }
    }
   
    $scope.SearchsaleTransferProperty = function () {
        $scope.Error = "";
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        $scope.AddDiscountInit();
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
    $scope.SearchDisountHistory = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Sale/PaymentDiscount/SearchDiscont',
            params: { SearchType: $scope.SearchBy, FlatNo: $scope.Flat.FlatID, DateFrom: $("#datefrom").val(), DateTo: $("#dateto").val(), UserName: $scope.UserName, IsApproved :$("#chkIsApproved").is(":checked")}
        }).success(function (data) {
            $scope.DiscountList = data;
            $("#loading").hide();
        });
    }
    $scope.ChangePaymentype = function () {

        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                ddlPaymentType: {
                    required: true
                },
                txtDiscountAmt: {
                    required: true
                },
            },

            // Messages for form validation
            messages: {
                ddlPaymentType: {
                    required: "Please select discount amount type."
                },
                txtDiscountAmt: {
                    required: "Floor no can't blank."
                },
            },

            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {

                    }
                });
                $("#MessageArea").hide();
                $("#myModal").modal("show");
            },
            // Do not change code below.
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Enter Discount details.";
            }
        });
    }
    $scope.ClickSaveDiscount = function () {

    }
    $scope.ConfirmedAddDiscount = function () {
        var IsAuth = [];
        var ArUser = [];

        $('.ProAuth').each(function () {
            if ($(this).is(":checked")) {
                IsAuth.push($(this).val());
            }
        });

        $('.mydays').each(function () {
            ArUser.push($(this).find(":selected").val() + ":" + $(this).attr('name'));
        });
        $scope.Pay.UserNames = ArUser.toString();
        $scope.Pay.AuthUserNames = IsAuth.toString();
        $("#loading").show();
        $http({
            method: 'POST',
            url: '/Sale/PaymentDiscount/AddPaymentDiscount',
            data: JSON.stringify($scope.Pay)
        }).success(function (data) {
            $("#loading").hide();
            $("#myModal").modal("hide");
            if (data == "0") {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = " Error";
                $scope.Message = "Internal Error, Please trying again.";
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = " Success";
                $scope.Message = "Discount request added successfully.";
            }
        })
    }

    $scope.ApproveDiscountInit = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/Security/AuthUserList',
            params: {}
        }).success(function (data) {
            $("#loading").hide();
            $scope.UserList = data;
        })
    }
    $scope.UserChange = function () {

    }
    $scope.SearchUserDiscountList = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Sale/PaymentDiscount/GetPDByUserName',
            params: { userName: $scope.UserName, isApproved: false }
        }).success(function (data) {
            $("#loading").hide();
            $scope.PaymentList = data;
        });
       
    }
    $scope.ApprovePaymentClick = function (paydisid) {
        $scope.PaymentDiscountID = paydisid;
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Sale/PaymentDiscount/GetPaymentDiscountApproveList',
            params: { paymentDiscountid: paydisid }
        }).success(function (data) {
            $("#loading").hide();
            $scope.PaymentApproveList = data;
            var cnt = 0;
            var ttl = 0;
            $scope.IsLast = false;
            for (var i = 0; i < $scope.PaymentApproveList.length; i++) {
                var list = $scope.PaymentApproveList[i];
                ttl = ttl + 1;
                if (list.IsApproved == true) {
                    cnt = cnt + 1;
                }
            }
            if (ttl == cnt + 1) {
                $scope.IsLast = true;
            }
        });
    }
    $scope.ConfirmedPaymentDiscount = function () {
        $("#loading").show();
        $http({
            method: 'Post',
            url: '/Sale/PaymentDiscount/ApproveDiscountPaymentApprove',
            data: { PaymentDiscountID: $scope.PaymentDiscountID, Remarks: $("#txtRemarks").val(), IsLast: $scope.IsLast }
        }).success(function (data) {
            $("#myModal").modal("hide");
            $("#loading").hide();
            if (data == "0") {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = " Error";
                $scope.Message = "Can't Updated, Please trying again.";
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = " Success";
                $scope.Message = "Discount request added successfully.";
            }
        })
    }
    $scope.HistoryDiscountInit = function () {
        $scope.SearchBy = "Flat";
        $("#divUserName").hide();
        $("#divFlat").show();
        $("#divDate").hide();
        GetLoadTowerFlat();
        $scope.UserName = "";
        $scope.Flat = {};
        $scope.Flat.FlatID = 0;
        $http({
            method: 'Get',
            url: '/Admin/Security/AuthUserList',
            params: {}
        }).success(function (data) {
            $("#loading").hide();
            $scope.UserList = data;
        })
    }
    $scope.ChangeSearchBy = function () {
        $("#chkIsApproved").removeAttr("Disabled");

        if ($scope.SearchBy == "Flat") {
            $("#divUserName").hide();
            $("#divFlat").show();
            $("#divDate").hide();
        }
        else if ($scope.SearchBy == "RequestDate") {
            $("#divUserName").hide();
            $("#divFlat").hide();
            $("#divDate").show();
        }
        else if ($scope.SearchBy == "ApproveDate") {
            $("#divUserName").hide();
            $("#divFlat").hide();
            $("#divDate").show();
            $("#checkbox").is(":checked") = true;
            $("#chkIsApproved").attr("Disabled");
        }
        else if ($scope.SearchBy == "UserName") {
            $("#divUserName").show();
            $("#divFlat").hide();
            $("#divDate").hide();
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

    function GetOtherPayment(flatid) {

        // Table=Flat
        //Table1=SaleFlat
        // Table2=Customer
        // Table3=FlatInstallmentDetail
        //Table4=FlatPLC
        //Table5=FlatCharge
        //Table6=FlatOCharge
        //Table7=ServiceTax
        //Table8=PaymentMaster
        //Table9=PaymentOther
        $http({
            method: 'Get',
            url: '/Sale/OtherPayment/GetOtherPaymentBySP',
            params: { flatID: flatid }
        }).success(function (data) {
            $scope.Flat = data.Table;
            $scope.Sale = data.Table1;
            $scope.Customer = data.Table2;
            $scope.Installment = data.Table3;
            $scope.FlatPLC = data.Table4;
            $scope.FlatCharge = data.Table5;
            $scope.FlatOCharge = data.Table6;
            $scope.PaymentMaster = data.Table8;
            $scope.PaymentOther = data.Table9;
            $scope.LatePayment = data.Table10;
            $scope.PropertyTransfer = data.Table11;
            $scope.PaymentClearance = data.Table12;

            var stax = data.Table7[0].ServiceTaxPer;
            if ($scope.Sale.length > 0) {
                $("#divTrans").show();
                $("#loading").hide();
                $("#MessageArea").hide();

            }
            else {
                $("#loading").hide();
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Flat Sale Not Found, Please First Sale Flat.";
            }
            $scope.CustomerName = $scope.Customer[0].AppTitle + " " + $scope.Customer[0].FName + " " + $scope.Customer[0].MName + " " + $scope.Customer[0].LName;
            var flatsize = $scope.Flat[0].FlatSize;

            var tinstall = 0;
            var tinstalltax = 0;
            var tinstalltotal = 0;
            for (var i = 0; i < $scope.Installment.length; i++) {
                var list = $scope.Installment;
                tinstall += list[i].BSPAmount;
                tinstalltotal += list[i].TotalAmount;
                tinstalltax += list[i].TaxAmount;
            }
            $scope.TotalBSPAmount = tinstall;
            $scope.TotalInstallmentAmount = tinstalltotal;
            $scope.TotalInstallTax = tinstalltax;

            var tplc = 0;
            for (var i = 0; i < $scope.FlatPLC.length; i++) {
                var list = $scope.FlatPLC;
                tplc += list[i].TaxAmount;
            }
            $scope.TotalFlatPLC = tplc;

            var tadditional = 0;
            for (var i = 0; i < $scope.FlatCharge.length; i++) {
                var list = $scope.FlatCharge;
                if (list[i].ChargeType == 'One Time') {
                    $scope.FlatCharge[i].TaxAmount = list[i].Amount * list[i].TaxPer / 100
                    tadditional += $scope.FlatCharge[i].TaxAmount;
                }
                else if (list[i].ChargeType == 'Free') {
                    $scope.FlatCharge[i].TaxAmount = 0
                }
                else {
                    $scope.FlatCharge[i].TaxAmount = list[i].Amount * flatsize * list[i].TaxPer / 100
                    tadditional += $scope.FlatCharge[i].TaxAmount;
                }
            }
            $scope.TotalFlatCharge = tadditional;

            var taddonamount = 0;
            for (var i = 0; i < $scope.FlatOCharge.length; i++) {
                var list = $scope.FlatOCharge;
                if (list[i].ChargeType == 'One Time') {
                    $scope.FlatOCharge[i].TaxAmount = list[i].Amount * list[i].TaxPer / 100
                    taddonamount += $scope.FlatOCharge[i].TaxAmount;
                }
                else if (list[i].ChargeType == 'Free')
                    $scope.FlatOCharge[i].TaxAmount = 0
                else {
                    $scope.FlatOCharge[i].TaxAmount = list[i].Amount * flatsize * list[i].TaxPer / 100
                    taddonamount += $scope.FlatOCharge[i].TaxAmount;
                }
            }
            $scope.TotalFlatOCharge = taddonamount;

            var tlatep = 0;
            for (var i = 0; i < $scope.LatePayment.length; i++) {
                var list = $scope.LatePayment;
                tlatep += list[i].InterestAmount;
            }
            $scope.TotalLatePayment = tlatep;

            var ttransferamt = 0;
            for (var i = 0; i < $scope.PropertyTransfer.length; i++) {
                var list = $scope.PropertyTransfer;
                ttransferamt += list[i].TransferAmount;
            }
            $scope.TotalPropertyTransfer = ttransferamt;

            var tpclearamt = 0;
            for (var i = 0; i < $scope.PaymentClearance.length; i++) {
                var list = $scope.PaymentClearance;
                tpclearamt += list[i].Amount;
            }
            $scope.TotalPaymentClearance = tpclearamt;

            // Payment Other calculate
            var ST = 0;
            var LP = 0;
            var TP = 0;
            var CC = 0;
            var PT = 0;
            var AT = 0;
            var OT = 0;
            for (var i = 0; i < $scope.PaymentOther.length; i++) {
                var list = $scope.PaymentOther;
                if (list[i].PaymentFor == "Installment Service Tax") {
                    ST += list[i].Amount;
                }
                else if (list[i].PaymentFor == "Late Payment Charges") {
                    LP += list[i].Amount;
                }
                else if (list[i].PaymentFor == "Transfer Fee") {
                    TP += list[i].Amount;
                }
                else if (list[i].PaymentFor == "Clearance Charges") {
                    CC += list[i].Amount;
                }
                else if (list[i].PaymentFor == "PLC Service Tax") {
                    PT += list[i].Amount;
                }
                else if (list[i].PaymentFor == "Additional Charge Tax") {
                    AT += list[i].Amount;
                }
                else if (list[i].PaymentFor == "AddOn Charge Tax") {
                    OT += list[i].Amount;
                }
            }
            $scope.TotalST = ST;
            $scope.TotalLP = LP;
            $scope.TotalTP = TP;
            $scope.TotalCC = CC;
            $scope.TotalPT = PT;
            $scope.TotalAT = AT;
            $scope.TotalOT = OT;

            // $("#btnSubmitPayment").removeClass("disabled");
            $("#loading").hide();
        });

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
    function GetFlatInfo(flatid) {
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getFlatByID',
            params: { flatid: flatid }
        }).success(function (data, status, headers, config) {
            $scope.FlatDetails = data;
            GetAuthority(data.ProjectID);
        });
    }
    function GetAuthority(projectid) {
        $('#loading').show();

        $http({
            method: 'Get',
            url: '/Admin/Security/GetPropertyByProjectID',
            params: { projectid: projectid }
        }).success(function (data) {
            $("#tbodyhtml").html(data);
            $('#loading').hide();

        })
    }
    $("#loading").hide();

});