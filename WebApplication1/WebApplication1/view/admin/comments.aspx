<%@ Page Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="comments.aspx.cs" Inherits="WebApplication1.view.admin.comments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">
        <div class="row">
            <div class="col-12">
                <div class="card shadow border-0">
                    <div class="card-header bg-dark text-white d-flex justify-content-between align-items-center">
                        <span>Управление отзывами</span>
                        <span class="badge bg-info text-dark">Админ</span>
                    </div>
                    <div class="card-body">
                        <asp:Label ID="lblStatus" runat="server" Visible="false" CssClass="d-block mb-3"></asp:Label>
                        <asp:GridView ID="gvComments" runat="server" CssClass="table table-striped table-hover"
                            AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="gvComments_RowCommand"
                            OnRowDeleting="gvComments_RowDeleting"
                            EmptyDataText="Отзывов пока нет" AllowPaging="true" PageSize="15"
                            OnPageIndexChanging="gvComments_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True" ItemStyle-Width="60px" />
                                <asp:BoundField DataField="UserName" HeaderText="Пользователь" />
                                <asp:TemplateField HeaderText="Оценка">
                                    <ItemTemplate>
                                        <%# RenderStars(Eval("Rating")) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CommentText" HeaderText="Комментарий" />
                                <asp:BoundField DataField="CreatedAt" HeaderText="Создан" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                                <asp:TemplateField HeaderText="Статус">
                                    <ItemTemplate>
                                        <span class='badge <%# Convert.ToBoolean(Eval("IsHidden")) ? "bg-warning text-dark" : "bg-success" %>'>
                                            <%# Convert.ToBoolean(Eval("IsHidden")) ? "Скрыт" : "Виден" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField Text="Скрыть/Показать" CommandName="togglehide" ControlStyle-CssClass="btn btn-sm btn-outline-secondary" />
                                <asp:ButtonField Text="Удалить" CommandName="delete" ControlStyle-CssClass="btn btn-sm btn-outline-danger" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
