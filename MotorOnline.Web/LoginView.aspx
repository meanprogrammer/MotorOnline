<%@ Page Title="" Language="C#" MasterPageFile="~/Empty.Master" AutoEventWireup="true"
    CodeBehind="LoginView.aspx.cs" Inherits="MotorOnline.Web.LoginView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="js/login.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<%--    <div id="login">
        <h3>Login</h3>--%>
        <br />
        <div class="panel panel-primary" style="width: 450px;margin:0 auto;">
          <div class="panel-heading">
            <h3 class="panel-title">Login</h3>
          </div>
          <div class="panel-body">
                 <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
        <%--<div class="login-form">--%>
            <table cellpadding="8" align="center" class="login-table">
            <tr>
                <td colspan="2">
                    <asp:Label ID="UserNameLabel" class="login-label" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="UserName" class="form-control input-lg not-block" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" class="not-block"
                        ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required."
                        ValidationGroup="MotorOnlineLogin">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="PasswordLabel" class="login-label" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="Password" class="form-control input-lg" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="MotorOnlineLogin">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <%--            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                </td>
            </tr>--%>
            <tr>
                <td align="right" colspan="2">
                    <asp:Button ID="LoginButton" runat="server" class="btn btn-primary btn-lg width-100"
                        CommandName="Login" Text="Log In" ValidationGroup="MotorOnlineLogin" ClientIDMode="Static"
                        Font-Size="Medium" OnClick="LoginButton_Click" />
                </td>
            </tr>
        </table>
          </div>
</div>
  <%--      </div>
    </div>--%>
</asp:Content>
