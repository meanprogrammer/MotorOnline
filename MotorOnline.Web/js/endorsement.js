$(document).ready(initialize);

function initialize() {
    $('#endsavebutton').click(saveendorsement);
    $('#endcancelbutton').click(cancelendorsement);
}

function saveendorsement () {
    var selected = $('#EndorsementDropdown').val();
    var transactionid = $('#IdHiddenField').val();
    var etext = $('#endorsementtext').val();
    var edate = $('#effectivitydate').val();

    var expdate = ($('#pagetypehidden').val() == 'detail' ? $('#policyperiodto').html() : $('#PeriodToTextbox').val());

    resetvalidations();

    if (edate == '' || !validateString(edate)) {
        $('#effectivitydate').addClass('control-validation-error');
        return;
    }

    switch (selected) {
        case '3':
            updatecocno(selected, transactionid, etext, edate, expdate);
            break;
        case '15':
            updateinsuredname(selected, transactionid, etext, edate, expdate);
            break;
        case '19':
        case '17':
            updateaddress(selected, transactionid, etext, edate, expdate);
            break;
        case '20':
        case '22':
            updatemortgagee(selected, transactionid, etext, edate, expdate);
            break;
        case '21':
            deletemortgagee(selected, transactionid, etext, edate, expdate);
            break;
        case '25':
            updatepolicyperiod(selected, transactionid, etext, edate, expdate);
            break;
        case '33':
            updatevehicledescription(selected, transactionid, etext, edate, expdate);
            break;
        case '23':
            updatetransferownership(selected, transactionid, etext, edate, expdate);
            break;
        default:
            break;
    }
    //savetransaction();
}

function resetvalidations() {
    $('.control-validation-error').removeClass('control-validation-error');
}

function cancelendorsement() {
    resetvalidations();
    $('#EndorsementDropdown').val('0');
    $('#endorsementtext').html('');
    $('#endorsement-controls').html('');
    $('#endorsement-dialog').dialog('close');
}

