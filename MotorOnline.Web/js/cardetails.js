function createcardetails() {
    var cardetails = {
        



        "Year": $('#YearDropdown').val(),
        "YearText": $('#YearDropdown option:selected').text(),
        "CarYear": $('#YearDropdown').val(),

        "CarCompanyText": $('#CarCompaniesDropdown option:selected').text(),
        "CarCompanyValue": $('#CarCompaniesDropdown').val(),
        "CarCompany": $('#CarCompaniesDropdown').val(),

        "CarMakeText": $('#CarMakeDropdown option:selected').text(),
        "CarMakeValue": $('#CarMakeDropdown').val(),
        "CarMake": $('#CarMakeDropdown').val(),

        "TypeOfBody": $('#TypeOfBodyDropdown').val(),

        "TypeOfCoverText": $('#TypeOfCoverDropdown option:selected').text(),
        "TypeOfCoverValue": $('#TypeOfCoverDropdown').val(),
        "TypeOfCover": $('#TypeOfCoverDropdown').val(),

        "EngineSeriesText": $('#EngineDropdown option:selected').text(),
        "EngineSeriesValue": $('#EngineDropdown').val(),
        "EngineSeries": $('#EngineDropdown').val(),

        "MotorType": $('#MotorTypeDropdown').val(),
        "Engine": $('#txtEngine').val(),
        "Color": $('#txtColor').val(),
        "ConductionNo": $('#txtConductionNo').val(),
        "ChassisNo": $('#txtChasis').val(),
        "PlateNo": $('#txtPlateNo').val(),
        "Accessories": $('#txtAccesories').val(),
        "AuthenticationNo": $('#txtAuthenticationNo').val(),
        "COCNo": $('#txtCOCNo').val()
    };
    return cardetails;
}


function populatecardetaildisplay(cardetail) {
    //alert(cardetail.TypeOfCoverText);
    $('#lblSublineType').html($('#SublineDropdown option:selected').text());
    $('#lblTypeOfCover').html(cardetail.TypeOfCoverText);
    $('#lblYear').html(cardetail.YearText);
    $('#lblCarCompany').html(cardetail.CarCompanyText);
    $('#lblMake').html(cardetail.CarMakeText);
    $('#lblMotorType').html(cardetail.MotorType);
    $('#lblEngineNo').html(cardetail.Engine);
    $('#lblChasisNo').html(cardetail.ChassisNo);

    $('#lblColor').html(cardetail.Color);
    $('#lblPlateNo').html(cardetail.PlateNo);
    $('#lblConductionNo').html(cardetail.ConductionNo);
    $('#lblAccessories').html(cardetail.Accessories);
    $('#lblAuthenticationNo').html(cardetail.AuthenticationNo);
    $('#lblCOCNo').html(cardetail.COCNo);

}

function loadtypesofbody() {
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: { "action": 'getbodytypes' },
        success: function (result) {
            var obj = JSON.parse(result);
            if (obj != null) {
                var html = '<option value="0">-- SELECT --</option>';
                $.each(obj, function (key, value) {
                    html += '<option value="' + value.Value + '">' + value.Text + '</option>';
                });
                $('#TypeOfBodyDropdown').html(html);
            }

        },
        error: function () {
        }
    });
}

function loadcarcompanies() {
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: { "action": 'getcarcompanies' },
        success: function (result) {
            var obj = JSON.parse(result);
            if (obj != null) {
                var html = '<option value="0">-- SELECT --</option>';
                $.each(obj, function (key, value) {
                    html += '<option value="' + value.Value + '">' + value.Text + '</option>';
                });
                $('#CarCompaniesDropdown').html(html);
            }

        },
        error: function () {
        }
    });
}

function builddropdownoptions(elementid, values) {
    var html = '<option value="0">-- SELECT --</option>';
    $.each(values, function (key, value) {
        html += '<option value="' + value.Value + '">' + value.Text + '</option>';
    });
    $('#' + elementid).html(html);
}

//function loadcardetailsoptions() {
//    $.ajax({
//        url: "ajax/TransactionAjax.aspx",
//        type: "post",
//        data: { "action": 'getcardetailsoptions' },
//        success: function (result) {
//            var obj = JSON.parse(result);
//            if (obj != null) {

//                var caryears = obj["caryears"];
//                var carcompanies = obj["carcompanies"];
//                var typesofbody = obj["typesofbody"];
//                var typesofcover = obj["typesofcover"];
//                builddropdownoptions('CarCompaniesDropdown', carcompanies);
//                builddropdownoptions('TypeOfBodyDropdown', typesofbody);
//                builddropdownoptions('TypeOfCoverDropdown', typesofcover);
//                builddropdownoptions('YearDropdown', caryears);
//                togglecardetailscontrols(false);
//            }

//        },
//        error: function () {
//        }
//    });
//}