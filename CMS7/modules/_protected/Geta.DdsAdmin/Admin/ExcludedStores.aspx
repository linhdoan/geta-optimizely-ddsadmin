<%@ Page AutoEventWireup="true" CodeBehind="ExcludedStores.aspx.cs" Inherits="Geta.DdsAdmin.Admin.ExcludedStores" Language="C#" EnableViewState="true" %>

<!DOCTYPE html>
<html>
    <head runat="server">
        <meta http-equiv="Content-type" content="text/html; charset=UTF-8" />
        <title>Excluded stores</title>
        <link rel="stylesheet" type="text/css" href="/cms/Shell/ClientResources/ShellCore.css" />
        <link rel="stylesheet" type="text/css" href="/cms/Shell/ClientResources/ShellCoreLightTheme.css" />
        <link href="../../../App_Themes/Default/Styles/system.css" type="text/css" rel="stylesheet" />
    </head>
    <body>
        <div class="epi-contentContainer epi-padding-large">
            <form runat="server">
                <h3>Excluded Stores List</h3>
                <p>
                    Add store name(namespace) part to be excluded in left menu(Filtering uses Contains).
                </p>
                <p>
                    <asp:Label runat="server" AssociatedControlID="item">Add filter:</asp:Label>
                    <asp:TextBox runat="server" ID="item"></asp:TextBox>
                    <span class="epi-cmsButton">
                        <asp:Button runat="server" ID="add" Text="Add" OnClick="AddClick" CssClass="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Add"/>
                    </span>
                </p>
                <p>Your filters:</p>
                <p>
                    <asp:ListBox runat="server" ID="list" DataValueField="Id" DataTextField="Filter" Width="400px" Height="200px"></asp:ListBox>
                    <br />
                    <span class="epi-cmsButton">
                        <asp:Button runat="server" ID="remove" Text="Remove selected filter" OnClick="RemoveClick" CssClass="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Delete"/>
                    </span>
                </p>
            </form>
        </div>
    </body>
</html>