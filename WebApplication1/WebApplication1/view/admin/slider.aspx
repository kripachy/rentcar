<%@ Page Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="slider.aspx.cs" Inherits="WebApplication1.view.admin.slider" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-4">
        <div class="row g-3">
            <div class="col-12">
                <div class="card shadow border-0">
                    <div class="card-header bg-dark text-white d-flex justify-content-between align-items-center">
                        <span>Слайдер главной страницы</span>
                        <span class="badge bg-info text-dark">Админ</span>
                    </div>
                    <div class="card-body">
                        <asp:Label ID="lblStatus" runat="server" Visible="false" CssClass="d-block mb-3"></asp:Label>

                        <div class="mb-4">
                            <div class="d-flex justify-content-between align-items-center mb-2">
                                <span class="fw-semibold">Текущие слайды на главной</span>
                                <asp:Label ID="lblCurrentSlidesHint" runat="server" CssClass="text-muted small" Visible="false"></asp:Label>
                            </div>
                            <div class="row g-3">
                                <asp:Repeater ID="repCurrentSlides" runat="server">
                                    <ItemTemplate>
                                        <div class="col-md-4">
                                            <div class="card h-100 border-0 shadow-sm">
                                                <img alt="slide" src='<%# Eval("ImageUrl") %>' style="width:100%;height:140px;object-fit:cover;border-top-left-radius:0.375rem;border-top-right-radius:0.375rem;" />
                                                <div class="card-body">
                                                    <div class="fw-semibold"><%# Eval("Title") %></div>
                                                    <div class="text-muted small"><%# Eval("Subtitle") %></div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>

                        <asp:HiddenField ID="hfSlideId" runat="server" />

                        <div class="row g-3">
                            <div class="col-lg-6">
                                <label class="form-label fw-semibold">Заголовок</label>
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" MaxLength="200" />
                            </div>
                            <div class="col-lg-6 d-none">
                                <label class="form-label fw-semibold">Порядок</label>
                                <asp:TextBox ID="txtSortOrder" runat="server" CssClass="form-control" Text="0" />
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-semibold">Подзаголовок</label>
                                <asp:TextBox ID="txtSubtitle" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" MaxLength="400" />
                            </div>
                            <div class="col-lg-6">
                                <label class="form-label fw-semibold">Изображение</label>
                                <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-lg-6 d-flex align-items-end">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkActive" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label class="form-check-label" for="chkActive">Активен</label>
                                </div>
                            </div>
                            <div class="col-12 d-flex gap-2">
                                <asp:Button ID="btnSave" runat="server" Text="Сохранить" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Очистить" CssClass="btn btn-outline-secondary" OnClick="btnClear_Click" CausesValidation="false" />
                            </div>
                        </div>

                        <hr class="my-4" />

                        <div class="mb-4">
                            <div class="d-flex justify-content-between align-items-center mb-2">
                                <span class="fw-semibold">Порядок слайдов</span>
                                <div class="d-flex gap-2">
                                    <asp:HiddenField ID="hfOrder" runat="server" />
                                    <asp:Button ID="btnSaveOrder" runat="server" Text="Сохранить порядок" CssClass="btn btn-outline-primary" OnClick="btnSaveOrder_Click" />
                                </div>
                            </div>
                            <ul id="slidesOrderList" class="list-group">
                                <asp:Repeater ID="repOrder" runat="server">
                                    <ItemTemplate>
                                        <li class="list-group-item d-flex align-items-center gap-3" draggable="true" data-id='<%# Eval("SlideId") %>'>
                                            <span class="text-muted" style="cursor:grab;">≡</span>
                                            <img alt="slide" src='<%# Eval("ImageUrl") %>' style="width:72px;height:42px;object-fit:cover;border-radius:8px;border:1px solid rgba(0,0,0,0.08);" />
                                            <div class="flex-grow-1">
                                                <div class="fw-semibold"><%# Eval("Title") %></div>
                                                <div class="text-muted small"><%# Convert.ToBoolean(Eval("IsActive")) ? "Активен" : "Не активен" %></div>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>

                        <asp:GridView ID="gvSlides" runat="server" CssClass="table table-striped table-hover"
                            AutoGenerateColumns="False" DataKeyNames="SlideId" OnRowCommand="gvSlides_RowCommand"
                            OnRowDeleting="gvSlides_RowDeleting" EmptyDataText="Слайдов пока нет">
                            <Columns>
                                <asp:BoundField DataField="SlideId" HeaderText="ID" ReadOnly="True" ItemStyle-Width="70px" />
                                <asp:TemplateField HeaderText="Изображение">
                                    <ItemTemplate>
                                        <img alt="slide" src='<%# Eval("ImageUrl") %>' style="width:90px;height:50px;object-fit:cover;border-radius:8px;border:1px solid rgba(0,0,0,0.08);" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Title" HeaderText="Заголовок" />
                                <asp:BoundField DataField="Subtitle" HeaderText="Подзаголовок" />
                                <asp:TemplateField HeaderText="Активен" ItemStyle-Width="110px">
                                    <ItemTemplate>
                                        <span class='badge <%# Convert.ToBoolean(Eval("IsActive")) ? "bg-success" : "bg-secondary" %>'>
                                            <%# Convert.ToBoolean(Eval("IsActive")) ? "Да" : "Нет" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:ButtonField Text="Редактировать" CommandName="editSlide" ControlStyle-CssClass="btn btn-sm btn-outline-secondary" />
                                <asp:ButtonField Text="Удалить" CommandName="delete" ControlStyle-CssClass="btn btn-sm btn-outline-danger" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        (function () {
            function buildOrderValue(list) {
                var ids = [];
                list.querySelectorAll('li[data-id]').forEach(function (li) {
                    ids.push(li.getAttribute('data-id'));
                });
                return ids.join(',');
            }

            document.addEventListener('DOMContentLoaded', function () {
                var list = document.getElementById('slidesOrderList');
                var hf = document.getElementById('<%= hfOrder.ClientID %>');
                if (!list || !hf) return;

                hf.value = buildOrderValue(list);

                var dragged = null;

                list.addEventListener('dragstart', function (e) {
                    var li = e.target && e.target.closest ? e.target.closest('li[data-id]') : null;
                    if (!li) return;
                    dragged = li;
                    e.dataTransfer.effectAllowed = 'move';
                    try { e.dataTransfer.setData('text/plain', li.getAttribute('data-id')); } catch (err) { }
                    li.classList.add('opacity-50');
                });

                list.addEventListener('dragend', function () {
                    if (dragged) dragged.classList.remove('opacity-50');
                    dragged = null;
                    hf.value = buildOrderValue(list);
                });

                list.addEventListener('dragover', function (e) {
                    if (!dragged) return;
                    e.preventDefault();
                    e.dataTransfer.dropEffect = 'move';

                    var target = e.target && e.target.closest ? e.target.closest('li[data-id]') : null;
                    if (!target || target === dragged) return;

                    var rect = target.getBoundingClientRect();
                    var next = (e.clientY - rect.top) > rect.height / 2;
                    list.insertBefore(dragged, next ? target.nextSibling : target);
                });
            });
        })();
    </script>
</asp:Content>
