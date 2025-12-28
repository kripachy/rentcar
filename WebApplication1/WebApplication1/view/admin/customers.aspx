<%@ Page Title="Управление клиентами" Language="C#" MasterPageFile="~/view/admin/adminmaster.master"
    AutoEventWireup="true" CodeBehind="customers.aspx.cs" Inherits="WebApplication1.view.admin.customers" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div class="py-4 admin-cars-page">
            <div class="row g-4 mt-3">
                <div class="col-lg-4">
                    <div class="admin-form-card admin-card">
                        <div class="admin-card-header">
                            <div>
                                <div class="admin-card-title">
                                    <i class="fas fa-users me-2"></i>Управление клиентами
                                </div>
                                <div class="admin-card-subtitle">Добавляйте и редактируйте данные клиентов</div>
                            </div>
                        </div>

                        <div class="admin-card-body">
                            <div class="mb-3">
                                <label class="form-label">Имя клиента</label>
                                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control"
                                    placeholder="Введите имя клиента" autocomplete="off" />
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Город клиента</label>
                                <asp:DropDownList ID="ddlCustomerCity" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Выберите город" Value="" />
                                    <asp:ListItem Text="Минск" Value="Минск" />
                                    <asp:ListItem Text="Брест" Value="Брест" />
                                    <asp:ListItem Text="Витебск" Value="Витебск" />
                                    <asp:ListItem Text="Гомель" Value="Гомель" />
                                    <asp:ListItem Text="Гродно" Value="Гродно" />
                                    <asp:ListItem Text="Могилев" Value="Могилев" />
                                    <asp:ListItem Text="Барановичи" Value="Барановичи" />
                                    <asp:ListItem Text="Борисов" Value="Борисов" />
                                    <asp:ListItem Text="Пинск" Value="Пинск" />
                                    <asp:ListItem Text="Орша" Value="Орша" />
                                    <asp:ListItem Text="Мозырь" Value="Мозырь" />
                                    <asp:ListItem Text="Солигорск" Value="Солигорск" />
                                    <asp:ListItem Text="Новополоцк" Value="Новополоцк" />
                                    <asp:ListItem Text="Лида" Value="Лида" />
                                    <asp:ListItem Text="Молодечно" Value="Молодечно" />
                                    <asp:ListItem Text="Полоцк" Value="Полоцк" />
                                    <asp:ListItem Text="Жлобин" Value="Жлобин" />
                                    <asp:ListItem Text="Светлогорск" Value="Светлогорск" />
                                    <asp:ListItem Text="Речица" Value="Речица" />
                                    <asp:ListItem Text="Жодино" Value="Жодино" />
                                    <asp:ListItem Text="Слуцк" Value="Слуцк" />
                                    <asp:ListItem Text="Кобрин" Value="Кобрин" />
                                    <asp:ListItem Text="Волковыск" Value="Волковыск" />
                                    <asp:ListItem Text="Калинковичи" Value="Калинковичи" />
                                    <asp:ListItem Text="Сморгонь" Value="Сморгонь" />
                                    <asp:ListItem Text="Рогачев" Value="Рогачев" />
                                    <asp:ListItem Text="Дзержинск" Value="Дзержинск" />
                                    <asp:ListItem Text="Новогрудок" Value="Новогрудок" />
                                    <asp:ListItem Text="Бобруйск" Value="Бобруйск" />
                                </asp:DropDownList>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Телефон клиента</label>
                                <asp:TextBox ID="txtCustomerPhone" runat="server" CssClass="form-control"
                                    placeholder="+375 (XX) XXX-XX-XX" autocomplete="off" />
                            </div>

                            <div class="d-grid gap-2">
                                <asp:Button ID="Edit" runat="server" Text="Редактировать клиента" 
                                    CssClass="btn btn-primary" OnClick="Edit_Click" />
                                
                                <asp:Button ID="Delete" runat="server" Text="Удалить клиента" 
                                    CssClass="btn btn-danger"
                                    OnClick="Delete_Click"
                                    OnClientClick="return confirm('Вы уверены, что хотите удалить этого клиента?');" />
                            </div>
                            
                            <asp:Label ID="ErrorMsg" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="col-lg-8">
                    <div class="admin-table-container admin-card">
                        <div class="admin-table-header admin-card-header">
                            <h5><i class="fas fa-list-ul me-2"></i>Клиенты</h5>
                        </div>

                        <div class="admin-card-body p-0">
                            <div class="table-responsive">
                                <asp:GridView ID="gvCustomers" runat="server" CssClass="admin-table table table-hover table-sm mb-0"
                                    AutoGenerateColumns="False" OnSelectedIndexChanged="gvCustomers_SelectedIndexChanged"
                                    DataKeyNames="CustId" GridLines="None"
                                    HeaderStyle-CssClass="admin-table-head"
                                    RowStyle-CssClass="admin-table-row"
                                    AlternatingRowStyle-CssClass="admin-table-row alt">
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" ButtonType="Button"
                                            ControlStyle-CssClass="btn btn-sm btn-outline-secondary" SelectText="Выбрать" />
                                        <asp:BoundField DataField="CustId" HeaderText="ID" />
                                        <asp:BoundField DataField="CustName" HeaderText="Имя клиента" />
                                        <asp:BoundField DataField="CustAdd" HeaderText="Город" />
                                        <asp:BoundField DataField="CustPhone" HeaderText="Телефон" />
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div class="admin-table-footer">
                                <asp:Button ID="btnExport" runat="server" Text="Экспорт в Excel" 
                                    CssClass="btn btn-outline-secondary" OnClick="btnExport_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Content>