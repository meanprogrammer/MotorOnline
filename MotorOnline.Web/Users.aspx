<%@ Page Title="" Language="C#" MasterPageFile="~/Root.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="MotorOnline.Web.Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/users.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="users-container"></div>
    <input id="adduserbutton" type="button" value="Add User" class="btn btn-primary" />

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
        <table class="table table-bordered" cellpadding="4">
            <tr>
                <td>Username</td>
                <td><input type="text" id="usernametext" /></td>
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
        </table>
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