function updatecocno(type, transactionid, etext, edate, expdate) {
    var newCocNo = $('#e_cocno').val();
    var policyno = $('#lblPolicyNo').html();

    if (newCocNo == '' || !validateString(newCocNo)) {
        $('#e_cocno').addClass('control-validation-error');
        return;
    }

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            newcocno: newCocNo,
            transactionid: transactionid,
            policyno: policyno,
            etext: etext,
            edate: edate,
            expdate: expdate
        },
        success: function (result) {
            var obj = JSON.parse(result);
            handlesaveendorsement(obj);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function updateinsuredname(type, transactionid, etext, edate, expdate) {
    var newlastname = $('#e_lastname').val();
    var newfirstname = $('#e_firstname').val();
    var newmi = $('#e_mi').val();
    var customerid = $('#CustomerInfo').val();
    var policyno = $('#lblPolicyNo').html();

    var isValid = true;
    if (newlastname == '' || !validateString(newlastname)) {
        $('#e_lastname').addClass('control-validation-error');
        isValid = false;
    }

    if (newfirstname == '' || !validateString(newfirstname)) {
        $('#e_firstname').addClass('control-validation-error');
        isValid = false;
    }

    if (newmi == '' || !validateString(newmi)) {
        $('#e_mi').addClass('control-validation-error');
        isValid = false;
    }

    if (isValid == false) {
        return;
    }

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            newlastname: newlastname,
            newfirstname: newfirstname,
            newmi: newmi,
            customerid: customerid,
            policyno: policyno,
            etext: etext,
            edate: edate,
            expdate: expdate,
            transactionid: transactionid
        },
        success: function (result) {
            var obj = JSON.parse(result);
            handlesaveendorsement(obj);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function updateaddress(type, transactionid, etext, edate, expdate) {
    var newaddress = $('#e_address').val();
    var customerid = $('#CustomerInfo').val();
    var policyno = $('#lblPolicyNo').html();

    if (newaddress == '' || !validateString(newaddress)) {
        $('#e_address').addClass('control-validation-error');
        return;
    }

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            newaddress: newaddress,
            customerid: customerid,
            policyno: policyno,
            etext: etext,
            edate: edate,
            expdate: expdate,
            transactionid: transactionid
        },
        success: function (result) {
            var obj = JSON.parse(result);
            handlesaveendorsement(obj);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function updatemortgagee(type, transactionid, etext, edate, expdate) {
    var newmortgagee = $('#e_mortgagee').val();
    var policyno = $('#lblPolicyNo').html();

    if (newmortgagee == '0') {
        $('#e_mortgagee').addClass('control-validation-error');
        return;
    }

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            newmortgagee: newmortgagee,
            transactionid: transactionid,
            policyno: policyno,
            etext: etext,
            edate: edate,
            expdate: expdate
        },
        success: function (result) {
            var obj = JSON.parse(result);
            handlesaveendorsement(obj);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function deletemortgagee(type, transactionid, etext, edate, expdate) {
    var policyno = $('#lblPolicyNo').html();
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            transactionid: transactionid,
            policyno: policyno,
            etext: etext,
            edate: edate,
            expdate: expdate
        },
        success: function (result) {
            var obj = JSON.parse(result);
            handlesaveendorsement(obj);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function updatepolicyperiod(type, transactionid, etext, edate, expdate) {
    var periodfrom = $('#e_policyperiodfrom').val();
    var periodto = $('#e_policyperiodto').val();
    var policyno = $('#lblPolicyNo').html();

    if (periodfrom == '' || !validateString(periodfrom)) {
        $('#e_policyperiodfrom').addClass('control-validation-error');
        return;
    }

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            transactionid: transactionid,
            periodfrom: periodfrom,
            periodto: periodto,
            etext: etext,
            edate: edate,
            expdate: expdate,
            policyno: policyno
        },
        success: function (result) {
            var obj = JSON.parse(result);
            handlesaveendorsement(obj);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function updatevehicledescription(type, transactionid, etext, edate, expdate) {
    var carcompany = $('#e_carcompanydropdown').val();
    var carmake = $('#e_carmakedropdown').val();
    var engineseries = $('#e_enginedropdown').val();
    var policyno = $('#lblPolicyNo').html();

    var isvalid = true;
    if (carcompany == '0') {
        $('#e_carcompanydropdown').addClass('control-validation-error');
        isvalid = false;
    }

    if (carmake == '0') {
        $('#e_carmakedropdown').addClass('control-validation-error');
        isvalid = false;
    }

    if (engineseries == '0') {
        $('#e_enginedropdown').addClass('control-validation-error');
        isvalid = false;
    }

    if (isvalid == false) {
        return;
    }

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            transactionid: transactionid,
            carcompany: carcompany,
            carmake: carmake,
            engineseries: engineseries,
            etext: etext,
            edate: edate,
            expdate: expdate,
            policyno: policyno
        },
        success: function (result) {
            var obj = JSON.parse(result);
            handlesaveendorsement(obj);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function updatetransferownership(type, transactionid, etext, edate, expdate) {
    var policyno = $('#lblPolicyNo').html();
    var toi = $('#e_typeofinsurance').val();
    var designation = $('#e_designation').val();
    var lastname = $('#e_lastname').val();
    var firstname = $('#e_firstname').val();
    var mi = $('#e_mi').val();
    var multicorpname = $('#e_multinamecorporatetext').val();

    var isvalid = true;

    if (lastname == '' || !validateString(lastname)) {
        $('#e_lastname').addClass('control-validation-error');
        isvalid = false;
    }

    if (firstname == '' || !validateString(firstname)) {
        $('#e_firstname').addClass('control-validation-error');
        isvalid = false;
    }

    if (mi == '' || !validateString(mi)) {
        $('#e_mi').addClass('control-validation-error');
        isvalid = false;
    }

    if (toi == '0') {
        $('#e_typeofinsurance').addClass('control-validation-error');
        isvalid = false;
    }

    if (toi == '2' || toi == '3') {
        if (multicorpname == '' || !validateString(multicorpname)) {
            $('#e_multinamecorporatetext').addClass('control-validation-error');
            isvalid = false;
        }
    }

    if (!isvalid) {
        return;
    }

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            transactionid: transactionid,
            etext: etext,
            edate: edate,
            expdate: expdate,
            policyno: policyno,
            toi: toi,
            designation: designation,
            lastname: lastname,
            firstname: firstname,
            mi: mi,
            multicorpname: multicorpname
        },
        success: function (result) {
            var obj = JSON.parse(result);
            handlesaveendorsement(obj);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function handlesaveendorsement(result) {
    if (result.Status == 'true') {
    //NOTE: Just don't ask if they want to print
//        if(confirm('The transaction has been successfully endorsed and changes have been made. Do you want to view the print out?'))
//        {
//            var id = $('#IdHiddenField').val();
//            //Show print out in new window
//            var url = 'EndorsementPrintOut.aspx?id=' + id;
//            window.open(url, "_blank", 'toolbar=0,location=0,menubar=0');
//        }
        //redirect here
        window.location.href = "TransactionDetailsView.aspx?id=" + result.NewID;
    } else {
        alert('endorsement failed!');
    }
}

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
    var pagetype = $('#pagetypehidden').val();
    if (pagetype == 'detail') {
        $.ajax({
            url: "ajax/TransactionAjax.aspx",
            type: "post",
            cache: true,
            data: {
                action: "loadtypeofinsurance"
            },
            success: function (result) {
                var obj = JSON.parse(result);
                if (obj != null) {
                    html = '<option value="0">Select Type Of Insurance</option>';
                    $.each(obj, function (key, value) {
                        html += '<option value="' + value.TypeOfInsuranceID + '">' + value.TypeOfInsuranceName + '</option>';
                    });
                    $('#e_typeofinsurance').html(html);
                }
            },
            error: function () {
                hideloader();
            }
        });
    } else {
        var insurances = $('#TypeOfInsuranceDropdown').html();
        $('#e_typeofinsurance').html(insurances);
    }
}

function copymortgagee() {
    var pagetype = $('#pagetypehidden').val();
    if (pagetype == 'detail') {
        $.ajax({
            url: "ajax/TransactionAjax.aspx",
            type: "post",
            cache: true,
            data: {
                action: "loadmortgagee"
            },
            success: function (result) {
                var obj = JSON.parse(result);
                if (obj != null) {
                    html = '<option value="0">Select Mortgagee</option>';
                    $.each(obj, function (key, value) {
                        html += '<option value="' + value.MortgageeID + '">' + value.MortgageeName + '</option>';
                    });
                    $('#e_mortgagee').html(html);
                    $('#e_mortgagee').val($('#mortgageehidden').val());
                }
            },
            error: function () {
                hideloader();
            }
        });
    } else {
        var motgagee = $('#ddlMortgagee').html();
        $('#e_mortgagee').html(motgagee);
    }
}

function copymortgageeselected() {
    var pagetype = $('#pagetypehidden').val();
    if (pagetype != 'detail') {
        var orig_motgagee_selected = $('#ddlMortgagee').val();
        $('#e_mortgagee').val(orig_motgagee_selected);
    }
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
    var pagetype = $('#pagetypehidden').val();
    if (pagetype == 'detail') {
        var from = $('#policyperiodfrom').html();
        var to = $('#policyperiodto').html();

        $('#e_policyperiodfrom').val(from);
        $('#e_policyperiodto').val(to);
    } else {
        var from = $('#PeriodFromTextbox').val();
        var to = $('#PeriodToTextbox').val();

        $('#e_policyperiodfrom').val(from);
        $('#e_policyperiodto').val(to);
    }
}

function copycarcompanies() {
    var pagetype = $('#pagetypehidden').val();
    if (pagetype == 'detail') {
        $.ajax({
            url: "ajax/TransactionAjax.aspx",
            type: "post",
            cache: true,
            data: {
                action: "getcarcompanies"
            },
            success: function (result) {
                var obj = JSON.parse(result);
                if (obj != null) {
                    html = '<option value="0">Select Car Company</option>';
                    $.each(obj, function (key, value) {
                        html += '<option value="' + value.Value + '">' + value.Text + '</option>';
                    });
                    $('#e_carcompanydropdown').html(html);
                    var company = $('#carcompanycode').val();
                    $('#e_carcompanydropdown').val(company);
                    loadcarmake(company);

                }
            },
            error: function () {
                hideloader();
            },
        });
    } else {
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

    }

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


function loadcarmake(companyid){
    $.ajax({
            url: "ajax/TransactionAjax.aspx",
            type: "post",
            cache: true,
            data: {
                action: "filtercarmake",
                compid: companyid
            },
            success: function (result) {
                var obj = JSON.parse(result);
                if (obj != null) {
                    html = '<option value="0">Select Car Make/Series</option>';
                    $.each(obj, function (key, value) {
                        html += '<option value="' + value.Value + '">' + value.Text + '</option>';
                    });
                    $('#e_carmakedropdown').html(html);
                    var carmakecode = $('#carmakecode').val();
                    $('#e_carmakedropdown').val(carmakecode);
                    loadengine(companyid, carmakecode);
                }
            },
            error: function () {
                hideloader();
            },
     });
}

function loadengine(companyid, makeandseries){
            var ids = makeandseries.split("|");
            var carMakeId = ids[0];
            var carSeriedId = ids[1];
    $.ajax({
            url: "ajax/TransactionAjax.aspx",
            type: "post",
            cache: true,
            data: {
                action: "filterengine",
                compid: companyid,
                makeid: carMakeId,
                seriesid: carSeriedId
            },
            success: function (result) {
                var obj = JSON.parse(result);
                if (obj != null) {
                    html = '<option value="0">Select Engine</option>';
                    $.each(obj, function (key, value) {
                        html += '<option value="' + value.Value + '">' + value.Text + '</option>';
                    });
                    $('#e_enginedropdown').html(html);
                    $('#e_enginedropdown').val($('#enginecode').val());


                }
            },
            error: function () {
                hideloader();
            },
     });
}