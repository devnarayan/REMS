var myApp = angular.module('PaymentApp', []);
myApp.controller('CancelController', function ($scope, $http, $filter) {
    var orderBy = $filter('orderBy');
    $("#loading").show();
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
                url: '/Sale/Payment/PaymentCancelSave',
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
                location.reload();
                $('#loading').hide();
            }).error(function (error) {
                //Showing error message
                alert("Unable to save payment cancelation: " + error.message);
                $scope.status = 'Unable to save payment cancelation: ' + error.message;
                $('#loading').hide();
            });
        }
    }

    $("#loading").hide();

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

});