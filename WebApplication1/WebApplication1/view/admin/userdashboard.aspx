<%@ Page Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true" CodeBehind="userdashboard.aspx.cs" Inherits="WebApplication1.view.admin.userdashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    body {
        background-color: #f8f9fa;
        font-family: 'Segoe UI', sans-serif;
        overflow-x: hidden; 
    }

    .hero-section {
        background: linear-gradient(135deg, #dc3545 0%, #bb2d3b 100%);
        color: white;
        padding: 5rem 0;
        margin-bottom: 3rem;
        border-radius: 0 0 20px 20px;
        box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
        opacity: 0;
        animation: fadeIn 2s ease-out forwards;
    }

    @keyframes fadeIn {
        from {
            opacity: 0;
        }
        to {
            opacity: 1;
        }
    }

    .hero-title {
        font-size: 3.5rem;
        font-weight: 700;
        margin-bottom: 1rem;
        text-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    }

    .hero-subtitle {
        font-size: 1.5rem;
        font-weight: 300;
        margin-bottom: 2rem;
        opacity: 0.9;
    }

    .feature-card {
        background: white;
        border-radius: 15px;
        padding: 2rem;
        margin-bottom: 2rem;
        box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
        transition: transform 0.3s ease, box-shadow 0.3s ease, background-color 0.3s ease;
        height: 100%;
        opacity: 0;
        animation: fadeInUp 1.5s ease-out forwards;
    }

    .feature-card:hover {
        transform: scale(1.05);
        box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
        background-color: #f8f9fa;
    }

    @keyframes fadeInUp {
        from {
            opacity: 0;
            transform: translateY(20px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }

    .feature-icon {
        font-size: 2.5rem;
        color: #dc3545;
        margin-bottom: 1.5rem;
        transition: transform 0.3s ease;
    }

    .feature-icon:hover {
        transform: rotate(20deg);
    }

    .feature-title {
        font-size: 1.5rem;
        font-weight: 600;
        margin-bottom: 1rem;
        color: #343a40;
    }

    .feature-text {
        color: #6c757d;
        line-height: 1.6;
    }

    .benefits-section {
        background-color: #fff;
        padding: 4rem 0;
        margin: 3rem 0;
        border-radius: 20px;
        box-shadow: 0 5px 20px rgba(0, 0, 0, 0.05);
        opacity: 0;
        animation: fadeInUp 2s ease-out forwards;
    }

    .section-title {
        font-size: 2.5rem;
        font-weight: 700;
        color: #343a40;
        margin-bottom: 2rem;
        text-align: center;
    }

    .benefit-item {
        display: flex;
        align-items: center;
        margin-bottom: 1.5rem;
        opacity: 0;
        animation: fadeInUp 1.5s ease-out forwards;
        transition: transform 0.3s ease;
    }

    .benefit-item:hover {
        transform: translateX(10px);
    }

    .benefit-icon {
        font-size: 1.5rem;
        color: #dc3545;
        margin-right: 1rem;
        min-width: 30px;
    }

    .cta-section {
        background: linear-gradient(135deg, #343a40 0%, #212529 100%);
        color: white;
        padding: 4rem 0;
        border-radius: 20px;
        margin: 3rem 0;
        text-align: center;
    }

    .cta-title {
        font-size: 2.5rem;
        font-weight: 700;
        margin-bottom: 1.5rem;
    }

    .btn-cta {
  text-decoration: none; 
  display: inline-block; 
  padding: 10px 20px; 
  background-color: #dc3545; 
  color: white;
  border-radius: 50px;
  font-weight: 600;
  font-size: 1.1rem;
  border: none;
  transition: transform 0.3s ease, background-color 0.3s ease;
}

.btn-cta:hover {
  background-color: #bb2d3b; 
  transform: scale(1.2);
  box-shadow: 0 5px 15px rgba(0,0,0,0.2);
  text-decoration: none;
}



    .testimonial-card {
        background: white;
        border-radius: 15px;
        padding: 2rem;
        box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
        margin-bottom: 2rem;
        opacity: 0;
        animation: fadeInUp 1.5s ease-out forwards;
    }

    .testimonial-text {
        font-style: italic;
        color: #6c757d;
        line-height: 1.6;
        margin-bottom: 1.5rem;
    }

    .testimonial-author {
        font-weight: 600;
        color: #343a40;
    }

</style>

<div class="hero-section text-center">
    <div class="container">
        <h1 class="hero-title">WheelDeal</h1>
        <p class="hero-subtitle">Сервис аренды автомобилей</p>
    </div>
</div>

<div class="container">
    <div class="row">
        <div class="col-lg-4">
            <div class="feature-card text-center">
                <div class="feature-icon">
                    <i class="fas fa-car"></i>
                </div>
                <h3 class="feature-title">Эксклюзивные автомобили</h3>
                <p class="feature-text">Доступ к самым престижным моделям от ведущих мировых производителей</p>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="feature-card text-center">
                <div class="feature-icon">
                    <i class="fas fa-shield-alt"></i>
                </div>
                <h3 class="feature-title">Полная страховка</h3>
                <p class="feature-text">Комплексная защита без скрытых платежей и дополнительных условий</p>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="feature-card text-center">
                <div class="feature-icon">
                    <i class="fas fa-concierge-bell"></i>
                </div>
                <h3 class="feature-title">Персональный сервис</h3>
                <p class="feature-text">Индивидуальный подход к каждому клиенту и круглосуточная поддержка</p>
            </div>
        </div>
    </div>
</div>

<div class="benefits-section">
    <div class="container">
        <h2 class="section-title">Наши преимущества</h2>
        <div class="row">
            <div class="col-lg-6">
                <div class="benefit-item">
                    <div class="benefit-icon">
                        <i class="fas fa-check-circle"></i>
                    </div>
                    <div>
                        <h4>Гарантия качества</h4>
                        <p>Все автомобили проходят тщательную проверку перед каждой арендой</p>
                    </div>
                </div>
                <div class="benefit-item">
                    <div class="benefit-icon">
                        <i class="fas fa-euro-sign"></i>
                    </div>
                    <div>
                        <h4>Прозрачные цены</h4>
                        <p>Никаких скрытых платежей - вы платите только за то, что видите</p>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="benefit-item">
                    <div class="benefit-icon">
                        <i class="fas fa-map-marker-alt"></i>
                    </div>
                    <div>
                        <h4>Доставка и возврат</h4>
                        <p>Заберем и вернем автомобиль в любом удобном для вас месте</p>
                    </div>
                </div>
                <div class="benefit-item">
                    <div class="benefit-icon">
                        <i class="fas fa-clock"></i>
                    </div>
                    <div>
                        <h4>Гибкие условия</h4>
                        <p>Аренда от нескольких часов до нескольких месяцев</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="container">
    <div class="cta-section">
        <h2 class="cta-title">Готовы к незабываемым впечатлениям?</h2>
        <p class="lead mb-4">Выберите автомобиль своей мечты прямо сейчас</p>
        <a href="rentauto.aspx" class="btn-cta">Арендовать сейчас</a>
    </div>
</div>

<script src="https://kit.fontawesome.com/a076d05399.js" crossorigin="anonymous"></script>

</asp:Content>
