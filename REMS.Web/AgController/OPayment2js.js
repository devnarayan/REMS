var myApp = angular.module('OPaymentApp', []);
//Defining a Controller 
myApp.controller('OtherPaymentController', function ($scope, $http, $filter) {
    // Get all Property list for dropdownlist
    var orderBy = $filter('orderBy');
    $("#loading").show();
    $http.get('/Sale/Payment/GetBank/').success(function (response) { $scope.Banks = response; });

    $scope.OPaymnetInit = function () {
        GetLoadTowerFlat();
        var sid = $("#hidFlatID").val();
        if (sid == 0) {
            $("#step3").hide();
        }
        else {
            $("#step3").show();
            GetOtherPayment(sid)
        }
    }


    $scope.TowerChange = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetFlatByTowerID',
            params: { towerid: $scope.FlatSearch.TowerID }
        }).success(function (data) {
            $scope.FlatList = data;
            $("#loading").hide();
        })
    }

    $scope.SearchFlat = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.FlatSearch.FlatID);
        var pid = $("#hidFlatID").val();
        if (pid > 0 && pid != null) {
            GetOtherPayment(pid);
            $("#step3").show();
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
                if (list[i].ChargeType == 'One Time'){
                    $scope.FlatCharge[i].TaxAmount = list[i].Amount * list[i].TaxPer / 100
                    tadditional += $scope.FlatCharge[i].TaxAmount;
                }
                else if (list[i].ChargeType == 'Free'){
                    $scope.FlatCharge[i].TaxAmount = 0
                }
                else
                {
                    $scope.FlatCharge[i].TaxAmount = list[i].Amount * flatsize * list[i].TaxPer / 100
                    tadditional += $scope.FlatCharge[i].TaxAmount;
                }
            }
            $scope.TotalFlatCharge = tadditional;

            var taddonamount = 0;
            for (var i = 0; i < $scope.FlatOCharge.length; i++) {
                var list = $scope.FlatOCharge;
                if (list[i].ChargeType == 'One Time'){
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
                if (list[i].PaymentFor == "BSP Service Tax") {
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

            $("#btnSubmitPayment").removeClass("disabled");
            $("#loading").hide();
        })

    }

    $scope.OtherPaymentChange2 = function () {

    }
    $scope.SaveOtherPayment = function () {
        var vli = ValidateSubmitPayment();
        if (vli == false) {
            $('#loading').hide();
            $('#myErrorModal').modal('show');
        }
        else {
            var v2 = ValidateSubmitedAmountPayment();
            if (v2 == false) {
                $('#loading').hide();
                $('#myErrorModal').modal('show');
            }
            else {
                $('#btnSubmitPayment').hide();
                $('#loading').show();
                $.ajax({
                    method: 'POST',
                    url: '/Sale/Payment/InsertOtherPayments',
                    data: { InstallmentNo: $("#ddlPayment").find(":selected").text(), Saleid: $("#SaleID").val(), Flatname: $("#FlatName").val(), PaymentMode: $("#PaymentMode").val(), ChequeNo: $("#ChequeNo").val(), ChequeDate: $("#chequeDate").val(), BankName: $("#BankName").find(":selected").text(), BankBranch: $("#BankBranch").val(), Remarks: $("#Remarks").val(), PayDate: $("#paymentDate").val(), Amtrcvdinwrds: $("#PaymentAmountInWords").val(), ReceivedAmount: $("#ReceivedAmount").val(), IsPrint: $("#chkPrint").is(":checked"), IsEmailSent: $("#chkSendEmail").is(":checked"), EmailTo: $("#txtSendEmail").val(), CustomerName: $("#hidCustomerName").val(), CustomerID: $("#hidCustomerID").val() },
                }).success(function (data) { //Showing success message $scope.status = "The Person Saved Successfully!!!";
                    if (data != "No") {
                        alert("Payment Details has been saved successfully!");
                        //OPaymentConfirm();
                        if ($("#chkPrint").is(":checked") == true)
                            window.open(data, '_blank');
                        GetOtherPayment($("#hidFlatID").val());
                    }
                    else {
                        alert("Payment Details has been saved successfully !!!");
                        $scope.Error = 'Unable to save Payment Details: ';
                    }
                    $('#btnSubmitPayment').show();
                    $('#loading').hide();
                    // $scope.Error = 'Payment Details saved successfully.';
                }).error(function (error) {
                    alert("Payment Details Not Saved !!!");
                    $scope.Error = 'Unable to save Payment Details: ' + error.message;
                    $('#btnSubmitPayment').show();
                    $('#loading').hide();
                });
            }
        }
        // end save payment.
    }


    $("#loading").hide();
    function ValidateSubmitPayment() {
        $('#loading').hide();
        var vl = true;
        var message = "";
        if ($("#ReceivedAmount").val() == "") {
            vl = false;
            message += "Insert receive amount. <br/>";
        }
        if ($("#paymentDate").val() == "") {
            vl = false;
            message += "Insert payment date. <br/>";
        }
        if ($("#PaymentMode").val() == "") {
            vl = false;
            message += "Insert PaymentMode.<br/>";
        }
        else if ($("#PaymentMode").val() != "Cash" && $("#PaymentMode").val() != "Transfer Entry") {
            if ($("#ChequeNo").val() == "") {
                vl = false;
                message += "Insert ChequeNo details.<br/>";
            }
            if ($("#chequeDate").val() == "") {
                vl = false;
                message += "Insert Cheque Date details.<br/>";
            }
        }

        //if ($("#DueAmount").val() == "") {
        //    vl = false;
        //    message += "Please insert Due Amount.<br/>";
        //}
        //if ($("#TotalAmount").val() == "") {
        //    vl = false;
        //    message += "Please insert Total Amount. <br/>";
        //}
        else if ($("#PaymentAmountInWords").val() == "") {
            vl = false;
            message += "Please insert Amount in words.";
        }
      

        $("#ErrorMessage").html(message);
        return vl;
    }
    function ValidateSubmitedAmountPayment() {
        $('#loading').hide();
        var vl = true;
        var message = "";
        $scope.PaymentForText=$("#ddlPayment").find(":selected").text();
        if ($scope.PaymentForText == "BSP Service Tax") {
            if (($scope.TotalInstallTax - $scope.TotalST) < $("#ReceivedAmount").val()) {
                 vl = false;
                 message += "Received amount can't more then payable " + $scope.PaymentForText + " . <br/>";
            }
        }
        else if ($scope.PaymentForText == "Late Payment Charges") {
            if (($scope.TotalLatePayment -$scope.TotalLP) < $("#ReceivedAmount").val()) {
                vl = false;
                message += "Received amount can't more then payable " + $scope.PaymentForText + " . <br/>";
            }
        }
        else if ($scope.PaymentForText == "Transfer Fee") {
          
            if (($scope.TotalPropertyTransfer - $scope.TotalTP) < $("#ReceivedAmount").val()) {
                vl = false;
                message += "Received amount can't more then payable " + $scope.PaymentForText + " . <br/>";
            }
        }
        else if ($scope.PaymentForText == "Clearance Charges") {
            if (($scope.TotalPaymentClearance - $scope.TotalCC) < $("#ReceivedAmount").val()) {
                vl = false;
                message += "Received amount can't more then payable " + $scope.PaymentForText + " . <br/>";
            }
        }
        else if ($scope.PaymentForText == "PLC Service Tax") {
            if (($scope.TotalFlatPLC - $scope.TotalPT) < $("#ReceivedAmount").val()) {
                vl = false;
                message += "Received amount can't more then payable " + $scope.PaymentForText + " . <br/>";
            }
        }
        else if ($scope.PaymentForText == "Additional Charge Tax") {
            if (($scope.TotalFlatCharge - $scope.TotalAT) < $("#ReceivedAmount").val()) {
                vl = false;
                message += "Received amount can't more then payable " + $scope.PaymentForText + " . <br/>";
            }
        }
        else if ($scope.PaymentForText == "AddOn Charge Tax") {
            if (($scope.TotalFlatOCharge - $scope.TotalOT) < $("#ReceivedAmount").val()) {
                vl = false;
                message += "Received amount can't more then payable " + $scope.PaymentForText + " . <br/>";
            }
        }
        $("#ErrorMessage").html(message);
        return vl;
    }
})


