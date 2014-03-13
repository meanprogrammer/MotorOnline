var validatoroptions = {
    message: 'This value is not valid',
    live: 'submitted',
    submitHandler: function(validator, form, submitButton) {
        saveuser();
    },
    //        feedbackIcons: {
    //            valid: 'glyphicon glyphicon-ok',
    //            invalid: 'glyphicon glyphicon-remove',
    //            validating: 'glyphicon glyphicon-refresh'
    //        },
    fields: {
        username: {
            message: 'The username is not valid',
            validators: {
                notEmpty: {
                    message: 'The username is required.'
                }
            }
        },
        password: {
            message: 'The password is not valid',
            validators: {
                notEmpty: {
                    message: 'The password is required.'
                }
            }
        },
        retypepassword: {
            message: 'The re-type password is not valid',
            validators: {
                notEmpty: {
                    message: 'The re-type password is required.'
                },
                callback: {
                    message: 'Password does not match.',
                    callback: function (fieldValue, validator) {
                        var password = $('#passwordtext').val();
                        return password == fieldValue;
                    }
                }
            }
        },
        lastname: {
            message: 'The lastname is not valid',
            validators: {
                notEmpty: {
                    message: 'The lastname is required.'
                }
            }
        },
        firstname: {
            message: 'The firstname is not valid',
            validators: {
                notEmpty: {
                    message: 'The firstname is required.'
                }
            }
        },
        roledropdown: {
            message: 'Select a role.',
            validators: {
                callback: {
                    message: 'Select a role.',
                    callback: function(fieldValue, validator) {
                        return $('#roledropdown').val() > 0;
                    }
                }
            }
        },
    }
};

$(document).ready(function () {
    loadusers();
    $('#adduserbutton').click(function () {
        clearusermodal();
        $('#user-modal').modal({ keyboard: false });
    });
    //    $('#saveuserbutton').click(function () {
    //        saveuser();
    //    });

    //validator = $('#userform').bootstrapValidator();

    $('#saveuserbutton').click(function () {
        //validator.bootstrapValidator('resetForm');
        if($('#saveuserbutton').val() == 'Update') {
            $('#passwordtext').prop('disabled', false);
            $('#retypepasswordtext').prop('disabled', false);
        }

        $('#userform').bootstrapValidator(validatoroptions).bootstrapValidator('validate');
        $('#passwordtext').prop('disabled', true);
        $('#retypepasswordtext').prop('disabled', true);
    });
});

function clearusermodal() {
    $('#usernametext').val('');
    $('#passwordtext').val('');
    $('#retypepasswordtext').val('');
    $('#passwordtext').prop('disabled', false);
    $('#retypepasswordtext').prop('disabled', false);
    $('#lastnametext').val('');
    $('#firstnametext').val('');
    $('#middlenametext').val('');
    $('#roledropdown').val(0);
    $('#saveuserbutton').val('Save');
    $('#saveuserbutton').html('Save');
}

function showuserroledetails(id) {
    var data = $('#userhidden' + id).val();
    var user = JSON.parse(data);
    $('#canaddtransaction').prop('src', showicon(user.UserRole.CanAddTransaction));
    $('#canedittransaction').prop('src', showicon(user.UserRole.CanEditTransaction));
    $('#canviewtransaction').prop('src', showicon(user.UserRole.CanViewTransaction));
    $('#candeletetransaction').prop('src', showicon(user.UserRole.CanDeleteTransaction));
    $('#canposttransaction').prop('src', showicon(user.UserRole.CanPostTransaction));
    $('#canendorsetransaction').prop('src', showicon(user.UserRole.CanEndorse));
    $('#canadduser').prop('src', showicon(user.UserRole.CanAddUser));
    $('#canedituser').prop('src', showicon(user.UserRole.CanEditUser));
    $('#candeleteuser').prop('src', showicon(user.UserRole.CanDeleteUser));
    $('#caneditperils').prop('src', showicon(user.UserRole.CanEditPerils));

    $('#rolenamelabel').html(user.UserRole.RoleName);
    $('#userrole-details').modal({keyboard:false});
}

