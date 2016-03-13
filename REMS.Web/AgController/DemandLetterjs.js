var myApp = angular.module('BIApp', []);
myApp.controller('DemandLetterController', function ($scope, $http, $filter) {
    //$('#loading').show();
    var orderBy = $filter('orderBy');

    $scope.Error = "";

    // Tower and Flat property bind on page init
    $scope.CreateLetterInit = function () {
        GetLoadTowerFlat();
    }
    $scope.TowerChange = function () {
        //$('#loading').show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetReservedFlatListByTowerID',
            params: { towerid: $scope.Flat.TowerID }
        }).success(function (data) {
            $scope.FlatList = data;
            //$('#loading').hide();
        })
    }
    $scope.SearchDemandLetter = function () {
        var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                TowerID: {
                    required: true
                },
                FlatID: {
                    required: true,

                },
                datefrom: {
                    required: true,
                }
            },

            // Messages for form validation
            messages: {
                Name: {
                    required: 'Please choose Tower'
                },
                FlatID: {
                    required: 'Please choose Flat No.',
                },
                datefrom: {
                    required: 'Please enter due date',
                }
            },

            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();
                        var search = $("#searchby").val();
                        var searchtext = $("#searchtext").val();

                        var saleid = "";
                        var datefrom = $("#datefrom").val();
                        $http({
                            method: 'Get',
                            contentType: "application/json; charset=utf-8",
                            url: '/BI/DemandLetter/SearchDemandLetter',
                            params: { TowerID: $scope.Flat.TowerID, FlatID: $scope.Flat.FlatID, datefrom: datefrom },
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
                });
            },

            // Do not change code below
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
            }
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
            url: '/BI/DemandLetter/SaveDimandLetterDimand',
            params: { datefrom: datefrom, saleid: v },
            dataType: "json"
        }).success(function (data) {
            alert(data)
            if (data == '"1"') {
                $("#MessageArea").show();
                $scope.MessageClass = "success";
                $scope.MessageTitle = "Success";
                $scope.Message = "Demand Letter Saved Successfully ";
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Demand Letter can't saved , Please try again.";
            }
            $('#loading').hide();
        });
    }

    $scope.IndexLetterInit = function () {

    }
    $scope.ViewSearchDemandLetter = function () {
         var $checkoutForm = $('#checkout-form').validate({
            // Rules for form validation
            rules: {
                datefrom: {
                    required: true,
                }
            },

            // Messages for form validation
            messages: {
                datefrom: {
                    required: 'Please enter due date',
                }
            },

            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $("#loading").show();
                        var search = $("#searchby").val();
                        var searchtext = $("#searchtext").val();

                        var saleid = "";
                        var datefrom = $("#datefrom").val();
                        var dateto=$("#dateto").val();
                        $http({
                            method: 'Get',
                            contentType: "application/json; charset=utf-8",
                            url: '/BI/DemandLetter/GetDemandLetter',
                            params: { searchby: $("#Searchby").find(":selected").val(), datefrom: datefrom, dateto: dateto },
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
                });
            },

            // Do not change code below
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
            }
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
          var murl = "/BI/Report/DemandLettertPrintAction/" + v;
          window.open(murl, '_blank');
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



    $('#loading').hide();
});