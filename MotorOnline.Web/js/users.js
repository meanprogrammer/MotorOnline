$(document).ready(function () {
    loadusers();
    $('#adduserbutton').click(function () {
        $('#user-modal').modal({ keyboard: false });
    });
    $('#saveuserbutton').click(function () {
        saveuser();
    });
});

function clearusermodal() {
    $('#usernametext').val('');
    $('#passwordtext').val('');
    $('#retypepasswordtext').val('');
    $('#lastnametext').val('');
    $('#firstnametext').val('');
    $('#middlenametext').val('');
    $('#roledropdown').val(0);
}

function showuserroledetails(id) {
    var data = $('#userhidden' + id).val();
    var user = JSON.parse(data);
    $('#canaddtransaction').prop('checked', user.UserRole.CanAddTransaction);
    $('#canedittransaction').prop('checked', user.UserRole.CanEditTransaction);
    $('#canviewtransaction').prop('checked', user.UserRole.CanViewTransaction);
    $('#candeletetransaction').prop('checked', user.UserRole.CanDeleteTransaction);
    $('#canposttransaction').prop('checked', user.UserRole.CanPostTransaction);
    $('#canendorsetransaction').prop('checked', user.UserRole.CanEndorse);
    $('#canadduser').prop('checked', user.UserRole.CanAddUser);
    $('#canedituser').prop('checked', user.UserRole.CanEditUser);
    $('#candeleteuser').prop('checked', user.UserRole.CanDeleteUser);
    $('#caneditperils').prop('checked', user.UserRole.CanEditPerils);

    $('#rolenamelabel').html(user.UserRole.RoleName);
    $('#userrole-details').modal({keyboard:false});
}

function saveuser() {
   var username = $('#usernametext').val();
   var password = $('#retypepasswordtext').val();
   var lastname = $('#lastnametext').val();
   var firstname = $('#firstnametext').val();
   var middlename = $('#middlenametext').val();
   var role = $('#roledropdown').val();
   $.ajax({
       url: "ajax/TransactionAjax.aspx",
       type: "post",
       data: {
           "action": 'saveuser',
           "username": username,
           "password": password,
           "lastname": lastname,
           "firstname": firstname,
           "middlename": middlename,
           "role": role
       },
       success: function (result) {
           var obj = JSON.parse(result);
           if (obj.Result == 'true') {
               clearusermodal();
               $('#user-modal').modal('hide');
               loadusers();
           }
       },
       error: function () {
       }
   });
}

function loadusers() {
    $.ajax({
        url: "ajax/TransactionAjax.aspx",
        type: "post",
        data: { "action": 'gelallusers' },
        success: function (result) {
            var obj = JSON.parse(result);
            var html = '<table class="table table-bordered"><tr>';
            html += '<th width="50"></th>';
            html += '<th>Username</th>';
            html += '<th>Password</th>';
            html += '<th>LastName</th>';
            html += '<th>FirstName</th>';
            html += '<th>MI</th>';
            html += '<th>Role</th>';
            html += '<th>Last Activity Date</th></tr>';
            if (obj != null) {
                $.each(obj.Users, function (key, value) {
                    html += '<tr>';
                    html += '<td><a href="#" class="btn btn-primary btn-xs">Edit</td>';
                    html += '<td>' + value.Username + '</td>';
                    html += '<td>********</td>';
                    html += '<td>' + value.LastName + '</td>';
                    html += '<td>' + value.FirstName + '</td>';
                    html += '<td>' + value.MI + '</td>';
                    html += '<td>' + value.UserRole.RoleName + '&nbsp;&nbsp;<a onclick="showuserroledetails('+ value.UserID +');" class="btn btn-primary btn-xs">View Details</a></td>';
                    html += '<td>' + value.FormattedLastActivityDate + '</td>';
                    html += "<input type='hidden' id='userhidden" + value.UserID + "' value='" + JSON.stringify(value) + "' />";
                    html += '</tr>';
                });

                var options = '<option value="0">-- SELECT ROLE --</option>';
                $.each(obj.Roles, function (k, v) {
                    options += '<option value="' + v.Value + '">' + v.Text + '</option>';
                });
                $('#roledropdown').html(options);
            }
            html += '</table>';
            $('#users-container').html(html);
        },
        error: function () {
        }
    });
}

