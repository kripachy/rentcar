<%@ Page Title="Verifications" Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true"
    CodeBehind="ManageVerification.aspx.cs" Inherits="WebApplication1.view.admin.ManageVerification" %>
    <%@ Import Namespace="System.Data" %>
        <%@ Import Namespace="System.Data.SqlClient" %>
            <%@ Import Namespace="WebApplication1.Models" %>



                <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
                    <style>
                        .verification-container {
                            background: #fff;
                            padding: 30px;
                            border-radius: 12px;
                            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
                            margin-bottom: 50px;
                        }

                        .page-title {
                            font-weight: 700;
                            color: #333;
                            margin-bottom: 25px;
                            border-left: 5px solid #00bcd4;
                            padding-left: 15px;
                        }

                        .verification-table {
                            border: none;
                        }

                        .verification-table th {
                            background-color: #f8f9fa;
                            border-bottom: 2px solid #eee;
                            color: #555;
                            font-weight: 600;
                            text-transform: uppercase;
                            font-size: 0.85rem;
                            padding: 15px;
                        }

                        .verification-table td {
                            vertical-align: middle;
                            padding: 15px;
                            border-bottom: 1px solid #eee;
                        }

                        .category-list label {
                            margin-right: 15px;
                            margin-left: 5px;
                            font-size: 0.9rem;
                            cursor: pointer;
                        }

                        .category-list input[type="checkbox"] {
                            width: 16px;
                            height: 16px;
                            cursor: pointer;
                        }

                        .doc-preview {
                            border-radius: 8px;
                            transition: transform 0.2s;
                            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                        }

                        .doc-preview:hover {
                            transform: scale(1.1);
                        }
                    </style>
                    <div class="container mt-4 verification-container">
                        <h2 class="page-title">Проверка документов и назначение категорий</h2>
                        <asp:Label ID="lblMsg" runat="server" CssClass="d-block mb-3"></asp:Label>

                        <asp:GridView ID="gvLicenses" runat="server" CssClass="table verification-table"
                            AutoGenerateColumns="False" OnRowCommand="gvLicenses_RowCommand"
                            OnRowDataBound="gvLicenses_RowDataBound" DataKeyNames="LicenseId">
                            <Columns>
                                <asp:BoundField DataField="Username" HeaderText="Пользователь" />
                                <asp:TemplateField HeaderText="Фото">
                                    <ItemTemplate>
                                        <a href='<%# ResolveUrl("~/ImageHandler.ashx?id=" + Eval("FileId")) %>'
                                            target="_blank">
                                            <img src='<%# ResolveUrl("~/ImageHandler.ashx?id=" + Eval("FileId")) %>'
                                                class="doc-preview" style="max-height: 60px;" />
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Status" HeaderText="Статус" />
                                <asp:TemplateField HeaderText="Доступные классы">
                                    <ItemTemplate>
                                        <asp:CheckBoxList ID="cblCategories" runat="server" RepeatDirection="Horizontal"
                                            CssClass="category-list">
                                        </asp:CheckBoxList>
                                        <asp:HiddenField ID="hfCustomerId" runat="server"
                                            Value='<%# Eval("CustomerId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Действия">
                                    <ItemTemplate>
                                        <asp:Button ID="btnApprove" runat="server" Text="Одобрить и назначить"
                                            CommandName="Approve" CommandArgument='<%# Container.DataItemIndex %>'
                                            CssClass="btn btn-success btn-sm"
                                            Visible='<%# Eval("Status").ToString() == "Pending" %>' />
                                        <asp:Button ID="btnReject" runat="server" Text="Отклонить" CommandName="Reject"
                                            CommandArgument='<%# Eval("LicenseId") %>' CssClass="btn btn-danger btn-sm"
                                            Visible='<%# Eval("Status").ToString() == "Pending" %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Content>