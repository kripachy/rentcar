<%@ Page Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="WebApplication1.view.admin.home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    body {
        display: flex;
        flex-direction: column;
        min-height: 100vh; /* Minimum height of the viewport */
        background-color: #f8f9fa;
        font-family: 'Segoe UI', sans-serif;
    }

    .main-content {
        flex-grow: 1; /* Main content takes up all available space */
        padding: 20px 0;
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
    <!-- Быстрые действия -->
    <div class="row">
        <div class="col-md-12">
            <div class="quick-actions animate-fade-in">
                <h3 class="section-title">Быстрые действия</h3>
                <div class="row">
                    <div class="col-md-3">
                        <a href="cars.aspx" class="action-button">
                            <i class="fas fa-car"></i>
                            Управление автомобилями
                        </a>
                    </div>
                    <div class="col-md-3">
                        <a href="customers.aspx" class="action-button">
                            <i class="fas fa-users"></i>
                            Управление клиентами
                        </a>
                    </div>
                    <div class="col-md-3">
                        <a href="rents.aspx" class="action-button">
                            <i class="fas fa-key"></i>
                            Управление арендой
                        </a>
                    </div>
                    <div class="col-md-3">
                        <a href="returns.aspx" class="action-button">
                            <i class="fas fa-undo"></i>
                            Возврат автомобилей
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

</asp:Content>
