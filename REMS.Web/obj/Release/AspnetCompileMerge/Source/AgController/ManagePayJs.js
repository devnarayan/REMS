var myApp = angular.module('ManageApp', []);
myApp.controller('ManageController', function ($scope, $http, $filter) {
    var orderBy = $filter('orderBy');

    //$http.get('/flat/GetPropertyList/').success(function (response) { $scope.Properties = response; });
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
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetFlatByTowerID',
            params: { towerid: $scope.Flat.TowerID }
        }).success(function (data) {
            $scope.FlatList = data;

        })
    }

    // CancelPayments.cshtml
    $scope.TowerChange1 = function () {
        $("#loading").show();
        if ($scope.Flat.TowerID == "All" || $scope.Flat.TowerID == "Customer Name" || $scope.Flat.TowerID == "CancelDate" || $scope.Flat.TowerID == "PaymentDate" || $scope.Flat.TowerID == "This Month" || $scope.Flat.TowerID == "Last 7 Days")
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
                url: '/Sale/Manage/PaymentCancelSave',
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

    $scope.CancelPaymentInit = function () {

    }

    $scope.SearchCancelPayment = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
        var propertyName = $("#PropertyTypeID :selected").text()
        var search = $("#searchby").val();
        var propertyid = $("#PropertyID").val();
        var propertySubTypeID = $("#PropertyTypeID").val();
        var proSize = $("#PropertySizeID").val();
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/Sale/Manage/PaymentCancelSearch',
            params: { search: search, Flatid: pid, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
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

    $scope.SearchBackupReceipt = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
       // var propertyName = $("#PropertyTypeID :selected").text()
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
            url: '/Sale/Manage/SearchBackupReceipt',
            params: { search: search, FlatId: pid, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
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

    $scope.BackupReceiptExport = function () {
        $('#loading').show();
        var yourarray = [];
        $('input.selectone:checkbox:checked').each(function () {
            yourarray.push($(this).val());
        });
        $http({
            method: 'Post',
            url: '/Sale/Manage/ExportReceipt/',
            data: { transids: yourarray }
        }).success(function (data) {
            $('#loading').hide();
            var v = data;
            window.open("/PDF/Temp/" + v, "_blank");
        })
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
                url: '/Sale/Manage/ExportAndMailReceipt/',
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
                url: '/Sale/Manage/BackupReceiptSendMail/',
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

        var murl = "/Sale/Manage/BackupReceiptPrintAction/" + yourarray;
        window.open(murl, '_blank');
    }

    $scope.GetBackupReceiptPrintInit = function () {
        var id = $("#hidID").val();       
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/Sale/Manage/BackupReceiptPrint',
            params: { transids: id },
            dataType: "json"
        }).success(function (data) {
            $scope.PrintReceiptList = data;
            $http({
                method: 'Get',
                url: '/Sale/Manage/BackupReceiptPrint',
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
            url: '/Sale/Manage/SearchBackupReceiptData',
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
        });
    }

    $scope.ExportTest = function () {
        var yourarray = [];
        $('input.selectone:checkbox:checked').each(function () {
            yourarray.push($(this).val());
        });
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/Sale/Manage/ExportTest',
            params: { transids: 1 },
            dataType: "json"
        }).success(function (data) {
            // $scope.PrintReceiptList = data;
        })
    }

    $scope.orderSearchList = function (predicate, reverse) {
        $scope.SearchList = orderBy($scope.SearchList, predicate, reverse);
    };

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