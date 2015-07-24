var myApp = angular.module('DocApp', []);
//Defining a Controller 
myApp.controller('DocumentController', function ($scope, $http) {
    $('#loading').show();
    // Get all Property list for dropdownlist
    $scope.Error = "";
    //$http.get('/flat/GetPropertyList/').success(function (response) { $scope.Properties = response; });
    //$http.get('/flat/GetBanks/').success(function (response) { $scope.Banks = response; });

    $scope.GenDocInit = function () {
        $('#loading').show();
        GetLoadTowerFlat();
        var sid = $("#SaleId").val();       
        if (sid != "0") {
            $("#dvSearch").hide();
            $("#dvStep").show();
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
                $http({
                    method: 'Get',
                    url: '/Customer/Transfer/GetPropertyInfoByFlatID',
                    params: { fid: data.Sale[0].FlatID, saleid: sid }
                }).success(function (data) {
                    $scope.Pro = data;
                    $http({
                        method: 'Get',
                        url: '/Customer/Document/AgreementInfo',
                        params: { saleid: sid }
                    }).success(function (data) {
                        $scope.Agreement = data;

                        $("#PlanType").find(":selected").val(data.PlanID);
                        $("#PlanType").find(":selected").text(data.PlanName);
                        $("#lblFlatNo").html(data.FlatName);
                        $("#lblSaleDate").html($scope.SaleDate)
                        $("#lblPlanName").text(data.PlanName);
                        $("#hidPlanType").val(data.PlanName);
                        $('#loading').hide();
                    })
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
    $scope.SearchPropertyForGenAgreement = function () {
        $('#loading').show();
        $("#hidFlatID").val($scope.Flat.FlatID);
        //var pname = $("#propertyname").val();
        //var pid = $("#PropertyID").find(":selected").val();       
        var pid = $("#hidFlatID").val();        
        $http({
            method: 'Get',
            url: '/Customer/Transfer/GetSaleIDByPName',
            params: { PID: pid }
        }).success(function (data, status, headers, config) {
            $scope.SaleID = data;
            $("#SaleId").val(data);
            $http({
                method: 'Get',
                url: '/Sale/Payment/GetFlatSaleBySaleID',
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
                    $scope.Pro = data;
                    $http({
                        method: 'Get',
                        url: '/Customer/Document/AgreementInfo',
                        params: { saleid: $scope.SaleID }
                    }).success(function (data) {
                        $scope.Agreement = data;
                        if (data == "NO") {
                            $("#dvAgreement").hide();
                            $("#dvGenrate").show();
                        }
                        else {
                            $("#dvAgreement").show();
                            $("#dvGenrate").hide();
                        }
                        $("#lblFlatNo").html(pname);
                        $("#lblSaleDate").html($scope.SaleDate)
                        $("#lblPlanName").text(data.PlanName);
                        $("#hidPlanType").val(data.PlanName);

                        $('#loading').hide();
                        $("#dvSearch").hide();
                        $("#dvStep").show();
                    })
                }).then(function () {
                    $('#loading').hide();
                    $("#dvSearch").hide();
                    $("#dvStep").show();
                })
            })
        })
    }

    $scope.GenerateAgreement = function () {
        $('#loading').show();
        var adate = $("#AgDate").val();
        var atype = $("#AgreementType").find(":selected").val();
        var sid = $("#SaleId").val();        
        $http({
            method: 'Get',
            url: '/Customer/Document/GenerateAgreement',
            params: { atype: atype, adate: adate, saleid: sid }
        }).success(function (data, status, headers, config) {
            $scope.SaleID = data;
            $http({
                method: 'Get',
                url: '/Customer/Document/AgreementInfo',
                params: { saleid: sid }
            }).success(function (data) {
                $scope.Agreement = data;
                if (data == "NO") {
                    $("#dvAgreement").hide();
                    $("#dvGenrate").show();
                }
                else {
                    $("#dvAgreement").show();
                    $("#dvGenrate").hide();
                }
            })
            $('#loading').hide();
        })
    }

    $scope.DownloadAgreement = function () {
        var sid = $("#SaleId").val();
        if ($("#rbtntypeemail").is(":checked")) {
            var vli = ValidateSendAgreementMail();
            if (vli == false) {
                $('#myModal').modal('show');
            }
            else {
                $('#loading').show();
                $http({
                    method: 'Get',
                    url: '/Customer/Document/EmailAgreement',
                    params: { saleid: sid, name: $scope.Pro.PropertyName, email: $("#Email").val(), url: $scope.Agreement.HTMLURL }
                }).success(function (data) {
                    if (data == "OK") {
                        alert("Agreement mail sent.");
                    }
                    else {
                        alert("Please try again.");
                    }
                    $('#loading').hide();
                })
            }
        }
        else {
            //download
            window.open($scope.Agreement.HTMLURL, "_blank");
            //window.location.href = $scope.Agreement.DocURL;
        }
    }

    $scope.DownloadWelcome = function () {

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

    $scope.UploadAgreement = function () {

    }

    function ValidateSendAgreementMail() {
        var vl = true;
        var message = "";
        if ($("#Email").val() == "") {
            vl = false;
            message += "Insert Email Address. <br/>";
        }
        if ($("#Email").val() != "") {
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
    $('#loading').hide();
})