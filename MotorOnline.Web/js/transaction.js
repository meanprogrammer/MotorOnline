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
            if (result == '1') {
                hideloader();
                alert('save success');
                clearallcontrols();
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
    document.location = 'TransactionView.aspx';
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
    
    $('#ddlCreditingBranch').val(json.CreditingBranch);
    $('#lblParNo').html(json.ParNo);
    $('#lblPolicyNo').html(json.PolicyNo);
    $('#lblGeniisysNo').html(json.GeniisysNo);
    $('#lblCurrentDate').html(json.DateCreatedText);
    $('#PeriodFromTextbox').val(json.PolicyPeriodFromText);
    $('#PeriodToTextbox').val(json.PolicyPeriodToText);
    $('#ddBusinessType').val(json.BussinessType);
    $('#lblPolicyStatus').html(json.PolicyStatus);
    $('#SublineDropdown').val(json.SubLineCode);
    $('#ddlMortgagee').val(json.MortgageCode);

    $('#ddInterMediary').val(json.IntermediaryCode);

    $('#TypeOfInsuranceDropdown').val(json.TypeOfInsurance);

    $('#TypeOfCoverDropdown').val(json.CarDetail.TypeOfCover);
    $('#CarCompaniesDropdown').val(json.CarDetail.CarCompany);

    //TODO: Change this! It sucks!
    //NOTE: This is nested because we wait for the ajax call to finish, 
    //we need the result to continue populating the edit view
    carcompanychangewithcallback(function () {
        $('#CarMakeDropdown').val(json.CarMakeAndSeriesText);
        carmakechangewithcallback(function () {
            $('#EngineDropdown').val(json.CarEngineText);

            var cardetail = createcardetails();
            populatecardetaildisplay(cardetail);

            $('#CarDetailsHidden').val(JSON.stringify(cardetail));

            displayperilsedit(json.Perils, json.Remarks);

            //NOTE: This is for edit mode, if the transaction is loaded and
            //the current selected type of insurance must show the hidden controls
            var loadedTypeOfIns = $('#TypeOfInsuranceDropdown').val();

            if (parseInt(loadedTypeOfIns) > 1) {
                loadedTypeOfIns == 2 ? $('#CorporateMultipleLabel').html('Multiple Name') :
                                        $('#CorporateMultipleLabel').html('Corporate Name');
                toggleaddtionaltextbox(true);

            }
            //HACK
            hideloader();

            //END NOTE
        });
    });
    //END NOTE

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

    $('#ddDesignation').val(json.Customer.Designation);
    $('#txtLastName').val(json.Customer.LastName);
    $('#txtFirstName').val(json.Customer.FirstName);
    $('#txtMI').val(json.Customer.MiddleName);
    $('#txtMailAdress').val(json.Customer.Address);
    $('#txtTelephone').val(json.Customer.Telephone);
    $('#txtMobileNo').val(json.Customer.MobileNo);
    $('#txtEmailAdd').val(json.Customer.Email);
    $('#CorporateMultipleTextbox').val(json.Customer.MultipleCorporateName);
    $('#CustomerInfo').val(json.CustomerID);
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
    $('#intermediary').html(json.IntermediaryName);
    $('#typeofinsurance').html(json.TypeOfInsuranceName);

    $('#TypeOfCoverDropdown').val(json.CarDetail.TypeOfCover);
    $('#CarCompaniesDropdown').val(json.CarDetail.CarCompany);

    //TODO: Change this! It sucks!
    //NOTE: This is nested because we wait for the ajax call to finish, 
    //we need the result to continue populating the edit view
//    carcompanychangewithcallback(function () {
//        $('#CarMakeDropdown').val(json.CarMakeAndSeriesText);
//        carmakechangewithcallback(function () {
//            $('#EngineDropdown').val(json.CarEngineText);
    var cardetail = json.CarDetail;
    $('#lblSublineType').html(json.SublineText);
    //$('#lblSublineType').html($('#SublineDropdown option:selected').text());
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
//    var cardetail = createcardetails();
//            populatecardetaildisplay(cardetail);

           $('#CarDetailsHidden').val(JSON.stringify(cardetail));

           displayperilsdetails(json.Perils, json.Remarks, json.Computations, cardetail);

//            //NOTE: This is for edit mode, if the transaction is loaded and
//            //the current selected type of insurance must show the hidden controls
           var loadedTypeOfIns = json.TypeOfInsurance;

           if (parseInt(loadedTypeOfIns) > 1) {
               loadedTypeOfIns == 2 ? $('#CorporateMultipleLabel').html('Multiple Name') :
                                        $('#CorporateMultipleLabel').html('Corporate Name');
               toggleaddtionaltextbox(true);
           } else {
               toggleaddtionaltextbox(false);
           }
//            //HACK
//            hideloader();

//            //END NOTE
//        });
//    });
    //END NOTE

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
    } else {
        $('#endorsebutton').css('display', 'none');
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


function onendorsementselectchanged() {
    var s = $(this).val();
    //check if the selected is valid, if not dont do ajax call
    //just reset the values
    if (s == '0') {
        $('#endorsement-controls').html('');
        $('#endorsementtext').val('');
        return;
    }

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            "action": 'getendorsementbycode',
            "ecode": s
        },
        success: function (result) {
            var json = JSON.parse(result);
            if (json != null) {
                $('#endorsementtext').html(json.EndorsementText);
                html = '';
                switch (json.EndorsementCode) {
                    case 15:
                        html += '<table cellpadding="8"> <tr> <td> Last Name </td> <td> <input id="e_lastname" type="text" /> </td> </tr> <tr> <td> First Name </td> <td> <input id="e_firstname" type="text" /> </td> </tr> <tr> <td> MI Name </td> <td> <input id="e_mi" type="text" /> </td> </tr> </table>';
                        $('#endorsement-controls').html(html);
                        copynames();
                        break;
                    case 17:
                    case 19:
                        html += '<table cellpadding="8"> <tr> <td> Address </td> <td> <input id="e_address" type="text" style="width:300px;" /> </td> </tr> </table>';
                        $('#endorsement-controls').html(html);
                        copyaddress();
                        break;
                    case 20:
                        html += '<table cellpadding="8"> <tr> <td> Mortgagee </td> <td> <select id="e_mortgagee"> </select> </td> </tr> </table>';
                        $('#endorsement-controls').html(html);
                        copymortgagee();
                        copymortgageeselected();
                        break;
                    case 22:
                        html += '<table cellpadding="8"> <tr> <td> Mortgagee </td> <td> <select id="e_mortgagee"> </select> </td> </tr> </table>';
                        $('#endorsement-controls').html(html);
                        copymortgagee();
                        break;
                    case 21:
                        html += '<h3>Saving will delete the mortgagee for this transaction.</h3>';
                        $('#endorsement-controls').html(html);
                        break;
                    case 3:
                        html += ' <table cellpadding="8"> <tr> <td> COC No </td> <td> <input id="e_cocno" type="text" /> </td> </tr> </table>';
                        $('#endorsement-controls').html(html);
                        copycocno();
                        break;
                    case 25:
                        html += ' <table cellpadding="8"> <tr> <td> Policy Period From </td> <td> <input id="e_policyperiodfrom" type="text" /> </td> </tr> <tr> <td> Policy Period To </td> <td> <input id="e_policyperiodto" type="text" readonly="readonly" /> </td> </tr> </table>';
                        $('#endorsement-controls').html(html);
                        copypolicyperiods();
                        $('#e_policyperiodfrom').datepicker({
                            showOn: "button",
                            buttonImage: "images/Calendar-icon.png",
                            buttonImageOnly: true,
                            onSelect: handlecalendarselectendorsement
                        });
                        break;
                    case 33:
                        html += '<table cellpadding="8"> <tr> <td> Car Company </td> <td> </asp:DropDownList> <select id="e_carcompanydropdown" style="width: 200px;"> </select> <span class="required-field">*</span> </td> </tr> <tr> <td> Make </td> <td> <select id="e_carmakedropdown" style="width: 200px;"> </select> <span class="required-field">*</span> </td> </tr> <tr> <td> Engine </td> <td> <select id="e_enginedropdown"> </select> <span class="required-field">*</span> </td> </tr> </table>';
                        $('#endorsement-controls').html(html);
                        copycarcompanies();
                        break;
                    case 23:
                        html += '<table cellpadding="8"> <tr> <td> <strong>Type of insurance</strong> </td> <td colspan="3"> <select id="e_typeofinsurance"> <option></option> </select> &nbsp;<span class="required-field">*</span> </td> </tr> <tr> <td> <strong>Designation:</strong> </td> <td> <select id="e_designation"> <option value="Mr.">Mr.</option> <option value="Mrs.">Mrs.</option> </select> &nbsp;<span class="required-field">*</span> </td> <td> <strong><span id="e_multinamecorporatelabel" style="display:none;"></span></strong></td> <td> <input id="e_multinamecorporatetext" type="text" style="display:none;" /></td> </tr> <tr> <td> <strong>Last Name:</strong> </td> <td colspan="3"> <input id="e_lastname" type="text" /> &nbsp;<span class="required-field">*</span> </td> </tr> <tr> <td> <strong>First Name:</strong> </td> <td colspan="3"> <input id="e_firstname" type="text" /> &nbsp;<span class="required-field">*</span> </td> </tr> <tr> <td> <strong>M.I.:</strong> </td> <td colspan="3"> <input id="e_mi" type="text" /> &nbsp;<span class="required-field">*</span> </td> </tr> </table>';
                        $('#endorsement-controls').html(html);
                        copytypeofinsurance();
                        $('#e_typeofinsurance').change(function () {
                            var selectedValue = $('#e_typeofinsurance').val();
                            //multiname
                            if (selectedValue == 2) {
                                $('#e_multinamecorporatelabel').html('Multiple Name');
                                $('#e_multinamecorporatetext').show();
                                $('#e_multinamecorporatelabel').show();
                            } else if
                            //corporate
                            (selectedValue == 3) {
                                $('#e_multinamecorporatelabel').html('Corporate Name');
                                $('#e_multinamecorporatetext').show();
                                $('#e_multinamecorporatelabel').show();
                            } else {
                                $('#e_multinamecorporatetext').hide();
                                $('#e_multinamecorporatelabel').hide();
                            }
                        });
                        break;
                    default:
                        break;
                }
            }
        },
        error: function () {

        }
    });
}

