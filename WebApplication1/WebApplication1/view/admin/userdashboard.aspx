<%@ Page Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true" CodeBehind="userdashboard.aspx.cs" Inherits="WebApplication1.view.admin.userdashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<section class="slider-container">
    <div class="container">
        <div id="dynamicSlider" class="carousel slide hero-slider" data-bs-ride="carousel">
            <div class="carousel-indicators">
                <asp:Literal ID="litSliderIndicators" runat="server" />
            </div>
            <div class="carousel-inner">
                <asp:Literal ID="litSliderItems" runat="server" />
            </div>
            <button class="carousel-control-prev" type="button" data-bs-target="#dynamicSlider" data-bs-slide="prev">
                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                <span class="visually-hidden">Previous</span>
            </button>
            <button class="carousel-control-next" type="button" data-bs-target="#dynamicSlider" data-bs-slide="next">
                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                <span class="visually-hidden">Next</span>
            </button>
        </div>
        <div class="site-title-container text-center py-4">
            <h1 class="site-title">WheelDeal</h1>
        </div>
    </div>
</section>

<section class="feature-section-light">
    <div class="container">
        <div class="row text-center">
            <div class="col-lg-4">
                <div class="feature-card-light">
                    <div class="feature-icon-light"><i class="fas fa-car-side"></i></div>
                    <h3 class="feature-title-light">Эксклюзивные автомобили</h3>
                    <p>Доступ к самым престижным моделям от ведущих мировых производителей.</p>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="feature-card-light">
                    <div class="feature-icon-light"><i class="fas fa-shield-alt"></i></div>
                    <h3 class="feature-title-light">Полная страховка</h3>
                    <p>Комплексная защита без скрытых платежей и дополнительных условий.</p>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="feature-card-light">
                    <div class="feature-icon-light"><i class="fas fa-headset"></i></div>
                    <h3 class="feature-title-light">Персональный сервис</h3>
                    <p>Индивидуальный подход к каждому клиенту и круглосуточная поддержка.</p>
                </div>
            </div>
        </div>
    </div>
</section>

<section class="unified-section">
    <div class="container">
        <!-- Как это работает -->
        <div class="section-block">
            <div class="section-header text-center mb-5">
                <h2 class="section-title">Как это работает</h2>
                <p class="section-subtitle">Простой процесс аренды в несколько шагов</p>
            </div>
            <div class="row g-4">
                <div class="col-lg-3 col-md-6">
                    <div class="process-card">
                        <div class="process-number">1</div>
                        <div class="process-icon"><i class="fas fa-search"></i></div>
                        <h4>Выберите автомобиль</h4>
                        <p>Просмотрите наш каталог и выберите автомобиль, который вам подходит</p>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="process-card">
                        <div class="process-number">2</div>
                        <div class="process-icon"><i class="fas fa-calendar-check"></i></div>
                        <h4>Забронируйте дату</h4>
                        <p>Укажите даты аренды и заполните необходимую информацию</p>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="process-card">
                        <div class="process-number">3</div>
                        <div class="process-icon"><i class="fas fa-file-signature"></i></div>
                        <h4>Оформите договор</h4>
                        <p>Подпишите договор аренды и внесите необходимые документы</p>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="process-card">
                        <div class="process-number">4</div>
                        <div class="process-icon"><i class="fas fa-key"></i></div>
                        <h4>Получите ключи</h4>
                        <p>Заберите автомобиль и наслаждайтесь поездкой</p>
                    </div>
                </div>
            </div>
        </div>

        <!-- Почему выбирают нас -->
        <div class="section-block">
            <div class="section-header text-center mb-5">
                <h2 class="section-title">Почему выбирают нас</h2>
            </div>
            <div class="row g-4 justify-content-center">
                <div class="col-lg-4 col-md-6">
                    <div class="benefit-item">
                        <div class="benefit-icon"><i class="fas fa-check-circle"></i></div>
                        <div class="benefit-content">
                            <h4>Гибкие условия аренды</h4>
                            <p>Адаптируемся под ваши потребности и предлагаем удобные условия оплаты</p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6">
                    <div class="benefit-item">
                        <div class="benefit-icon"><i class="fas fa-check-circle"></i></div>
                        <div class="benefit-content">
                            <h4>Новейший автопарк</h4>
                            <p>Все автомобили регулярно обслуживаются и находятся в отличном состоянии</p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6">
                    <div class="benefit-item">
                        <div class="benefit-icon"><i class="fas fa-check-circle"></i></div>
                        <div class="benefit-content">
                            <h4>Прозрачные цены</h4>
                            <p>Никаких скрытых платежей - вы платите только за то, что указано в договоре</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>

