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
        padding: 5rem 0;
        margin-bottom: 3rem;
        border-radius: 0 0 20px 20px;
        box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
        opacity: 0;
        animation: fadeIn 2s forwards;
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

    .car-box {
        display: flex;
        flex-direction: column;
        align-items: center;
        height: 300px;
        justify-content: center;
        margin-bottom: 20px;
        opacity: 0;
        animation: fadeInUp 1s ease-out forwards;
    }

    .car-box img {
        width: 250px;
        height: 150px;
        object-fit: contain;
        margin-bottom: 10px;
        transform: scale(0.8);
        animation: zoomIn 0.5s forwards;
    }

    .car-box h6 {
        margin: 0;
        color: #dc3545;
        font-weight: 600;
        font-family: 'Segoe UI', sans-serif; 
    }

    .custom-jumbotron {
        background-color: #dc3545;
        border-radius: 10px;
        box-shadow: 0 6px 20px rgba(0, 0, 0, 0.15);
        padding: 5rem 0;
    }

    .custom-jumbotron h1 {
        font-size: 3.5rem;
        font-weight: 700;
        margin-bottom: 1rem;
        text-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    }

    .custom-jumbotron p {
        font-size: 1.25rem;
        font-weight: 300;
        opacity: 0.9;
        margin-top: 10px;
    }

    .cta-section {
        background: linear-gradient(135deg, #343a40 0%, #212529 100%);
        color: white;
        padding: 4rem 0;
        border-radius: 20px;
        margin: 3rem 0;
        text-align: center;
        opacity: 0;
        animation: fadeInUp 2s ease-out forwards;
    }

    .cta-title {
        font-size: 2.5rem;
        font-weight: 700;
        margin-bottom: 1.5rem;
    }

    .btn-cta {
        background-color: #dc3545;
        color: white;
        padding: 0.8rem 2.5rem;
        border-radius: 50px;
        font-weight: 600;
        font-size: 1.1rem;
        border: none;
        transition: all 0.3s ease;
    }

    .btn-cta:hover {
        background-color: #bb2d3b;
        transform: translateY(-2px);
        box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
    }

    @keyframes fadeIn {
        0% {
            opacity: 0;
        }
        100% {
            opacity: 1;
        }
    }

    @keyframes fadeInUp {
        0% {
            opacity: 0;
            transform: translateY(30px);
        }
        100% {
            opacity: 1;
            transform: translateY(0);
        }
    }

    @keyframes zoomIn {
        0% {
            transform: scale(0.8);
        }
        100% {
            transform: scale(1);
        }
    }

</style>

<div class="hero-section text-center">
    <div class="container">
        <h1 class="hero-title">WheelDeal Management System</h1>
        <p class="hero-subtitle">Администратор может управлять автомобилями, клиентами, арендой и возвратами</p>
    </div>
</div>

<div class="container">
    <div class="row text-center mb-4">
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/aston_martin.png" alt="Aston Martin" />
                <h6>Aston Martin</h6>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/porshe.png" alt="Porsche" />
                <h6>Porsche</h6>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/jagyar.png" alt="Jaguar" />
                <h6>Jaguar</h6>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/audi.png" alt="Audi" />
                <h6>Audi</h6>
            </div>
        </div>
    </div>

    <div class="row text-center mb-4">
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/mazerati.png" alt="Maserati" />
                <h6>Maserati</h6>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/chevrolet.png" alt="Chevrolet" />
                <h6>Chevrolet</h6>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/lamba.png" alt="Lamborghini" />
                <h6>Lamborghini</h6>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/mustang.png" alt="Ford Mustang" />
                <h6>Ford Mustang</h6>
            </div>
        </div>
    </div>
</div>

</asp:Content>
