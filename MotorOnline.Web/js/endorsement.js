$(document).ready(initialize);

function initialize() {
    $('#endsavebutton').click(
        function () {
            var selected = $('#EndorsementDropdown').val();
            var transactionid = $('#IdHiddenField').val();
            switch (selected) {
                case '3':
                    updatecocno(selected, transactionid);
                    break;
                default:
                    break;
            }
        }
    );
}


function updatecocno(type, transactionid) {
    var newCocNo = $('#e_cocno').val();
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: {
            action: "updateendorsement",
            type: type,
            newcocno: newCocNo,
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

function handlesaveendorsement(result) {
    if (result == 'true') {
        alert('updated');
        //window.location.href = ""
    } else {
        alert('endorsement failed!');
    }
}