function showicon(checked) {
    var src = '';
    if (checked) {
        src = 'images/tick-icon.png';
    } else {
        src = 'images/delete-icon.png';
    }
    return src;
}

function saveuser() {
   var username = $('#usernametext').val();
   var password = $('#retypepasswordtext').val();
   var lastname = $('#lastnametext').val();
   var firstname = $('#firstnametext').val();
   var middlename = $('#middlenametext').val();
   var role = $('#roledropdown').val();
   var userid = $('#currentuserid').val();
   var action = ($('#saveuserbutton').val() == 'Save' ? 'saveuser' : 'updateuser');
   $.ajax({
       url: "ajax/TransactionAjax.aspx",
       type: "post",
       data: {
           "action": action,
           "username": username,
           "password": password,
           "lastname": lastname,
           "firstname": firstname,
           "middlename": middlename,
           "role": role,
           "userid": userid
       },
       success: function (result) {
           var obj = JSON.parse(result);
           if (obj.Result == 'true') {
               clearusermodal();
               $('#userform').data('bootstrapValidator').resetForm(true);
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
            html += '<th width="50"></th>';
            html += '<th>Username</th>';
            html += '<th>Password</th>';
            html += '<th>LastName</th>';
            html += '<th>FirstName</th>';
            html += '<th>MI</th>';
            html += '<th>Role</th>';
            html += '<th>Last Activity Date</th></tr>';
            if (obj != null) {
                var currentuser = obj.CurrentUser;
                var canedit = currentuser.UserRole.CanEditUser == true ? '' : 'disabled';
                var candelete = currentuser.UserRole.CanDeleteUser == true ? '' : 'disabled';
                $.each(obj.Users, function (key, value) {
                    html += '<tr>';
                    html += '<td><a onclick="edituser(' + value.UserID + ')" class="btn btn-primary btn-xs" ' + canedit + '>Edit</td>';
                    html += '<td><a onclick="deleteuser(' + value.UserID + ')" class="btn btn-danger btn-xs" ' + candelete + '>Delete</td>';
                    html += '<td>' + value.Username + '</td>';
                    html += '<td>********</td>';
                    html += '<td>' + value.LastName + '</td>';
                    html += '<td>' + value.FirstName + '</td>';
                    html += '<td>' + value.MI + '</td>';
                    html += '<td>' + value.UserRole.RoleName + '&nbsp;&nbsp;<a onclick="showuserroledetails('+ value.UserID +');" class="btn btn-info btn-xs">View Details</a></td>';
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

function edituser(id) {
    var data = $('#userhidden' + id).val();

    //Visually disable passwords
    $('#passwordtext').val('********');
    $('#retypepasswordtext').val('********');
    $('#passwordtext').prop('disabled', true);
    $('#retypepasswordtext').prop('disabled', true);

    var user = JSON.parse(data);
    $('#usernametext').val(user.Username);
    $('#lastnametext').val(user.LastName);
    $('#firstnametext').val(user.FirstName);
    $('#middlenametext').val(user.MI);
    $('#roledropdown').val(user.RoleID);
    $('#currentuserid').val(user.UserID);
    $('#saveuserbutton').val('Update');
    $('#saveuserbutton').html('Update');
    $('#user-modal').modal({ keyboard: false });
}

function deleteuser(id) {
    if(confirm('Are you sure to delete the user?')) {
        $.ajax({
            url: "ajax/TransactionAjax.aspx",
            type: "post",
            data: {
                "action": "deleteuser",
                "id": id
            },
            success: function (result) {
                var obj = JSON.parse(result);
                if (obj.Result == 'true') {
                    clearusermodal();
                    loadusers();
                }
            },
            error: function () {
            }
        });
    }
}