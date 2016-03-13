var myApp = angular.module('TransferApp', []);
myApp.controller('TransferController', function ($scope, $http, $filter) {
    $('#loading').show();
    var orderBy = $filter('orderBy');

    //$http.get('/flat/GetPropertyList/').success(function (response) { $scope.Properties = response; });
    $scope.TransferInit = function () {
        $('#loading').show();
        GetLoadTowerFlat();
        var sid = $("#SaleId").val();

        if (sid != "0") {
            $("#dvSearch").hide();
            $("#dvStep").show();
            $http({
                method: 'Get',
                url: '/Payment/GetFlatSaleBySaleID',
                params: { saleid: sid }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                var ddate = new Date(parseInt($scope.Sale.Sale[0].SaleDate.substr(6)));// value.DueDate;
                var mth = ddate.getMonth() + 1;
                if (mth < 10) mth = "0" + mth;
                $scope.SaleDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();
                $http({
                    method: 'Get',
                    url: '/Customer/Transfer/GetPropertyInfoByFlatID',
                    params: { fid: data.Sale[0].FlatID, saleid: sid }
                }).success(function (data) {
                    $scope.Pro = data;
                    $("#PlanType").find(":selected").val(data.PlanID);
                    $("#PlanType").find(":selected").text(data.PlanName);
                    $("#lblFlatNo").html(data.FlatName);
                    $("#lblSaleDate").html($scope.SaleDate)
                    $("#lblPlanName").text(data.PlanName);
                    $("#hidPlanType").val(data.PlanName);

                    $('#loading').hide();
                }).then(function () {
                    $('#loading').hide();
                })
            })
        }
        else {
            // visisble search property fields
            $("#dvSearch").show();
            $("#dvStep").hide();
            $('#loading').hide();
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
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetFlatByTowerID',
            params: { towerid: $scope.Flat.TowerID }
        }).success(function (data) {
            $scope.FlatList = data;
            $('#loading').hide();
        })
    }
    $scope.SearchPropertyForTrans = function () {
        $('#loading').show();
        var pname = $("#propertyname").val();
        var pid = $("#PropertyID").find(":selected").val();

        $http({
            method: 'Get',
            url: '/Customer/Transfer/GetSaleIDByPName',
            params: { PID: pid, propertyname: pname }
        }).success(function (data, status, headers, config) {
            $scope.SaleID = data;
            $("#SaleId").val(data);



            $http({
                method: 'Get',
                url: '/Payment/GetFlatSaleBySaleID',
                params: { saleid: data }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                var ddate = new Date(parseInt($scope.Sale.Sale[0].SaleDate.substr(6)));// value.DueDate;
                var mth = ddate.getMonth() + 1;
                if (mth < 10) mth = "0" + mth;
                $scope.SaleDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();
                $http({
                    method: 'Get',
                    url: '/Customer/Transfer/GetPropertyInfoByFlatID',
                    params: { fid: data.Sale[0].FlatID, saleid: $scope.SaleID }
                }).success(function (data) {

                    $http({
                        method: 'Get',
                        url: '/Payment/GetTotalPayment',
                        params: { saleid: $scope.SaleID }
                    }).success(function (data, status, headers, config) {
                        $scope.Amount = data;
                    });

                    $scope.Pro = data;
                    $("#PlanType").find(":selected").val(data.PlanID);
                    $("#PlanType").find(":selected").text(data.PlanName);
                    $("#lblFlatNo").html(pname);
                    $("#lblSaleDate").html($scope.SaleDate)
                    $("#lblPlanName").text(data.PlanName);
                    $("#hidPlanType").val(data.PlanName);

                    $('#loading').hide();
                    $("#dvSearch").hide();
                    $("#dvStep").show();
                }).then(function () {
                    $('#loading').hide();
                    $("#dvSearch").hide();
                    $("#dvStep").show();
                })
            })
        }).then(function () {
            $('#loading').hide();
        })
    }

    $scope.SaveCustomer = function () {

        var vli = ValidateSaveCustomer();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            $('#btnSubmit').hide();
            if (typeof $scope.newCust != "undefined") {
                $scope.newCust.SaleDate = $("#saleDate").val();
                $scope.newCust.DateOfBirth = $("#AppDOB").val();
                $scope.newCust.CoDOB = $("#CoAppDOB").val();
                $scope.newCust.SecCoDOB = $("#SecCoDOB").val();
                $scope.newCust.AppFName = $("#fNmae").val();
                $scope.newCust.AppMName = $("#mName").val();
                $scope.newCust.AppLName = $("#lName").val();
                $scope.newCust.PlanName = $("#PlanType").find(":selected").text();
                $scope.newCust.OldCustomerID = $("#OldCustomerID").val();
                $scope.newCust.OldPlanType = $("#OldPlanType").val();
                $scope.newCust.PropertyID = $("#PropertyID").val();
                $scope.newCust.PropertyTypeID = $("#PropertyTypeID").val();
                $scope.newCust.PropertySizeID = $("#PropertySizeID").val();
                $scope.newCust.SaleID = $("#SaleId").val();
                $scope.newCust.PlanID = $("#PlanType").find(":selected").val();
                $scope.newCust.IsChangeInstallmentPlan = $("#chkInstall").is(":checked");
                $scope.newCust.SrtSaleDate = $("#saleDate").val();
            }

            $http({
                method: 'POST',
                url: '/Customer/Transfer/SaveCustomer',
                data: JSON.stringify($scope.newCust),
                headers: { 'Content-Type': 'application/JSON' }
            }).success(function (data) { //Showing success message $scope.status = "The Person Saved Successfully!!!";
                //Updating persons Model
                alert("Customer has been saved successfully");
                //  GetSaleDetailByFlatId();
                $('#loading').hide();
                $('#btnSubmit').show();
                if ($("#chkInstall").is(":checked")) {
                    $("#step2").addClass("active");
                    $("#step2").css("display: block !important;")
                    $("#step1").removeClass("active");
                }
                else {
                }
            }).error(function (error) {
                //Showing error message
                $scope.status = 'Unable to save Customer: ' + error.message;
                $('#loading').hide();
                $('#btnSubmit').show();
            });
        }
    }

    $scope.GenerateInstallment = function (saledate, saleprice, ddlInstallment, ddlInterval, PlanType, plcPrice) {
        var vli = ValidatePaymentGen();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            saledate = document.getElementById(saledate).innerHTML;
            saleprice = document.getElementById(saleprice).value;
            ddlInstallment = document.getElementById(ddlInstallment).value;
            ddlInterval = document.getElementById(ddlInterval).value;
            PlanType = document.getElementById(PlanType).value;
            var PlanTypeID = document.getElementById('hidPlanType').value;
            plcPrice = document.getElementById(plcPrice).value;
            var sid = $("#SaleId").val();
            var fid = $("#FlatID").val();
            $http({
                method: 'Post',
                url: '/Customer/Transfer/GetInstallments',
                data: { saledate: saledate, saleprice: saleprice, ddlInstallment: ddlInstallment, ddlInterval: ddlInterval, PlanType: PlanType, plcPrice: plcPrice, saleID: sid, flatid: fid, PlanTypeID: PlanTypeID },
            }).success(function (result) {
                document.getElementById("divinstallment").innerHTML = result;
                $('#loading').hide();
                //window.location.href = urlPath + '/';
            }).error(function (err) {
                alert('Error')
                $('#loading').hide();
                if (err.responseText == "success") {
                    alert("Error, Please try again.");
                }
                else {
                    document.getElementById("divinstallment").innerHTML = err.responseText;
                }
            }).then(function () {
                $('#loading').hide();
            })
        }
    }

    $scope.SaveInstallment = function (saledate, saleprice, ddlInstallment, ddlInterval, PlanType) {
        $('#loading').show();
        getid();
        saledate = document.getElementById(saledate).innerHTML;
        saleprice = document.getElementById(saleprice).value;
        ddlInstallment = document.getElementById(ddlInstallment).value;
        ddlInterval = document.getElementById(ddlInterval).value;
        EventName = document.getElementById("EventId").value;
        var sid = $("#SaleId").val();
        var fid = $("#FlatID").val();
        var bspPercentage = "";
        $('.bsppercentage').each(function () {
            bspPercentage += document.getElementById(this.id).value + ",";
        });
        $('.dueamount').each(function () {
            document.getElementById("EventAmount").value += parseFloat(document.getElementById(this.id).innerHTML) + ",";
        });
        var evName = document.getElementById("EventName").value;
        alert(evName)
        $http({
            method: 'Post',
            url: '/Customer/Transfer/SaveInstallments',
            data: { saledate: saledate, saleprice: saleprice, salepriceword: '', ddlInstallment: ddlInstallment, ddlInterval: ddlInterval, EventName: EventName, bspPercentage: bspPercentage, saleID: sid, flatID: fid, EvName: evName, Amount: document.getElementById("EventAmount").value },
        }).success(function (result) {
            alert("Data is added successfully.");
            $(".row .centre_sec").css('display', 'none');
            $(".row").find(".centre_sec").css('display', 'none');
        }).error(function (err) {
            if (err.responseText == "success") {
                alert("Data is added successfully.");

                $(".row .centre_sec").css('display', 'none');
                $(".row").find(".centre_sec").css('display', 'none');
            }
            else {
                $(".row .centre_sec").css('display', 'none');
                $(".row").find(".centre_sec").css('display', 'none');
                document.getElementById("divinstallment").innerHTML = err.responseText;
            }
            $('#loading').hide();
        }).then(function () {
            $('#loading').hide();
        })
    }

    $scope.checkandFillDetail = function () {
        if ($("#chkInstall").is(":checked")) {
        }
        else {
            $("#step1").show();
        }
    }
    // Refund functions
    $scope.RefundInit = function () {
        $http.get('/Sale/Payment/GetBank/').success(function (response) { $scope.Banks = response; });
        //$('#loading').show();
        GetLoadTowerFlat();
        var sid = $("#SaleID").val();
        if (sid != "0") {
            $("#dvSearch").hide();
            $("#dvShow").show();
            $http({
                method: 'Get',
                url: '/Sale/Payment/GetFlatSaleBySaleID',
                params: { saleid: sid }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                var ddate = new Date(parseInt($scope.Sale.Sale[0].SaleDate.substr(6)));// value.DueDate;
                var mth = ddate.getMonth() + 1;
                if (mth < 10) mth = "0" + mth;
                $scope.SaleDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();
                $scope.FlatID = data.Sale[0].FlatID;
                $http({
                    method: 'Get',
                    url: '/Customer/Transfer/GetPropertyInfoByFlatID',
                    params: { fid: data.Sale[0].FlatID, saleid: $scope.SaleID }
                }).success(function (data) {
                    $scope.Pro = data;
                    $("#PlanType").find(":selected").val(data.PlanID);
                    $("#PlanType").find(":selected").text(data.PlanName);
                    $("#lblFlatNo").html(pname);
                    $("#lblSaleDate").html($scope.SaleDate)
                    $("#lblPlanName").text(data.PlanName);
                    $("#hidPlanType").val(data.PlanName);

                    $('#loading').hide();
                    $("#dvSearch").hide();
                    $("#dvShow").show();
                    $http({
                        method: 'Get',
                        contentType: "application/json; charset=utf-8",
                        url: '/sale/Payment/EditSearchPayment',
                        params: { Flatid: $scope.FlatID },
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
                        inWords(total);
                    }).then(function () {
                        $('#loading').hide();
                        $("#dvSearch").hide();
                        $("#dvShow").show();
                    })
                })
            })
        }
        else {
            $("#dvSearch").show();
            $("#dvShow").hide();
        }
        $('#loading').hide();
    }
    $scope.SearchPropertyForRefund = function () {
        $('#loading').show();
        //var pname = $("#propertyname").val();
        //var pid = $("#PropertyID").find(":selected").val();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
        $http({
            method: 'Get',
            url: '/Customer/Transfer/GetSaleIDByPName',
            params: { PID: pid }
        }).success(function (data, status, headers, config) {
            $scope.SaleID = data;
            if (data != 0) {

            $("#SaleID").val(data);
            $http({
                method: 'Get',
                url: '/sale/Payment/GetFlatSaleBySaleID',
                params: { saleid: data }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                var ddate = new Date(parseInt($scope.Sale.Sale[0].SaleDate.substr(6)));// value.DueDate;
                var mth = ddate.getMonth() + 1;
                if (mth < 10) mth = "0" + mth;
                $scope.SaleDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();
                $scope.FlatID = data.Sale[0].FlatID;
                $http({
                    method: 'Post',
                    url: '/Customer/Transfer/CheckRefundProperty',
                    data: { SaleID: saleid },
                }).success(function (data) {
                    var ata = data.replace('"', '').replace('"', '');
                    if (ata == "Booked" || ata == "Sale") {
                        $http({
                            method: 'Get',
                            url: '/Customer/Transfer/GetPropertyInfoByFlatID',
                            params: { fid: data.Sale[0].FlatID, saleid: $scope.SaleID }
                        }).success(function (data) {
                            $scope.Pro = data;
                            //$("#PlanType").find(":selected").val(data.PlanID);
                            //$("#PlanType").find(":selected").text(data.PlanName);
                            //$("#lblFlatNo").html(pname);
                            //$("#lblSaleDate").html($scope.SaleDate)
                            //$("#lblPlanName").text(data.PlanName);
                            //$("#hidPlanType").val(data.PlanName);

                            $('#loading').hide();
                            $("#dvSearch").hide();
                            $("#dvShow").show();
                            $http({
                                method: 'Get',
                                contentType: "application/json; charset=utf-8",
                                url: '/sale/Payment/EditSearchPayment',
                                params: { Flatid: $scope.FlatID },
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
                                inWords(total);
                            }).then(function () {
                                $('#loading').hide();
                                $("#dvSearch").hide();
                                $("#dvShow").show();
                            })
                        })
                    }
                    else {
                        alert("This property not available for refund. not sale yet.");
                    }
                })
               
            })
            }
            else {
                alert("This property not available for refund, not Sale yet.");
            }
        }).then(function () {
            $("#loading").hide();
        })
            
       
    }

    $scope.RefundProperty = function () {
        // Save refund
        var vli = ValidateSubmitPayment();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            var flatid = $("#FlatID").val();
            var saleid = $("#SaleID").val();
            var amount = $("#ReceivedAmount").val();
            var date = $("#paymentDate").val();
            var paymode = $("#PaymentMode").find(":selected").val();
            var chequeno = $("#ChequeNo").val();
            var chequedate = $("#chequeDate").val();
            var bankname = $("#BankName").find(":selected").text();
            var branch = $("#BankBranch").val();
            var ramark = $("#Remarks").val();
            var fname = $("#FlatName").val();

            if ($("#chkConfirm").is(":checked")) {
                $http({
                    method: 'Post',
                    url: '/Customer/Transfer/CheckRefundProperty',
                    data: { SaleID: saleid },
                }).success(function (data) {
                    var ata = data.replace('"', '').replace('"', '');
                    if (ata == "Booked" || ata=="Sale") {
                        $http({
                            method: 'Post',
                            url: '/Customer/Transfer/RefundPropertySave',
                            data: { FlatID: flatid, SaleID: saleid, Amount: amount, Date: date, PayMode: paymode, ChequeNo: chequeno, ChequeDate: chequedate, BankName: bankname, Branch: branch, Remarks: ramark, FlatName: fname },
                        }).success(function (data) {
                            $scope.Status = data;
                            if (data != 'No')
                                alert("Property refund successfully.")
                            else alert("Invalid information, please try again.");
                            $('#loading').hide();
                        })
                    }
                    else {
                        alert("Property is available on sale. can't  refund.");
                        $('#loading').hide();
                    }
                })
            }
            else {
                $('#loading').hide();
                alert("Please check terms and conditions for property refund.");
            }
        }
    }
    $scope.orderSearchList = function (predicate, reverse) {
        $scope.SearchList = orderBy($scope.SearchList, predicate, reverse);
    };
    function getid() {
        document.getElementById("EventId").value = "";
        $('.dropdwn').each(function () {
            if (this.value) {
                document.getElementById("EventId").value += this.value + ",";
                document.getElementById("EventName").value += $(this).find(":selected").text() + ",";
            }
        });
    }

    function ValidatePaymentGen() {
        var vl = true;
        var message = "";
        if ($("#txtsaleprice").val() == "") {
            vl = false;
            message += "Insert Sale Price of Property. <br/>";
        }
        $("#ErrorMessage").html(message);
        return vl;
    }
    function ValidateSaveCustomer() {
        var vl = true;
        var message = "";

        if ($("#PlanType").find(":selected").text() == "") {
            vl = false;
            message += "Select Plan Type of property.<br/>";
        }
        if ($("#fNmae").val() == "") {
            vl = false;
            message += "Insert Name of Applicant.<br/>";
        }
        if ($("#PanNo").val() == "") {
            vl = false;
            message += "Insert Pan Card No.<br/>";
        }
        if ($("#saleDate").val() == "") {
            vl = false;
            message += "Please insert Date.";
        }

        $("#ErrorMessage").html(message);
        return vl;
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
    $('#loading').hide();
})


function calculatebsp(value, id) {
    var txtID = id;
    var initialBSP = id.split('_')[1];
    id = id.split('_')[0];
    // if (value != initialBSP) {
    var val = document.getElementById(txtID).value;
    if (val != "") {
        var damt = document.getElementById("txtsaleprice").value;
        var dueamount = (parseFloat(damt) * val) / 100;

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
    // }
}


function EnalbleButton() {
    $(".row .centre_sec").css('display', '');
    $(".row").find(".centre_sec").css('display', '');
}

function PlanTypeChange(obj) {
    var sv = $(obj).val();
    var ps = $(obj).find(":selected").text();
    if (sv == '3') {
        $("#normaldiv").show();
    }
    else {
        $("#normaldiv").hide();
    }
    $("#hidPlanType").val(ps)
    $("#lblPlanName").text(ps);
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