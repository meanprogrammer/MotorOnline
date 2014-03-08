var validationcontrols = [];
validationcontrols.push('ddlCreditingBranch');
validationcontrols.push('SublineDropdown');
validationcontrols.push('ddInterMediary');
validationcontrols.push('PeriodFromTextbox');
validationcontrols.push('PeriodToTextbox');
validationcontrols.push('TypeOfInsuranceDropdown');
validationcontrols.push('txtFirstName');
validationcontrols.push('txtLastName');
validationcontrols.push('txtMI');
validationcontrols.push('txtMailAdress');
validationcontrols.push('txtMobileNo');

function resetvalidationhighlights() {
    $.each(validationcontrols, function (key, value) {
        $('#' + value).removeClass('control-validation-error');
    });
    $('#cardetails-span').removeClass('validation-msg');
}

function validatebeforeaddingcardetails() {
    var isvalid = true;

    var subline = $('#SublineDropdown').val();
    if (subline === '0') {
        isvalid = false;
    }

    return isvalid;
}

//NOTE: Disgusting validation
function validateall() {
    resetvalidationhighlights();
    var isvalid = true;
    var messages = [];

    if ($('#ddlCreditingBranch').val() == '0') {
        isvalid = false;
        messages.push('Please select a Crediting Branch.');
        $('#ddlCreditingBranch').addClass('control-validation-error');
    }

    if ($('#SublineDropdown').val() == '0') {
        isvalid = false;
        messages.push('Please select a Subline.');
        $('#SublineDropdown').addClass('control-validation-error');
    }

    if ($('#ddInterMediary').val() == '0') {
        isvalid = false;
        messages.push('Please select a Intermediary.');
        $('#ddInterMediary').addClass('control-validation-error');
    }

    if ($('#PeriodFromTextbox').val() == '' || $('#PeriodToTextbox').val() == '') {
        isvalid = false;
        messages.push('Select select a Period From.');
        $('#PeriodFromTextbox').addClass('control-validation-error');
        $('#PeriodToTextbox').addClass('control-validation-error');
    }

    if ($('#TypeOfInsuranceDropdown').val() == '0') {
        isvalid = false;
        messages.push('Select select a Type of insurance.');
        $('#TypeOfInsuranceDropdown').addClass('control-validation-error');
    }

    var firstname = $('#txtFirstName').val();
    if (firstname == '' || !validateString(firstname)) {
        isvalid = false;
        messages.push('First name cannot be empty.');
        $('#txtFirstName').addClass('control-validation-error');
    }

    var lastname = $('#txtLastName').val();
    if (lastname == '' || !validateString(lastname)) {
        isvalid = false;
        messages.push('Last name cannot be empty.');
        $('#txtLastName').addClass('control-validation-error');
    }

    var mi = $('#txtMI').val();
    if (mi == '' || !validateString(mi)) {
        isvalid = false;
        messages.push('Middle name cannot be empty.');
        $('#txtMI').addClass('control-validation-error');
    }

    var mailaddress = $('#txtMailAdress').val();
    if (mailaddress == '' || !validateString(mailaddress)) {
        isvalid = false;
        messages.push('Address cannot be empty.');
        $('#txtMailAdress').addClass('control-validation-error');
    }

    var mobileno = $('#txtMobileNo').val();
    if (mobileno == '' || !validateString(mobileno)) {
        isvalid = false;
        messages.push('Please enter a valid mobile no.');
        $('#txtMobileNo').addClass('control-validation-error');
    }

    var cardetailsjson = $('#CarDetailsHidden').val();
    if (cardetailsjson == '' || !validateString(cardetailsjson)) {
        isvalid = false;
        messages.push('There is no car details yet.');
        $('#cardetails-span').addClass('validation-msg');
    }

    return messages;
}
//END NOTE

