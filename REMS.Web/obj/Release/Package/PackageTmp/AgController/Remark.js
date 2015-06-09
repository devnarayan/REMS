var myApp = angular.module('BIApp', []);
myApp.controller('ReportController', function ($scope, $http, $filter) {
    $('#loading').show();
    var orderBy = $filter('orderBy');

    //$http.get('/flat/GetPropertyList/').success(function (response) { $scope.Properties = response; });
    //$http.get('/flat/GetBanks/').success(function (response) { $scope.Banks = response; });

    $scope.AddPropertyRemarkInit = function () {
        var saleid = $("#hidSaleID").val();
        if (saleid == "0") {
            $("#dvSale").show();
            GetLoadTowerFlat();
        }
        else {
            $http({
                method: 'Get',
                url: '/Customer/Remark/GetAllRemark',
                params: { saleid: $scope.SaleID }
            }).success(function (data, status, headers, config) {
                $scope.SearchList = data;
            })

        }
    }
    //$scope.SearchPropertyRemak = function () {

    //    $("#hidFlatID").val($scope.Flat.FlatID);
    //    alert($scope.Flat.FlatID);
    //    var PID = $("#hidFlatID").val();
    //    $http({
    //        method: 'Get',
    //        url: '/sale/Payment/GetFlatList',
    //        params: { pid: PID }
    //    }).success(function (data, status, headers, config) {
    //        $scope.Flat = data;
    //        if (data.Result.length == 0) {
    //            $scope.Error = "Property flat not found.";
    //            $('#loading').hide();
    //            var FID = $scope.Flat.Result[0].FlatID;
    //        } else {
    //            var FID = $scope.Flat.Result[0].FlatID;
    //        }
    //        $("#flantname").val($scope.Flat.Result[0].FlatName);
    //        $("#FlatID").val($scope.Flat.Result[0].FlatID);
    //        $http({
    //            method: 'Get',
    //            url: '/Payment/GetFlatSale',
    //            params: { flatid: FID }
    //        }).success(function (data, status, headers, config) {
    //            $scope.Sale = data;
    //            $("#btnsave").removeClass("disabled");
    //            $('#loading').hide();
    //            $scope.SaleID = $scope.Sale.Sale[0].SaleID;
    //            $http({
    //                method: 'Get',
    //                url: '/Customer/Remark/GetAllRemark',
    //                params: { saleid: $scope.SaleID }
    //            }).success(function (data, status, headers, config) {
    //                $scope.SearchList = data;
    //            }).error(function (ex) {
    //                alert(ex);
    //                $('#loading').hide();
    //            }).error(function (ex) {
    //                alert(ex);
    //                $('#loading').hide();
    //            })

    //        })
    //    })
    //}
    $scope.SearchPropertyRemak = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        var PID = $("#hidFlatID").val();
        $http({
            method: 'Get',
            url: '/sale/Payment/GetFlatList',
            params: { pid: PID }
        }).success(function (data, status, headers, config) {
            $scope.Flat = data;
            if (data.Result.length == 0) {
                $scope.Error = "Property flat not found.";
                $('#loading').hide();
                var FID = $scope.Flat.Result[0].FlatID;
                return FID;
            } else {
                var FID = $scope.Flat.Result[0].FlatID;
            }
            $("#flantname").val($scope.Flat.Result[0].FlatName);
            $("#FlatID").val($scope.Flat.Result[0].FlatID);
            var FName = $("#flantname").val();
            $http({
                method: 'Get',
                contentType: "application/json; charset=utf-8",
                url: '/Customer/Remark/SearchPropertyRemak',
                params: { search: FName },
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
        })
    }
    $scope.SavePropertyRemak = function () {
        $('#loading').show();
        //var propertyName = $("#PropertyTypeID :selected").text()
        var propertyName = $("#flantname").val();
        var Remark = $("#Remark").val();

        var saleid = $scope.SaleID;
        //if (propertyName.trim() == "") {
        //    alert("Enter Propery Name ");
        //    $('#loading').hide();
        //    return;
        //}
        if (Remark.trim() == "") {
            alert("Enter Remark");
            $('#loading').hide();
            return;
        }
        var propertyid = $("#FlatID").val();
        //if (propertyid == "? undefined:undefined ?") {
        //    alert("Select Property");
        //    $('#loading').hide();
        //    return;
        //}
        var datefrom = $("#datefrom").val();
        if (datefrom == "") {
            alert("Select Date");
            $('#loading').hide();
            return;
        }

        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/Customer/Remark/SavePropertyRemak',
            params: { propertyid: propertyid, amt: $("#DueAmt").val(), propertyName: propertyName, datefrom: datefrom, Remark: Remark, saleid: saleid },
            dataType: "json"
        }).success(function (data) {

            alert("Remark Add Sucessfully");
            $("#Remark").val("");
            $http({
                method: 'Get',
                url: '/Customer/Remark/GetAllRemark',
                params: { saleid: $scope.SaleID }
            }).success(function (data, status, headers, config) {

                $scope.SearchList = data;
            }).error(function (ex) {
                alert(ex);
                $('#loading').hide();
            })


            $('#loading').hide();
        });
    }

    $scope.AddNewRemarkInit = function () {
        var saleid = $("#hidSaleID").val();
        if (saleid == "0") {
            $("#dvSale").show();
            GetLoadTowerFlat();
        }
        else {
            $('#loading').show();
            $("#dvSale").hide();
            $http({
                method: 'Get',
                url: '/Sale/Payment/GetPidProptyname',
                params: { saleid: saleid }
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
                var FID = $scope.seachFlat.Result[0].FlatID;
                // var PID = $scope.seachFlat.Result[0].Pid;
                $("#flantname").val($scope.seachFlat.Result[0].FlatName);
                $("#FlatID").val($scope.seachFlat.Result[0].FlatID);
                //$("#ProprtyName").val($scope.seachFlat.Result[0].FlatName);
               // $("#dvSale").hide();
                $http({
                    method: 'Get',
                    url: '/Sale/Payment/GetFlatSaleBySaleID',
                    params: { saleid: saleid }
                }).success(function (data, status, headers, config) {
                    $scope.Sale = data;
                    $("#btnsave").removeClass("disabled");
                    $('#loading').hide();
                    $scope.SaleID = $scope.Sale.Sale[0].SaleID;
                    $http({
                        method: 'Get',
                        url: '/Customer/Remark/GetAllRemark',
                        params: { saleid: $scope.SaleID }
                    }).success(function (data, status, headers, config) {
                        $scope.SearchList = data;
                        $http({
                            method: 'Get',
                            url: '/Payment/GetTotalCurrentDueAmount',
                            params: { FlatId: FID, date: $("#datefrom").val() }
                        }).success(function (data, status, headers, config) {
                            $scope.TotalDueAmount = data;
                        })
                    }).error(function (ex) {
                        alert(ex);
                        $('#loading').hide();
                    }).error(function (ex) {
                        alert(ex);
                        $('#loading').hide();
                    })
                }).error(function (ex) {
                    alert(ex);
                    $('#loading').hide();
                });
            })
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
                params: { towerid: data[0].TowerID }
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
    $scope.DueDateChange = function () {
        var saleid = $("#hidSaleID").val();
        $http({
            method: 'Get',
            url: '/Payment/GetTotalCurrentDueAmount',
            params: { saleid: saleid, date: $("#datefrom").val() }
        }).success(function (data, status, headers, config) {
            $scope.TotalDueAmount = data;
        })
    }
    $scope.SearchFlat = function () {
        $scope.Error = "";
        $('#loading').show();

        //var FName = $("#ProprtyName").val();;
        //var PID = $("#PropertyID").val();
        $("#hidFlatID").val($scope.Flat.FlatID);
      
        var PID = $("#hidFlatID").val();
        $http({
            method: 'Get',
            url: '/sale/Payment/GetFlatList',
            params: { pid: PID }
        }).success(function (data, status, headers, config) {
            $scope.Flat = data;
            if (data.Result.length == 0) {
                $scope.Error = "Property flat not found.";
                $('#loading').hide();
                var FID = $scope.Flat.Result[0].FlatID;
            } else {
                var FID = $scope.Flat.Result[0].FlatID;
            }
            $("#flantname").val($scope.Flat.Result[0].FlatName);
            $("#FlatID").val($scope.Flat.Result[0].FlatID);
            $http({
                method: 'Get',
                url: '/Payment/GetFlatSale',
                params: { flatid: FID }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                $("#btnsave").removeClass("disabled");
                $scope.SaleID = $scope.Sale.Sale[0].SaleID;
                $http({
                    method: 'Get',
                    url: '/Customer/Remark/GetAllRemark',
                    params: { saleid: $scope.SaleID }
                }).success(function (data, status, headers, config) {
                    $scope.SearchList = data;
                    $http({
                        method: 'Get',
                        url: '/Payment/GetTotalCurrentDueAmount',
                        params: { FlatId: FID, date: $("#datefrom").val() }
                    }).success(function (data, status, headers, config) {
                        $scope.TotalDueAmount = data;
                    })
                    $('#loading').hide();

                }).error(function (ex) {
                    alert(ex);
                    $('#loading').hide();
                }).error(function (ex) {
                    alert(ex);
                    $('#loading').hide();
                })
            }).error(function (ex) {
                alert(ex);
                $('#loading').hide();
            });

        });
    }
    $scope.DeleteRemarks = function (rid) {
        $("#hidRID").val(rid);
        $('#myDelModal').modal('show');

    }
    $scope.ConfirmDeleteRemark = function () {
        var rid = $("#hidRID").val();

        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Customer/Remark/DeleteRemark',
            params: { rid: rid }
        }).success(function (data, status, headers, config) {
            if (data.toString() == "1") {
                $('#myDelModal').modal('hide');
            }
            $http({
                method: 'Get',
                url: '/Customer/Remark/GetAllRemark',
                params: { saleid: $scope.SaleID }
            }).success(function (data, status, headers, config) {
                $('#loading').hide();
                $scope.SearchList = data;
            }).error(function (ex) {
                alert(ex);
                $('#loading').hide();
            }).error(function (ex) {
                alert(ex);
                $('#loading').hide();
            })
        }).error(function (ex) {
            alert(ex);
            $('#loading').hide();
        });
    }

    $scope.orderSearchList = function (predicate, reverse) {
        $scope.SearchList = orderBy($scope.SearchList, predicate, reverse);
    };


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
            $('#loading').hide();
            window.open(data, "_blank");

        })
    }

    $scope.ReportPrint = function () {
        localStorage.setItem("ReportContent", $("#rptTableContent").html());
        window.open("/BI/Report/PrintReport", "_blank");
    }
    $scope.PrintReportInint = function () {
        $("#PrintReportdata").html(localStorage.getItem("ReportContent"))
        window.print();
    }

    $(".datecal").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: '1970:2090',
        dateFormat: 'dd/mm/yy'
    }).on('change', function (e) {
        var saleid = $("#hidSaleID").val();
        $http({
            method: 'Get',
            url: '/Payment/GetTotalCurrentDueAmount',
            params: { saleid: saleid, date: $("#datefrom").val() }
        }).success(function (data, status, headers, config) {
            $scope.TotalDueAmount = data;
        })
    });

    $('#loading').hide();
})