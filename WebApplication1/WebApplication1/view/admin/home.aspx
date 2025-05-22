<%@ Page Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="WebApplication1.view.admin.home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    body {
        background-color: #f8f9fa;
        font-family: 'Segoe UI', sans-serif;
    }

    .hero-section {
        background: linear-gradient(135deg, #dc3545 0%, #bb2d3b 100%);
        color: white;
        padding: 3rem 0;
        margin-bottom: 2rem;
        border-radius: 0 0 20px 20px;
        box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
    }

    .hero-title {
        font-size: 2.5rem;
        font-weight: 700;
        margin-bottom: 1rem;
        text-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    }

    .hero-subtitle {
        font-size: 1.2rem;
        font-weight: 300;
        margin-bottom: 1.5rem;
        opacity: 0.9;
    }

    .stats-card {
        background: white;
        border-radius: 15px;
        padding: 1.5rem;
        margin-bottom: 1.5rem;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05);
        transition: transform 0.3s ease;
        border: none;
        border-left: 4px solid #dc3545;
    }

    .stats-card:hover {
        transform: translateY(-5px);
    }

    .stats-icon {
        font-size: 2rem;
        color: #dc3545;
        margin-bottom: 1rem;
    }

    .stats-number {
        font-size: 2rem;
        font-weight: 700;
        color: #343a40;
        margin-bottom: 0.5rem;
    }

    .stats-label {
        color: #6c757d;
        font-size: 1rem;
        font-weight: 500;
    }

    .quick-actions {
        background: white;
        border-radius: 15px;
        padding: 2rem;
        margin-bottom: 2rem;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05);
    }

    .action-button {
        display: flex;
        align-items: center;
        padding: 1rem;
        margin-bottom: 1rem;
        border-radius: 10px;
        background: #f8f9fa;
        color: #343a40;
        text-decoration: none;
        transition: all 0.3s ease;
        border: 1px solid #dee2e6;
    }

    .action-button:hover {
        background: #dc3545;
        color: white;
        text-decoration: none;
        transform: translateX(5px);
    }

    .action-button i {
        font-size: 1.5rem;
        margin-right: 1rem;
        width: 24px;
        text-align: center;
    }

    .car-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
        gap: 1.5rem;
        margin-top: 2rem;
    }

    .car-card {
        background: white;
        border-radius: 15px;
        padding: 1.5rem;
        text-align: center;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05);
        transition: all 0.3s ease;
        border: none;
    }

    .car-card:hover {
        transform: translateY(-5px);
        box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
    }

    .car-card img {
        width: 100%;
        height: 120px;
        object-fit: contain;
        margin-bottom: 1rem;
    }

    .car-card h5 {
        color: #dc3545;
        font-weight: 600;
        margin: 0;
    }

    .section-title {
        color: #343a40;
        font-weight: 700;
        margin-bottom: 1.5rem;
        padding-bottom: 0.5rem;
        border-bottom: 2px solid #dc3545;
        display: inline-block;
    }

    @keyframes fadeIn {
        from { opacity: 0; transform: translateY(20px); }
        to { opacity: 1; transform: translateY(0); }
    }

    .animate-fade-in {
        animation: fadeIn 0.5s ease-out forwards;
    }
</style>

<div class="hero-section">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-md-8">
                <h1 class="hero-title">Панель управления WheelDeal</h1>
                <p class="hero-subtitle">Управляйте автомобилями, клиентами и арендой в одном месте</p>
            </div>
            <div class="col-md-4 text-center">
                <i class="fas fa-car-side fa-4x"></i>
            </div>
        </div>
    </div>
</div>

<div class="container">
    <!-- Статистика -->
    <div class="row mb-4">
        <div class="col-md-3">
            <div class="stats-card animate-fade-in">
                <i class="fas fa-car stats-icon"></i>
                <div class="stats-number">8</div>
                <div class="stats-label">Всего автомобилей</div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="stats-card animate-fade-in" style="animation-delay: 0.1s">
                <i class="fas fa-key stats-icon"></i>
                <div class="stats-number">5</div>
                <div class="stats-label">Доступно для аренды</div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="stats-card animate-fade-in" style="animation-delay: 0.2s">
                <i class="fas fa-users stats-icon"></i>
                <div class="stats-number">12</div>
                <div class="stats-label">Активных клиентов</div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="stats-card animate-fade-in" style="animation-delay: 0.3s">
                <i class="fas fa-file-invoice-dollar stats-icon"></i>
                <div class="stats-number">3</div>
                <div class="stats-label">Активных аренд</div>
            </div>
        </div>
    </div>

    <div class="row">
        <!-- Быстрые действия -->
        <div class="col-md-4">
            <div class="quick-actions animate-fade-in">
                <h3 class="section-title">Быстрые действия</h3>
                <a href="cars.aspx" class="action-button">
                    <i class="fas fa-plus-circle"></i>
                    Добавить автомобиль
                </a>
                <a href="carlistt.aspx" class="action-button">
                    <i class="fas fa-list"></i>
                    Список автомобилей
                </a>
                <a href="rents.aspx" class="action-button">
                    <i class="fas fa-key"></i>
                    Управление арендой
                </a>
                <a href="returns.aspx" class="action-button">
                    <i class="fas fa-undo"></i>
                    Возврат автомобилей
                </a>
            </div>
        </div>

        <!-- Список автомобилей -->
        <div class="col-md-8">
            <h3 class="section-title">Наш автопарк</h3>
            <div class="car-grid">
                <div class="car-card animate-fade-in">
                    <img src="../../assets/images/aston_martin.png" alt="Aston Martin" />
                    <h5>Aston Martin Vanquish</h5>
                </div>
                <div class="car-card animate-fade-in" style="animation-delay: 0.1s">
                    <img src="../../assets/images/porshe.png" alt="Porsche" />
                    <h5>Porsche 911</h5>
                </div>
                <div class="car-card animate-fade-in" style="animation-delay: 0.2s">
                    <img src="../../assets/images/lamba.png" alt="Lamborghini" />
                    <h5>Lamborghini Huracan</h5>
                </div>
                <div class="car-card animate-fade-in" style="animation-delay: 0.3s">
                    <img src="../../assets/images/mazerati.png" alt="Maserati" />
                    <h5>Maserati GranTurismo</h5>
                </div>
            </div>
        </div>
    </div>
</div>

</asp:Content>
