var myApp = angular.module('CustomerApp', []);
myApp.controller('customerController', function ($scope, $http) {
    $("#loading").show();
    $scope.showCustomer = function () {
        var saleid = $("#hidSaled").val();
        
        $http({
            method: 'Get',
            url: '/Customer/ManageCustomer/custDetail',
            params: { saleid: saleid },
        }).success(function(data){
            $scope.custDetail = data;
        })
    }
    //$scope.UpdateCustomer = function () {
    //    var hidCustId = $("#CustomerID").val();
    //    $http
    //}
    $scope.UpdateCustomer = function () {
        $('#loading').show();        
        console.log($scope.custDetail);     
        $http({
            method: 'POST',
            url: '/Customer/ManageCustomer/updateCustomer',
            data: JSON.stringify($scope.custDetail),
           // headers: { 'Content-Type': 'application/JSON' }          
        }).success(function (data) {
            console.log($scope.custDetail);
            //Showing success message $scope.status = "The Person Saved Successfully!!!";
            //Updating persons Model
            if (data == 1) {
                alert("Customer has been Update saved successfully");
                //GetSaleDetailByFlatId();
            }
            else {
                alert("Please provide correct info.");
            }
            $('#loading').hide();
            $('#btnSubmit').show();
        }).error(function (error) {
            //Showing error message
            $scope.status = 'Unable to save Customer: ' + error.message;
            $('#loading').hide();
            $('#btnSubmit').show();
        });
    }

    $("#loading").hide();
});