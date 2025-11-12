<%@ Page Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true" CodeBehind="userdashboard.aspx.cs" Inherits="WebApplication1.view.admin.userdashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<section class="slider-container">
    <div class="container">
        <div id="dynamicSlider" class="carousel slide hero-slider" data-bs-ride="carousel">
            <div class="carousel-indicators">
                <button type="button" data-bs-target="#dynamicSlider" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                <button type="button" data-bs-target="#dynamicSlider" data-bs-slide-to="1" aria-label="Slide 2"></button>
                <button type="button" data-bs-target="#dynamicSlider" data-bs-slide-to="2" aria-label="Slide 3"></button>
            </div>
            <div class="carousel-inner">
                <div class="carousel-item active" style="background-image: url('../../colorcars/Aston Martin Vanquish/white/3.jpg');">
                    <div class="carousel-caption">
                        <h1>Британская элегантность</h1>
                        <p>Aston Martin Vanquish для истинных ценителей.</p>
                    </div>
                </div>
                <div class="carousel-item" style="background-image: url('../../colorcars/Lamborghini Huracan/purple/3.jpg');">
                    <div class="carousel-caption">
                        <h1>Неукротимая мощь</h1>
                        <p>Lamborghini Huracan — эмоции в чистом виде.</p>
                    </div>
                </div>
                <div class="carousel-item" style="background-image: url('../../colorcars/Maserati GranTurismo/red/5.jpg');">
                    <div class="carousel-caption">
                        <h1>Итальянская страсть</h1>
                        <p>Maserati GranTurismo не оставит вас равнодушным.</p>
                    </div>
                </div>
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
    </div>
</section>

<div class="site-title-container text-center py-4">
    <h1 class="site-title" style="color: #00ffff;">WheelDeal</h1>
</div>

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

<section class="cta-section">
    <div class="container text-center">
        <h2 class="cta-title">Готовы к незабываемым впечатлениям?</h2>
        <p class="lead mb-4">Выберите автомобиль своей мечты прямо сейчас.</p>
        <a href="carlistt.aspx" class="btn btn-outline-info btn-lg">Арендовать сейчас</a>
    </div>
</section>

</asp:Content>
