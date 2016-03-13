"use strict";


var app = angular.module('fileUpload', [ 'angularFileUpload' ]);
var version = '2.0.4';

app.controller('MyCtrl', [ '$scope', '$http', '$timeout', '$compile', '$upload', function($scope, $http, $timeout, $compile, $upload) {
    $('#loading').show();
    $scope.usingFlash = FileAPI && FileAPI.upload != null;
	$scope.fileReaderSupported = window.FileReader != null && (window.FileAPI == null || FileAPI.html5 != false);
	$scope.changeAngularVersion = function() {
		window.location.hash = $scope.angularVersion;
		window.location.reload(true);
	};
	
	$scope.angularVersion = window.location.hash.length > 1 ? (window.location.hash.indexOf('/') === 1 ? 
			window.location.hash.substring(2): window.location.hash.substring(1)) : '1.2.20';

	$scope.Error = "";
	//$http.get('/flat/GetPropertyList/').success(function (response) { $scope.Properties = response; });
	//$http.get('/flat/GetBanks/').success(function (response) { $scope.Banks = response; });

	$scope.UplaodInint = function () {
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
	    else
	    {
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
	$scope.SearchPropertyForGenAgreement = function () {
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

	// you can also $scope.$watch('files',...) instead of calling upload from ui
	$scope.upload = function(files) {
		$scope.formUpload = false;
		if (files != null) {
			for (var i = 0; i < files.length; i++) {
				$scope.errorMsg = null;
				(function(file) {
					$scope.generateThumb(file);
					eval($scope.uploadScript);
				})(files[i]);
			}
		}
		storeS3UploadConfigInLocalStore();
	};
	
	$scope.uploadPic = function (files) {
	    if ($("#rbtntypeemail").is(":checked")) {
	        var vli = ValidateUpload();
	        if (vli == false) {
	            $('#myModal').modal('show');
	        }
	        else {
	            $("#btnUPload").addClass("disabled");
	            $scope.formUpload = true;
	            if (files != null) {
	                var file = files[0];
	                $scope.generateThumb(file);
	                $scope.errorMsg = null;
	                eval($scope.uploadScript);
	            }
	            //$files: an array of files selected, each file has name, size, and type.
	            //for (var i = 0; i < files.length; i++) {
	            //    var file = $files[i];
	            this.upload = $upload.upload({
	                url: '/js/UploadHandler.ashx',
	                data: { name: 'dev', saleid: $("#SaleId").val(), adate: $("#AgDate").val() },
	                file: file, // or list of files ($files) for html5 only
	            }).progress(function (evt) {
	                //console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
	            }).success(function (data, status, headers, config) {
	                alert('Uploaded successfully ' + file.name);
	                $http({
	                    method: 'Get',
	                    url: '/Customer/Document/AgreementInfo',
	                    params: { saleid: $("#SaleId").val() }
	                }).success(function (data) {
	                    $scope.Agreement = data;
	                })
	            }).error(function (err) {
	                alert('Error occured during upload');
	                $("#btnUPload").removeClass("disabled");
	            });
	        }
		}
	}

	$scope.generateThumb = function(file) {
		if (file != null) {
			if ($scope.fileReaderSupported && file.type.indexOf('image') > -1) {
				$timeout(function() {
					var fileReader = new FileReader();
					fileReader.readAsDataURL(file);
					fileReader.onload = function(e) {
						$timeout(function() {
							file.dataUrl = e.target.result;
						});
					}
				});
			}
		}
	}
	
	$scope.generateSignature = function() {
		$http.post('/s3sign?aws-secret-key=' + encodeURIComponent($scope.AWSSecretKey), $scope.jsonPolicy).
			success(function(data) {
				$scope.policy = data.policy;
				$scope.signature = data.signature;
			});
	}

	if (localStorage) {
		$scope.s3url = localStorage.getItem("s3url");
		$scope.AWSAccessKeyId = localStorage.getItem("AWSAccessKeyId");
		$scope.acl = localStorage.getItem("acl");
		$scope.success_action_redirect = localStorage.getItem("success_action_redirect");
		$scope.policy = localStorage.getItem("policy");
		$scope.signature = localStorage.getItem("signature");
	}

	$scope.success_action_redirect = $scope.success_action_redirect || window.location.protocol + "//" + window.location.host;
	$scope.jsonPolicy = $scope.jsonPolicy || '{\n  "expiration": "2020-01-01T00:00:00Z",\n  "conditions": [\n    {"bucket": "angular-file-upload"},\n    ["starts-with", "$key", ""],\n    {"acl": "private"},\n    ["starts-with", "$Content-Type", ""],\n    ["starts-with", "$filename", ""],\n    ["content-length-range", 0, 524288000]\n  ]\n}';
	$scope.acl = $scope.acl || 'private';

	function storeS3UploadConfigInLocalStore() {
		if ($scope.howToSend == 3 && localStorage) {
			localStorage.setItem("s3url", $scope.s3url);
			localStorage.setItem("AWSAccessKeyId", $scope.AWSAccessKeyId);
			localStorage.setItem("acl", $scope.acl);
			localStorage.setItem("success_action_redirect", $scope.success_action_redirect);
			localStorage.setItem("policy", $scope.policy);
			localStorage.setItem("signature", $scope.signature);
		}
	}
	

	$scope.confirm = function() {
		return confirm('Are you sure? Your local changes will be lost.');
	}
	
	$scope.getReqParams = function() {
		return $scope.generateErrorOnServer ? "?errorCode=" + $scope.serverErrorCode + 
				"&errorMessage=" + $scope.serverErrorMsg : "";
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

	$scope.UploadAgreement = function () {

	}

	function ValidateSendAgreementMail() {
	    var vl = true;
	    var message = "";
	    if ($("#Email").val() == "") {
	        vl = false;
	        message += "Insert Email Address. <br/>";
	    }
	    $("#ErrorMessage").html(message);
	    return vl;
	}
	function ValidateUpload() {
	    var vl = true;
	    var message = "";
	    if ($("#AgDate").val() == "") {
	        vl = false;
	        message += "Insert Date. <br/>";
	    }
	    if ($("#file").val() == "") {
	        vl = false;
	        message += "Choose agreement file. <br/>";
	    }
	    $("#ErrorMessage").html(message);
	    return vl;
	}
	
	$('#loading').hide();
	
} ]);
