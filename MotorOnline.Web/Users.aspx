<%@ Page Title="" Language="C#" MasterPageFile="~/Root.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="MotorOnline.Web.Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/bootstrapValidator.min.css" rel="stylesheet" type="text/css" />
    <script src="js/bootstrapValidator.js" type="text/javascript"></script>
    <script src="js/users.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="user-content" style="width:90%;margin-left:10px;">
    <% if (this.CurrentUser.UserRole.CanAddUser)
       {%>
        <div class="row" style="margin:10px 0px 10px -15px;">
            <div class="col-md-12">
                    <input id="adduserbutton" type="button" value="Add User" class="btn btn-primary" />
            </div>
        </div>
    <% } %>
    <div id="users-container"></div>
</div>

<div class="modal fade" id="userrole-details">
  <div class="modal-dialog width-800">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <h4 class="modal-title">User Role Details</h4>
      </div>
      <div class="modal-body">
        <table class="table table-bordered" cellpadding="4">
            <tr>
                <td>Role Name</td>
                <td colspan="4"><span id="rolenamelabel"></span></td>
            </tr>
            <tr>
                <td><%--<input type="checkbox" id="canaddtransaction" />--%><img id="canaddtransaction" alt="" /> Add Transaction</td>
                <td><%--<input type="checkbox" id="canedittransaction" />--%><img id="canedittransaction" alt="" /> Edit Transaction </td>
                <td><%--<input type="checkbox" id="canviewtransaction" />--%><img id="canviewtransaction" alt="" /> View Transaction</td>
                <td><%--<input type="checkbox" id="candeletetransaction" />--%><img id="candeletetransaction" alt="" /> Delete Transaction</td>
            </tr>
            <tr>
                <td><%--<input type="checkbox" id="canposttransaction" />--%><img id="canposttransaction" alt="" /> Post Transaction</td>
                <td><%--<input type="checkbox" id="canendorsetransaction" />--%><img id="canendorsetransaction" alt="" /> Endorse Transaction</td>
                <td><%--<input type="checkbox" id="canadduser" />--%><img id="canadduser" alt="" /> Add User</td>
                <td><%--<input type="checkbox" id="canedituser" />--%><img id="canedituser" alt="" /> Edit User</td>
            </tr>
            <tr>
                <td><%--<input type="checkbox" id="candeleteuser" />--%><img id="candeleteuser" alt="" /> Delete User</td>
                <td colspan="3"><%--<input type="checkbox" id="caneditperils" />--%><img id="caneditperils" alt="" /> Edit Default Perils</td>
            </tr>
        </table>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div>

<div class="modal fade" id="user-modal">
  <div class="modal-dialog width-800">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <h4 class="modal-title">Add User</h4>
      </div>
      <div class="modal-body">
            <form id="userform" class="form-inline" method="post" novalidate="novalidate">
                <div class="row">
                    <div class="form-group has-feedback col-md-12">
                        <label class="control-label" for="usernametext">Username</label>
                        <input type="text" id="usernametext" name="username" class="form-control" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group has-feedback col-md-6">
                        <label class="control-label" for="passwordtext">Password</label>
                        <input type="password" id="passwordtext" name="password"  class="form-control" />
                    </div>
                    <div class="form-group has-feedback col-md-6">
                        <label class="control-label" for="retypepassword">Retype Password</label>
                        <input type="password" id="retypepasswordtext" name="retypepassword" class="form-control" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group has-feedback col-md-4">
                        <label class="control-label" for="lastnametext">Lastname</label>
                        <input type="text" id="lastnametext" name="lastname" class="form-control"  />
                    </div>
                    <div class="form-group has-feedback col-md-4">
                        <label class="control-label" for="firstnametext">FirstName</label>
                        <input type="text" id="firstnametext" name="firstname" class="form-control"  />
                    </div>
                    <div class="form-group has-feedback col-md-4">
                        <label class="control-label">MI</label>
                        <input type="text" id="middlenametext" name="middlename" class="form-control"  />
                    </div>
                </div>
                <div class="row" style="margin-top:15px;">
                    <div class="form-group has-feedback col-md-12">
                        <label class="control-label" for="roledropdown">Role</label>
                        <select id="roledropdown" style="width:300px;" name="roledropdown" class="form-control" ></select>
                    </div>
                </div>
                <%--<table class="table table-bordered" cellpadding="4">
                    <tr>
                        <td>Username</td>
                        <td><div class="form-group has-feedback"><input type="text" id="usernametext" name="username" class="form-control" /></div></td>
                        <td>Password</td>
                        <td><input type="password" id="passwordtext" /></td>
                        <td>Retype Password</td>
                        <td><input type="password" id="retypepasswordtext" /></td>
                    </tr>
                    <tr>
                        <td>Lastname</td>
                        <td><input type="text" id="lastnametext" /></td>
                        <td>FirstName</td>
                        <td><input type="text" id="firstnametext" /></td>
                        <td>MI</td>
                        <td><input type="text" id="middlenametext" /></td>
                    </tr>
                    <tr>
                        <td>Role</td>
                        <td colspan="5"><select id="roledropdown"></select></td>
                    </tr>
                </table>--%>
            </form>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary" id="saveuserbutton">Save</button>
      </div>
    </div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->
<input type="hidden" id="currentuserid" />
</asp:Content>
