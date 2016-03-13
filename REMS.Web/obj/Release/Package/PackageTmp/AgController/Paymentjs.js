
var myApp = angular.module('PaymentApp', []);
//Defining a Controller 
myApp.controller('PaymentController', function ($scope, $http, $filter) {
    // Get all Property list for dropdownlist
    $scope.Error = "";
    var orderBy = $filter('orderBy');

  //  $http.get('/flat/GetPropertyList/').success(function (response) { $scope.Properties = response; });
    $http.get('/Sale/Payment/GetBank/').success(function (response) { $scope.Banks = response; });
    $scope.inWords = function inWords(num) {
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

    $scope.RefundInit = function () {
        GetLoadTowerFlat();
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
        if ($scope.Flat.TowerID == "All")
        {
            $("#loading").hide();
        }
        else {
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetFlatByTowerID',
            params: { towerid: $scope.Flat.TowerID }
        }).success(function (data) {
            $scope.FlatList = data;
            $("#loading").hide();
        })
        }
    }

    // Search Payment Page.
    $scope.TowerChange3 = function () {
        $("#loading").show();
        if ($scope.Flat.TowerID == "All" || $scope.Flat.TowerID == "ReceiptNo" || $scope.Flat.TowerID == "Customer Name" || $scope.Flat.TowerID == "PaymentDate" || $scope.Flat.TowerID == "This Month" || $scope.Flat.TowerID=="Last 7 Days" ) {
            $("#loading").hide();
        }
        else {
            $http({
                method: 'Get',
                url: '/Admin/CreateProperty/GetFlatByTowerID',
                params: { towerid: $scope.Flat.TowerID }
            }).success(function (data) {
                $scope.FlatList = data;
                $("#loading").hide();
            })
        }
    }
    $scope.TowerChange2 = function () {
        $("#loading").show();
        if ($scope.Flat.TowerID == "All" || $scope.Flat.TowerID == "Customer Name" || $scope.Flat.TowerID == "BookingDate" || $scope.Flat.TowerID == "SaleDate" || $scope.Flat.TowerID == "This Month" || $scope.Flat.TowerID == "Last 7 Days") {
            $("#loading").hide();
        }
        else {
            $http({
                method: 'Get',
                url: '/Admin/CreateProperty/GetFlatByTowerID',
                params: { towerid: $scope.Flat.TowerID }
            }).success(function (data) {
                $scope.FlatList = data;
                $("#loading").hide();
            })
        }
    }

    $scope.EditPayment = function () {
        var pid = $("#TransactionID").val()
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Payment/GetPaymentbyTIDSt',
            params: { transactionid: pid }
        }).success(function (data, status, headers, config) {
            $scope.Payment = data;
            var ddate = new Date(parseInt(data.PaymentDate.substr(6)));
            var mth = ddate.getMonth() + 1;
            if (mth < 10) mth = "0" + mth;
            $scope.PaymentDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();
            $scope.inWords(data.Amount);
            $("#PaymentMode").find(":selected").text(data.PaymentMode);
            if (data.PaymentMode == "Cash" || data.PaymentMode == "Transfer Entry") {
                //orderInstall
                $("#divCheque").hide();
            }
            else {
                $("#divCheque").show();
               
                $("#BankName").find(":selected").text(data.BankName);
            }

            var SID = data.SaleID;
            $http({
                method: 'Get',
                url: '/Payment/GetFlatSaleBySaleID',
                params: { saleid: SID }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                $http({
                    method: 'Get',
                    url: '/Payment/GetTotalPayment',
                    params: { saleid: $scope.Sale.Sale[0].SaleID }
                }).success(function (data, status, headers, config) {
                    $scope.Amount = data;

                    // Get Payment Details
                    $http({
                        method: 'Get',
                        url: '/Payment/GetPaymentList',
                        params: { saleid: $scope.Sale.Sale[0].SaleID }
                    }).success(function (data) {
                        $scope.PaymentList = data;
                        var total3 = 0;
                        for (var i = 0; i < $scope.PaymentList.length; i++) {
                            var list = $scope.PaymentList;
                            total3 += list[i].Amount;
                        }
                        $scope.TotalPaid = total3;
                        var total2 = 0;                       

                        // Enable save button
                        $("#btnSubmitPayment").removeClass("disabled");
                        $("#loading").hide();
                    })
                });
            })
        });

    }
    $scope.PaymentClearanceInit = function () {
        var pid = $("#TransactionID").val()
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Payment/GetPaymentbyTIDSt',
            params: { transactionid: pid }
        }).success(function (data, status, headers, config) {
            $scope.Payment = data;          
            var SID = data.SaleID;
            $http({
                method: 'Get',
                url: '/Payment/GetFlatSaleBySaleID',
                params: { saleid: SID }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                $http({
                    method: 'Get',
                    url: '/Payment/GetTotalPayment',
                    params: { saleid: $scope.Sale.Sale[0].SaleID }
                }).success(function (data, status, headers, config) {
                    $scope.Amount = data;
                    $("#loading").hide();
                });
            })
        });

    }
    $scope.UpdatePayment = function () {
        var vli = ValidateEditPayment();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {
            if ($("#paymentDate").val() == "")
                $("#paymentDate").val($("#paymentDate").attr("placeholder"));
            if ($("#BankName").find(":selected").text() == "") {
                $("#BankName").find(":selected").text($("#BankName").attr("placeholder"));
            }
            $http({
                method: 'POST',
                url: '/Payment/UpdatePaymentDetails',
                data: { TransactionID: $("#TransactionID").val(), InstallmentNo: $("#ddlPayment").val(), Saleid: $("#SaleID").val(), Flatname: $("#FlatName").val(), DueAmount: $("#DueAmount").val(), DueDate: $("#DueDate").val(), PaymentMode: $("#PaymentMode").find(":selected").text(), ChequeNo: $("#ChequeNo").val(), ChequeDate: $("#chequeDate").val(), BankName: $("#BankName").find(":selected").text(), BankBranch: $("#BankBranch").val(), Remarks: $("#Remarks").val(), PayDate: $("#paymentDate").val(), Amtrcvdinwrds: $("#PaymentAmountInWords").val(), ReceivedAmount: $("#ReceivedAmount").val(), IsPrint: $("#IsReceipt").val() },
            }).success(function (data) { //Showing success message $scope.status = "The Person Saved Successfully!!!";
                alert("Payment Details has been updated successfully!");
                $scope.Error = 'Payment Details updated successfully.';
                $('#btnSubmitPayment').show();
                $('#loading').hide();

                // Get Payment Details
                $http({
                    method: 'Get',
                    url: '/Payment/GetPaymentList',
                    params: { saleid: $scope.Sale.Sale[0].SaleID }
                }).success(function (data) {
                    $scope.PaymentList = data;
                    var total3 = 0;
                    for (var i = 0; i < $scope.PaymentList.length; i++) {
                        var list = $scope.PaymentList;
                        total3 += list[i].Amount;
                    }
                    $scope.TotalPaid = total3;
                    var total2 = 0;

                    // Enable save button
                    $("#btnSubmitPayment").removeClass("disabled");
                    $("#loading").hide();
                })

            }).error(function (error) {
                alert("Unable to updated Payment Details !!!");
                $scope.Error = 'Unable to updated Payment Details: ' + error.message;
                $('#btnSubmitPayment').show();
                $('#loading').hide();
            });
        }
    }
    $scope.UpdateClearance = function () {
        $("#loading").show();
        $http({
            method: 'POST',
            url: '/Sale/Payment/UpdateClearanceDetails',
            data: { TransactionID: $("#TransactionID").val(), ChargeAmount: $("#ClearAmt").val(), Remarks: $("#Remarks").val(), ClearanceDate: $("#ClearanceDate").val(), IsBounce: $("#isBounce").is(":checked") },
        }).success(function (data) { //Showing success message $scope.status = "The Person Saved Successfully!!!";
            if (data == 'Yes') {
                alert("Payment Clearance has been updated successfully!");
                $scope.Error = 'Payment Clearance updated successfully.';
                $("#loading").hide();
                location.reload();
            }
            else {
                alert("Payment Clearance has not been updated successfully!");
                $scope.Error = 'Payment Clearance has not been updated successfully! ';
                $("#loading").hide();
            }
        }).error(function (error) {
            alert("Unable to updated Payment Clearance !!!");
            $scope.Error = 'Unable to updated Payment Clearance: ' + error.message;
            $("#loading").hide();
        });
    }
    $scope.SearchFlat = function () {
        $scope.Error = "";
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
       
        $scope.ThisInstallmentAmount = 500000;
        $scope.ThisInsTotalSum = 500000;
        $http({
            method: 'Get',
            url: '/Payment/GetFlatList',
            params: {pid: pid }
        }).success(function (data, status, headers, config) {
            $scope.Flat = data;
            if (data.Result.length == 0) {
                $scope.Error = "Property flat not found.";
                alert("Property flat not found.");
                $('#loading').hide();
                return;
            } else {
                var FID = $scope.Flat.Result[0].FlatID;
            }
            $http({
                method: 'Get',
                url: '/Payment/GetFlatSale',
                params: { flatid: FID }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                $("#btnSubmitPayment").removeClass("disabled");
                if (data.Sale[0].PaymentFor != "Sale") {
                    // This is Booking.
                    $scope.ThisInstallmentNo = "Advance Booking Amount";
                    $scope.ThisInstallmentName = "Advance Booking Amount";
                    var ddate = new Date();//parseInt(value.DueDate.substr(6)));// value.DueDate;
                    var mth = ddate.getMonth() + 1;
                    if (mth < 10) mth = "0" + mth;
                    $scope.DueDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();

                    $scope.ThisInstallmentAmount = 500000;
                    $scope.ThisInsTotalSum = 500000;
                }
                $scope.SaleID = $scope.Sale.Sale[0].SaleID;
                $http({
                    method: 'Get',
                    contentType: "application/json; charset=utf-8",
                    url: '/Payment/EditSearchPayment',
                    params: { Flatid: pid },
                    dataType: "json"
                }).success(function (data) {
                    $scope.SearchList = data;
                   
                    var total = 0;
                    for (var i = 0; i < $scope.SearchList.length; i++) {
                        var list = $scope.SearchList[i];
                        total += list.Amount;
                    }
                    $scope.TotalSearchAmount = total;
                    // Get total receive amount
                    $http({  // 
                        method: 'Get',
                        url: '/OtherPayment/GetOtherPaymentMaster',
                        params: {}
                    }).success(function (data, status, headers, config) {
                        $scope.PaymentMaster = data;
                        $http({
                            method: 'Get',
                            url: '/Payment/GetTotalPayment',
                            params: { saleid: $scope.Sale.Sale[0].SaleID }
                        }).success(function (data, status, headers, config) {
                            $scope.Amount = data;
                            $http({
                                method: 'Get',
                                url: '/Payment/GetInstallmentForDue',
                               // params: { saleid: $scope.Sale.Sale[0].SaleID }
                                params: { saleid: pid }
                            }).success(function (data, status, headers, config) {
                                $http({
                                    method: 'Get',
                                    url: '/Customer/Info/GetCustomerByCustID',
                                    params: { saleid: $scope.Sale.Sale[0].CustomerID }
                                }).success(function (data, status, headers, config) {
                                    $scope.Customer = data;
                                    $scope.CustomerName = data.AppTitle + " " + data.FName + " " + data.MName + " " + data.LName;
                                }).then(function () {
                                    $scope.Install = data;
                                    var total2 = 0;
                                    for (var i = 0; i < $scope.Install.length; i++) {
                                        var list = $scope.Install;
                                        total2 += list[i].DueAmount;
                                    }
                                    $scope.TotalInstallmnetDueAmount = total2;
                                    var installCount = 0;
                                    var installLastCount = 0;
                                    var flag = "0", flag2 = "0";

                                    angular.forEach($scope.Install, function (value, index) {
                                        installCount += value.DueAmount;

                                        if ($scope.Amount.TotalPaid < installCount) {
                                            //if (flag2 == "0") {
                                            //    flag2 = "1"; flag3 = "1";
                                            //    installLastCount += value.DueAmount;
                                            //    $scope.LastInstallmentTotal = installLastCount
                                            //    }
                                            if (flag == "0") {
                                                flag = "1";
                                                $scope.ThisInstallmentNo = value.InstallmentNo;
                                                $scope.ThisInstallmentName = value.InstallmentNo;
                                                if (value.DueDate != null) {
                                                    // var ddate = new Date(parseInt(value.DueDate.substr(6)));// value.DueDate;
                                                    // var mth = ddate.getMonth() + 1;
                                                    // if (mth < 10) mth = "0" + mth;
                                                    $scope.DueDate = value.DueDateST;// ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();
                                                }
                                                $scope.ThisInstallmentAmount = value.DueAmount;
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
                                        $scope.CurrentDue = $scope.LastInstallmentTotal + $scope.ThisInstallmentAmount - $scope.Amount.TotalPaid;
                                        // alert('not eq ' + $scope.CurrentDue)
                                    }
                                    else {
                                        $scope.CurrentDue = $scope.ThisInstallmentAmount - $scope.Amount.TotalPaid;
                                        // alert('==' + $scope.CurrentDue);
                                    }
                                    //$('#loading').hide();

                                })


                            })
                            //$('#loading').hide();
                        }).then(function () {
                            $http({  // 
                                method: 'Get',
                                url: '/OtherPayment/GetOPaymentServiceTaxCharged',
                                params: { saleid: $scope.SaleID }
                            }).success(function (data, status, headers, config) {
                                $scope.STPayment = data;
                                var total = 0;
                                var stotal = 0;
                                for (var i = 0; i < $scope.STPayment.length; i++) {
                                    var list = $scope.STPayment[i];
                                    total += list.Amount;
                                    stotal += list.ServiceTaxAmount;
                                }
                                $scope.STTotalAmount = total;
                                $scope.STTotalServiceTaxAmount = stotal;
                            }).then(function () {
                                $http({  // 
                                    method: 'Get',
                                    url: '/OtherPayment/GetOPaymentLatePaymentCharged',
                                    params: { saleid: $scope.SaleID }
                                }).success(function (data, status, headers, config) {
                                    $scope.LPPayment = data;
                                    var total = 0;
                                    var ltotal = 0;
                                    for (var i = 0; i < $scope.LPPayment.length; i++) {
                                        var list = $scope.LPPayment[i];
                                        total += list.ReceiveAmount;
                                        ltotal += list.InterestAmount;
                                    }
                                    $scope.LPTotalAmount = total;
                                    $scope.LPTotalInterestAmount = ltotal;
                                }).then(function () {
                                    $http({  // 
                                        method: 'Get',
                                        url: '/OtherPayment/GetOPaymentPropertyTransferCharged',
                                        params: { saleid: $scope.SaleID }
                                    }).success(function (data, status, headers, config) {
                                        $scope.TFPayment = data;
                                        var total = 0;
                                        for (var i = 0; i < $scope.TFPayment.length; i++) {
                                            var list = $scope.TFPayment[i];
                                            total += list.Amount;
                                        }
                                        $scope.TFTotalAmount = total;
                                    }).then(function () {
                                        $http({  // 
                                            method: 'Get',
                                            url: '/OtherPayment/GetOPaymentClearanceCharged',
                                            params: { saleid: $scope.SaleID }
                                        }).success(function (data, status, headers, config) {
                                            $scope.CCPayment = data;
                                            var total = 0;
                                            var ctotal = 0;
                                            for (var i = 0; i < $scope.CCPayment.length; i++) {
                                                var list = $scope.CCPayment[i];
                                                total += list.Amount;
                                                ctotal += list.ClearanceCharge;
                                            }
                                            $scope.CCTotalAmount = total;
                                            $scope.CCTotalClearanceCharge = ctotal;
                                            $('#loading').hide();
                                            //$http({
                                            //    method: 'Get',
                                            //    url: '/Payment/GetOtherPaymentGroupedBySaleID',
                                            //    params: { sid: $scope.Sale.Sale[0].SaleID }
                                            //}).success(function (data, status, headers, config) {
                                            //    $scope.GTotalAmount = data;
                                            //    $('#loading').hide();
                                            //    var gtotal = 0;
                                            //    for (var i = 0; i < $scope.GTotalAmount.GroupPay.length; i++) {
                                            //        var list = $scope.GTotalAmount.GroupPay[i];
                                            //        gtotal += list.TAmount;
                                            //    }
                                            //    $scope.GrantTotalAmount = gtotal;
                                            //})
                                        })
                                    })
                                })
                            })
                        })
                    })
                })
            })
        });

    }
    // Change Property Name and fill propertyType.



    $scope.PropertyName = function () {

        var PID = $("#PropertyID").find(":selected").val();
        $http({
            method: 'Get',
            url: '/flat/GetPropertyTypeList/',
            params: { pid: PID }
        }).success(function (data, status, headers, config) {
            $scope.PropertyTypes = data;
            $http({
                method: 'Get',
                url: '/Admin/Property/GetPropertySizeList',
                params: { propertyid: PID }
            }).success(function (data, status, headers, config) {
                $scope.PropertySizes = data;
            });
        });
        $("#PropertySizeID").find('option')
    .remove()
    .end()
    .append('<option value="All" selected>All</option>')
        //$http.get('/flat/GetPropertyTypeList/').success(function (response) { $scope.PropertyTypes = response; });
    }





    // Change Proeprty Type and fill property size
    $scope.PropertyType = function () {
        $("#protypename").val($("#PropertyTypeID :selected").text());

        $http({
            method: 'Get',
            url: '/Admin/Property/GetPropertySizeListByPTypeID/',
            params: { propertyid: $scope.newCust.PropertyTypeID }
        }).success(function (data, status, headers, config) {
            $scope.PropertySizes = data;
        });
    }
    // Get all Property list for dropdownlist
    //$http.get('/flat/GetPropertyList/').success(function (response) { $scope.Properties = response; });
    // Save Payment
    $scope.SavePayment = function () {
        var vli = ValidateSubmitPayment();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {
            $("#loading").show();

            $.ajax({
                method: 'POST',
                url: '/Payment/InsertPaymentDetails',
                data: { InstallmentNo: $("#ddlPayment").find(":selected").val(), Saleid: $("#SaleID").val(), Flatname: $("#FlatName").val(), DueAmount: $("#DueAmount").val(), DueDate: $("#DueDate").val(), PaymentMode: $("#PaymentMode").val(), ChequeNo: $("#ChequeNo").val(), ChequeDate: $("#chequeDate").val(), BankName: $("#BankName").find(":selected").text(), BankBranch: $("#BankBranch").val(), Remarks: $("#Remarks").val(), PayDate: $("#paymentDate").val(), Amtrcvdinwrds: $("#PaymentAmountInWords").val(), ReceivedAmount: $("#ReceivedAmount").val(), IsPrint: $("#chkPrint").is(":checked"), IsEmailSent: $("#chkSendEmail").is(":checked"), EmailTo: $("#txtSendEmail").val(), CustomerName: $("#hidCustomerName").val(), CustomerID: $("#hidCustomerID").val(), chkInterest: $("#chkInterest").is(":checked") },
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

                    $scope.SearchFlat();
                    //SearchFlats();
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
    $scope.SaveOtherPayment = function () {
        var vli = ValidateSubmitPayment();
        if (vli == false) {
            $('#loading').hide();
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            $.ajax({
                method: 'POST',
                url: '/Payment/InsertOtherPayments',
                data: { InstallmentNo: $("#ddlPayment").find(":selected").text(), Saleid: $("#SaleID").val(), Flatname: $("#FlatName").val(), PaymentMode: $("#PaymentMode").val(), ChequeNo: $("#ChequeNo").val(), ChequeDate: $("#chequeDate").val(), BankName: $("#BankName").find(":selected").text(), BankBranch: $("#BankBranch").val(), Remarks: $("#Remarks").val(), PayDate: $("#paymentDate").val(), Amtrcvdinwrds: $("#PaymentAmountInWords").val(), ReceivedAmount: $("#ReceivedAmount").val(), IsPrint: $("#chkPrint").is(":checked"), IsEmailSent: $("#chkSendEmail").is(":checked"), EmailTo: $("#txtSendEmail").val(), CustomerName: $("#hidCustomerName").val(), CustomerID: $("#hidCustomerID").val() },
            }).success(function (data) { //Showing success message $scope.status = "The Person Saved Successfully!!!";
                if (data != "No") {
                    alert("Payment Details has been saved successfully!");
                    OPaymentConfirm();
                    if ($("#chkPrint").is(":checked") == true)
                        window.open(data, '_blank');
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
        // end save payment.
    }


    // Search Book to sale
    $scope.SearchBookToSale = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
        //var ptypeName = $("#protypename").val();
        //var proeprtyid = $("#PropertyID").val();
        //var ptype = $("#PropertyTypeID").val();
        //var psize = $("#PropertySizeID").val();
        var search = $("#searchby").val();
       
        $http({
            method: 'Get',
            url: '/Sale/BooK/BookingSearch',
            params: { FlatId: pid, Search: search }
        }).success(function (data) {
            $scope.SearchList = data;
            var total = 0;
            for (var i = 0; i < $scope.SearchList.length; i++) {
                var list = $scope.SearchList[i];
                total += list.BookingAmount;
            }
            $scope.TotalBookingAmount = total;
            $('#loading').hide();
        }).error(function () {
        });
    }
    $scope.SearchPayment = function () {
        $("#search1").val("1");
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
            url: '/Payment/SearchPayment',
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
    $scope.SearchPayment0 = function () {
        $("#search1").val("0");
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
        //var proID = $("#PropertyID").val();
        //var proTID = $("#PropertyTypeID").val();
        //var proSID = $("#PropertySizeID").val();
        //var search1 = $("#search1").val();
        var search = $("#searchby").val();
        var sortBy = $("#sortby").val();
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/Sale/Payment/SearchPayment',
            params: { search: search,Flatid: pid, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
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
    $scope.EditSearchPayment = function () {
        $('#loading').show();
        var proID = $("#PropertyID").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/Payment/EditSearchPayment',
            params: { PropertyID: proID, searchtext: searchtext },
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
    $scope.SearchProperty = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
        //var propertyName = $("#PropertyTypeID :selected").text()
        var search = $("#searchby").val();
        //var propertyid = $("#PropertyID").val();
        //var propertySubTypeID = $("#PropertyTypeID").val();
        //var proSize = $("#PropertySizeID").val();
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/Home/PropertySearch',
            params: { search: search, FlatId: pid,datefrom: datefrom, dateto: dateto, searchtext: searchtext },
            dataType: "json"
        }).success(function (data) {
            $scope.SearchList = data;
            var total = 0;
            for (var i = 0; i < $scope.SearchList.length; i++) {
                var list = $scope.SearchList[i];
                total += list.SaleRate;
            }
            $scope.TotalSaleAmount = total;
            $('#loading').hide();
        });
    }

    function ValidateEditPayment() {
        var vl = true;
        var message = "";
        if ($("#ReceivedAmount").val() == "") {
            vl = false;
            message += "Insert receive amount. <br/>";
        }
        if ($("#PaymentMode").find(":selected").text() == "") {
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

        if ($("#PaymentAmountInWords").val() == "") {
            vl = false;
            message += "Please insert Amount in words.";
        }

        $("#ErrorMessage").html(message);
        return vl;
    }
    function SearchFlats() {
        // $("#FlatID").val(FName);
        // var PID = $("#PropertyID").val();


        var FName = $scope.Flat.Result[0].FlatName;
        var PID = $scope.PropertyID;
        // alert(PID+" "+FName);
        $http({
            method: 'Get',
            url: '/Payment/GetFlatList',
            params: { flatname: FName, pid: PID }
        }).success(function (data, status, headers, config) {
            $scope.Flat = data;
            if (data.Result.length == 0) $scope.Error = "Property flat not found.";
            var FID = $scope.Flat.Result[0].FlatID;
            $http({
                method: 'Get',
                url: '/Payment/GetFlatSale',
                params: { flatid: FID }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                $("#btnSubmitPayment").removeClass("disabled");
                if (data.Sale[0].PaymentFor != "Sale") {
                    // This is Booking.
                    $scope.ThisInstallmentNo = "Advance Booking Amount";
                    $scope.ThisInstallmentName = "Advance Booking Amount";
                    var ddate = new Date();//parseInt(value.DueDate.substr(6)));// value.DueDate;
                    var mth = ddate.getMonth() + 1;
                    if (mth < 10) mth = "0" + mth;
                    $scope.DueDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();

                    $scope.ThisInstallmentAmount = 500000;
                    $scope.ThisInsTotalSum = 500000;
                }
                $scope.SaleID = $scope.Sale.Sale[0].SaleID;
                $http({
                    method: 'Get',
                    contentType: "application/json; charset=utf-8",
                    url: '/Payment/EditSearchPayment',
                    params: { PropertyID: PID, searchtext: FName },
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
                    $http({
                        method: 'Get',
                        url: '/Payment/GetTotalPayment',
                        params: { saleid: $scope.Sale.Sale[0].SaleID }
                    }).success(function (data, status, headers, config) {
                        $scope.Amount = data;
                        $http({
                            method: 'Get',
                            url: '/Payment/GetInstallmentForDue',
                            params: { saleid: $scope.Sale.Sale[0].SaleID }
                        }).success(function (data, status, headers, config) {
                            $http({
                                method: 'Get',
                                url: '/Customer/Info/GetCustomerByCustID',
                                params: { saleid: $scope.Sale.Sale[0].CustomerID }
                            }).success(function (data, status, headers, config) {
                                $scope.Customer = data;
                                $scope.CustomerName = data.AppTitle + " " + data.FName + " " + data.MName + " " + data.LName;
                            }).then(function () {
                                $scope.Install = data;
                                var total2 = 0;
                                for (var i = 0; i < $scope.Install.length; i++) {
                                    var list = $scope.Install[i];
                                    total2 += list[i].DueAmount;
                                }
                                $scope.TotalInstallmnetDueAmount = total2;
                                var installCount = 0;
                                var installLastCount = 0;
                                var flag = "0", flag2 = "0";

                                angular.forEach($scope.Install.Installments, function (value, index) {
                                    installCount += value.DueAmount;

                                    if ($scope.Amount.TotalPaid < installCount) {
                                        //if (flag2 == "0") {
                                        //    flag2 = "1"; flag3 = "1";
                                        //    installLastCount += value.DueAmount;
                                        //    $scope.LastInstallmentTotal = installLastCount
                                        //    }
                                        if (flag == "0") {
                                            flag = "1";
                                            $scope.ThisInstallmentNo = value.InstallmentNo;
                                            $scope.ThisInstallmentName = value.InstallmentNo;
                                            if (value.DueDate != null) {
                                                var ddate = new Date(parseInt(value.DueDate.substr(6)));// value.DueDate;
                                                var mth = ddate.getMonth() + 1;
                                                if (mth < 10) mth = "0" + mth;
                                                $scope.DueDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();
                                            }
                                            $scope.ThisInstallmentAmount = value.DueAmount;
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
                                    $scope.CurrentDue = $scope.LastInstallmentTotal + $scope.ThisInstallmentAmount - $scope.Amount.TotalPaid;
                                    // alert('not eq ' + $scope.CurrentDue)
                                }
                                else {

                                    $scope.CurrentDue = $scope.ThisInstallmentAmount - $scope.Amount.TotalPaid;
                                    // alert('==' + $scope.CurrentDue);
                                }

                            })

                        })
                    })
                })

            })
        });
        $("#ReceivedAmount").val("");
        $("#PaymentAmountInWords").val("");
        $("#Remarks").val("");
        $("#paymentDate").val("");
        $("#ChequeNo").val("");
        $("#chequeDate").val("");
        $("#BankName").val("");
        $("#BankBranch").val("");
        $("#txtSendEmail").val("");

    }
    function OPaymentConfirm() {
        // $("#FlatID").val(FName);
        // var PID = $("#PropertyID").val();
        var FName = $scope.Flat.Result[0].FlatName;
        var PID = $scope.PropertyID;
        $http({
            method: 'Get',
            url: '/Payment/GetFlatList',
            params: { flatname: FName, pid: PID }
        }).success(function (data, status, headers, config) {
            $scope.Flat = data;
            if (data.Result.length == 0) $scope.Error = "Property flat not found.";
            var FID = $scope.Flat.Result[0].FlatID;
            $http({
                method: 'Get',
                url: '/Payment/GetFlatSale',
                params: { flatid: FID }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                $("#btnSubmitPayment").removeClass("disabled");
                if (data.Sale[0].PaymentFor != "Sale") {
                    // This is Booking.
                    $scope.ThisInstallmentNo = "Advance Booking Amount";
                    $scope.ThisInstallmentName = "Advance Booking Amount";
                    var ddate = new Date();//parseInt(value.DueDate.substr(6)));// value.DueDate;
                    var mth = ddate.getMonth() + 1;
                    if (mth < 10) mth = "0" + mth;
                    $scope.DueDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();

                    $scope.ThisInstallmentAmount = 500000;
                    $scope.ThisInsTotalSum = 500000;
                }
                $scope.SaleID = $scope.Sale.Sale[0].SaleID;
                $http({  // 
                    method: 'Get',
                    url: '/Payment/GetPayment',
                    params: { saleid: $scope.Sale.Sale[0].SaleID }
                }).success(function (data, status, headers, config) {
                    $scope.Payment = data;
                    // Get total receive amount
                    $http({
                        method: 'Get',
                        url: '/Payment/GetTotalPayment',
                        params: { saleid: $scope.Sale.Sale[0].SaleID }
                    }).success(function (data, status, headers, config) {
                        $scope.Amount = data;
                        $http({
                            method: 'Get',
                            url: '/Payment/GetInstallmentForDue',
                            params: { saleid: $scope.Sale.Sale[0].SaleID }
                        }).success(function (data, status, headers, config) {
                            $http({
                                method: 'Get',
                                url: '/Customer/Info/GetCustomerBySaleID',
                                params: { saleid: $scope.Sale.Sale[0].SaleID }
                            }).success(function (data, status, headers, config) {
                                $scope.Customer = data;
                                $scope.CustomerName = data.AppTitle + " " + data.FName + " " + data.MName + " " + data.LName;
                            }).then(function () {
                                $scope.Install = data;
                                var installCount = 0;
                                var installLastCount = 0;
                                var flag = "0", flag2 = "0";
                                angular.forEach($scope.Install.Installments, function (value, index) {
                                    installCount += value.DueAmount;
                                    if ($scope.Amount.TotalPaid < installCount) {
                                        //if (flag2 == "0") {
                                        //    flag2 = "1"; flag3 = "1";
                                        //    installLastCount += value.DueAmount;
                                        //    $scope.LastInstallmentTotal = installLastCount
                                        //    }
                                        if (flag == "0") {
                                            flag = "1";
                                            $scope.ThisInstallmentNo = value.InstallmentNo;
                                            $scope.ThisInstallmentName = value.InstallmentNo;
                                            var ddate = new Date(parseInt(value.DueDate.substr(6)));// value.DueDate;
                                            var mth = ddate.getMonth() + 1;
                                            if (mth < 10) mth = "0" + mth;
                                            $scope.DueDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();
                                            $scope.ThisInstallmentAmount = value.DueAmount;
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
                                    $scope.CurrentDue = $scope.LastInstallmentTotal + $scope.ThisInstallmentAmount - $scope.Amount.TotalPaid;
                                    // alert('not eq ' + $scope.CurrentDue)
                                }
                                else {

                                    $scope.CurrentDue = $scope.ThisInstallmentAmount - $scope.Amount.TotalPaid;
                                    // alert('==' + $scope.CurrentDue);
                                }
                            }).then(function () {
                                $http({  // 
                                    method: 'Get',
                                    url: '/OtherPayment/GetOPaymentServiceTaxCharged',
                                    params: { saleid: $scope.SaleID }
                                }).success(function (data, status, headers, config) {
                                    $scope.STPayment = data;
                                    var total = 0;
                                    var stotal = 0;
                                    for (var i = 0; i < $scope.STPayment.length; i++) {
                                        var list = $scope.STPayment[i];
                                        total += list.Amount;
                                        stotal += list.ServiceTaxAmount;
                                    }
                                    $scope.STTotalAmount = total;
                                    $scope.STTotalServiceTaxAmount = stotal;
                                }).then(function () {
                                    $http({  // 
                                        method: 'Get',
                                        url: '/OtherPayment/GetOPaymentLatePaymentCharged',
                                        params: { saleid: $scope.SaleID }
                                    }).success(function (data, status, headers, config) {
                                        $scope.LPPayment = data;
                                        var total = 0;
                                        var ltotal = 0;
                                        for (var i = 0; i < $scope.LPPayment.length; i++) {
                                            var list = $scope.LPPayment[i];
                                            total += list.ReceiveAmount;
                                            ltotal += list.InterestAmount;
                                        }
                                        $scope.LPTotalAmount = total;
                                        $scope.LPTotalInterestAmount = ltotal;
                                    }).then(function () {
                                        $http({  // 
                                            method: 'Get',
                                            url: '/OtherPayment/GetOPaymentPropertyTransferCharged',
                                            params: { saleid: $scope.SaleID }
                                        }).success(function (data, status, headers, config) {
                                            $scope.TFPayment = data;
                                            var total = 0;
                                            for (var i = 0; i < $scope.TFPayment.length; i++) {
                                                var list = $scope.TFPayment[i];
                                                total += list.Amount;
                                            }
                                            $scope.TFTotalAmount = total;
                                        }).then(function () {
                                            $http({  // 
                                                method: 'Get',
                                                url: '/OtherPayment/GetOPaymentClearanceCharged',
                                                params: { saleid: $scope.SaleID }
                                            }).success(function (data, status, headers, config) {
                                                $scope.CCPayment = data;
                                                var total = 0;
                                                var ctotal = 0;
                                                for (var i = 0; i < $scope.CCPayment.length; i++) {
                                                    var list = $scope.CCPayment[i];
                                                    total += list.Amount;
                                                    ctotal += list.ClearanceCharge;
                                                }
                                                $scope.CCTotalAmount = total;
                                                $scope.CCTotalClearanceCharge = ctotal;
                                                $http({
                                                    method: 'Get',
                                                    url: '/Payment/GetOtherPaymentGroupedBySaleID',
                                                    params: { sid: $scope.Sale.Sale[0].SaleID }
                                                }).success(function (data, status, headers, config) {
                                                    $scope.GTotalAmount = data;
                                                    $('#loading').hide();
                                                    var gtotal = 0;
                                                    for (var i = 0; i < $scope.GTotalAmount.GroupPay.length; i++) {
                                                        var list = $scope.GTotalAmount.GroupPay[i];
                                                        gtotal += list.TAmount;
                                                    }
                                                    $scope.GrantTotalAmount = gtotal;
                                                })
                                            })
                                        })
                                    })
                                })
                            })

                        })
                    })
                })
            })
        });
        $("#ReceivedAmount").val("");
        $("#PaymentAmountInWords").val("");
        $("#Remarks").val("");
        $("#paymentDate").val("");
        $("#ChequeNo").val("");
        $("#chequeDate").val("");
        $("#BankName").val("");
        $("#BankBranch").val("");
        $("#txtSendEmail").val("");
    }
    $("#tblST").hide(); $("#tblTF").hide(); $("#tblLP").hide(); $("#tblCC").hide();
    $scope.OtherPaymentChange2 = function () {
        var payfor = $scope.PaymentFor
        var paymentfor = "";
        var tblST = $("#tblST");
        var tblTF = $("#tblTF");
        var tblLP = $("#tblLP");
        var tblCC = $("#tblCC");
        if (payfor == 1)  // Service Tax
        {
            $("#paymenthead").text("Service Tax Payment Details")
            tblST.show(); tblTF.hide(); tblLP.hide(); tblCC.hide();
            paymentfor = "Service Tax";
            $http({  // 
                method: 'Get',
                url: '/OtherPayment/GetOPaymentServiceTaxCharged',
                params: { saleid: $("#SaleID").val() }
            }).success(function (data, status, headers, config) {
                $scope.STPayment = data;
                var total = 0;
                var stotal = 0;
                for (var i = 0; i < $scope.STPayment.length; i++) {
                    var list = $scope.STPayment[i];
                    total += list.Amount;
                    stotal += list.ServiceTaxAmount;
                }
                $scope.STTotalAmount = total;
                $scope.STTotalServiceTaxAmount = stotal;
            });
        }
        else if (payfor == 2)  // Late Payment Charges
        {
            $("#paymenthead").text("Late Payment Charges Payment Details")
            tblST.hide(); tblTF.hide(); tblLP.show(); tblCC.hide();
            paymentfor = "Late Payment Charges";
            $http({  // 
                method: 'Get',
                url: '/OtherPayment/GetOPaymentLatePaymentCharged',
                params: { saleid: $("#SaleID").val() }
            }).success(function (data, status, headers, config) {
                $scope.LPPayment = data;
                var total = 0;
                var ltotal = 0;
                for (var i = 0; i < $scope.LPPayment.length; i++) {
                    var list = $scope.LPPayment[i];
                    total += list.ReceiveAmount;
                    ltotal += list.InterestAmount;
                }
                $scope.LPTotalAmount = total;
                $scope.LPTotalInterestAmount = ltotal;
            });
        }
        else if (payfor == 3)  // Transfer Fee
        {
            $("#paymenthead").text("Transfer Fee Payment Details")
            tblST.hide(); tblTF.show(); tblLP.hide(); tblCC.hide();
            paymentfor = "Transfer Fee";
            $http({  // 
                method: 'Get',
                url: '/OtherPayment/GetOPaymentPropertyTransferCharged',
                params: { saleid: $("#SaleID").val() }
            }).success(function (data, status, headers, config) {
                $scope.TFPayment = data;
                var total = 0;
                for (var i = 0; i < $scope.TFPayment.length; i++) {
                    var list = $scope.TFPayment[i];
                    total += list.Amount;
                }
                $scope.TFTotalAmount = total;
            });
        }
        else if (payfor == 4)  // Misc Amount
        {
            $("#paymenthead").text("Misc Amount Payment Details")
            tblST.hide(); tblTF.hide(); tblLP.hide(); tblCC.hide();
            paymentfor = "Misc Amount";
        }
        else if (payfor == 5)  //Clearance Charges
        {
            $("#paymenthead").text("Clearance Charges Payment Details")
            tblST.hide(); tblTF.hide(); tblLP.hide(); tblCC.show();
            paymentfor = "Clearance Charges";
            $http({  // 
                method: 'Get',
                url: '/OtherPayment/GetOPaymentClearanceCharged',
                params: { saleid: $("#SaleID").val() }
            }).success(function (data, status, headers, config) {
                $scope.CCPayment = data;
                var total = 0;
                var ctotal = 0;
                for (var i = 0; i < $scope.CCPayment.length; i++) {
                    var list = $scope.CCPayment[i];
                    total += list.Amount;
                    ctotal += list.ClearanceCharge;
                }
                $scope.CCTotalAmount = total;
                $scope.CCTotalClearanceCharge = ctotal;
            });
        }
        $http({  // 
            method: 'Get',
            url: '/OtherPayment/GetOtherPaymentBySaleIDPaymentFor',
            params: { saleid: $("#SaleID").val(), paymentfor: paymentfor }
        }).success(function (data, status, headers, config) {
            $scope.OPayment = data;
            var total = 0;
            for (var i = 0; i < $scope.OPayment.length; i++) {
                var list = $scope.OPayment[i];
                total += list.Amount;
            }
            $scope.OTotalSearchAmount = total;
        });
    }
    $scope.GetPropertyPayment = function () {
        var sid = $("#hidSaleID").val();
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Payment/GetPidProptyname',
            params: { saleid: sid }
        }).success(function (data, status, headers, config) {
            if (data.Result.length == 0) {
                $('#loading').hide();
                return;
            }
            $scope.seachFlat = data;
            $scope.Error = "";
            var FName = $scope.seachFlat.Result[0].FlatName;
            if (FName == undefined) {
                var FName = $scope.seachFlat.Result[0].FlatName;
            }
            var PID = $scope.seachFlat.Result[0].Pid;
            $("#PropertyID").val($scope.seachFlat.Result[0].Pid);
            //if (PID == "undefined") {
            //    PID = $("#PropertyID").find(":selected").val();
            //    $scope.PropertyID = PID;
            //}
            $scope.ThisInstallmentAmount = 500000;
            $scope.ThisInsTotalSum = 500000;
            $http({
                method: 'Get',
                url: '/Payment/GetFlatList',
                params: { flatname: FName, pid: PID }
            }).success(function (data, status, headers, config) {
                $scope.Flat = data;
                if (data.Result.length == 0) {
                    $scope.Error = "Property flat not found.";
                    alert("Property flat not found.");
                    $('#loading').hide();
                    return;
                } else {
                    var FID = $scope.Flat.Result[0].FlatID;
                }
                $http({
                    method: 'Get',
                    url: '/Payment/GetFlatSale',
                    params: { flatid: FID }
                }).success(function (data, status, headers, config) {
                    $scope.Sale = data;
                    $("#btnSubmitPayment").removeClass("disabled");
                    if (data.Sale[0].PaymentFor != "Sale") {
                        // This is Booking.
                        $scope.ThisInstallmentNo = "Advance Booking Amount";
                        $scope.ThisInstallmentName = "Advance Booking Amount";
                        var ddate = new Date();//parseInt(value.DueDate.substr(6)));// value.DueDate;
                        var mth = ddate.getMonth() + 1;
                        if (mth < 10) mth = "0" + mth;
                        $scope.DueDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();

                        $scope.ThisInstallmentAmount = 500000;
                        $scope.ThisInsTotalSum = 500000;
                    }
                    $scope.SaleID = $scope.Sale.Sale[0].SaleID;
                    $http({
                        method: 'Get',
                        contentType: "application/json; charset=utf-8",
                        url: '/Payment/EditSearchPayment',
                        params: { PropertyID: PID, searchtext: FName },
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
                        // Get total receive amount
                        $http({  // 
                            method: 'Get',
                            url: '/OtherPayment/GetOtherPaymentMaster',
                            params: {}
                        }).success(function (data, status, headers, config) {
                            $scope.PaymentMaster = data;
                            $http({
                                method: 'Get',
                                url: '/Payment/GetTotalPayment',
                                params: { saleid: $scope.Sale.Sale[0].SaleID }
                            }).success(function (data, status, headers, config) {
                                $scope.Amount = data;
                                $http({
                                    method: 'Get',
                                    url: '/Payment/GetInstallmentForDue',
                                    params: { saleid: $scope.Sale.Sale[0].SaleID }
                                }).success(function (data, status, headers, config) {
                                    $http({
                                        method: 'Get',
                                        url: '/Customer/Info/GetCustomerByCustID',
                                        params: { saleid: $scope.Sale.Sale[0].CustomerID }
                                    }).success(function (data, status, headers, config) {
                                        $scope.Customer = data;
                                        $scope.CustomerName = data.AppTitle + " " + data.FName + " " + data.MName + " " + data.LName;
                                    }).then(function () {
                                        $scope.Install = data;
                                        var total2 = 0;
                                        for (var i = 0; i < $scope.Install.length; i++) {
                                            var list = $scope.Install;
                                            total2 += list[i].DueAmount;
                                        }
                                        $scope.TotalInstallmnetDueAmount = total2;
                                        var installCount = 0;
                                        var installLastCount = 0;
                                        var flag = "0", flag2 = "0";

                                        angular.forEach($scope.Install, function (value, index) {
                                            installCount += value.DueAmount;

                                            if ($scope.Amount.TotalPaid < installCount) {
                                                //if (flag2 == "0") {
                                                //    flag2 = "1"; flag3 = "1";
                                                //    installLastCount += value.DueAmount;
                                                //    $scope.LastInstallmentTotal = installLastCount
                                                //    }
                                                if (flag == "0") {
                                                    flag = "1";
                                                    $scope.ThisInstallmentNo = value.InstallmentNo;
                                                    $scope.ThisInstallmentName = value.InstallmentNo;
                                                    if (value.DueDate != null) {
                                                        var ddate = new Date(parseInt(value.DueDate.substr(6)));// value.DueDate;
                                                        var mth = ddate.getMonth() + 1;
                                                        if (mth < 10) mth = "0" + mth;
                                                        $scope.DueDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();
                                                    }
                                                    $scope.ThisInstallmentAmount = value.DueAmount;
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
                                            $scope.CurrentDue = $scope.LastInstallmentTotal + $scope.ThisInstallmentAmount - $scope.Amount.TotalPaid;
                                            // alert('not eq ' + $scope.CurrentDue)
                                        }
                                        else {
                                            $scope.CurrentDue = $scope.ThisInstallmentAmount - $scope.Amount.TotalPaid;
                                            // alert('==' + $scope.CurrentDue);
                                        }

                                    })


                                })
                            }).then(function () {
                                $http({  // 
                                    method: 'Get',
                                    url: '/OtherPayment/GetOPaymentServiceTaxCharged',
                                    params: { saleid: $scope.SaleID }
                                }).success(function (data, status, headers, config) {
                                    $scope.STPayment = data;
                                    var total = 0;
                                    var stotal = 0;
                                    for (var i = 0; i < $scope.STPayment.length; i++) {
                                        var list = $scope.STPayment[i];
                                        total += list.Amount;
                                        stotal += list.ServiceTaxAmount;
                                    }
                                    $scope.STTotalAmount = total;
                                    $scope.STTotalServiceTaxAmount = stotal;
                                }).then(function () {
                                    $http({  // 
                                        method: 'Get',
                                        url: '/OtherPayment/GetOPaymentLatePaymentCharged',
                                        params: { saleid: $scope.SaleID }
                                    }).success(function (data, status, headers, config) {
                                        $scope.LPPayment = data;
                                        var total = 0;
                                        var ltotal = 0;
                                        for (var i = 0; i < $scope.LPPayment.length; i++) {
                                            var list = $scope.LPPayment[i];
                                            total += list.ReceiveAmount;
                                            ltotal += list.InterestAmount;
                                        }
                                        $scope.LPTotalAmount = total;
                                        $scope.LPTotalInterestAmount = ltotal;
                                    }).then(function () {
                                        $http({  // 
                                            method: 'Get',
                                            url: '/OtherPayment/GetOPaymentPropertyTransferCharged',
                                            params: { saleid: $scope.SaleID }
                                        }).success(function (data, status, headers, config) {
                                            $scope.TFPayment = data;
                                            var total = 0;
                                            for (var i = 0; i < $scope.TFPayment.length; i++) {
                                                var list = $scope.TFPayment[i];
                                                total += list.Amount;
                                            }
                                            $scope.TFTotalAmount = total;
                                        }).then(function () {
                                            $http({  // 
                                                method: 'Get',
                                                url: '/OtherPayment/GetOPaymentClearanceCharged',
                                                params: { saleid: $scope.SaleID }
                                            }).success(function (data, status, headers, config) {
                                                $scope.CCPayment = data;
                                                var total = 0;
                                                var ctotal = 0;
                                                for (var i = 0; i < $scope.CCPayment.length; i++) {
                                                    var list = $scope.CCPayment[i];
                                                    total += list.Amount;
                                                    ctotal += list.ClearanceCharge;
                                                }
                                                $scope.CCTotalAmount = total;
                                                $scope.CCTotalClearanceCharge = ctotal;
                                                $http({
                                                    method: 'Get',
                                                    url: '/Payment/GetOtherPaymentGroupedBySaleID',
                                                    params: { sid: $scope.Sale.Sale[0].SaleID }
                                                }).success(function (data, status, headers, config) {
                                                    $scope.GTotalAmount = data;
                                                    $('#loading').hide();
                                                    var gtotal = 0;
                                                    for (var i = 0; i < $scope.GTotalAmount.GroupPay.length; i++) {
                                                        var list = $scope.GTotalAmount.GroupPay[i];
                                                        gtotal += list.TAmount;
                                                    }
                                                    $scope.GrantTotalAmount = gtotal;
                                                })
                                            })
                                        })
                                    })
                                })
                            })
                        })
                    })
                })
            });


        })
    }
    $scope.orderSearchList = function (predicate, reverse) {
        $scope.SearchList = orderBy($scope.SearchList, predicate, reverse);
    };
    $scope.orderInstall1 = function (predicate, reverse) {
        $scope.Install = orderBy($scope.Install, predicate, reverse);
    };
    $('#loading').hide();

});
myApp.controller('HomeController', function ($scope, $http, $filter) {
    var orderBy = $filter('orderBy');
    $scope.GetPropertySummary = function () {
        var sid = $("#hidSaleID").val();
        $http({
            method: 'Get',
            url: '/Customer/Info/GetCustomerBySaleIDby',
            params: { saleid: sid }
        }).success(function (data, status, headers, config) {
            $scope.Customer = data;
            $scope.CustomerName = data.AppTitle + " " + data.FName + " " + data.MName + " " + data.LName;
            $http({
                method: 'Get',
                url: '/Customer/Info/GetFlatSaleBySaleID',
                params: { saleid: sid }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                $http({
                    method: 'Get',
                    url: '/Admin/Property/GetPropertyByPIDSize', //Pro = pr, PType = pt, PSize = ps
                    params: { propertyid: $scope.Sale.PropertyID, PropertyTypeID: $scope.Sale.PropertyTypeID, PropertySizeID: $scope.Sale.PropertySizeID }
                }).success(function (data, status, headers, config) {
                    $scope.Property = data;
                }).then(function () {
                    $http({
                        method: 'Get',
                        url: '/Customer/Installment/GetInstallment',
                        params: { saleid: $scope.Sale.SaleID }
                    }).success(function (response) {
                        $scope.Install = response;
                    }).then(function () {
                        $http({
                            method: 'Get',
                            url: '/Payment/GetTotalPayment',
                            params: { saleid: $scope.Sale.SaleID }
                        }).success(function (data, status, headers, config) {
                            $scope.Amount = data;
                        }).then(function () {
                            $http({
                                method: 'Get',
                                url: '/Customer/Info/GetPaymentBySaleID',
                                params: { saleid: $scope.Sale.SaleID }
                            }).success(function (data, status, headers, config) {
                                $scope.Payments = data;
                            }).then(function () {
                                $http({
                                    method: 'Get',
                                    url: '/Customer/Info/GetFlatInfoByFlatID', //1
                                    params: { Flatid: $scope.Sale.FlatID }
                                }).success(function (data, status, headers, config) {
                                    $scope.Flat = data;
                                }).then(function () {
                                    $http({
                                        method: 'Get',
                                        url: '/Payment/GetTotalServiceTax', //1
                                        params: { saleid: $scope.Sale.SaleID }
                                    }).success(function (data, status, headers, config) {
                                        $scope.STax = data;
                                    }).then(function () {
                                        $http({  // 
                                            method: 'Get',
                                            url: '/Payment/GetServiceTaxPaymentBySaleID',
                                            params: { sid: $scope.Sale.SaleID }
                                        }).success(function (data, status, headers, config) {
                                            $scope.STaxPayment = data;
                                            //$.each(data, function (index, obj) {
                                            //    alert(obj[index].Amount)
                                            //})
                                            $http({  // 
                                                method: 'Get',
                                                url: '/Payment/GetTransferPaymentBySaleID',
                                                params: { sid: $scope.Sale.SaleID }
                                            }).success(function (data, status, headers, config) {
                                                $scope.TransPayment = data;

                                                $http({  // 
                                                    method: 'Get',
                                                    url: '/Payment/GetLateFeePaymentBySaleID',
                                                    params: { sid: $scope.Sale.SaleID }
                                                }).success(function (data, status, headers, config) {
                                                    $scope.LatePayment = data;

                                                    $http({  // 
                                                        method: 'Get',
                                                        url: '/Payment/GetMaintanancePaymentBySaleID',
                                                        params: { sid: $scope.Sale.SaleID }
                                                    }).success(function (data, status, headers, config) {
                                                        $scope.MainPayment = data;


                                                        $http({  // 
                                                            method: 'Get',
                                                            url: '/Payment/GetInterestPaymentBySaleID',
                                                            params: { sid: $scope.Sale.SaleID }
                                                        }).success(function (data, status, headers, config) {
                                                            $scope.IntPayment = data;


                                                            $http({  // 
                                                                method: 'Get',
                                                                url: '/Payment/GetMiscPaymentBySaleID',
                                                                params: { sid: $scope.Sale.SaleID }
                                                            }).success(function (data, status, headers, config) {
                                                                $scope.MiscPayment = data;

                                                            }).then(function () {
                                                                $http({  // 
                                                                    method: 'Get',
                                                                    url: '/Payment/GetLateChargeBySaleID',
                                                                    params: { sid: $scope.Sale.SaleID }
                                                                }).success(function (data, status, headers, config) {
                                                                    $scope.LatePaymentCharge = data;
                                                                    var rtotal = 0; var itotal = 0;
                                                                    for (var i = 0; i < $scope.LatePaymentCharge.length; i++) {
                                                                        var list = $scope.LatePaymentCharge[i];
                                                                        rtotal += list.ReceiveAmount;
                                                                        itotal += list.InterestAmount;
                                                                    }
                                                                    $scope.RLTotal = rtotal;
                                                                    $scope.ILTotal = itotal;
                                                                })
                                                            })
                                                        })
                                                    })
                                                })
                                            })
                                        }).then(function () {
                                            $http({
                                                method: 'Get',
                                                url: '/Payment/GetTotalOPayment',
                                                params: { saleid: $scope.Sale.SaleID }
                                            }).success(function (data, status, headers, config) {
                                                $scope.OAmount = data;
                                            }).then(function () {
                                                $http({
                                                    method: 'Get',
                                                    url: '/Payment/GetOtherPaymentGroupedBySaleID',
                                                    params: { sid: $scope.Sale.SaleID }
                                                }).success(function (data, status, headers, config) {
                                                    $scope.GTotalAmount = data;
                                                    var gtotal = 0;
                                                    for (var i = 0; i < $scope.GTotalAmount.GroupPay.length; i++) {
                                                        var list = $scope.GTotalAmount.GroupPay[i];
                                                        gtotal += list.TAmount;
                                                    }
                                                    $scope.GrantTotalAmount = gtotal;
                                                })
                                            })
                                        })
                                    })
                                })
                            })
                        });
                    })
                })
            })
        }).then(function () {

        })
    }

    $scope.GetPropertySummaryPrint = function () {
        var sid = $("#hidSaleID").val();
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Customer/Info/GetCustomerBySaleID',
            params: { saleid: sid }
        }).success(function (data, status, headers, config) {
            $scope.Customer = data;
            $scope.CustomerName = data.AppTitle + " " + data.FName + " " + data.MName + " " + data.LName;
            $http({
                method: 'Get',
                url: '/Customer/Info/GetFlatSaleBySaleID',
                params: { saleid: sid }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                $http({
                    method: 'Get',
                    url: '/Admin/Property/GetPropertyByPIDSize', //Pro = pr, PType = pt, PSize = ps
                    params: { propertyid: $scope.Sale.PropertyID, PropertyTypeID: $scope.Sale.PropertyTypeID, PropertySizeID: $scope.Sale.PropertySizeID }
                }).success(function (data, status, headers, config) {
                    $scope.Property = data;
                    $http({
                        method: 'Get',
                        url: '/Customer/Installment/GetInstallment',
                        params: { saleid: $scope.Sale.SaleID }
                    }).success(function (response) {
                        $scope.Install = response;
                        $http({
                            method: 'Get',
                            url: '/Payment/GetTotalPayment',
                            params: { saleid: $scope.Sale.SaleID }
                        }).success(function (data, status, headers, config) {
                            $scope.Amount = data;
                            $http({
                                method: 'Get',
                                url: '/Customer/Info/GetPaymentBySaleID',
                                params: { saleid: $scope.Sale.SaleID }
                            }).success(function (data, status, headers, config) {
                                $scope.Payments = data;
                                $http({
                                    method: 'Get',
                                    url: '/Customer/Info/GetFlatInfoByFlatID', //1
                                    params: { Flatid: $scope.Sale.FlatID }
                                }).success(function (data, status, headers, config) {
                                    $scope.Flat = data;
                                    $http({
                                        method: 'Get',
                                        url: '/Payment/GetTotalServiceTax', //1
                                        params: { saleid: $scope.Sale.SaleID }
                                    }).success(function (data, status, headers, config) {
                                        $scope.STax = data;
                                        $http({  // 
                                            method: 'Get',
                                            url: '/Payment/GetServiceTaxPaymentBySaleID',
                                            params: { sid: $scope.Sale.SaleID }
                                        }).success(function (data, status, headers, config) {
                                            $scope.STaxPayment = data;
                                            $http({  // 
                                                method: 'Get',
                                                url: '/Payment/GetTransferPaymentBySaleID',
                                                params: { sid: $scope.Sale.SaleID }
                                            }).success(function (data, status, headers, config) {
                                                $scope.TransPayment = data;

                                                $http({  // 
                                                    method: 'Get',
                                                    url: '/Payment/GetLateFeePaymentBySaleID',
                                                    params: { sid: $scope.Sale.SaleID }
                                                }).success(function (data, status, headers, config) {
                                                    $scope.LatePayment = data;

                                                    $http({  // 
                                                        method: 'Get',
                                                        url: '/Payment/GetMaintanancePaymentBySaleID',
                                                        params: { sid: $scope.Sale.SaleID }
                                                    }).success(function (data, status, headers, config) {
                                                        $scope.MainPayment = data;


                                                        $http({  // 
                                                            method: 'Get',
                                                            url: '/Payment/GetInterestPaymentBySaleID',
                                                            params: { sid: $scope.Sale.SaleID }
                                                        }).success(function (data, status, headers, config) {
                                                            $scope.IntPayment = data;


                                                            $http({  // 
                                                                method: 'Get',
                                                                url: '/Payment/GetMiscPaymentBySaleID',
                                                                params: { sid: $scope.Sale.SaleID }
                                                            }).success(function (data, status, headers, config) {
                                                                $scope.MiscPayment = data;
                                                                $http({
                                                                    method: 'Get',
                                                                    url: '/Payment/GetTotalOPayment',
                                                                    params: { saleid: $scope.Sale.SaleID }
                                                                }).success(function (data, status, headers, config) {
                                                                    $scope.OAmount = data;
                                                                    $http({
                                                                        method: 'Get',
                                                                        url: '/Payment/GetOtherPaymentGroupedBySaleID',
                                                                        params: { sid: $scope.Sale.SaleID }
                                                                    }).success(function (data, status, headers, config) {
                                                                        $scope.GTotalAmount = data;
                                                                        $http({
                                                                            method: 'Get',
                                                                            url: '/Payment/GetOtherPaymentGroupedBySaleID',
                                                                            params: { sid: $scope.Sale.SaleID }
                                                                        }).success(function (data, status, headers, config) {
                                                                            $scope.GTotalAmount = data;
                                                                            //Print Page
                                                                            var opn = $("#hidid2").val();
                                                                            if (opn == "Print") {
                                                                                $('#loading').hide();
                                                                                window.print();

                                                                            }
                                                                            else if (opn == "Export") {


                                                                                $('#loading').remove();
                                                                                var sshtml = document.body.innerHTML;
                                                                                sshtml = sshtml.trim("<img id='loading-image' src='~/images/loading.gif' alt='Loading... />")
                                                                                $http({
                                                                                    method: 'Post',
                                                                                    url: '/Customer/Info/ExportSummary',
                                                                                    data: { shtml: sshtml }
                                                                                }).success(function (data, status, headers, config) {
                                                                                    $('#loading').hide();
                                                                                    window.location.href = '/pdf/temp/' + data;

                                                                                    // window.close();
                                                                                })
                                                                            }
                                                                            else {
                                                                                // var sshtml = JSON.stringify($("#SummaryHtml").text());
                                                                                var sshtml = document.body.innerHTML;
                                                                                var em = opn;
                                                                                //alert(em);
                                                                                $http({
                                                                                    method: 'Post',
                                                                                    url: '/Customer/Info/SummarySendMail',
                                                                                    data: { shtml: sshtml, email: em }
                                                                                }).success(function (data, status, headers, config) {
                                                                                    $('#loading').hide();
                                                                                    alert("Summary Email sent successfully.");
                                                                                    $('#loading').hide();
                                                                                })
                                                                            }

                                                                        })
                                                                    })
                                                                })
                                                            })
                                                        })
                                                    })
                                                })
                                            })
                                        })
                                    })
                                })
                            })
                        });
                    })
                })
            })
        })
    }
    $scope.orderSearchList = function (predicate, reverse) {
        $scope.SearchList = orderBy($scope.SearchList, predicate, reverse);
    };
    $scope.order = function (predicate, reverse) {
        $scope.OPayment = orderBy($scope.OPayment, predicate, reverse);
    };
    $scope.orderInstall = function (predicate, reverse) {
        $scope.Install = orderBy($scope.Install, predicate, reverse);
    };
    $scope.orderSTaxPayment = function (predicate, reverse) {
        $scope.STaxPayment = orderBy($scope.STaxPayment, predicate, reverse);
    };

    $scope.orderPayments = function (predicate, reverse) {
        $scope.Payments = orderBy($scope.Payments, predicate, reverse);
    };
    $scope.orderTransPayment = function (predicate, reverse) {
        $scope.TransPayment = orderBy($scope.TransPayment, predicate, reverse);
    };
    $scope.orderLatePayment = function (predicate, reverse) {
        $scope.LatePayment = orderBy($scope.LatePayment, predicate, reverse);
    };
    $scope.orderMainPayment = function (predicate, reverse) {
        $scope.MainPayment = orderBy($scope.MainPayment, predicate, reverse);
    };
    $scope.orderIntPayment = function (predicate, reverse) {
        $scope.IntPayment = orderBy($scope.IntPayment, predicate, reverse);
    };
    $scope.orderMiscPayment = function (predicate, reverse) {
        $scope.MiscPayment = orderBy($scope.MiscPayment, predicate, reverse);
    };

    $scope.ExportSummary = function () {
        var sshtml = $("#SummaryHtml").html();
        var ht = $("#tblSTax").html();
        sshtml = sshtml + ht;
        //  var sshtml = JSON.stringify($("#SummaryHtml").text());
        $http({
            method: 'Post',
            url: '/Customer/Info/ExportSummary',
            data: { shtml: sshtml }
        }).success(function (data, status, headers, config) {
            $("#btnExport").hide();
            $("#btnDownload").show();
        })
    }

    $scope.SummarySendMail = function () {
        var url = $("#btnExport").attr('href');
        var em = $("#emailid").val();

        url = url.replace('Export', em);

        window.open(url, '_blank');
        //window.location.href
        //var sshtml = JSON.stringify($("#SummaryHtml").text());

        //alert(em);
        //$http({
        //    method: 'Post',
        //    url: '/Customer/Info/SummarySendMail',
        //    data: { shtml: sshtml, email:em }
        //}).success(function (data, status, headers, config) {
        //    alert("Summary Email sent successfully.");
        //})
    }

    $scope.TestApi = function () {
        $http({
            method: 'Get',
            url: 'http://api-extranet-physmodo.azurewebsites.net/api/Account/UserInfo',
            data: { shtml: sshtml, email: em }
        }).success(function (data, status, headers, config) {
            alert("Summary Email sent successfully.");
        })
    }


    $('#loading').hide();

});

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
function calculatePLC(value, id) {




    var txtID = id;
    var initialBSP = id.split('_')[1];
    id = id.split('_')[0];
    var Plsvalue;
    var plsAmountvalue = 0;
    var Plcval = document.getElementById(id).value;
    if (Plcval != "") {
        Plsvalue = document.getElementById("txtplcprice").value;
        plsAmountvalue = ((parseFloat(Plsvalue) * Plcval) / 100);
    }

    // if (value != initialBSP) {


    var test = document.getElementsByClassName("bsppercentage_" + id);

    if (test != "") {
        var val = test[0].value;
        var damt = document.getElementById("txtsaleprice").value;
        var dueamount = (((parseFloat(damt) * val) / 100) + parseFloat(plsAmountvalue));

        document.getElementById("bspdueamount" + id).innerHTML = parseFloat(dueamount).toFixed(2);
        document.getElementById("bspdueamount" + id).value = parseFloat(dueamount).toFixed(2);
    }
    else {
        document.getElementById("bspdueamount" + id).innerHTML = dueamount = document.getElementById("bspbaseamount" + id).innerHTML;
    }
    document.getElementById("bsptotal").innerHTML = "";
    var total = 0;
    $('.dueamount').each(function () {
        total = parseFloat(document.getElementById(this.id).innerHTML) + parseFloat(total);
        // alert(total);
    });
    document.getElementById("bsptotal").innerHTML = total.toFixed(0);




    //var txtID = id;
    //var initialBSP = id.split('_')[1];
    //id = id.split('_')[0];
    //// if (value != initialBSP) {
    //var val = document.getElementById(txtID).value;
    //if (val != "") {



    //    var damt = document.getElementById("txtplcprice").value;
    //    var dueamount = (parseFloat(damt) * val) / 100;
    //    dueamount = (parseFloat(dueamount) + parseFloat(document.getElementById("bspdueamount" + id).innerHTML));
    //    document.getElementById("bspdueamount" + id).innerHTML = parseFloat(dueamount).toFixed(2);
    //    document.getElementById("bspdueamount" + id).value = parseFloat(dueamount).toFixed(2);
    //}
    //else {
    //    document.getElementById("bspdueamount" + id).innerHTML = dueamount = document.getElementById("bspbaseamount" + id).innerHTML;
    //}
    //document.getElementById("bsptotal").innerHTML = "";
    //var total = 0;
    //$('.dueamount').each(function () {
    //    total = parseFloat(document.getElementById(this.id).innerHTML) + parseFloat(total);
    //    // alert(total);
    //});
    //document.getElementById("bsptotal").innerHTML = total.toFixed(0);
    // }
}



function ValidateBookToSale(saledate, saleprice, plcPrice) {
    var vl = true;
    var message = "";
    if (saledate == "") {
        vl = false;
        message += "Insert Property Sale Date <br/>";
    }
    if (saleprice == "") {
        vl = false;
        message += "Insert Sale Price. <br/>";
    }
    if (plcPrice == "") {
        vl = false;
        message += "Insert PLC Amount. <br/>";
    }
    $("#ErrorMessage").html(message);
    return vl;
}

function GetInstallments(saledate, saleprice, ddlInstallment, ddlInterval, PlanType, plcPrice, saleid, flatid) {

    saledate = document.getElementById(saledate).value;
    saleprice = document.getElementById(saleprice).value;
    ddlInstallment = document.getElementById(ddlInstallment).value;
    ddlInterval = document.getElementById(ddlInterval).value;
    PlanType = document.getElementById(PlanType).value;
    var PlanTypeID = document.getElementById('PlanType').value;
    plcPrice = document.getElementById(plcPrice).value;
    saleid = document.getElementById(saleid).value;
    flatid = document.getElementById(flatid).value;
    var vli = ValidateBookToSale(saledate, saleprice, plcPrice);
    if (vli == false) {
        $('#loading').hide();
        $('#myModal').modal('show');
    }
    else {
        if (PlanType == 3) {
            //  ddlInstallment = document.getElementById("property").value;
        }
        $.ajax({
            url: '/flat/GetInstallments',
            type: 'post',
            dataType: 'json',
            data: '{saledate: "' + saledate + '",saleprice: "' + saleprice + '",ddlInstallment: "' + ddlInstallment + '",ddlInterval: "' + ddlInterval + '",PlanType: "' + PlanType + '",plcPrice:"' + plcPrice + '", saleID:"' + saleid + '",flatid:"' + flatid + '",PlanTypeID:"' + PlanTypeID + '" }',
            contentType: 'application/json',
            success: function (result) {
                window.location.href = urlPath + '/';
            },
            error: function (err) {
                if (err.responseText == "success") {
                    alert("Data is deleted successfully.");
                    pageload();
                }
                else {
                    document.getElementById("divinstallment").innerHTML = err.responseText;
                }
            },
            complete: function () {
            }
        });
    }
}
function SaveInstallments(saledate, saleprice, ddlInstallment, ddlInterval, PlanType, saleID, flatID) {
    if (saledate == "") alert("Please insert Sale Date.");
    if (saleprice == "") alert("Please insert Sale Price.");
    getid();
    saledate = document.getElementById(saledate).value;
    PlanType = document.getElementById(PlanType).value;
    saleprice = document.getElementById(saleprice).value;
    ddlInstallment = document.getElementById(ddlInstallment).value;
    ddlInterval = document.getElementById(ddlInterval).value;
    EventName = document.getElementById("EventId").value;
    saleID = document.getElementById(saleID).value;
    flatID = document.getElementById(flatID).value;

    var pplprice = document.getElementById("txtplcprice").value;
    var evName = document.getElementById("EventName").value;

    var bspPercentage;
    $('.bsppercentage').each(function () {
        bspPercentage += document.getElementById(this.id).value + ",";
    });
    $('.dueamount').each(function () {
        document.getElementById("EventAmount").value += parseFloat(document.getElementById(this.id).innerHTML) + ",";
    });

    $.ajax({
        url: '/flat/SaveInstallments',
        type: 'post',
        dataType: 'json',
        data: '{saledate: "' + saledate + '",saleprice: "' + saleprice + '", salepriceword:"", ddlInstallment: "' + ddlInstallment + '",ddlInterval: "' + ddlInterval + '",EventName:"' + EventName + '",bspPercentage:"' + bspPercentage + '",saleID:"' + saleID + '",flatID:"' + flatID + '",EvName:"' + evName + '",Amount:"' + document.getElementById("EventAmount").value + '",PPLPrice:"' + pplprice + '" }',
        contentType: 'application/json',
        success: function (result) {
            //  window.location.href = urlPath + '/';
            alert("Data is added successfully.");
            // pageload();
            // GetInstallmetnDetailsBySaleId(ddlInterval);
            $(".row .centre_sec").css('display', 'none');
            $(".row").find(".centre_sec").css('display', 'none');
        },
        error: function (err) {
            if (err.responseText == "success") {
                alert("Data is added successfully.");

                $(".row .centre_sec").css('display', 'none');
                $(".row").find(".centre_sec").css('display', 'none');
                pageload();
                GetInstallmetnDetailsBySaleId(ddlInterval);
            }
            else {
                $(".row .centre_sec").css('display', 'none');
                $(".row").find(".centre_sec").css('display', 'none');
                document.getElementById("divinstallment").innerHTML = err.responseText;
                GetInstallmetnDetailsBySaleId(ddlInterval);
            }
        },
        complete: function () {
        }
    });
}
function GetInstallmetnDetailsBySaleId(SaleId) {
    $.ajax({
        url: '/flat/GetInstallmetnDetailsBySaleId',
        type: 'post',
        dataType: 'json',
        data: '{FlatId: "' + SaleId + '" }',
        contentType: 'application/json',
        success: function (result) {
            window.location.href = urlPath + '/';
        },
        error: function (err) {
            if (err.responseText == "success") {
                //alert("Data is deleted successfully.");
                document.getElementById("divMainInstallment").innerHTML = err.responseText;
            }
            else {
                document.getElementById("divMainInstallment").innerHTML = err.responseText;
            }
        },
        complete: function () {
        }
    });
}
function getid() {
    document.getElementById("EventId").value = "";
    $('.dropdwn').each(function () {
        if (this.value) {
            document.getElementById("EventName").value += $(this).find(":selected").text() + ",";
            document.getElementById("EventId").value += this.value + ",";
        }
    });


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