function PaymentModeChange(obj) {
    var selectedValue = $(obj).val();
    if (selectedValue == "Cash" || selectedValue == 'Transfer Entry') {
        $("#divCheque").hide();
    } else {
        $.ajax({
            url: 'GetBanks',
            type: 'post',
            success: function (states) {
                // states is your JSON array
                var $select = $('#BankName');
                $.each(states, function (i, state) {
                    $('<option>', {
                        value: state.BankID
                    }).html(state.BankName).appendTo($select);
                });
            }
        });
        $("#divCheque").show();
    }
}

var a = ['', 'One ', 'Two ', 'Three ', 'Four ', 'Five ', 'Six ', 'Seven ', 'Eight ', 'Nine ', 'Ten ', 'Eleven ', 'Twelve ', 'Thirteen ', 'Fourteen ', 'Fifteen ', 'Sixteen ', 'Seventeen ', 'Eighteen ', 'Nineteen '];
var b = ['', '', 'Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];
function inWords(num) {

    if ((num = num.toString()).length > 9) return 'overflow';
    n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
    if (!n) return;
    var str = '';
    str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'Crore ' : '';
    str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'Lakh ' : '';
    str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'Thousand ' : '';
    str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'Hundred ' : '';
    str += (n[5] != 0) ? ((str != '') ? 'and ' : '') + (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + 'Only ' : '';
    $('#PaymentAmountInWords').val(str)
}