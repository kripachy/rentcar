<%@ Page Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="WebApplication1.view.admin.home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .admin-dashboard-hero {
        background: radial-gradient(1200px 300px at 10% 0%, rgba(0, 188, 212, 0.22), transparent 55%),
            linear-gradient(180deg, #0b0d0f, #000000);
        color: #ffffff;
        border: 1px solid rgba(255, 255, 255, 0.08);
        border-radius: 18px;
        padding: 22px 26px;
        margin-bottom: 18px;
        box-shadow: 0 16px 40px rgba(0, 0, 0, 0.16);
    }

    .admin-dashboard-hero-title {
        font-weight: 800;
        font-size: 1.6rem;
        margin: 0;
    }

    .admin-dashboard-hero-subtitle {
        opacity: 0.85;
        margin-top: 6px;
        margin-bottom: 0;
    }

    .dash-stat {
        background: #ffffff;
        border: 1px solid #dee2e6;
        border-radius: 16px;
        padding: 16px;
        box-shadow: 0 10px 28px rgba(0, 0, 0, 0.06);
        height: 100%;
        transition: transform 0.2s ease, box-shadow 0.2s ease, border-color 0.2s ease;
    }

    .dash-stat:hover {
        transform: translateY(-2px);
        border-color: rgba(0, 188, 212, 0.6);
        box-shadow: 0 14px 36px rgba(0, 0, 0, 0.10);
    }

    .dash-stat-value {
        font-size: 1.8rem;
        font-weight: 800;
        color: #000000;
        line-height: 1.1;
    }

    .dash-stat-label {
        color: #6c757d;
        font-weight: 600;
        margin-top: 6px;
    }

    .dash-stat-link {
        display: inline-block;
        margin-top: 10px;
        font-weight: 600;
        color: #007c8a;
        text-decoration: none;
    }

    .dash-stat-link:hover {
        text-decoration: underline;
    }

    .dash-card {
        background: #ffffff;
        border: 1px solid #dee2e6;
        border-radius: 16px;
        padding: 18px;
        box-shadow: 0 10px 28px rgba(0, 0, 0, 0.06);
    }

    .dash-card-title {
        font-weight: 800;
        color: #000000;
        margin-bottom: 12px;
    }

    .dash-action {
        display: block;
        padding: 10px 12px;
        border-radius: 12px;
        border: 1px solid #dee2e6;
        background: #f8f9fa;
        color: #000000;
        text-decoration: none;
        font-weight: 600;
        transition: background-color 0.2s ease, border-color 0.2s ease;
    }

    .dash-action:hover {
        background: rgba(0, 188, 212, 0.12);
        border-color: rgba(0, 188, 212, 0.45);
        text-decoration: none;
        color: #000000;
    }
</style>

<div class="admin-dashboard-hero">
    <div class="d-flex justify-content-between align-items-start flex-wrap gap-2">
        <div>
            <h1 class="admin-dashboard-hero-title">Панель администратора</h1>
            <p class="admin-dashboard-hero-subtitle">Быстрый доступ к управлению системой</p>
        </div>
        <div class="text-end">
            <a href="cars.aspx" class="btn btn-outline-info btn-sm">Открыть автомобили</a>
        </div>
    </div>
</div>

<div class="row g-3">
    <div class="col-6 col-lg-3">
        <div class="dash-stat">
            <div class="dash-stat-value"><asp:Literal ID="litCarsCount" runat="server" /></div>
            <div class="dash-stat-label">Автомобили</div>
            <a class="dash-stat-link" href="cars.aspx">Открыть</a>
        </div>
    </div>
    <div class="col-6 col-lg-3">
        <div class="dash-stat">
            <div class="dash-stat-value"><asp:Literal ID="litCustomersCount" runat="server" /></div>
            <div class="dash-stat-label">Клиенты</div>
            <a class="dash-stat-link" href="customers.aspx">Открыть</a>
        </div>
    </div>
    <div class="col-6 col-lg-3">
        <div class="dash-stat">
            <div class="dash-stat-value"><asp:Literal ID="litRentsCount" runat="server" /></div>
            <div class="dash-stat-label">Аренды</div>
            <a class="dash-stat-link" href="rents.aspx">Открыть</a>
        </div>
    </div>
    <div class="col-6 col-lg-3">
        <div class="dash-stat">
            <div class="dash-stat-value"><asp:Literal ID="litCommentsCount" runat="server" /></div>
            <div class="dash-stat-label">Отзывы</div>
            <a class="dash-stat-link" href="comments.aspx">Открыть</a>
        </div>
    </div>
</div>

<div class="row g-3 mt-1">
    <div class="col-lg-8">
        <div class="dash-card">
            <div class="dash-card-title">Быстрые действия</div>
            <div class="row g-2">
                <div class="col-md-6">
                    <a class="dash-action" href="cars.aspx">Управление автомобилями</a>
                </div>
                <div class="col-md-6">
                    <a class="dash-action" href="customers.aspx">Управление клиентами</a>
                </div>
                <div class="col-md-6">
                    <a class="dash-action" href="rents.aspx">Управление арендой</a>
                </div>
                <div class="col-md-6">
                    <a class="dash-action" href="ManageVerification.aspx">Проверка документов</a>
                </div>
                <div class="col-md-6">
                    <a class="dash-action" href="comments.aspx">Отзывы пользователей</a>
                </div>
                <div class="col-md-6">
                    <a class="dash-action" href="slider.aspx">Слайдер главной страницы</a>
                </div>
            </div>
        </div>
    </div>
    <div class="col-lg-4">
        <div class="dash-card">
            <div class="dash-card-title">Требует внимания</div>
            <div class="d-flex justify-content-between align-items-center">
                <div class="text-muted fw-semibold">Ожидают проверки</div>
                <div class="fw-bold"><asp:Literal ID="litPendingVerifications" runat="server" /></div>
            </div>
            <div class="mt-2">
                <a class="dash-stat-link" href="ManageVerification.aspx">Перейти к проверкам</a>
            </div>
        </div>
    </div>
</div>

</asp:Content>