function savetransaction() {
    showloader('Saving Transaction...');
    var validations = validateall();
    if (validations.length > 0) {
        hideloader();
        return false;
    }

    //Force compute
    if (!handlecompute()) {
        hideloader();
        return false;
    }
    
    var t = buildtransactionobject();
    var perils = buildtransactionperils();
    var cardetails = $('#CarDetailsHidden').val();
    var net = parseFloat($('#basic-premiumnettext').html());
    var gross = parseFloat($('#basic-premiumgrosstext').html());

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            "action": 'savetransaction',
            "transaction": JSON.stringify(t),
            "cardetails": cardetails,
            "perils": JSON.stringify(perils),
            "basicpremiumnet": net,
            "basicpremiumgross": gross
        },
        success: function (result) {
            var json = JSON.parse(result);
            if (json.SaveSuccess == '1') {
                hideloader();
                alert('save success');
                if(parseInt(json.TransactionID) > 0) {
                    window.location.href = 'TransactionDetailsView.aspx?id=' + json.TransactionID;
                }
            }
            else {
                hideloader();
                alert('save failed');
            }
        },
        error: function () {
            hideloader();
        }
    });

    return false;
}

function updatetransaction() {
    showloader('Updating ...');
    var validations = validateall();
    if (validations.length > 0) {
        return false;
    }

    //Force compute
    if (!handlecompute()) {
        return false;
    }

    var t = buildtransactionobject();

    var perils = buildtransactionperils();
    var cardetails = $('#CarDetailsHidden').val();
    var net = parseFloat($('#basic-premiumnettext').html());
    var gross = parseFloat($('#basic-premiumgrosstext').html());
    var id = $('#IdHiddenField').val();

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            "action": 'updatetransaction',
            "transaction": JSON.stringify(t),
            "cardetails": cardetails,
            "perils": JSON.stringify(perils),
            "basicpremiumnet": net,
            "basicpremiumgross": gross,
            "transactionid": id
        },
        success: function (result) {
            if (result == '1') {
                hideloader();
                alert('update success');
                clearallcontrols();
            }
            else {
                hideloader();
                alert('update failed');
            }
        },
        error: function () {
            hideloader();
        }
    });

    return false;
}

function clearallcontrols() {
    //NOTICE: redirecting to own page for now
    document.location = 'TransactionDetailsView.aspx?id=' + $('#IdHiddenField').val();
}

function buildtransactionperils() {
    var perils = [];
    var selectedCovertype = $('#TypeOfCoverDropdown').val();
    $.each(computationmatrix[selectedCovertype].ids, function (key, value) {
        var limit = $('#' + associatedcontrols[value].limitsi).val();
        if (value === 194 || value === 195) {
            limit = $('#' + associatedcontrols[value].limitsi + ' option:selected').text()
        }
        perils.push({
            "NewLimitSI": limit,
            "NewRate": $('#' + associatedcontrols[value].rate).val(),
            "NewPremium": $('#' + associatedcontrols[value].premium).html(),
            "NewPolicyRate": $('#' + associatedcontrols[value].policyrate).val(),
            "NewPolicyPremium": $('#' + associatedcontrols[value].policypremium).html(),
            "Data": $('#' + associatedcontrols[value].data).val()
        });
    });
    return perils;
}

function buildtransactionobject() {
    var transaction = {
        "CreditingBranch": $('#ddlCreditingBranch').val(),
        "ParNo": $('#lblParNo').html(),
        "PolicyNo": $('#lblPolicyNo').html(),
        "GeniisysNo": $('#lblGeniisysNo').html(),
        "DateCreated": $('#lblCurrentDate').html(),
        "PolicyPeriodFrom": $('#PeriodFromTextbox').val(),
        "PolicyPeriodTo": $('#PeriodToTextbox').val(),
        "BussinessType": $('#ddBusinessType').val(),
        "PolicyStatus": $('#lblPolicyStatus').html(),
        "SubLineCode": $('#SublineDropdown').val(),
        "MortgageCode": $('#ddlMortgagee').val(),
        "IntermediaryCode": $('#ddInterMediary').val(),
        "TypeOfInsured": $('#TypeOfInsuranceDropdown').val(),
        "IsPosted": false,
        "IsPrinted": false,
        "IsEndoresed": false,
        "Remarks": $('#remarks').val(),
        "Designation": $('#ddDesignation').val(),
        "LastName": $('#txtLastName').val(),
        "FirstName": $('#txtFirstName').val(),
        "MiddleName": $('#txtMI').val(),
        "Address": $('#txtMailAdress').val(),
        "Telephone": $('#txtTelephone').val(),
        "MobileNo": $('#txtMobileNo').val(),
        "Email": $('#txtEmailAdd').val(),
        "MultipleCorporateName": $('#CorporateMultipleTextbox').val(),
        "CustomerID": $('#CustomerInfo').val()
    };
    return transaction;
}

