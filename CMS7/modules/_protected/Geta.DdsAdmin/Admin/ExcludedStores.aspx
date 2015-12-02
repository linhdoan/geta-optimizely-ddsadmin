<%@ Page AutoEventWireup="true" CodeBehind="ExcludedStores.aspx.cs" Inherits="Geta.DdsAdmin.Admin.ExcludedStores" Language="C#" EnableViewState="true" %>

<asp:content runat="server" contentplaceholderid="FullRegion">
    <div style="margin-left: 20px; margin-top: 20px">
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
            <br/>
            <span class="epi-cmsButton">
                <asp:Button runat="server" ID="remove" Text="Remove selected filter" OnClick="RemoveClick" CssClass="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Delete"/>
            </span>
        </p>
    </div>
</asp:content>