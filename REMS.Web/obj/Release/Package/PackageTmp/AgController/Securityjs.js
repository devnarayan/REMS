var myApp = angular.module('SecurityApp', []);
myApp.controller('SecurityController', function ($scope, $http, $filter) {
    var orderBy = $filter('orderBy');

    $scope.AddNewUserInit = function () {
        $http.get('/Admin/Security/GetPropertyList/').success(function (response) { $scope.Properties = response; });
    }

    $scope.AddNewUser = function () {
        var vli = ValidateAddUser();
        if (vli == false) {
            $('#myModal').modal('show');
        }
        else {

            var plist = [];
            $('.Prolist').each(function () {
                if ($(this).is(":checked")) {
                    plist.push($(this).val());
                }
            });
            $scope.UserModel.Property = plist.toString();
            if (plist.length > 0) {
                $('#loading').show();
                $http({
                    method: 'Post',
                    url: '/Admin/Security/AddUser',
                    data: JSON.stringify($scope.UserModel)
                }).success(function (data) {
                    alert(data)
                    if (data == "User Added Successfully.") {
                        //  window.open(window.location.href, "_self")
                    }
                    $('#loading').hide();

                }).error(function (ex) {
                    alert(ex);
                    $('#loading').hide();

                })

            }
            else {
                alert("Please select any one Property for User.")
            }
        }
    }

    $scope.AssingRoleInit = function () {
        $http.get('/Admin/Security/GetUserList/').success(function (response) { $scope.UserList = response; });
    }
    $scope.UserModuleSearch = function () {
        $('#loading').show();

        $http({
            method: 'Get',
            url: '/Admin/Security/GetModuleListByUser',
            params: { username: $scope.UserModel.UserName }
        }).success(function (data) {
            $scope.UserModelList = data;
            $("#tbodyhtml").html(data);
            $('#loading').hide();

        })
    }
    $scope.SaveUserModule = function () {
        var plist = [];
        var llist = [];

        $('.Prolist').each(function () {
            if ($(this).is(":checked")) {
                plist.push($(this).val());
            }
            else {
                llist.push($(this).val());
            }
        });
        $scope.UserModel.ModuleIDs = plist.toString();
        $('#loading').show();

        $http({
            method: 'Post',
            url: '/Admin/Security/SaveUsersRights',
            data: { username: $scope.UserModel.UserName, modulelist: $scope.UserModel.ModuleIDs, notinaccess: llist.toString() }
        }).success(function (data) {
            alert(data)
            $('#loading').hide();

        })
    }

    // Edit User Property
    $scope.EditUserPropertyInit = function () {
        $http.get('/Admin/Security/GetUserList/').success(function (response) { $scope.UserList = response; });
    }
    $scope.UsersPropertyAccessSearch = function () {
        $('#loading').show();

        $http({
            method: 'Get',
            url: '/Admin/Security/GetPropertyByUser',
            params: { username: $scope.UserModel.UserName }
        }).success(function (data) {
            $scope.UserModelList = data;
            $("#tbodyhtml").html(data);
            $('#loading').hide();

        })
    }
    $scope.SaveEditUserProperty = function () {
        var plist = [];
        var llist = [];

        $('.Prolist').each(function () {
            if ($(this).is(":checked")) {
                plist.push($(this).val());
            }
            else {
                llist.push($(this).val());
            }
        });
        $scope.UserModel.ModuleIDs = plist.toString();
        $('#loading').show();

        $http({
            method: 'Post',
            url: '/Admin/Security/SaveUsersPropertyRights',
            data: { username: $scope.UserModel.UserName, modulelist: $scope.UserModel.ModuleIDs, notinaccess: llist.toString() }
        }).success(function (data) {
            alert(data)
            $('#loading').hide();

        })
    }

    // EditUserRole
    $scope.UsersRoleAccessSearch = function () {
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Admin/Security/GetUserRole',
            params: { username: $scope.UserModel.UserName }
        }).success(function (data) {
            $scope.UserModel.CurrentRoleName = data;
            $('#loading').hide();
        })
    }
    $scope.SaveEditUserRole = function () {
        $('#loading').show();
        $http({
            method: 'Post',
            url: '/Admin/Security/UpdateUserRole',
            data: { username: $scope.UserModel.UserName, OldRoleName: $scope.UserModel.CurrentRoleName, NewRoleName: $scope.UserModel.RoleName }
        }).success(function (data) {
            alert(data)
            $('#loading').hide();
        })
    }

    // Role Access Rights
    $scope.RoleModuleSearch = function () {
        $('#loading').show();
        $http({
            method: 'Get',
            url: '/Admin/Security/GetModuleListByRole',
            params: { rolename: $scope.UserModel.RoleName }
        }).success(function (data) {
            $scope.UserModelList = data;
            $("#tbodyhtml").html(data);
            $('#loading').hide();
        })
    }

    $scope.SaveRoleAccess = function () {
        var plist = [];
        var llist = [];

        $('.Prolist').each(function () {
            if ($(this).is(":checked")) {
                plist.push($(this).val());
            }
            else {
                llist.push($(this).val());
            }
        });
        $scope.UserModel.ModuleIDs = plist.toString();
        $('#loading').show();

        $http({
            method: 'Post',
            url: '/Admin/Security/SaveRoleRights',
            data: { rolename: $scope.UserModel.RoleName, modulelist: $scope.UserModel.ModuleIDs, notinaccess: llist.toString() }
        }).success(function (data) {
            alert(data)
            $('#loading').hide();

        })
    }

    $scope.GetUserAccess = function () {

        var userid = $("#Email").val();
        var pass = $("#Password").val();
        $('#loading').show();
        $http({
            method: 'Post',
            url: '/Account/Login',
            data: JSON.stringify($scope.UserModel)
        }).success(function (data) {
            var res = data;

            $http({
                method: 'Get',
                url: '/Admin/Security/GetUersAccess',
                params: { UserName: userid }
            }).success(function (data) {
                $scope.UserAccessList = data;
                var plist = [];
                for (var i = 0; i < $scope.UserAccessList.length; i++) {
                    var list = $scope.UserAccessList[i];
                    plist.push(list.ModuleListID);
                }
                localStorage.setItem("UserAccess", plist.toString());

                $('#loading').hide();
                if (res == "OK") {
                    window.open("/", "_self")
                }
                else {

                    alert(res);
                }
            })
        }).error(function (ex) {
            alert(ex);
            $('#loading').hide();
        })

    }
    $scope.ViewUsersInit = function () {
        $http.get('/Admin/Security/GetUsersInfoList/').success(function (response) { $scope.UserInfoList = response; });
    }
    $scope.orderUserInfoList = function (predicate, reverse) {
        $scope.UserInfoList = orderBy($scope.UserInfoList, predicate, reverse);
    };
    function ValidateAddUser() {
        var vl = true;
        var message = "";

        if ($('#RoleName :selected').text() == "") {
            vl = false;
            message += "Select User Role <br/>";
        }

        if ($("#UserName").val() == "") {
            vl = false;
            message += "Please insert User Name.<br/>";
        }
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
        if ($("#Password").val() == "") {
            vl = false;
            message += "Please insert Password.<br/>";
        }

        if ($("#ConfirmPassword").val() == "") {
            vl = false;
            message += "Please insert Confirm Password.<br/>";
        }
        if ($("#ConfirmPassword").val() != $("#Password").val()) {
            vl = false;
            message += "Confirm password does not match.<br/>";
        }
        $("#ErrorMessage").html(message);
        return vl;
    }
    $('#loading').hide();
});