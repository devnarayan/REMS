var myApp = angular.module('OPaymentApp', []);
//Defining a Controller 
myApp.controller('OtherPaymentController', function ($scope, $http, $filter) {
    // Get all Property list for dropdownlist
    var orderBy = $filter('orderBy');

    $scope.Error = "";

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
                $http({
                    method: 'Get',
                    url: '/Sale/Payment/GetPaymentMasterList',
                }).success(function (data) {
                    $scope.PaymentMaster = data;
                })
            })
        })

    }
    $scope.TowerChange = function () {
       // $("#loading").show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetFlatByTowerID',
            params: { towerid: $scope.FlatSearch.TowerID }
        }).success(function (data) {
            $scope.FlatList = data;
           // $("#loading").hide();

        })
    }
        //$("#loading").show();
        //var payfor = $("#PaymentFor").find(":selected").text();
        //if (payfor == "All") {
        //    $("#divSearchTxt").addClass('hidden');
        //    $("#divSearchDDL").hide();
        //    $("#loading").hide();

        //}
        //else if (payfor == "Service Tax" || payfor == "Late Payment Charges" || payfor == "Transfer Fee" || payfor == "Misc Amount" || payfor == "Clearance Charges") {
        //    $("#divSearchTxt").removeClass('hidden');
        //    $("#divSearchDDL").hide();
        //    $("#loading").hide();
        //}
        //else {
        //    $("#divSearchDDL").show();
        //    $("#divSearchTxt").addClass('hidden');
        //    $http({
        //        method: 'Get',
        //        url: '/Admin/CreateProperty/GetFlatByTowerID',
    //        params: { towerid: $scope.FlatSearch.TowerID }
        //    }).success(function (data) {
        //        $scope.FlatList = data;
        //        $("#loading").hide();

        //    })
        //}
   // }
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
    // Save Payment
    $scope.EditSearchPayment = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.FlatSearch.FlatID);
        var pid = $("#hidFlatID").val();
        //var proID = $("#PropertyID").val();
        //var searchtext = $("#searchtext").val();
        var payfor = $("#PaymentFor").find(":selected").text();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/OtherPayment/EditSearchPayment',
            params: { FlatId: pid, paymentfor: payfor },
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

    $scope.ClearanceInit = function () {
        $('#loading').show();
        var pid = $("#TransactionID").val()
        $http({
            method: 'Get',
            url: '/OtherPayment/GetPaymentbyTID',
            params: { transactionid: pid }
        }).success(function (data, status, headers, config) {
            $scope.Payment = data;
            $scope.PaymentDate = data.PaymentDateSt;// ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();
            // $scope.inWords(data.Amount);
            var SID = data.SaleID;
            $http({
                method: 'Get',
                url: '/Payment/GetFlatSaleBySaleID',
                params: { saleid: SID }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                $http({
                    method: 'Get',
                    url: '/Payment/GetTotalOPayment',
                    params: { saleid: $scope.Sale.Sale[0].SaleID }
                }).success(function (data, status, headers, config) {
                    $scope.Amount = data;
                    $('#loading').hide();
                });
            })
        });
    }

    $scope.UpdateClearance = function () {
        $('#loading').show();
        $http({
            method: 'POST',
            url: '/OtherPayment/UpdateClearanceDetails',
            data: { TransactionID: $("#TransactionID").val(), ChargeAmount: $("#ClearAmt").val(), Remarks: $("#Remarks").val(), ClearanceDate: $("#ClearanceDate").val(), IsBounce: $("#isBounce").is(":checked") },
        }).success(function (data) { //Showing success message $scope.status = "The Person Saved Successfully!!!";
            // if (data == "Yes") {
            alert("Payment Clearance has been updated successfully!");
            $scope.Error = 'Payment Clearance updated successfully.';
            $('#loading').hide();
            //}
            //else {
            //    alert("Unable to updated Payment Clearance !!!");
            //    $scope.Error = 'Unable to updated Payment Clearance. ';
            //}
        }).error(function (error) {
            alert("Unable to updated Payment Clearance !!!");
            $scope.Error = 'Unable to updated Payment Clearance: ' + error.message;
            $('#loading').hide();
        });
    }

    $scope.EditPaymentInit = function () {
        $('#loading').show();
        var pid = $("#TransactionID").val()
        $http({
            method: 'Get',
            url: '/OtherPayment/GetPaymentbyTID',
            params: { transactionid: pid }
        }).success(function (data, status, headers, config) {
            $scope.Payment = data;
            $scope.PaymentDate = data.PaymentDateSt;

            $("#PaymentMode").find(":selected").text(data.PaymentMode);
            if (data.PaymentMode == "Cash" || data.PaymentMode == "Transfer Entry") {
                $("#divCheque").hide();
            }
            else {
                $("#divCheque").show();
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
                    url: '/Payment/GetTotalOPayment',
                    params: { saleid: $scope.Sale.Sale[0].SaleID }
                }).success(function (data, status, headers, config) {
                    $scope.Amount = data;
                    $('#loading').hide();
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
            $('#loading').show();
            if ($("#paymentDate").val() == "")
                $("#paymentDate").val($("#paymentDate").attr("placeholder"));
            $http({
                method: 'POST',
                url: '/OtherPayment/UpdatePaymentDetails',
                data: { TransactionID: $("#TransactionID").val(), InstallmentNo: $("#ddlPayment").val(), Saleid: $("#SaleID").val(), Flatname: $("#FlatName").val(), DueAmount: $("#DueAmount").val(), DueDate: $("#DueDate").val(), PaymentMode: $("#PaymentMode").find(":selected").text(), ChequeNo: $("#ChequeNo").val(), ChequeDate: $("#chequeDate").val(), BankName: $("#BankName").find(":selected").text(), BankBranch: $("#BankBranch").val(), Remarks: $("#Remarks").val(), PayDate: $("#paymentDate").val(), Amtrcvdinwrds: $("#PaymentAmountInWords").val(), ReceivedAmount: $("#ReceivedAmount").val(), IsPrint: $("#IsReceipt").val() },
            }).success(function (data) { //Showing success message $scope.status = "The Person Saved Successfully!!!";
                alert("Payment Details has been updated successfully!");
                $scope.Error = 'Payment Details updated successfully.';
                $('#loading').hide();
            }).error(function (error) {
                alert("Unable to updated Payment Details !!!");
                $scope.Error = 'Unable to updated Payment Details: ' + error.message;
                $('#loading').hide();
            });
        }
    }


    $scope.SearchCancelPayment = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.FlatSearch.FlatID);
        var Flatid = $("#hidFlatID").val();
       // var propertyName = $("#FlatID :selected").text()
        var search = $("#searchby").val();
      //  var propertyid = $("#TowerID").val();
       // var Flatid = $("#FlatID").val();
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/Sale/Manage/PaymentCancelSearch',
            params: { search: search, Flatid: Flatid, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
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
    $scope.CancelPayment = function () {
        var vli = ValidateCancelPayment();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            $scope.Pay.CancelDate = $("#CancelDate").val();
            $scope.Pay.TransactionID = $("#TransactionID").val();
            $scope.Pay.Status = $("#PayStatus").val();
            $http({
                method: 'POST',
                url: '/OtherPayment/PaymentCancelSave',
                data: JSON.stringify($scope.Pay),
                headers: { 'Content-Type': 'application/JSON' }
            }).success(function (data) {
                if (data != "No") {
                    $scope.status = "Payment " + data + " Successfully.";
                    alert("Payment " + data + " Successfully.");
                }
                else {
                    alert("Unable to save payment cancelation.");
                    $scope.status = 'Unable to save payment cancelation. ';
                }
                $('#loading').hide();
            }).error(function (error) {
                //Showing error message
                alert("Unable to save payment cancelation: " + error.message);
                $scope.status = 'Unable to save payment cancelation: ' + error.message;
                $('#loading').hide();
            });
        }
    }

    $scope.OtherBackupReceiptInint = function () {
        $http({  // 
            method: 'Get',
            url: '/OtherPayment/GetOtherPaymentMaster',
            params: {}
        }).success(function (data, status, headers, config) {
            $scope.PaymentMaster = data;
        })
    }
    $scope.SearchBackupReceipt = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.FlatSearch.FlatID);
        var pid = $("#hidFlatID").val();
        var propertyName = $("#PropertyTypeID :selected").text()
        var search = $("#searchby").val();
        var propertyid = $("#PropertyID").val();
        var propertySubTypeID = $("#PropertyTypeID").val();
        var proSize = $("#PropertySizeID").val();
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        var paymenttype = $("#PaymentType").find(":selected").text();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/OtherPayment/SearchBackupReceipt',
            params: { propertyName: propertyName, search: search, FlatId: pid,datefrom: datefrom, dateto: dateto, searchtext: searchtext, paymenttype: paymenttype },
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
    $scope.BackupReceiptExportAndMail = function () {
        var vli = ValidateBackupReceiptEmail();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            var yourarray = [];
            $('input.selectone').each(function () {
                yourarray.push($(this).val());
            });
            var emailid = $("#EmailID").val();
            $http({
                method: 'Post',
                url: '/OtherPayment/ExportAndMailReceipt/',
                data: { transids: yourarray, emailid: emailid }
            }).success(function (data) {
                if (data != "No") {
                    alert("Mail sent to your inbox.");
                }
                else {
                    alert("Can't Sent mail.");
                }
                $('#loading').hide();
            })
        }
    }
    $scope.BackupReceiptSendMail = function () {
        var vli = ValidateBackupReceiptEmail();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            var yourarray = [];
            $('input.selectone:checkbox:checked').each(function () {
                yourarray.push($(this).val());
            });
            var emailid = $("#EmailID").val();
            $http({
                method: 'Post',
                url: '/OtherPayment/BackupReceiptSendMail/',
                data: { transids: yourarray, emailid: emailid }
            }).success(function (data) {
                if (data != "No") {
                    alert("Mail sent to your inbox.");
                }
                else {
                    alert("Can't Sent mail.");
                }
                $('#loading').hide();
            })
        }
    }

    $scope.BackupReceiptPrint = function () {
        $('#loading').show();
        var yourarray = [];
        $('input.selectone:checkbox:checked').each(function () {
            yourarray.push($(this).val());
        });
        $('#loading').hide();

        var murl = "/Sale/OtherPayment/BackupReceiptPrintAction/" + yourarray;
        window.open(murl, '_blank');
    }

    $scope.GetBackupReceiptPrintInit = function () {
        var id = $("#hidID").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/sale/OtherPayment/BackupReceiptPrint',
            params: { transids: id },
            dataType: "json"
        }).success(function (data) {
            $scope.PrintReceiptList = data;
            $http({
                method: 'Get',
                url: '/Payment/GetPaymentbyTIDSt',
                params: { transactionid: id }
            }).success(function (data, status, headers, config) {
                $scope.Payment = data;
                window.print();

            })
        }).then(function () {
            //Print Page

        });
    }
    $scope.GetBackupReceiptPrintDataInit = function () {
        $('#loading').show();
        var searchtext = $("#hidID").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/sale/OtherPayment/SearchBackupReceiptData',
            params: { tids: searchtext },
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
                url: '/Payment/GetPaymentbyTIDSt',
                params: { transactionid: 0 }
            }).success(function (data, status, headers, config) {
                $scope.Payment = data;
                window.print();
            })
        }).error(function (er) {
            $('#loading').hide();
            alert(er.toString());
        });
    }

    $scope.orderSearchList = function (predicate, reverse) {
        $scope.SearchList = orderBy($scope.SearchList, predicate, reverse);
    };

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
    function ValidateCancelPayment() {
        var vl = true;
        var message = "";
        if ($("#CancelDate").val() == "") {
            vl = false;
            message += "Insert  Date. <br/>";
        }
        if ($("#Remarks").val() == "") {
            vl = false;
            message += "Insert payment cancelation remark. <br/>";
        }

        $("#ErrorMessage").html(message);
        return vl;
    }
    function ValidateBackupReceiptEmail() {
        var vl = true;
        var message = "";

        if ($("#EmailID").val() == "") {
            vl = false;
            message += "Please insert Email ID.<br/>";
        }
        if ($("#EmailID").val() != "") {
            var emailReg = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
            var valid = emailReg.test($("#EmailID").val());

            if (!valid) {
                vl = false;
                message += "Invalid Email ID.<br/>";
            }
        }
        $("#ErrorMessage").html(message);
        return vl;
    }
    $('#loading').hide();
});

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