//====

function validateString(text) {
    return !(text === '' || text == undefined || !text.match(/\S/));
}


function loadtransaction(json, id) {
    var transaction = json.Transaction;
    var companies = json.CarCompanies;
    var carmakes = json.CarMakes;
    var engines = json.CarEngines;
    $('#ddlCreditingBranch').val(transaction.CreditingBranch);
    $('#lblParNo').html(transaction.ParNo);
    $('#lblPolicyNo').html(transaction.PolicyNo);
    $('#lblGeniisysNo').html(transaction.GeniisysNo);
    $('#lblCurrentDate').html(transaction.DateCreatedText);
    $('#PeriodFromTextbox').val(transaction.PolicyPeriodFromText);
    $('#PeriodToTextbox').val(transaction.PolicyPeriodToText);
    $('#ddBusinessType').val(transaction.BussinessType);
    $('#lblPolicyStatus').html(transaction.PolicyStatus);
    $('#SublineDropdown').val(transaction.SubLineCode);
    $('#ddlMortgagee').val(transaction.MortgageCode);

    $('#ddInterMediary').val(transaction.IntermediaryCode);

    $('#TypeOfInsuranceDropdown').val(transaction.TypeOfInsurance);

    $('#TypeOfCoverDropdown').val(transaction.CarDetail.TypeOfCover);
    renderdropdown(companies, 'CarCompaniesDropdown');
    $('#CarCompaniesDropdown').val(transaction.CarDetail.CarCompany);
    renderdropdown(carmakes, 'CarMakeDropdown');
    $('#CarMakeDropdown').val(transaction.CarMakeAndSeriesText);
    renderdropdown(engines, 'EngineDropdown');
    $('#EngineDropdown').val(json.CarEngineText);

    var cardetail = createcardetails();
    populatecardetaildisplay(cardetail);

    $('#CarDetailsHidden').val(JSON.stringify(cardetail));

    displayperilsedit(transaction.Perils, 
                      transaction.Remarks,
                      json.Tariff);

    //NOTE: This is for edit mode, if the transaction is loaded and
    //the current selected type of insurance must show the hidden controls
    var loadedTypeOfIns = $('#TypeOfInsuranceDropdown').val();

    if (parseInt(loadedTypeOfIns) > 1) {
        loadedTypeOfIns == 2 ? $('#CorporateMultipleLabel').html('Multiple Name') :
                                $('#CorporateMultipleLabel').html('Corporate Name');
        toggleaddtionaltextbox(true);

    }


    //TODO: Change this! It sucks!
    //NOTE: This is nested because we wait for the ajax call to finish, 
    //we need the result to continue populating the edit view
//    carcompanychangewithcallback(function () {
//        
//        carmakechangewithcallback(function () {
//            

//            var cardetail = createcardetails();
//            populatecardetaildisplay(cardetail);

//            $('#CarDetailsHidden').val(JSON.stringify(cardetail));

//            displayperilsedit(json.Perils, json.Remarks);

//            //NOTE: This is for edit mode, if the transaction is loaded and
//            //the current selected type of insurance must show the hidden controls
//            var loadedTypeOfIns = $('#TypeOfInsuranceDropdown').val();

//            if (parseInt(loadedTypeOfIns) > 1) {
//                loadedTypeOfIns == 2 ? $('#CorporateMultipleLabel').html('Multiple Name') :
//                                        $('#CorporateMultipleLabel').html('Corporate Name');
//                toggleaddtionaltextbox(true);

//            }
//            //HACK
//            hideloader();

//            //END NOTE
//        });
//    });
    //END NOTE

    $('#txtEngine').val(transaction.CarDetail.EngineNo);
    $('#txtColor').val(transaction.CarDetail.Color);
    $('#txtConductionNo').val(transaction.CarDetail.ConductionNo);
    $('#txtChasis').val(transaction.CarDetail.ChassisNo);
    $('#txtPlateNo').val(transaction.CarDetail.PlateNo);
    $('#txtAccesories').val(transaction.CarDetail.Accessories);
    $('#YearDropdown').val(transaction.CarDetail.CarYear);
    $('#TypeOfBodyDropdown').val(transaction.CarDetail.CarTypeOfBodyID);
    $('#txtAuthenticationNo').val(transaction.CarDetail.AuthenticationNo);
    $('#txtCOCNo').val(transaction.CarDetail.COCNo);

    $('#ddDesignation').val(transaction.Customer.Designation);
    $('#txtLastName').val(transaction.Customer.LastName);
    $('#txtFirstName').val(transaction.Customer.FirstName);
    $('#txtMI').val(transaction.Customer.MiddleName);
    $('#txtMailAdress').val(transaction.Customer.Address);
    $('#txtTelephone').val(transaction.Customer.Telephone);
    $('#txtMobileNo').val(transaction.Customer.MobileNo);
    $('#txtEmailAdd').val(transaction.Customer.Email);
    $('#CorporateMultipleTextbox').val(transaction.Customer.MultipleCorporateName);
    $('#CustomerInfo').val(transaction.CustomerID);

    hideloader();
}