<section class="cta-section">
    <div class="container text-center">
        <h2 class="cta-title">Готовы к незабываемым впечатлениям?</h2>
        <p class="lead mb-4">Выберите автомобиль своей мечты прямо сейчас.</p>
        <a href="carlistt.aspx" class="btn btn-outline-info btn-lg">Арендовать сейчас</a>
    </div>
</section>

<section class="reviews-section">
    <div class="container">
        <div class="section-header text-center mb-4">
            <h2 class="section-title">Отзывы клиентов</h2>
            <p class="section-subtitle">Оставьте впечатления о сервисе и авто</p>
        </div>

        <asp:Panel ID="pnlLoginRequired" runat="server" CssClass="alert alert-dark d-none" Visible="false">
            Войдите в аккаунт, чтобы оставить комментарий.
            <a href="login.aspx" class="link-info ms-1">Войти</a>
        </asp:Panel>

        <asp:Panel ID="pnlUsernameRequired" runat="server" CssClass="alert alert-info d-none" Visible="false">
            Укажите имя пользователя в профиле, чтобы оставить отзыв.
            <a href="profile.aspx" class="link-dark fw-semibold ms-1">Перейти в профиль</a>
        </asp:Panel>

        <asp:Panel ID="pnlCommentForm" runat="server" CssClass="card shadow-sm border-0 mb-4">
            <div class="card-body">
                <div class="d-flex align-items-center justify-content-between mb-3">
                    <div>
                        <div class="text-muted small">Вы авторизованы как</div>
                        <div class="fw-bold" id="currentUserName" runat="server"><asp:Label ID="lblCurrentUserName" runat="server" /></div>
                    </div>
                    <div class="badge bg-dark text-uppercase">до 5 ★</div>
                </div>
                <div class="row g-3 align-items-end">
                    <div class="col-md-3">
                        <label class="form-label d-block">Оценка</label>
                        <div class="star-rating" id="starRating">
                            <span data-value="1">&#9733;</span>
                            <span data-value="2">&#9733;</span>
                            <span data-value="3">&#9733;</span>
                            <span data-value="4">&#9733;</span>
                            <span data-value="5" class="active">&#9733;</span>
                        </div>
                        <asp:HiddenField ID="hfRating" runat="server" Value="5" />
                    </div>
                    <div class="col-md-7">
                        <label for="txtComment" class="form-label">Комментарий</label>
                        <asp:TextBox ID="txtComment" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Поделитесь впечатлениями (до 1000 символов)"></asp:TextBox>
                    </div>
                    <div class="col-md-2 d-grid">
                        <asp:Button ID="btnSubmitComment" runat="server" CssClass="btn btn-dark mt-4" Text="Отправить" OnClick="btnSubmitComment_Click" />
                    </div>
                </div>
                <asp:Label ID="lblCommentStatus" runat="server" Visible="false" CssClass="d-block mt-3"></asp:Label>
            </div>
        </asp:Panel>

        <asp:Repeater ID="rptComments" runat="server" OnItemCommand="rptComments_ItemCommand">
            <HeaderTemplate>
                <div class="row g-3">
            </HeaderTemplate>
            <ItemTemplate>
                <div class="col-md-6 col-lg-4">
                    <div class="card h-100 shadow-sm border-0 review-card">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-start mb-2">
                                <div>
                                    <div class="fw-bold"><%# Eval("UserName") %></div>
                                    <div class="text-muted small"><%# Eval("CreatedAt", "{0:dd.MM.yyyy HH:mm}") %></div>
                                    <asp:Label ID="lblHidden" runat="server" CssClass="badge bg-warning text-dark mt-1" Visible="false">Скрыт</asp:Label>
                                </div>
                                <div class="d-flex align-items-center gap-2">
                                    <asp:Literal ID="litStars" runat="server" Text='<%# RenderStars(Eval("Rating")) %>'></asp:Literal>
                                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-outline-danger"
                                        CommandName="delete" CommandArgument='<%# Eval("Id") %>'
                                        OnClientClick="return confirm('Удалить свой отзыв?');">
                                        Удалить
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <p class="mb-0 text-dark"><%# HttpUtility.HtmlEncode(Eval("CommentText").ToString()) %></p>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</section>

<script type="text/javascript">
    (function () {
        var stars = document.querySelectorAll('#starRating span');
        var hidden = document.getElementById('<%= hfRating.ClientID %>');
        function setActive(val) {
            stars.forEach(function (s) {
                var v = parseInt(s.getAttribute('data-value'));
                if (v <= val) s.classList.add('active');
                else s.classList.remove('active');
            });
            hidden.value = val;
        }
        stars.forEach(function (star) {
            star.addEventListener('click', function () {
                var val = parseInt(star.getAttribute('data-value'));
                setActive(val);
            });
            star.addEventListener('mouseenter', function () {
                var val = parseInt(star.getAttribute('data-value'));
                setActive(val);
            });
        });
    })();
</script>

</asp:Content>