function copytypeofinsurance() {
    var insurances = $('#TypeOfInsuranceDropdown').html();
    $('#e_typeofinsurance').html(insurances);
}

function copymortgagee() {
    var motgagee = $('#ddlMortgagee').html();
    $('#e_mortgagee').html(motgagee);
}

function copymortgageeselected() {
    var orig_motgagee_selected = $('#ddlMortgagee').val();
    $('#e_mortgagee').val(orig_motgagee_selected);
}

function copycocno() {
    var cocno = $('#lblCOCNo').html();
    $('#e_cocno').val(cocno);
}

function copynames() {
    var pagetype = $('#pagetypehidden').val();
    if (pagetype == 'detail') {
        var lname = $('#lastname').html();
        var fname = $('#firstname').html();
        var mi = $('#middlename').html();


        $('#e_lastname').val(lname);
        $('#e_firstname').val(fname);
        $('#e_mi').val(mi);
    } else {
        var lname = $('#txtLastName').val();
        var fname = $('#txtFirstName').val();
        var miname = $('#txtMI').val();

        $('#e_lastname').val(lname);
        $('#e_firstname').val(fname);
        $('#e_mi').val(miname);
    }
}

function copyaddress() {
    var pagetype = $('#pagetypehidden').val();
    if (pagetype == 'detail') {
        var adds = $('#address').html();
        $('#e_address').val(adds);
    } else {
        var adds = $('#txtMailAdress').val();
        $('#e_address').val(adds);
    }
}

