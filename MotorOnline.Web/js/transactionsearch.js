$(document).ready(initialize);


function initialize() {
    $('#datecreatedfrom,#datecreated,#policyperiodfrom,#policyperiodto').datepicker({
        showOn: "button",
        buttonImage: "images/Calendar-icon.png",
        buttonImageOnly: true
    });

    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: { action: "loadsearchfilters" },
        success: function (result) {
            var obj = JSON.parse(result);
            var creditingBranch = obj["creditingbranches"];
            buildfilter('creditingbranch', creditingBranch);
            var sublines = obj["sublines"];
            buildfilter('subline', sublines);
            var mortgagee = obj["mortgagee"];
            buildfilter('mortgagee', mortgagee);
            var intermediary = obj["intermediary"];
            buildfilter('intermediary', intermediary);
            var typeofcovers = obj["typeofcovers"];
            buildfilter('typeofcover', typeofcovers);
            var carcompany = obj["carcompanies"];
            buildfilter('carcompany', carcompany);
        },
        error: function () {

        }
    });

    $('#searchbutton').click(searchtransactions);
    $('#resetbutton').click(resetsearch);
}

function buildfilter(element, list) {
    var html = '<option value="0">-- SELECT --</option>';
    $.each(list, function (key, value) {
        html += '<option value="' + value.Value + '">' + value.Text + '</option>';
    });
    $('#' + element).html(html);
}

function searchtransactions() {
    var creditingbranch = $('#creditingbranch').val();
    var parno = $('#parno').val();
    var policyno = $('#policyno').val();
    var subline = $('#subline').val();
    var datecreated = $('#datecreated').val();
    var policyperiodfrom = $('#policyperiodfrom').val();
    var policyperiodto = $('#policyperiodto').val();
    var typeofcover = $('#typeofcover').val();
    var mortgagee = $('#mortgagee').val();
    var intermediary = $('#intermediary').val();
    var carcompany = $('#carcompany').val();
    var motortype = $('#motortype').val();
    var chassisno = $('#chassisno').val();
    var engineno = $('#engineno').val();


    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "searchtransactions",
            creditingbranch: creditingbranch,
            parno: parno,
            policyno: policyno,
            subline: subline,
            datecreated: datecreated,
            policyperiodfrom: policyperiodfrom,
            policyperiodto: policyperiodto,
            typeofcover: typeofcover,
            mortgagee: mortgagee,
            intermediary: intermediary,
            carcompany: carcompany,
            motortype: motortype,
            chassisno: chassisno,
            engineno: engineno
        },
        success: function (result) {
            var obj = JSON.parse(result);
            html = '<table border="1" cellpadding="8"><tr>';
            html += '<th></th>';
            html += '<th>Crediting Branch</th>';
            html += '<th>Par No</th>';
            html += '<th>Policy No</th>';
            html += '<th>Subline</th>';
            html += '<th>Date Created</th>';
            html += '<th>Policy Period From</th>';
            html += '<th>Policy Period To</th>';
//            html += '<th>LastName</th>';
//            html += '<th>FirstName</th>';
//            html += '<th>MiddleName</th>';
            html += '<th>Type Of Cover</th>';
            html += '<th>Car Company</th>';
            html += '<th>Motor Type</th>';
            html += '<th>Chassis No</th>';
            html += '<th>Engine No</th>';
            html += '<th>Plate No</th>';
            html += '</tr>';
            if (obj != null) {
                $.each(obj, function (key, value) {
                    html += '<tr>';
                    html += '<td><a href="TransactionView.aspx?id=' + value.TransactionID + '">Edit</a></td>';
                    html += '<td>' + value.CreditingBranch + '</td>';
                    html += '<td>' + value.ParNo + '</td>';
                    html += '<td>' + value.PolicyNo + '</td>';
                    html += '<td>' + value.Subline + '</td>';
                    html += '<td>' + value.DateCreatedText + '</td>';
                    html += '<td>' + value.PolicyPeriodFromText + '</td>';
                    html += '<td>' + value.PolicyPeriodToText + '</td>';
//                    html += '<td>' + value.LastName + '</td>';
//                    html += '<td>' + value.FirstName + '</td>';
//                    html += '<td>' + value.MiddleName + '</td>';
                    html += '<td>' + value.TypeOfCover + '</td>';
                    html += '<td>' + value.CarCompany + '</td>';
                    html += '<td>' + value.MotorType + '</td>';
                    html += '<td>' + value.ChassisNo + '</td>';
                    html += '<td>' + value.EngineNo + '</td>';
                    html += '<td>' + value.PlateNo + '</td>';
                    html += '</tr>';
                });
            }


            $('#searchresult').html(html);
        },
        error: function () {

        }
    });
}

function resetsearch() {
    $('#creditingbranch').val('0');
    $('#parno').val('');
    $('#policyno').val('');
    $('#subline').val('0');
    $('#datecreated').val('');
    $('#policyperiodfrom').val('');
    $('#policyperiodto').val('');
    $('#typeofcover').val('0');
    $('#mortgagee').val('0');
    $('#intermediary').val('0');
    $('#carcompany').val('0');
    $('#motortype').val('0');
    $('#chassisno').val('');
    $('#engineno').val('');

    $('#searchresult').html('');
}