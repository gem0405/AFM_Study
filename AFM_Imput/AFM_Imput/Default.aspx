<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AFM_Imput._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <asp:Label ID="Label1" runat="server" Text="資料匯入結果<br/>"></asp:Label><br />
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button ID="Button1" runat="server" Text="資料匯入" OnClick="Button1_Click" />
        <br />
        
            </div>
    <div class="jumbotron">
        <asp:Label ID="Label2" runat="server" Text="圖片匯入結果<br/>"></asp:Label>
        <br />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="圖片匯入" />
        <br />
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="圖片匯入(單一)" />
       
    </div>
    <div class="jumbotron">
         <asp:Label ID="Label3" runat="server" Text="檔案加密結果<br/>"></asp:Label>
        <br />
        <asp:Button ID="Button4" runat="server" Text="檔案加密" OnClick="Button4_Click" />
    </div>
</asp:Content>
