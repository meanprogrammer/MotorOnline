$(document).ready(initialize);

function initialize() {
    $('#endsavebutton').click(saveendorsement);
    $('#endcancelbutton').click(cancelendorsement);
}

function saveendorsement () {
    var selected = $('#EndorsementDropdown').val();
    var transactionid = $('#IdHiddenField').val();
    switch (selected) {
        case '3':
            updatecocno(selected, transactionid);
            break;
        case '15':
            updateinsuredname(selected, transactionid);
            break;
        case '19':
        case '17':
            updateaddress(selected, transactionid);
            break;
        case '20':
        case '22':
            updatemortgagee(selected, transactionid);
            break;
        case '21':
            deletemortgagee(selected, transactionid);
            break;
        case '25':
            updatepolicyperiod(selected, transactionid);
            break;
        case '33':
            updatevehicledescription(selected, transactionid);
            break;
        default:
            break;
    }
    //savetransaction();
}


function cancelendorsement() {
    $('#EndorsementDropdown').val('0');
    $('#endorsementtext').html('');
    $('#endorsement-controls').html('');
    $('#endorsement-dialog').dialog('close');
}

function updatecocno(type, transactionid) {
    var newCocNo = $('#e_cocno').val();
    var policyno = $('#lblPolicyNo').html();
    var etext = $('#endorsementtext').val();
    var edate = $('#effectivitydate').val();
    var expdate = $('#PeriodToTextbox').val();
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

function updateinsuredname(type, transactionid) {
    var newlastname = $('#e_lastname').val();
    var newfirstname = $('#e_firstname').val();
    var newmi = $('#e_mi').val();
    var customerid = $('#CustomerInfo').val();
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            newlastname: newlastname,
            newfirstname: newfirstname,
            newmi: newmi,
            customerid: customerid
        },
        success: function (result) {
            handlesaveendorsement(result);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function updateaddress(type, transactionid) {
    var newaddress = $('#e_address').val();
    var customerid = $('#CustomerInfo').val();
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            newaddress: newaddress,
            customerid: customerid
        },
        success: function (result) {
            handlesaveendorsement(result);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function updatemortgagee(type, transactionid) {
    var newmortgagee = $('#e_mortgagee').val();
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            newmortgagee: newmortgagee,
            transactionid: transactionid
        },
        success: function (result) {
            handlesaveendorsement(result);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function deletemortgagee(type, transactionid) {
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            transactionid: transactionid
        },
        success: function (result) {
            handlesaveendorsement(result);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function updatepolicyperiod(type, transactionid) {
    var periodfrom = $('#e_policyperiodfrom').val();
    var periodto = $('#e_policyperiodto').val();
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            transactionid: transactionid,
            periodfrom: periodfrom,
            periodto: periodto
        },
        success: function (result) {
            handlesaveendorsement(result);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function updatevehicledescription(type, transactionid) {
    var carcompany = $('#e_carcompanydropdown').val();
    var carmake = $('#e_carmakedropdown').val();
    var engineseries = $('#e_enginedropdown').val();
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            transactionid: transactionid,
            carcompany: carcompany,
            carmake: carmake,
            engineseries: engineseries
        },
        success: function (result) {
            handlesaveendorsement(result);
            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

function handlesaveendorsement(result) {
    if (result.Status == 'true') {
        if(confirm('The transaction has been successfully endorsed and changes have been made. Do you want to view the print out?'))
        {
            var id = $('#IdHiddenField').val();
            //Show print out in new window
            var url = 'EndorsementPrintOut.aspx?id=' + id;
            window.open(url, "_blank", 'toolbar=0,location=0,menubar=0');
        }
        //redirect here
        window.location.href = "TransactionView.aspx?id=" + result.NewID;
    } else {
        alert('endorsement failed!');
    }
}