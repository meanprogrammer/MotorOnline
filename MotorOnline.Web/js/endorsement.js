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