function rendercarcompanies(companies) {
    var html = '<option value="0">-- SELECT --</option>';
    $.each(companies, function (key, value) {
        html += '<option value="' + value.Value + '">' + value.Text + '</option>';
    });
    $('#CarMakeDropdown').html(html);
}

function rendercarmakes(carmakes) {
    var html = '<option value="0">-- SELECT --</option>';
    $.each(carmakes, function (key, value) {
        html += '<option value="' + value.Value + '">' + value.Text + '</option>';
    });
    $('#EngineDropdown').html(html);
}

function rendercarengines(engines) {
    var html = '<option value="0">-- SELECT --</option>';
    $.each(engines, function (key, value) {
        html += '<option value="' + value.Value + '">' + value.Text + '</option>';
    });
    $('#CarMakeDropdown').html(html);
}

function renderdropdown(data, key) {
    var html = '<option value="0">-- SELECT --</option>';
    $.each(data, function (key, value) {
        html += '<option value="' + value.Value + '">' + value.Text + '</option>';
    });
    $('#'+key).html(html);
}

function loadtransactiondetails(json, id) {
    $('#creditingbranchlabel').html(json.CreditingBranchName);
    $('#lblParNo').html(json.ParNo);
    $('#lblPolicyNo').html(json.PolicyNo);
    $('#lblGeniisysNo').html(json.GeniisysNo);
    $('#lblCurrentDate').html(json.DateCreatedText);
    $('#policyperiodfrom').html(json.PolicyPeriodFromText);
    $('#policyperiodto').html(json.PolicyPeriodToText);
    $('#businesstype').html(json.BussinessType);
    $('#lblPolicyStatus').html(json.PolicyStatus);
    $('#subline').html(json.SublineText);
    $('#mortgagee').html(json.MortgageeName);
    $('#mortgageehidden').val(json.MortgageCode);
    $('#intermediary').html(json.IntermediaryName);
    $('#typeofinsurance').html(json.TypeOfInsuranceName);

    $('#TypeOfCoverDropdown').val(json.CarDetail.TypeOfCover);
    $('#CarCompaniesDropdown').val(json.CarDetail.CarCompany);

    var cardetail = json.CarDetail;
    $('#lblSublineType').html(json.SublineText);
    $('#lblTypeOfCover').html(cardetail.TypeOfCoverText);
    $('#lblYear').html(cardetail.CarYearText);
    $('#lblCarCompany').html(cardetail.CarCompanyText);
    $('#lblMake').html(cardetail.CarMakeText);
    $('#lblMotorType').html(cardetail.MotorType);
    $('#lblEngineNo').html(cardetail.EngineNo);
    $('#lblChasisNo').html(cardetail.ChassisNo);

    $('#lblColor').html(cardetail.Color);
    $('#lblPlateNo').html(cardetail.PlateNo);
    $('#lblConductionNo').html(cardetail.ConductionNo);
    $('#lblAccessories').html(cardetail.Accessories);
    $('#lblAuthenticationNo').html(cardetail.AuthenticationNo);
    $('#lblCOCNo').html(cardetail.COCNo);
    
    $('#CarDetailsHidden').val(JSON.stringify(cardetail));

    displayperilsdetails(json.Perils, json.Remarks, json.Computations, cardetail);

    var loadedTypeOfIns = json.TypeOfInsurance;

    if (parseInt(loadedTypeOfIns) > 1) {
        loadedTypeOfIns == 2 ? $('#CorporateMultipleLabel').html('Multiple Name') :
                                $('#CorporateMultipleLabel').html('Corporate Name');
        toggleaddtionaltextbox(true);
    } else {
        toggleaddtionaltextbox(false);
    }

    $('#txtEngine').val(json.CarDetail.EngineNo);
    $('#txtColor').val(json.CarDetail.Color);
    $('#txtConductionNo').val(json.CarDetail.ConductionNo);
    $('#txtChasis').val(json.CarDetail.ChassisNo);
    $('#txtPlateNo').val(json.CarDetail.PlateNo);
    $('#txtAccesories').val(json.CarDetail.Accessories);
    $('#YearDropdown').val(json.CarDetail.CarYear);
    $('#TypeOfBodyDropdown').val(json.CarDetail.CarTypeOfBodyID);
    $('#txtAuthenticationNo').val(json.CarDetail.AuthenticationNo);
    $('#txtCOCNo').val(json.CarDetail.COCNo);

    $('#carcompanycode').val(json.CarDetail.CarCompany);
    $('#carmakecode').val(json.CarMakeAndSeriesText);
    $('#enginecode').val(json.CarEngineText);

    $('#designation').html(json.Customer.Designation);
    $('#lastname').html(json.Customer.LastName);
    $('#firstname').html(json.Customer.FirstName);
    $('#middlename').html(json.Customer.MiddleName);
    $('#address').html(json.Customer.Address);
    $('#telephone').html(json.Customer.Telephone);
    $('#mobileno').html(json.Customer.MobileNo);
    $('#emailaddress').html(json.Customer.Email);
    $('#corporatemultiple').html(json.Customer.MultipleCorporateName);
    $('#CustomerInfo').val(json.CustomerID);

    if (json.IsPosted == true) {
        $('#endorsebutton').css('display', 'inline');
        $('#editbutton').css('display', 'none');
    } else {
        $('#endorsebutton').css('display', 'none');
        $('#editbutton').css('display', 'inline');
    }

    hideloader();
}

function showloader(message) {
    if (message != undefined && message != '') {
        $('#progress-message').html(message);
    }
    $('#progressbox').css('display', 'block');
    $('#progressbox').dialog({ modal: true, resizable: false, draggable: false, dialogClass: 'notitledialog', width: 300, height: 80 });
}

function hideloader() {
    $('#progressbox').dialog('close');
    $('#progressbox').css('display', 'none');
}

//function loadallendorsement() {
//    $.ajax({
//        url: "ajax/TransactionAjax.aspx",
//        type: "post",
//        data: {
//            "action": 'loadallendorement'
//        },
//        success: function (result) {
//            var json = JSON.parse(result);
//            if (json != null) {
//                html = '<option value="0">--SELECT--</option>';
//                $.each(json, function (key, value) {
//                    html += '<option value="' + value.EndorsementCode + '">' + value.EndorsementTitle + '</option>';
//                });
//                $('#endorsementdropdown').html(html);
//            }
//        },
//        error: function () {
//            hideloader();
//        }
//    });
//}