function copypolicyperiods() {
    var from = $('#PeriodFromTextbox').val();
    var to = $('#PeriodToTextbox').val();

    $('#e_policyperiodfrom').val(from);
    $('#e_policyperiodto').val(to);
}

function copycarcompanies() {

    var companies = $('#CarCompaniesDropdown').html();
    var selectedcompany = $('#CarCompaniesDropdown').val();
    $('#e_carcompanydropdown').html(companies);
    $('#e_carcompanydropdown').val(selectedcompany);

    var carmakes = $('#CarMakeDropdown').html();
    var selectedcarmake = $('#CarMakeDropdown').val();
    $('#e_carmakedropdown').html(carmakes);
    $('#e_carmakedropdown').val(selectedcarmake);

    var carengines = $('#EngineDropdown').html();
    var selectedengine = $('#EngineDropdown').val();
    $('#e_enginedropdown').html(carengines);
    $('#e_enginedropdown').val(selectedengine);

    $('#e_carcompanydropdown').change({
        carcompanydropdownid: 'e_carcompanydropdown',
        carmakedropdownid: 'e_carmakedropdown',
        enginedropdownid: 'e_enginedropdown'
    }, carcompanychange);
    $('#e_carmakedropdown').change({
        carcompanydropdownid: 'e_carcompanydropdown',
        carmakedropdownid: 'e_carmakedropdown',
        enginedropdownid: 'e_enginedropdown'
    }, carmakechange);
}