var myApp = angular.module('BIApp', []);
myApp.controller('ReportController', function ($scope, $http, $filter) {
    //$('#loading').show();
    var orderBy = $filter('orderBy');

    $scope.Error = "";

    //$http.get('/flat/GetPropertyList/').success(function (response) { $scope.Properties = response; });
    //$http.get('/flat/GetBanks/').success(function (response) { $scope.Banks = response; });

    // Tower and Flat property bind on page init
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
        //$('#loading').show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetFlatByTowerID',
            params: { towerid: $scope.Flat.TowerID }
        }).success(function (data) {
            $scope.FlatList = data;
            //$('#loading').hide();
        })
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

    $scope.SearchPayment = function () {
       // $("#search1").val("1");
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();       
        var search = $("#searchby").val();       
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/Payment/SearchPayment',
            params: { search: search, Flatid: pid, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
            //params: { PropertyID: proID, PropertyTypeID: proTID, PropertySizeID: proSID, PropertyTypeName: $("#PropertyTypeID :selected").text(), search1: search1, searchby: searchBy, sortby: sortBy, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
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

    $scope.SearchRefundProperty = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);

        var pid = $("#hidFlatID").val();
        $http({
            method: 'Get',
            url: '/Customer/Transfer/GetSaleIDByPName',
            params: { PID: pid }
        }).success(function (data, status, headers, config) {
            $scope.SaleID = data;
            $("#SaleID").val(data);
            $http({
                method: 'Get',
                contentType: "application/json; charset=utf-8",
                url: '/BI/Report/RefundPropertySearch',
                params: { SaleID: data },
                dataType: "json"
            }).success(function (data) {
                $scope.SearchList = data;
                var total = 0;
                for (var i = 0; i < $scope.SearchList.length; i++) {
                    var list = $scope.SearchList[i];
                    total += list.RefundAmount;
                }
                $scope.TotalSaleAmount = total;
                $('#loading').hide();
            })

        });

    }

    $scope.SearchRefundPropertyByCheque = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
       // var propertyName = $("#PropertyTypeID :selected").text()
        var search = $("#searchby").val();
      //  var propertyid = $("#PropertyID").val();
      //  var propertySubTypeID = $("#PropertyTypeID").val();
      //  var proSize = $("#PropertySizeID").val();
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/BI/Report/RefundPropertySearchByCheque',
            params: { search: search, FaltId: pid, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
            dataType: "json"
        }).success(function (data) {
            $scope.SearchList = data;
            var total = 0;
            for (var i = 0; i < $scope.SearchList.length; i++) {
                var list = $scope.SearchList[i];
                total += list.RefundAmount;
            }
            $scope.TotalSaleAmount = total;

            $('#loading').hide();
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
    //$scope.SearchAssuredReturn = function () {
    //    $('#loading').show();
    //    $("#hidFlatID").val($scope.Flat.FlatID);
    //    var pid = $("#hidFlatID").val();
    //    //var pname = $("#propertyname").val();
    //    //var pid = $("#PropertyID").find(":selected").val();
    //    //var status = $("#payStatus").find(":selected").val();
    //    //var cdate = $("#ChequeDate").val();
    //    //var cdateTo = $("#ChequeDateTo").val();
    //    $http({
    //        method: 'Get',
    //        url: '/Customer/AssuredReturn/SearchAssuredReturn',
    //        params: { PID: pid, propertyname: pname, status: status, chequeDate: cdate, chequeDateTo: cdateTo }
    //    }).success(function (data, status, headers, config) {
    //        $scope.SearchList = data;
    //        var gtotal = 0;
    //        for (var i = 0; i < $scope.SearchList.length; i++) {
    //            var list = $scope.SearchList[i];
    //            gtotal += list.Amount;
    //        }
    //        $scope.TotalSearchAmount = Math.round(gtotal);
    //        $('#loading').hide();
    //    })

    //}
    //$("#dvDate").show();
    //$("#dvPName").hide();
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

    $scope.SearchTransferProperty = function () {
        $('#loading').show();
       
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
       // var propertyName = $("#PropertyTypeID :selected").text()
        var search = $("#searchby").val();
       // var propertyid = $("#PropertyID").val();
       // var propertySubTypeID = $("#PropertyTypeID").val();
       // var proSize = $("#PropertySizeID").val();
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/BI/Report/TransferPropertySearch',
            params: { search: search,FlatId: pid,datefrom: datefrom, dateto: dateto, searchtext: searchtext },
            dataType: "json"
        }).success(function (data) {
            $scope.SearchList = data;
            var total = 0;
            for (var i = 0; i < $scope.SearchList.length; i++) {
                var list = $scope.SearchList[i];
                total += list.TransferAmount;
            }
            $scope.TotalSaleAmount = total;

            $('#loading').hide();
        });
    }

    //$scope.MonthlySaleSearchProperty = function () {
    //    $('#loading').show();
    //     $("#hidFlatID").val($scope.Flat.FlatID);

    //    var pid = $("#hidFlatID").val();
    //    $http({
    //        method: 'Get',
    //        url: '/Customer/Transfer/GetSaleIDByPName',
    //        params: { PID: pid }
    //    }).success(function (data, status, headers, config) {
    //        $scope.SaleID = data;
    //        $("#SaleID").val(data);
    //    $http({
    //        method: 'Get',
    //        contentType: "application/json; charset=utf-8",
    //        url: '/Home/PropertySearch',
    //        params: { SaleID: data },
    //        dataType: "json"
    //    }).success(function (data) {
    //        $scope.SearchList = data;
    //        var total = 0;
    //        for (var i = 0; i < $scope.SearchList.length; i++) {
    //            var list = $scope.SearchList[i];
    //            total += list.SaleRate;
    //        }
    //        $scope.TotalSaleAmount = total;
    //        $('#loading').hide();
    //    })
    //    });
    //}
    // Change Property Name and fill propertyType.

    $scope.MonthlySaleSearchProperty = function () {
        $('#loading').show();
      
        if ($scope.Flat.FlatID == "undefined") {
            $("#hidFlatID").val(0);
        }
        else {
            $("#hidFlatID").val($scope.Flat.FlatID);
        }
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
            url: '/Home/PropertySearch',
            params: { search: search, FlatId: pid, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
            //params: { propertyName: propertyName, search: search, propertyid: propertyid, propertySubTypeID: propertySubTypeID, proSize: proSize, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
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

    $scope.EditSearchPayment = function () {
        $('#loading').show();
        var proID = $("#PropertyID").val();
        var searchtext = $("#searchtext").val();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/Payment/EditSearchPayment',
            params: { Flatid: pid },
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
            params: { search: search,Flatid:pid, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
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
    $scope.OtherPaymentSearch = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
        //var proID = $("#PropertyID").val();
        //var searchtext = $("#searchtext").val();
        //var payfor = $("#PaymentFor").find(":selected").text();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/OtherPayment/EditSearchPayment',
            params: { FlatId: pid},
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
    $scope.SearchCancelPaymentOther = function () {
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
            url: '/OtherPayment/PaymentCancelSearch',
            params: { search: search,FlatId:pid, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
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
    $scope.SearchBroker = function () {
        $('#loading').show();
        var query = $("#Query").val()
        var srch = $("#Search").find(":selected").val();
        $http({
            method: 'Get',
            url: '/Customer/Broker/SearchBroker',
            params: { search: srch, query: query }
        }).success(function (data, status, headers, config) {
            $scope.SearchList = data;
            $('#loading').hide();
        })
    }

    // Approve Payment form

    $scope.ApprovePaymentBrokerInit = function () {
        $http({
            method: 'Get',
            url: '/Customer/Broker/GetAllBroker'
        }).success(function (data) {
            $scope.BrokerList = data;
        })
    }

    $scope.ApprovePaymentSearch = function () {
        var status = $("#StatusSr").find(":selected").val();
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Customer/Broker/GetBrokersPropertiesSearch',
            params: { brokerid: $("#BrokerName").find(":selected").val(), Status: status }
        }).success(function (data, status, headers, config) {
            $scope.BrokerPropertyList = data;
            var totall = 0;
            for (var i = 0; i < $scope.BrokerPropertyList.length; i++) {
                var list = $scope.BrokerPropertyList[i];
                totall += list.BrokerAmount;
            }
            $scope.TotalAmount = Math.round(totall);
            $('#loading').hide();
        })
    }
    $scope.BrokerPaymentSearch = function () {
        var status = $("#StatusSr").find(":selected").val();
        $http({
            method: 'Get',
            url: '/Customer/Broker/GetPaidAmountToBrokerByBrokerID',
            params: { brokerID: $("#BrokerName").find(":selected").val() }
        }).success(function (data) {
            $scope.BrokerPaidList = data;
            var total = 0;
            for (var i = 0; i < $scope.BrokerPaidList.length; i++) {
                var list = $scope.BrokerPaidList[i];
                total += list.AmountPaid;
            }
            $scope.TotalAmountPaid = Math.round(total);
        })
    }
    $scope.SearchPropertyForBroker = function () {
        $('#loading').show();
        var pid = $("#BrokerName").find(":selected").val();
        $http({
            method: 'Get',
            url: '/Customer/Broker/GetBrokersProperties',
            params: { brokerid: pid }
        }).success(function (data, status, headers, config) {
            $scope.BrokerPropertyList = data;
            $scope.BrokerPaidList = data;
            var total = 0;
            for (var i = 0; i < $scope.BrokerPaidList.length; i++) {
                var list = $scope.BrokerPaidList[i];
                total += list.AmountPaid;
            }
            $scope.TotalAmountPaid = Math.round(total);
            $('#loading').hide();
            $("#dvSearch").hide();
            $("#dvStep").show();
        }).error(function (ex) {
            alert(ex)
        })
    }

    //Email, Print and Export 
    $scope.ReportSendMail = function () {
        var vli = ValidateBackupReceiptEmail();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            $scope.ReportContent = $("#rptTableContent").html();
            $http({
                method: 'Post',
                url: '/BI/Report/MailReport',
                data: { ReportContent: $scope.ReportContent, emailid: $("#reportmail").val() }
            }).success(function (data) {
                alert("Report mailed successfully!")
                $('#loading').hide();
            })
        }
    }
    $scope.ReportExport = function () {
        $('#loading').show();
        $scope.ReportContent = $("#rptTableContent").html();
        $http({
            method: 'Post',
            url: '/BI/Report/ExportReport',
            data: { ReportContent: $scope.ReportContent }
        }).success(function (data) {
            alert(data);
            $('#loading').hide();
            window.open(data, "_blank");

        })
    }

    $scope.ReportPrint = function () {
        localStorage.setItem("ReportContent", $("#rptTableContent").html());
        //alert($("#rptTableContent").html());
        window.open("/BI/Report/PrintReport", "_blank");
    }
   
    $scope.PrintReportInint = function () {       
       
        $("#PrintReportdata").html(localStorage.getItem("ReportContent"))
        window.print();
    }



    function ValidateBackupReceiptEmail() {
        var vl = true;
        var message = "";

        if ($("#reportmail").val() == "") {
            vl = false;
            message += "Please insert Email ID.<br/>";
        }
        if ($("#reportmail").val() != "") {
            var emailReg = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
            var valid = emailReg.test($("#reportmail").val());

            if (!valid) {
                vl = false;
                message += "Invalid Email ID.<br/>";
            }
        }
        $("#ErrorMessage").html(message);
        return vl;
    }



    $scope.SearchPendingInstallment = function () {
        $('#loading').show();
        //var propertyName = $("#PropertyTypeID :selected").text()
        $("#hidFlatID").val($scope.Flat.FlatID);
        var pid = $("#hidFlatID").val();
        var search = $("#searchby").val();
        var searchtext = $("#searchtext").val();
        if (search == "PropertyName") {
            if (searchtext.trim() == "") {
                alert("Enter Propery Name ");
                $('#loading').hide();
                return;
            }
        }


        //var propertyid = $("#PropertyID").val();



        //if (propertyid == "? undefined:undefined ?") {
        //    alert("Select Property");
        //    $('#loading').hide();
        //    return;
        //}
        //var propertySubTypeID = $("#PropertyTypeID").val();
        //var proSize = $("#PropertySizeID").val();
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/BI/Report/SearchPendingInstallment',
            params: { search: search, FlatId: pid, datefrom: datefrom, dateto: dateto, searchtext: searchtext },
            dataType: "json"
        }).success(function (data) {
            $scope.SearchList = data;
            var total = 0;
            var paidamount = 0;
            var dueamount = 0;
            for (var i = 0; i < $scope.SearchList.length; i++) {
                var list = $scope.SearchList[i];
                total += list.SaleRate;
                paidamount += list.PaidAmount
                dueamount += (list.DueAmount);
            }
            $scope.TotalAmount = total;
            $scope.TotalPaidAmount = paidamount;
            $scope.TotalDueAmount = dueamount;
            $('#loading').hide();
        });
    }



    $scope.SearchDemandLetter = function () {
        $('#loading').show();
        //var propertyName = $("#PropertyTypeID :selected").text()
        // $("#hidFlatID").val($scope.Flat.FlatID);
        //var propertyid = $("#hidFlatID").val();
        var search = $("#searchby").val();
        var searchtext = $("#searchtext").val();
       
        var saleid = "";
        //var propertySubTypeID = $("#PropertyTypeID").val();
        //var proSize = $("#PropertySizeID").val();
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/BI/Report/SearchDemandLetter',
            params: { search: search, propertyid:$scope.Flat.FlatID, datefrom: datefrom },
            dataType: "json"
        }).success(function (data) {
            $scope.SearchList = data;
            var total = 0;
            var paidamount = 0;
            var dueamount = 0;
            for (var i = 0; i < $scope.SearchList.length; i++) {
                var list = $scope.SearchList[i];
                total += list.SaleRate;
                paidamount += list.PaidAmount
                dueamount += (list.DueAmount);
                if (i == 0) {
                    saleid = list.SaleID;
                } else {
                    saleid = saleid + "," + list.SaleID;
                }
            }
            $("#hdnSarchType").val(search);
            $scope.TotalAmount = total;
            $scope.TotalPaidAmount = paidamount;
            $scope.TotalDueAmount = dueamount;
            $('#loading').hide();
        });
    }


    $scope.GanrateLetter = function () {
        $('#loading').show();
        //var propertyName = $("#PropertyTypeID :selected").text()
         $("#hidFlatID").val($scope.Flat.FlatID);
        //var propertyid = $("#TowerID :selected").val()
        var propertyid = $("#hidFlatID").val();
        var search = $("#searchby").val();
        var searchtext = $("#searchtext").val();
        if (search == "PropertyName") {
            if (searchtext.trim() == "") {
                alert("Enter Propery Name ");
                $('#loading').hide();
                return;
            }
        }

        var allsaleID = $("#hdnSaleID").val();
        var v = "";
        $('input.selectone').each(function () {
            if ($(this).is(":checked")) {

                v = v + "," + $(this).val();

            }
        });

        //var propertyid = $("#PropertyID").val();

        //if (propertyid == "? undefined:undefined ?") {
        //    alert("Select Property");
        //    $('#loading').hide();
        //    return;
        //}
        //var propertySubTypeID = $("#PropertyTypeID").val();
        //var proSize = $("#PropertySizeID").val();
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/BI/Report/GanrateDimandLetterDimand',
            params: { search: search, propertyid: propertyid, datefrom: datefrom, saleid: v },
            dataType: "json"
        }).success(function (data) {
            $scope.SearchList = data;
            var total = 0;
            var paidamount = 0;
            var dueamount = 0;
            for (var i = 0; i < $scope.SearchList.length; i++) {
                var list = $scope.SearchList[i];
                total += list.SaleRate;
                paidamount += list.PaidAmount
                dueamount += (list.DueAmount);
            }
            $scope.TotalAmount = total;
            $scope.TotalPaidAmount = paidamount;
            $scope.TotalDueAmount = dueamount;
            $('#loading').hide();
        });
    }
    $scope.PrintLetter = function () {

        var allsaleID = $("#hdnSaleID").val();
        var v = "";
        $('input.selectone').each(function () {
            if ($(this).is(":checked")) {
                v = v + "," + $(this).val();

            }

        });
        var searchtext = $("#searchby").val();
        if (searchtext == "DemandLetter1") {
            var murl = "/BI/Report/DemandLettertPrintAction/" + v;
            window.open(murl, '_blank');
        } else {
            if (searchtext == "DemandLetter2") {
                var murl = "/BI/Report/DemandLettert2PrintAction/" + v;
                window.open(murl, '_blank');
            } else {
                if (searchtext == "DemandLetter3") {
                    var murl = "/BI/Report/DemandLettert3PrintAction/" + v;
                    window.open(murl, '_blank');
                }
            }
        }
    }

    $scope.DemandLetterPrintPrintAction = function () {
        var id = $("#hidID").val();
        $('#loading').show();

        $http({
            method: 'Get',
            url: '/BI/Report/DemandLettertPrintReport',
            params: { transactionid: id }
        }).success(function (data, status, headers, config) {
            $scope.PrintReceiptList = data;
            $http({
                method: 'Get',
                url: '/BI/Report/DemandLettertPrintReport',
                params: { transids: id },
            }).success(function (data) {
                $scope.Amount = "";
                $('#loading').hide();
                window.print();
            })
        })
    }


    //$scope.DemandLetterPrintPrintAction = function () {
    //    var id = $("#hidID").val();
    //    $('#loading').show();
    //    $http({
    //        method: 'Get',
    //        url: '/BI/Report/DemandLettertPrintReport',
    //        params: { transactionid: id }
    //    }).success(function (data, status, headers, config) {
    //        $scope.PrintReceiptList = data;
           
    //        $http({
    //            method: 'Get',
    //            url: '/BI/Report/DemandLettertPrintReport',
    //            params: { transids: id },
    //        }).success(function (data) {
    //            $scope.Amount = "";
    //            $('#loading').hide();
    //            window.print();
    //        })

    //    }).then(function () {
    //        //Print Page

    //    });
    //}



    $scope.DemandLettertPrintReport2 = function () {
        var id = $("#hidID").val();
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/BI/Report/DemandLettertPrintReport2',
            params: { transids: id },
        }).success(function (data) {
            $http({
                method: 'Get',
                url: '/BI/Report/DemandLettertPrintReport2',
                params: { transactionid: id }
            }).success(function (data, status, headers, config) {
                $scope.PrintReceiptList = data;
                $http({
                    method: 'Get',
                    url: '/BI/Report/DemandLettertPrintReport3',
                    params: { transids: id },
                }).success(function (data) {
                    $scope.Amount = "";
                    $('#loading').hide();
                    window.print();
                })

            })
        }).then(function () {
            //Print Page

        });
    }

    $scope.DemandLettertPrintReport3 = function () {
        var id = $("#hidID").val();
        $('#loading').show();
        $http({
            method: 'Get',

            url: '/BI/Report/DemandLettertPrintReport3',
            params: { transids: id },

        }).success(function (data) {

            $http({
                method: 'Get',
                url: '/BI/Report/DemandLettertPrintReport3',
                params: { transactionid: id }
            }).success(function (data, status, headers, config) {
                $scope.PrintReceiptList = data;
                $http({
                    method: 'Get',

                    url: '/BI/Report/DemandLettertPrintReport3',
                    params: { transids: id },

                }).success(function (data) {
                    $scope.Amount = "";
                    $('#loading').hide();
                    window.print();
                })
            })
        }).then(function () {
            //Print Page
        });
    }

    $scope.ViewSearchDemandLetter = function () {
        $('#loading').show();
        //var propertyName = $("#PropertyTypeID :selected").text()
        var search = $("#searchby").val();
        var searchtext = $("#searchtext").val();
        if (search == "PropertyName") {
            if (searchtext.trim() == "") {
                alert("Enter Propery Name ");
                $('#loading').hide();
                return;
            }
        }

       // var propertyid = $("#Towerid").val();
       
        var propertyid = $("#Towerid").val();
        if (propertyid == "? undefined:undefined ?") {
            alert("Select Property");
            $('#loading').hide();
            return;
        }
        var saleid = "";
        //var propertySubTypeID = $("#PropertyTypeID").val();
        //var proSize = $("#PropertySizeID").val();
        var datefrom = $("#datefrom").val();
        var dateto = $("#dateto").val();
        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/BI/Report/ViewSearchDemandLetter',
            params: { search: search, propertyid: $scope.Flat.FlatID, datefrom: datefrom },
            dataType: "json"
        }).success(function (data) {
            $scope.SearchList = data;
            var total = 0;
            var paidamount = 0;
            var dueamount = 0;
            for (var i = 0; i < $scope.SearchList.length; i++) {
                var list = $scope.SearchList[i];
                total += list.SaleRate;
                paidamount += list.PaidAmount
                dueamount += (list.DueAmount);
                if (i == 0) {
                    saleid = list.SaleID;
                } else {
                    saleid = saleid + "," + list.SaleID;
                }
            }
            $scope.TotalAmount = total;
            $scope.TotalPaidAmount = paidamount;
            $scope.TotalDueAmount = dueamount;
            $('#loading').hide();
        });
    }

    $scope.orderSearchList = function (predicate, reverse) {
        $scope.SearchList = orderBy($scope.SearchList, predicate, reverse);
    };
    $scope.orderBrokerPropertyList = function (predicate, reverse) {
        $scope.BrokerPropertyList = orderBy($scope.BrokerPropertyList, predicate, reverse);
    };
    $scope.orderBrokerPaidList = function (predicate, reverse) {
        $scope.BrokerPaidList = orderBy($scope.BrokerPaidList, predicate, reverse);
    };



    $('#loading').hide();
});