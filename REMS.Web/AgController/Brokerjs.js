var myApp = angular.module('BrokerApp', []);
myApp.controller('BrokerController', function ($scope, $http, $filter) {
    var orderBy = $filter('orderBy');

    $scope.SaveBroker = function () {
        debugger;
        var $checkoutForm = $('#checkoutform').validate({
            // Rules for form validation
            rules: {
                BrokerName: {
                    required: true
                },


            },
            // Messages for form validation
            messages: {
                BrokerName: {
                    required: "Please enter broker name."
                },

            },
            // Ajax form submition
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function () {
                        $('#loading').show();
                        $http({
                            method: 'POST',
                            url: '/Customer/Broker/SaveBroker',
                            data: JSON.stringify($scope.Broker),
                            headers: { 'Content-Type': 'application/JSON' }
                        }).success(function (data) {
                            if (data == "Yes") {
                                $('#loading').hide();
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Success";
                                $scope.MessageClass = "success";
                                $scope.Message = "Record inserted successfully.";
                                $scope.checkoutform.$setPristine();
                                $scope.comment = {};
                            }
                            else {
                                $('#loading').hide();
                                $("#MessageArea").show();
                                $scope.MessageTitle = "Error";
                                $scope.MessageClass = "danger";
                                $scope.Message = "Record already exists.";

                            }
                        }).error(function (error) {
                            //Showing error message
                            $("#MessageArea").show();
                            $scope.MessageTitle = "Error";
                            $scope.MessageClass = "danger";
                            $scope.Message = "Tower Record can not inserted.";
                            $('#loading').hide();
                            $scope.status = 'Unable to save property: ' + error.message;
                        });
                    }
                });
            },

            // Do not change code below.
            errorPlacement: function (error, element) {
                error.insertAfter(element.parent());
            }
        }
        );
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

    $scope.EditBrokerInit = function () {
        var pid = $("#hidBID").val();
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Customer/Broker/GetBrokerByBrokerID',
            params: { bid: pid }
        }).success(function (data, status, headers, config) {
            $scope.Broker = data;
            $('#loading').hide();
        })

    }
    $scope.UpdateBroker = function () {

        $('#loading').show();
        var pid = $("#hidBID").val();
        $scope.Broker.BrokerID = pid;
        $http({
            method: 'POST',
            url: '/Customer/Broker/UpdateBroker',
            data: JSON.stringify($scope.Broker),
            headers: { 'Content-Type': 'application/JSON' }
        }).success(function (data) {
            if (data == "Yes") {
                $('#loading').hide();
                $("#MessageArea").show();
                $scope.MessageTitle = "Success";
                $scope.MessageClass = "success";
                $scope.Message = "Record inserted successfully.";
                $scope.checkoutform.$setPristine();
                $scope.comment = {};
            }
            else {
                $('#loading').hide();
                $("#MessageArea").show();
                $scope.MessageTitle = "Error";
                $scope.MessageClass = "danger";
                $scope.Message = "Record already exists.";

            }
        })

    }

    $scope.AttachBrokerInit = function () {
        $("#btnAdd").show();
        $("#btnEdit").hide();
        if ($("#hidSaleID").val() == "0") {
            $("#dvSearch").show();
            GetLoadTowerFlat();
            //$("#dvStep").hide();
        }
        else {
            $scope.SaleID = $("#hidSaleID").val();           
            $http({
                method: 'Get',
                url: '/Sale/Payment/GetFlatSaleBySaleID',
                params: { saleid: $scope.SaleID }
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
                    $scope.Pro = data;                  
                    $http({
                        method: 'Get',
                        url: '/Customer/Broker/GetBrokerBySaleID',
                        params: { saleid: $scope.SaleID }
                    }).success(function (data) {
                        $scope.SaleBrokerList = data;

                        $("#hidSaleID").val(data);
                        $("#dvSearch").hide();
                        $("#dvStep").show();
                        $('#loading').hide();
                    })

                })
            })


            $("#dvSearch").hide();
            $("#dvStep").show();
        }
        $http({
            method: 'Get',
            url: '/Customer/Broker/GetAllBroker'
        }).success(function (data) {
            $scope.BrokerList = data;
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

    $scope.PayBrokerInit = function () {
        $http.get('/Sale/Payment/GetBank/').success(function (response) { $scope.Banks = response; });

        $("#btnAdd").show();
        $("#btnEdit").hide();
        if ($("#hidBrokerID").val() == "0") {
            $("#dvSearch").show();
            //$("#dvStep").hide();
        }
        else {
            $('#loading').show();
            var pid = $("#hidBrokerID").val();
            //$http({
            //    method: 'Get',
            //    url: '/Customer/Broker/GetBrokerIdBySaleID',
            //    params: { saleid: sid }
            //}).success(function (data) {
            //    $scope.BrokerId = data;                
            //    var pid = $scope.BrokerId;               
                $http({
                    method: 'Post',
                    url: '/Customer/Broker/GetBrokerByBrokerID',
                    data: { bid: pid }
                }).success(function (data, status, headers, config) {
                    $scope.Broker = data;
                    $http({
                        method: 'Get',
                        url: '/Customer/Broker/GetBrokersProperties',
                        params: { brokerid: data.BrokerID }
                    }).success(function (data, status, headers, config) {
                        $scope.BrokerPropertyList = data;
                        var totall = 0;
                        for (var i = 0; i < $scope.BrokerPropertyList.length; i++) {
                            var list = $scope.BrokerPropertyList[i];
                            totall += list.BrokerAmount;
                        }
                        $scope.TotalAmount = Math.round(totall);
                        $http({
                            method: 'Get',
                            url: '/Customer/Broker/GetPaidAmountToBrokerByBrokerID',
                            params: { brokerID: pid }
                        }).success(function (data) {
                            $scope.BrokerPaidList = data;
                            var total = 0;
                            for (var i = 0; i < $scope.BrokerPaidList.length; i++) {
                                var list = $scope.BrokerPaidList[i];
                                total += list.AmountPaid;
                            }
                            $scope.TotalAmountPaid = Math.round(total);
                        })
              
               
                        $('#loading').hide();
                    })
                })
           // })

            $("#dvSearch").hide();
            $("#dvStep").show();
        }
        $http({
            method: 'Get',
            url: '/Customer/Broker/GetAllBroker'
        }).success(function (data) {
            $scope.BrokerList = data;
        })
    }
    $scope.SearchPropertyForSaleID = function () {
        try {
            $('#loading').show();
            $("#hidFlatID").val($scope.Flat.FlatID);           
            //var pname = $("#propertyname").val();
            //var pid = $("#PropertyID").find(":selected").val();
            //var pid = $("#hidFlatID").val($scope.Flat.FlatID);
            var pid = $("#hidFlatID").val();
            $http({
                method: 'Get',
                //url: '/Admin/CreateProperty/getFlatByID',
                url: '/Customer/Transfer/GetSaleIDByPName',
                params: { PID: pid }
            }).success(function (data, status, headers, config) {
                $scope.SaleID = data;
            })
            $http({
                method: 'Get',
                url: '/Payment/GetFlatSaleBySaleID',
                params: { saleid: data }
            }).success(function (data, status, headers, config) {
                $scope.Sale = data;
                if (data.Sale.length == 0) {
                    $('#loading').hide();
                }
                var ddate = new Date(parseInt($scope.Sale.Sale[0].SaleDate.substr(6)));// value.DueDate;
                var mth = ddate.getMonth() + 1;
                if (mth < 10) mth = "0" + mth;
                $scope.SaleDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();
                $http({
                    method: 'Get',
                    url: '/Customer/Transfer/GetPropertyInfoByFlatID',
                    params: { fid: data.Sale[0].FlatID, saleid: $scope.SaleID }
                }).success(function (data) {
                    $scope.Pro = data;
                    $http({
                        method: 'Get',
                        url: '/Customer/Broker/GetBrokerBySaleID',
                        params: { saleid: $scope.SaleID }
                    }).success(function (data) {
                        $scope.SaleBrokerList = data;

                        var total = 0;
                        for (var i = 0; i < $scope.SaleBrokerList.length; i++) {
                            var list = $scope.SaleBrokerList[i];
                            total += list.BrokerAmount;
                        }
                        $scope.TotalAmount = Math.round(total);
                        $("#hidSaleID").val(data);
                        $("#dvSearch").hide();
                        $("#dvStep").show();
                        $('#loading').hide();
                    })

                })
            })

        }
        catch (e) {

            $('#loading').hide();
        }
    }

    $scope.SearchPropertyForBroker = function () {
        $('#loading').show();
        var pid = $("#BrokerName").find(":selected").val();

        if (pid == 0) {
            $("#hidBrokerID").val("0");
            $http({
                method: 'Get',
                url: '/Customer/Broker/GetBrokersProperties',
                params: { brokerid: 0 }
            }).success(function (data, status, headers, config) {
                $scope.BrokerPropertyList = data;
                var totall = 0;
                for (var i = 0; i < $scope.BrokerPropertyList.length; i++) {
                    var list = $scope.BrokerPropertyList[i];
                    totall += list.BrokerAmount;
                }
                $scope.TotalAmount = Math.round(totall);
                $http({
                    method: 'Get',
                    url: '/Customer/Broker/GetPaidAmountToBrokerByBrokerID',
                    params: { brokerID: 0 }
                }).success(function (data) {
                    $scope.BrokerPaidList = data;
                    var total = 0;
                    for (var i = 0; i < $scope.BrokerPaidList.length; i++) {
                        var list = $scope.BrokerPaidList[i];
                        total += list.AmountPaid;
                    }
                    $scope.TotalAmountPaid = Math.round(total);
                }).error(function (ex) {
                    alert(ex)
                })

                $('#loading').hide();
                $("#dvSearch").show();
                $("#dvStep").show();
            })

        } else {
            $http({
                method: 'Get',
                url: '/Customer/Broker/GetBrokerByBrokerID',
                params: { bid: pid }
            }).success(function (data, status, headers, config) {
                $scope.Broker = data;
                $("#hidBrokerID").val(data.BrokerID);
                $http({
                    method: 'Get',
                    url: '/Customer/Broker/GetBrokersProperties',
                    params: { brokerid: data.BrokerID }
                }).success(function (data, status, headers, config) {
                    $scope.BrokerPropertyList = data;
                    var totall = 0;
                    for (var i = 0; i < $scope.BrokerPropertyList.length; i++) {
                        var list = $scope.BrokerPropertyList[i];
                        totall += list.BrokerAmount;
                    }
                    $scope.TotalAmount = Math.round(totall);
                    $http({
                        method: 'Get',
                        url: '/Customer/Broker/GetPaidAmountToBrokerByBrokerID',
                        params: { brokerID: $("#hidBrokerID").val() }
                    }).success(function (data) {
                        $scope.BrokerPaidList = data;
                        var total = 0;
                        for (var i = 0; i < $scope.BrokerPaidList.length; i++) {
                            var list = $scope.BrokerPaidList[i];
                            total += list.AmountPaid;
                        }
                        $scope.TotalAmountPaid = Math.round(total);
                    }).error(function (ex) {
                        alert(ex)
                    })

                    $('#loading').hide();
                    $("#dvSearch").hide();
                    $("#dvStep").show();
                })

            })
        }
    }

    $scope.TowerChange = function () {
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Admin/CreateProperty/GetReservedFlatListByTowerID',
            params: { towerid: $scope.Flat.TowerID }
        }).success(function (data) {
            $scope.FlatList = data;
            $('#loading').hide();
        });
    }
    $scope.PayBrokerToProperty = function (bid, saleid, flatid, TotalAmount, brokerid) { // BrokerToPropertyID
        $scope.SaleID = saleid;
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Payment/GetFlatSaleBySaleID',
            params: { saleid: saleid }
        }).success(function (data, status, headers, config) {
            $scope.Sale = data;
            var ddate = new Date(parseInt($scope.Sale.Sale[0].SaleDate.substr(6)));// value.DueDate;
            var mth = ddate.getMonth() + 1;
            if (mth < 10) mth = "0" + mth;
            $scope.SaleDate = ddate.getDate() + "/" + mth + "/" + ddate.getFullYear();
            $http({
                method: 'Get',
                url: '/Customer/Transfer/GetPropertyInfoByFlatID',
                params: { fid: flatid, saleid: $scope.SaleID }
            }).success(function (data) {
                $scope.Pro = data;
                $http({
                    method: 'Get',
                    url: '/Customer/Broker/GetBrokerBySaleID',
                    params: { saleid: $scope.SaleID }
                }).success(function (data) {
                    $scope.SaleBrokerList = data;

                    var total = 0;
                    for (var i = 0; i < $scope.SaleBrokerList.length; i++) {
                        var list = $scope.SaleBrokerList[i];
                        total += list.BrokerAmount;
                    }
                    $scope.TotalAmount = Math.round(total);

                    $("#dvPay").show();
                    $("#hidSaleID").val(data);
                    $("#dvSearch").hide();
                    $("#dvStep").show();
                    $('#loading').hide();
                })

            })
        })
    }

    $scope.SubmitPayBrokerToProperty = function () {
        var vli = ValidateSubmitPayment();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            $http({
                method: 'Post',
                url: '/Customer/Broker/PaymentBrokerToProperty',
                data: { FlatID: 0, SaleID: 0, BrokerID: $("#BrokerName").find(":selected").val(), PaidDate: $("#paymentDate").val(), AmountPiad: $("#Amount").val(), PID: 0, PaymentMode: $("#PaymentMode").find(":selected").val(), ChequeNo: $("#ChequeNo").val(), ChequeDate: $("#chequeDate").val(), BankName: $("#BankName").find(":selected").text(), BranchName: $("#BankBranch").val(), Remarks: $("#Remarks").val() },
                //data: { FlatID: $scope.Sale.Sale[0].FlatID, SaleID: $scope.Sale.Sale[0].SaleID, BrokerID: $("#BrokerName").find(":selected").val(), PaidDate: $("#paymentDate").val(), AmountPiad: $("#Amount").val(), PID: $scope.Pro.PID, PaymentMode: $("#PaymentMode").find(":selected").val(), ChequeNo: $("#ChequeNo").val(), ChequeDate: $("#chequeDate").val(), BankName: $("#BankName").find(":selected").text(), BranchName: $("#BankBranch").val(), Remarks: $("#Remarks").val() },
            }).success(function (data) {              
                if (data == '"Yes"') {
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
                        $('#loading').hide();
                        SubmitPayment()
                    })
                    alert("PAYMENT ADDED TO BROKER ACCOUNT.");
                }
                else {
                    alert("Please try again.")
                    $('#loading').hide();
                }
            })
        }
    }


    $scope.UpdateStatus = function (status, id) {
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Customer/Broker/UpdatePropertyStatusByID',
            params: { id: id, status: status }
        }).success(function (data, status, headers, config) {
            alert("Status Update SucessFully");
            $('#loading').hide();
            //$("#StatusSr :selected").val(status);
            //$scope.SearchProperty();
        })

    }

    $scope.FlatChange = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Customer/Broker/GetBrokerByFlatID',
            params: { flatid: $scope.Flat.FlatID }
        }).success(function (data) {
            $scope.SaleBrokerList = data;
            $('#loading').hide();
        })
    }

    $scope.AddBrokerToProperty = function () {
        var vli = ValidateAttachBroker();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            //var PropertyID = $scope.Sale.Sale[0].PropertyID;         
            $http({
                method: 'Post',
                url: '/Customer/Broker/AttachBrokerToProperty',
                data: { brokerid: $("#BrokerName").find(":selected").val(), amount: $("#Amount").val(), date: $("#BDate").val(), remarks: $("#Remarks").val(), FlatID: $scope.Flat.FlatID, SaleID: 0 }
            }).success(function (data) {
                if (data == '"Yes"') {
                    $http({
                        method: 'Get',
                        url: '/Customer/Broker/GetBrokerByFlatID',
                        params: { flatid: $scope.Flat.FlatID }
                    }).success(function (data) {
                        $scope.SaleBrokerList = data;
                        $('#loading').hide();
                    })
                    AttachBrokerFrom();
                    alert("Broker added to Property.");
                }
                else {
                    alert("Please try again.")
                    $('#loading').hide();
                }
            })
        }
    }
    $scope.EditBrokerToProperty = function (bid) {
        $http({
            method: 'Get',
            url: '/Customer/Broker/GetBrokerByBrokerToPropertyID',
            params: { pid: bid }
        }).success(function (data) {
            // $scope.SaleBrokerList = data;
            $("#BrokerName").find(":selected").val(data[0].BrokerID)
            $("#BrokerName").find(":selected").text(data[0].BrokerName)
            $("#Amount").val(data[0].BrokerAmount);
            $("#BDate").val(data[0].DateSt);
            $("#Remarks").val(data[0].Remarks);
            $("#hidBroerToPropertyID").val(data[0].BrokerToPropertyID);
            $("#btnEdit").show();
        })
    }
    $scope.DeleteBrokerToProperty = function (bid) {
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Customer/Broker/DeleteBrokerToPropertyID',
            params: { pid: bid }
        }).success(function (data) {
            if (data == '"Yes"') {
                $http({
                    method: 'Get',
                    url: '/Customer/Broker/GetBrokerByFlatID',
                    params: { flatid: $scope.Flat.FlatID }
                }).success(function (data) {
                    $scope.SaleBrokerList = data;
                    $('#loading').hide();
                })
                alert("Broker to Property Removed Successfully.");
            }
            else {
                alert("Please try again.");
                $('#loading').hide();
            }
        })
    }
    $scope.UpdateBrokerToProperty = function () {
        var vli = ValidateAttachBroker();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {
            $('#loading').show();
            $http({
                method: 'Post',
                url: '/Customer/Broker/AttachBrokerToPropertyUpdate',
                data: { brokerToPropertyID: $("#hidBroerToPropertyID").val(), brokerid: $("#BrokerName").find(":selected").val(), amount: $("#Amount").val(), date: $("#BDate").val(), remarks: $("#Remarks").val(), PropertyID: 0, FlatID: $scope.Flat.FlatID, SaleID: 0}
            }).success(function (data) {
                if (data == '"Yes"') {
                    $http({
                        method: 'Get',
                        url: '/Customer/Broker/GetBrokerByFlatID',
                        params: { flatid: $scope.Flat.FlatID }
                    }).success(function (data) {
                        $scope.SaleBrokerList = data;
                        $('#loading').hide();
                    })
                    alert("Broker to Property Updated Successfully.");
                }
                else {
                    alert("Please try again.");
                    $('#loading').hide();
                }
            })
            //    .error(function (ex) {
            //    alert('Error!, Please try again')
            //    $('#loading').hide();
            //})
        }
    }


    $scope.SearchProperty = function () {
        $('#loading').show();
        var status = $("#StatusSr :selected").text()
        var search = $("#searchtext").val();
        var propertyid = $("#PropertyID").val();

        var searchtext = $("#searchtext").val();
        $http({
            method: 'Get',
            contentType: "application/json; charset=utf-8",
            url: '/Home/PropertySearchByApoove',
            params: { status: status, search: search, propertyid: propertyid },
            dataType: "json"
        }).success(function (data) {
            $scope.BrokerPropertyList = data;
            var totall = 0;
            for (var i = 0; i < $scope.BrokerPropertyList.length; i++) {
                var list = $scope.BrokerPropertyList[i];
                totall += list.SaleRate;
            }
            $scope.TotalAmount = Math.round(totall);
            $('#loading').hide();
        });

    }
    // Approve Payment form
    $scope.ApprovePaymentBrokerInit = function () {
        if ($("#hidBrokerID").val() == "0") {
            $("#dvSearch").show();
            $("#dvStep").show();
        }
        else {
            $('#loading').show();
            $("#dvSearch").hide();
            var sid = $("#hidBrokerID").val();
            $http({
                method: 'Get',
                url: '/Customer/Broker/GetBrokerIdBySaleID',
                params: { saleid: sid }
            }).success(function (data) {
                $scope.BrokerId = data;                
                var pid = $scope.BrokerId; 
                $http({
                    method: 'Get',
                    url: '/Customer/Broker/GetBrokersProperties',
                    params: { brokerid: pid }
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
            })
        }
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
            params: { brokerid: $("#hidBrokerID").val(), Status: status }
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

    // Get all Property list for dropdownlist
    $scope.Error = "";
    $scope.orderSearchList = function (predicate, reverse) {
        $scope.SearchList = orderBy($scope.SearchList, predicate, reverse);
    };
    $scope.orderSaleBrokerList = function (predicate, reverse) {
        $scope.SaleBrokerList = orderBy($scope.SaleBrokerList, predicate, reverse);
    };
    $scope.orderBrokerPropertyList = function (predicate, reverse) {
        $scope.BrokerPropertyList = orderBy($scope.BrokerPropertyList, predicate, reverse);
    };
    $scope.orderBrokerPaidList = function (predicate, reverse) {
        $scope.BrokerPaidList = orderBy($scope.BrokerPaidList, predicate, reverse);
    };
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
    function SubmitPayment() {
        $("#paymentDate").val(''), $("#Amount").val(''), $("#ChequeNo").val(''), $("#chequeDate").val(''), $("#BankBranch").val(''), $("#Remarks").val('')
    }
    function AttachBrokerFrom() {
        $("#Amount").val(''); $("#BDate").val(''); $("#Remarks").val("");
    }
    function ValidateAddBroker() {
        var vl = true;
        var message = "";
        if ($("#BName").val() == "") {
            vl = false;
            message += "Insert Broker Name. <br/>";
        }
        if ($("#PanNo").val() == "") {
            vl = false;
            message += "Insert Pan No. <br/>";
        }
        if ($("#Address1").val() == "") {
            vl = false;
            message += "Insert Address1.<br/>";
        }

        if ($("#Address1").val() == "") {
            vl = false;
            message += "Insert Address2.<br/>";
        }
        if ($("#MobileNo").val() == "") {
            vl = false;
            message += "Please insert MobileNo.<br/>";
        }
        $("#ErrorMessage").html(message);
        return vl;
    }
    function ValidateAttachBroker() {
        var vl = true;
        var message = "";
        if ($("#BrokerName").find(":selected").val() == "" || $("#BrokerName").find(":selected").val() == "? undefined:undefined ?") {
            vl = false;
            message += "Please Select Broker Name. <br/>";
        }
        // 
        if ($("#Amount").val() == "") {
            vl = false;
            message += "Insert Amount. <br/>";
        }
        if ($("#Amount").val() != "") {
            var emailReg = new RegExp(/^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$/);
            var valid = emailReg.test($("#Amount").val());
            if (!valid) {
                vl = false;
                message += "Invalid Amount Value<br/>";
            }
        }
        if ($("#BDate").val() == "") {
            vl = false;
            message += "Insert Date .<br/>";
        }

        if ($("#Remarks").text() == "") {
            vl = false;
            message += "Insert Remarks for broker.<br/>";
        }
        $("#ErrorMessage").html(message);
        return vl;
    }
    function ValidateSubmitPayment() {
        var vl = true;
        var message = "";
        if ($("#Amount").val() == "") {
            vl = false;
            message += "Insert receive amount. <br/>";
        }
        if ($("#Amount").val() != "") {
            var emailReg = new RegExp(/^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$/);
            var valid = emailReg.test($("#Amount").val());
            if (!valid) {
                vl = false;
                message += "Invalid Amount Value<br/>";
            }
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