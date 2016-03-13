var myApp = angular.module('ASReturnApp', []);
myApp.controller('ASReturnController', function ($scope, $http) {
    // Get all Property list for dropdownlist
    $scope.Error = "";

    //$http.get('/flat/GetPropertyList/').success(function (response) { $scope.Properties = response; });
    //$http.get('/flat/GetBanks/').success(function (response) { $scope.Banks = response; });
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

    $("#gendiv").hide();

    $scope.RefundInit = function () {
        //  alert('hi');
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
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetFlatByTowerID',
            params: { towerid: $scope.Flat.TowerID }
        }).success(function (data) {
            $scope.FlatList = data;
            $("#loading").hide();
        })
    }
    $scope.CheckGeneratedAssuredReturn = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
        //var pid = $("#PropertyID").find(":selected").val();
        //var fname = $("#propertyname").val();
        $http({
            method: 'Get',
            url: '/Customer/AssuredReturn/CheckInstallmentStatus',
            params: { PID: pid }
        }).success(function (data) {
            $scope.InstallStatus = data;
            if (data[0] == "No") {
                alert("Property name not found.")
                $("#gendiv").hide();
                $("#hidsid").val(0);
                $("#downdoc").hide();
            }
            else if (data[0] == "Yes") {
                $("#downdoc").show();
                alert("Property Assured Installment already Generated.");
                $("#gendiv").hide();
                $("#hidsid").val(data[1]);
                $http({
                    method: 'Get',
                    url: '/Customer/AssuredReturn/GetAssuredReturnBySaleID',
                    params: { saleid: data[1] }
                }).success(function (data) {
                    $scope.SearchList = data;
                    var gtotal = 0;
                    for (var i = 0; i < $scope.SearchList.length; i++) {
                        var list = $scope.SearchList[i];
                        gtotal += list.Amount;
                    }
                    $scope.TotalSearchAmount = Math.round(gtotal);
                    $http({
                        method: 'Get',
                        url: '/Customer/Document/AgreementInfo',
                        params: { saleid: $("#hidsid").val() }
                    }).success(function (data) {
                        $scope.Agreement = data;
                    })
                })
            }
            else if (data[0] == "LessPayment") {
                alert(data[1])
                $("#gendiv").hide();
                $("#hidsid").val(0);
            }
            else if (data[0] == "Error") {
                alert("Property name not found.")
                $("#gendiv").hide();
                $("#hidsid").val(0);
            }
            else {
                $("#hidsid").val(data[0]);
                $("#txtAmount").val(data[1]);
                $("#gendiv").show();
            }
            $('#loading').hide();
        })

    }

    $("#dvDate").show();
    $("#dvPName").hide();
    $scope.AssuredStatusChange = function () {
        var id = $("#payStatus").find(":selected").val();
        if (id == "Date") {
            $("#dvDate").show();
            $("#dvPName").hide();
        }
        else {

            $("#dvDate").hide();
            $("#dvPName").show();
        }
    }

    $scope.UpdateChequeDetails = function () {
        $('#loading').show();
        var ChequeNo = [];
        var ChequeDate = [];
        var asid = [];
        $('.clistno').each(function () {
            ChequeNo.push($(this).val());
        });
        $('.clistdate').each(function () {
            ChequeDate.push($(this).val());
        });
        $('.asidlist').each(function () {
            asid.push($(this).val());
        });

        $http({
            method: 'Post',
            url: '/Customer/AssuredReturn/UpdateAssuredChequeAll',
            data: { ChequeNos: ChequeNo, ChequeDates: ChequeDate, asids: asid }
        }).success(function (data) {
            if (data != 'No') {
                $http({
                    method: 'Get',
                    url: '/Customer/AssuredReturn/GetAssuredReturnBySaleID',
                    params: { saleid: data }
                }).success(function (data) {
                    $scope.SearchList = data;
                    var gtotal = 0;
                    for (var i = 0; i < $scope.SearchList.length; i++) {
                        var list = $scope.SearchList[i];
                        gtotal += list.Amount;
                    }
                    $scope.TotalSearchAmount = Math.round(gtotal);
                    $('#loading').hide();
                }).then(function () {
                    $('#loading').hide();
                })
                alert("Cheque Details updated successfully.");
            }
            else {
                alert("Invalid information");
            }
            $('#loading').hide();
        }).then(function () {
            $('#loading').hide();
        })
    }

    $scope.UpdateAssuredCheque = function (asid) {
        var cno = $("#" + asid + "-ChequeNo").val();
        var cdate = $("#" + asid + "-ChequeDate").val();
        var vli = ValidateAssuredCheque(cno, cdate);
        if (vli == false) {
            $('#loading').hide();
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            $http({
                method: 'Post',
                url: '/Customer/AssuredReturn/UpdateAssuredCheque',
                data: { asid: asid, chequeNo: cno, chequeDate: cdate }
            }).success(function (data) {
                if (data != 'No') {
                    $http({
                        method: 'Get',
                        url: '/Customer/AssuredReturn/GetAssuredReturnBySaleID',
                        params: { saleid: data }
                    }).success(function (data) {
                        $scope.SearchList = data;
                        var gtotal = 0;
                        for (var i = 0; i < $scope.SearchList.length; i++) {
                            var list = $scope.SearchList[i];
                            gtotal += list.Amount;
                        }
                        $scope.TotalSearchAmount = Math.round(gtotal);
                        $('#loading').hide();
                    }).then(function () {
                        $('#loading').hide();
                    })
                    alert("Cheque Details updated successfully.");
                }
                else {
                    alert("Invalid information");
                }
                $('#loading').hide();
            })
        }
    }

    $scope.ClearAssuredChequeUpdate = function (asid) {
        $('#loading').show();
        $http({
            method: 'Post',
            url: '/Customer/AssuredReturn/UpdateAssuredChequeClearance',
            data: { asid: asid }
        }).success(function (data) {
            if (data != 'No') {
                $http({
                    method: 'Get',
                    url: '/Customer/AssuredReturn/GetAssuredReturnBySaleID',
                    params: { saleid: data }
                }).success(function (data) {
                    $scope.SearchList = data;
                    var gtotal = 0;
                    for (var i = 0; i < $scope.SearchList.length; i++) {
                        var list = $scope.SearchList[i];
                        gtotal += list.Amount;
                    }
                    $scope.TotalSearchAmount = Math.round(gtotal);
                    $('#loading').hide();
                }).then(function () {
                    $('#loading').hide();
                })
                alert("Cheque Details updated successfully.");
            }
            else {
                alert("Invalid information");
            }
            $('#loading').hide();
        })
    }
    $scope.UnClearAssuredChequeUpdate = function (asid) {
        $('#loading').show();
        $http({
            method: 'Post',
            url: '/Customer/AssuredReturn/UpdateAssuredChequeUnClearance',
            data: { asid: asid }
        }).success(function (data) {
            if (data != 'No') {
                $http({
                    method: 'Get',
                    url: '/Customer/AssuredReturn/GetAssuredReturnBySaleID',
                    params: { saleid: data }
                }).success(function (data) {
                    $scope.SearchList = data;
                    var gtotal = 0;
                    for (var i = 0; i < $scope.SearchList.length; i++) {
                        var list = $scope.SearchList[i];
                        gtotal += list.Amount;
                    }
                    $scope.TotalSearchAmount = Math.round(gtotal);
                    $('#loading').hide();
                }).then(function () {
                    $('#loading').hide();
                })
                alert("Cheque Details updated successfully.");
            }
            else {
                alert("Invalid information");
            }
            $('#loading').hide();
        })
    }

    $scope.GenerateInstallmentClick = function () {
        $('#loading').show();
        var sid = $("#hidsid").val();
        var amt = $("#txtAmount").val();
        var startdate = $("#SDate").val();
        var PDate = $("#PDate").val();
        $http({
            method: 'Get',
            url: '/Customer/AssuredReturn/GenerateInstallment',
            params: { sid: sid, amt: amt, sdate: startdate, PDate: PDate }
        }).success(function (data) {
            $http({
                method: 'Get',
                url: '/Customer/AssuredReturn/GetAssuredReturnBySaleID',
                params: { saleid: sid }
            }).success(function (data) {
                $scope.SearchList = data;
                var gtotal = 0;
                for (var i = 0; i < $scope.SearchList.length; i++) {
                    var list = $scope.SearchList[i];
                    gtotal += list.Amount;
                }
                $scope.TotalSearchAmount = Math.round(gtotal);
                $('#loading').hide();
            }).then(function () {
                $('#loading').hide();
            })
        });
    }

    $scope.SearchAssuredReturn = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
       // var pname = $("#propertyname").val();
       // var pid = $("#PropertyID").find(":selected").val();
        var status = $("#searchby").find(":selected").val();
        var cdate = $("#datefrom").val();
        var cdateTo = $("#dateto").val();
        $http({
            method: 'Get',
            url: '/Customer/AssuredReturn/SearchAssuredReturn',
            params: { PID: pid, status: status, chequeDate: cdate, chequeDateTo: cdateTo }
        }).success(function (data, status, headers, config) {
            $scope.SearchList = data;
            var gtotal = 0;
            for (var i = 0; i < $scope.SearchList.length; i++) {
                var list = $scope.SearchList[i];
                gtotal += list.Amount;
            }
            $scope.TotalSearchAmount = Math.round(gtotal);
            $('#loading').hide();
        })

    }

    $scope.AssuredPayInit = function () {
        var asid = $("#hidId").val();
        $scope.Error = "";
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Customer/AssuredReturn/GetAssuredReturnByID',
            params: { id: asid }
        }).success(function (data, status, headers, config) {
            $scope.Payment = data;
            var PID = $scope.Payment.PropertyID;
            var SID = $scope.Payment.SaleID;
            $http({
                method: 'Get',
                url: '/Payment/GetFlatSaleBySaleID',
                params: { saleid: SID }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                $http({  // 
                    method: 'Get',
                    url: '/Customer/AssuredReturn/GetPayment',
                    params: { saleid: $scope.Sale.Sale[0].SaleID }
                }).success(function (data, status, headers, config) {
                    $scope.PaidPayment = data;
                    var total = 0;
                    for (var i = 0; i < $scope.PaidPayment.length; i++) {
                        var list = $scope.PaidPayment[i];
                        total += list.Amount;
                    }
                    $scope.TotalAmount = Math.round(total);
                    $http({
                        method: 'Get',
                        url: '/Customer/Info/GetCustomerBySaleID',
                        params: { saleid: $scope.Sale.Sale[0].SaleID }
                    }).success(function (data, status, headers, config) {
                        $scope.Customer = data;
                        $scope.CustomerName = data.AppTitle + " " + data.FName + " " + data.MName + " " + data.LName;
                    })
                })
            })
        });

    }
    $scope.SaveAssuredPayment = function () {
        var vli = ValidateSubmitPayment();
        if (vli == false) {
            $('#loading').hide();
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            var asid = $("#hidId").val();
            alert(asid);
            $.ajax({
                method: 'POST',
                url: '/Customer/AssuredReturn/SaveAssuredReturnPayment',
                data: { Saleid: $("#SaleID").val(), Flatname: $("#FlatName").val(), PaymentMode: $("#PaymentMode").val(), ChequeNo: $("#ChequeNo").val(), ChequeDate: $("#chequeDate").val(), BankName: $("#BankName").find(":selected").text(), BankBranch: $("#BankBranch").val(), Remarks: $("#Remarks").val(), PayDate: $("#paymentDate").val(), Amtrcvdinwrds: $("#PaymentAmountInWords").val(), ReceivedAmount: $("#ReceivedAmount").val(), IsPrint: $("#chkPrint").is(":checked"), IsEmailSent: $("#chkSendEmail").is(":checked"), EmailTo: $("#txtSendEmail").val(), CustomerName: $("#hidCustomerName").val(), CustomerID: $("#hidCustomerID").val(), AssuredID: asid },
            }).success(function (data) { //Showing success message $scope.status = "The Person Saved Successfully!!!";
                if (data != "No") {
                    alert("Payment Details has been saved successfully!");
                    // OPaymentConfirm();
                    if ($("#chkPrint").is(":checked") == true)
                        window.open(data, '_blank');
                    $('#loading').hide();
                }
                else if (data = "Found") {
                    alert("Payment already saved.");
                    $scope.Error = 'Unable to save Payment Details: ';
                    $('#loading').hide();
                }
                $('#btnSubmitPayment').show();
                $('#loading').hide();
                // $scope.Error = 'Payment Details saved successfully.';
            }).error(function (error) {
                alert("Payment Details Not Saved !!!");
                $scope.Error = 'Unable to save Payment Details: ' + error.message;
                $('#btnSubmitPayment').show();
                $('#loading').hide();
            }).then(function () {
                $('#loading').hide();
            });
        }
        // end save payment.
    }

    $scope.SearchAssuredPayment = function () {
        $('#loading').show();
        var pname = $("#propertyname").val();
        var pid = $("#PropertyID").find(":selected").val();
        var status = $("#payStatus").find(":selected").val();
        $http({
            method: 'Get',
            url: '/Customer/AssuredReturn/SearchAssuredPayment',
            params: { PID: pid, propertyname: pname, status: status }
        }).success(function (data, status, headers, config) {
            $scope.SearchList = data;
            var gtotal = 0;
            for (var i = 0; i < $scope.SearchList.length; i++) {
                var list = $scope.SearchList[i];
                gtotal += list.Amount;
            }
            $scope.TotalSearchAmount = Math.round(gtotal);
            $('#loading').hide();
        }).then(function () {
            $('#loading').hide();
        })

    }

    function ValidateSubmitPayment() {
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

        if ($("#PaymentAmountInWords").val() == "") {
            vl = false;
            message += "Please insert Amount in words.";
        }

        $("#ErrorMessage").html(message);
        return vl;
    }
    function ValidateAssuredCheque(cno, cdate) {
        var vl = true;
        var message = "";
        if ($("#ReceivedAmount").val() == "") {
            vl = false;
            message += "Insert receive amount. <br/>";
        }
        if (cno == "") {
            vl = false;
            message += "Insert Cheque No. <br/>";
        }
        if (cdate == "") {
            vl = false;
            message += "Insert Cheque Date <br/>";
        }
        $("#ErrorMessage").html(message);
        return vl;
    }


    $('#loading').hide();
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