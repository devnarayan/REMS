var myApp = angular.module('PaymentApp', []);
//Defining a Controller 
myApp.controller('PaymentController', function ($scope, $http, $filter) {
    // Get all Property list for dropdownlist
    $scope.Error = "";
    var orderBy = $filter('orderBy');

    $scope.SubmitPaymentInit = function () {
        $('#loading').show();
        GetLoadTowerFlat();
        $http.get('/Sale/Payment/GetBank/').success(function (response) { $scope.Banks = response; });


        if ($("#hidFlatID").val() == "0") {
            $("#step3").hide();
        }
        else {
           
            $("#step3").show();
            GetPaymentDetails();
        }
    }

   
    $scope.SearchFlatClick = function () {       
        $("#hidFlatID").val($scope.Flat.FlatID);
        GetPaymentDetails();
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
    function GetPaymentDetails() {
        // get flat details
      
        var flatid = $("#hidFlatID").val();       
        $("#step3").show();

        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/getFlatByID',
            params: { flatid: flatid }
        }).success(function (data) {
            $scope.Flat = data;
            // Get Sale Details
            $http({
                method: 'Get',
                url: '/Sale/Payment/GetSaleByFlatID',
                params: { flatid: flatid }
            }).success(function (data) {
                $scope.Sale = data;
                // if flat is sale/booked.
                if (data != "null") {
                    // Get Customer Details
                    $http({
                        method: 'Get',
                        url: '/Sale/Payment/GetCustomerbyFlatID',
                        params: { flatid: flatid }
                    }).success(function (data) {
                        $scope.Customer = data;
                        $scope.CustomerName = data.AppTitle + " " + data.FName + " " + data.MName + " " + data.LName;
                        // Get Installment Details
                        $http({
                            method: 'Get',
                            url: '/Sale/Property/GetFlatInstallmentWithCharges',
                            params: { flatid: flatid, flatsize: $scope.Flat.FlatSize, version:0}
                        }).success(function (data) {
                            $scope.Install = data;

                            // Get Payment Details
                            $http({
                                method: 'Get',
                                url: '/Sale/Payment/GetPaymentList',
                                params: { saleid: $scope.Sale.SaleID }
                            }).success(function (data) {
                                $scope.PaymentList = data;

                                var total3 = 0;
                                for (var i = 0; i < $scope.PaymentList.length; i++) {
                                    var list = $scope.PaymentList;
                                    total3 += list[i].Amount;
                                }
                                $scope.TotalPaid = total3;
                                var total2 = 0;
                                for (var i = 0; i < $scope.Install.length; i++) {
                                    var list = $scope.Install;
                                    total2 += list[i].TotalAmount;
                                }
                                $scope.TotalInstallmnetDueAmount = total2;
                                var installCount = 0;
                                var installLastCount = 0;
                                var flag = "0", flag2 = "0";
                                angular.forEach($scope.Install, function (value, index) {
                                    installCount += value.TotalAmount;

                                    if ($scope.TotalPaid < installCount) {
                                        if (flag == "0") {
                                            flag = "1";
                                            $scope.ThisInstallmentNo = value.InstallmentID;
                                            $scope.ThisInstallmentName = value.Installment;

                                            $scope.DueDate = value.DueDateSt;
                                            $scope.ThisInstallmentAmount = value.TotalAmount;
                                            $scope.ThisInsTotalSum = installCount;
                                            //  alert('yes' + $scope.ThisInsTotalSum);
                                        }
                                    }
                                    else {
                                        $scope.LastInstallmentTotal = installCount;
                                        // alert('no' + $scope.LastInstallmentTotal);
                                    }

                                }); // end loop
                                if ($scope.LastInstallmentTotal < $scope.ThisInsTotalSum) {
                                    $scope.CurrentDue = $scope.LastInstallmentTotal + $scope.ThisInstallmentAmount - $scope.TotalPaid;
                                    // alert('not eq ' + $scope.CurrentDue)
                                }
                                else {
                                    $scope.CurrentDue = $scope.ThisInstallmentAmount - $scope.TotalPaid;
                                    // alert('==' + $scope.CurrentDue);
                                }

                                // Enable save button
                                $("#btnSubmitPayment").removeClass("disabled");
                                $("#loading").hide();
                            })
                        })
                    })
                }
                else {
                    $("#step3").hide();

                    $("#MessageArea").show();
                    $scope.MessageClass = "danger";
                    $scope.MessageTitle = "Error";
                    $scope.Message = "Flat Sale not found";
                }
                $("#loading").hide();
            })
        })
    }
    $scope.TowerChange = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetFlatByTowerID',
            params: { towerid:$scope.Flat.TowerID }
        }).success(function (data) {
            $scope.FlatList = data;
            $("#loading").hide();
        })
    }

    $scope.SavePayment = function () {
        var vli = ValidateSubmitPayment();
        if (vli == false) {
            $('#myErrorModal').modal('show');
        }
        else {
            $("#loading").show();
            var paytype = "BSP";
            paytype = $('input:radio[name=radio]:checked').val();

            $.ajax({
                method: 'POST',
                url: '/Sale/Payment/InsertPaymentDetails',
                data: { InstallmentNo: $("#ddlPayment").find(":selected").val(), Saleid: $("#SaleID").val(), Flatname: $("#FlatName").val(), DueAmount: $("#DueAmount").val(), DueDate: $("#DueDate").val(), PaymentMode: $("#PaymentMode").val(), ChequeNo: $("#ChequeNo").val(), ChequeDate: $("#chequeDate").val(), BankName: $("#BankName").find(":selected").text(), BankBranch: $("#BankBranch").val(), Remarks: $("#Remarks").val(), PayDate: $("#paymentDate").val(), Amtrcvdinwrds: $("#PaymentAmountInWords").val(), ReceivedAmount: $("#ReceivedAmount").val(), IsPrint: $("#chkPrint").is(":checked"), IsEmailSent: $("#chkSendEmail").is(":checked"), EmailTo: $("#txtSendEmail").val(), CustomerName: $("#hidCustomerName").val(), CustomerID: $("#hidCustomerID").val(), chkInterest: $("#chkInterest").is(":checked"),PaymentType:paytype },
            }).success(function (data) { //Showing success message $scope.status = "The Person Saved Successfully!!!";
                if (data != "No") {
                    $("#loading").hide();
                    alert("Payment Details has been saved successfully!");
                    $("#ReceivedAmount").val("");
                    $("#PaymentAmountInWords").val("");
                    $("#Remarks").val("");
                    $("#paymentDate").val("");
                    $("#ChequeNo").val("");
                    $("#chequeDate").val("");
                    $("#BankName").val("");
                    $("#BankBranch").val("");
                    $("#txtSendEmail").val("");
                    $("#btnSubmitPayment").addClass("disabled");

                    GetPaymentsInfo();
                    if ($("#chkPrint").is(":checked") == true)
                        window.open(data, '_blank');
                }
                else {
                    alert("Please provide correct details.");
                    $scope.Error = 'Unable to save Payment Details: ';
                }
                $('#btnSubmitPayment').show();
                $('#loading').hide();
                // $scope.Error = 'Payment Details saved successfully.';
            }).error(function (error) {
                alert("Payment Details Not Saved !!!");
                $scope.Error = 'Unable to save Payment Details: ' + error.message;
                $('#loading').hide();
                $('#btnSubmitPayment').show();
            });
        }
        // end save payment.
    }

    function GetPaymentsInfo() {
        // Get Payment Details
        $http({
            method: 'Get',
            url: '/Sale/Payment/GetPaymentList',
            params: { saleid: $scope.Sale.SaleID }
        }).success(function (data) {
            $scope.PaymentList = data;

            var total3 = 0;
            for (var i = 0; i < $scope.PaymentList.length; i++) {
                var list = $scope.PaymentList;
                total3 += list[i].Amount;
            }
            $scope.TotalPaid = total3;
            var total2 = 0;
            for (var i = 0; i < $scope.Install.length; i++) {
                var list = $scope.Install;
                total2 += list[i].TotalAmount;
            }
            $scope.TotalInstallmnetDueAmount = total2;
            var installCount = 0;
            var installLastCount = 0;
            var flag = "0", flag2 = "0";
            angular.forEach($scope.Install, function (value, index) {
                installCount += value.TotalAmount;

                if ($scope.TotalPaid < installCount) {
                    if (flag == "0") {
                        flag = "1";
                        $scope.ThisInstallmentNo = value.InstallmentID;
                        $scope.ThisInstallmentName = value.Installment;

                        $scope.DueDate = value.DueDateSt;
                        $scope.ThisInstallmentAmount = value.TotalAmount;
                        $scope.ThisInsTotalSum = installCount;
                        //  alert('yes' + $scope.ThisInsTotalSum);
                    }
                }
                else {
                    $scope.LastInstallmentTotal = installCount;
                    // alert('no' + $scope.LastInstallmentTotal);
                }

            }); // end loop
            if ($scope.LastInstallmentTotal < $scope.ThisInsTotalSum) {
                $scope.CurrentDue = $scope.LastInstallmentTotal + $scope.ThisInstallmentAmount - $scope.TotalPaid;
                // alert('not eq ' + $scope.CurrentDue)
            }
            else {
                $scope.CurrentDue = $scope.ThisInstallmentAmount - $scope.TotalPaid;
                // alert('==' + $scope.CurrentDue);
            }

            // Enable save button
            $("#btnSubmitPayment").removeClass("disabled");
            $("#loading").hide();
        })
    }
    $scope.orderSearchList = function (predicate, reverse) {
        $scope.SearchList = orderBy($scope.SearchList, predicate, reverse);
    };
    $scope.orderInstall1 = function (predicate, reverse) {
        $scope.Install = orderBy($scope.Install, predicate, reverse);
    };


    /// Search Payment Start
    $scope.SearchPayment0 = function () {
        $("#search1").val("0");
        $('#loading').show();
        var proID = $("#PropertyID").val();
        var proTID = $("#PropertyTypeID").val();
        var proSID = $("#PropertySizeID").val();
        var search1 = $("#search1").val();
        var searchBy = $("#searchby").val();
        var sortBy = $("#sortby").val();
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/Sale/Payment/SearchPayment',
            params: { PropertyID: proID, PropertyTypeID: proTID, PropertySizeID: proSID, PropertyTypeName: $("#PropertyTypeID :selected").text(), search1: search1, searchby: searchBy, sortby: sortBy, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
            dataType: "json"
        }).success(function (data) {
            $scope.SearchList = data;
            $('#loading').hide();
            var total = 0;
            for (var i = 0; i < $scope.SearchList.length; i++) {
                var list = $scope.SearchList[i];
                total += list.Amount;
            }
            $scope.TotalSearchAmount = total;
        });
    }
    $scope.EditPaymentInit = function () {
        GetLoadTowerFlat();
        $("#loading").hide();
    }
    $scope.EditSearchPayment = function () {
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Sale/Payment/GetSaleByFlatID',
            params: { flatid:$scope.Flat.FlatID }
        }).success(function (data) {
            $scope.Sale = data;
            $http({
                method: 'Get',
                contentType: "application/json; charset=utf-8",
                url: '/Sale/Payment/EditSearchPayment',
                params: { SaleID: data.SaleID },
                dataType: "json"
            }).success(function (data) {
                $scope.SearchList = data;
                $('#loading').hide();
                var total = 0;
                for (var i = 0; i < $scope.SearchList.length; i++) {
                    var list = $scope.SearchList[i];
                    total += list.Amount;
                }
                $scope.TotalSearchAmount = total;
            });
        })
       
    }
    // Search Paymenet End
})


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

    if ($("#DueAmount").val() == "") {
        vl = false;
        message += "Please insert Due Amount.<br/>";
    }
    if ($("#TotalAmount").val() == "") {
        vl = false;
        message += "Please insert Total Amount. <br/>";
    }
    if ($("#PaymentAmountInWords").val() == "") {
        vl = false;
        message += "Please insert Amount in words.";
    }

    $("#ErrorMessage").html(message);
    return vl;
}
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