<%@ Page Language="C#" MasterPageFile="~/view/admin/adminmaster.master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="WebApplication1.view.admin.home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .car-box {
        display: flex;
        flex-direction: column;
        align-items: center;
        height: 300px; 
        justify-content: center;
        margin-bottom: 20px;
    }

    .car-box img {
        width: 250px;
        height: 150px;
        object-fit: contain;
        margin-bottom: 10px;
    }

    .car-box h6 {
        margin: 0;
    }

</style>


<div class="custom-jumbotron text-white text-center py-5 mb-5">
    <div class="container">
        <h1 class="display-4 font-weight-bold">WheelDeal Management System Admin Panel</h1>
        <p class="lead mb-0">The Admin Can Manage Cars, Customers, Rentals and Returns</p>
    </div>
</div>

<style>
    .custom-jumbotron {
        background-color: #dc3545; 
        border-radius: 10px;
        box-shadow: 0 6px 20px rgba(0, 0, 0, 0.15);
    }

    .custom-jumbotron h1 {
        font-size: 3rem;
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    }

    .custom-jumbotron p {
        font-size: 1.25rem;
        font-weight: 300;
        margin-top: 10px;
    }
</style>



    <div class="row text-center mb-4">
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/aston_martin.png" alt="Aston Martin" />
                <h6 class="text-danger">Aston Martin</h6>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/porshe.png" alt="Porsche" />
                <h6 class="text-danger">Porsche</h6>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/jagyar.png" alt="Jaguar" />
                <h6 class="text-danger">Jaguar</h6>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/audi.png" alt="Audi" />
                <h6 class="text-danger">Audi</h6>
            </div>
        </div>
    </div>

    <div class="row text-center mb-4">
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/mazerati.png" alt="Maserati" />
                <h6 class="text-danger">Maserati</h6>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/chevrolet.png" alt="Chevrolet" />
                <h6 class="text-danger">Chevrolet</h6>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/lamba.png" alt="Lamborghini" />
                <h6 class="text-danger">Lamborghini</h6>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="car-box">
                <img src="../../assets/images/mustang.png" alt="Ford Mustang" />
                <h6 class="text-danger">Ford Mustang</h6>
            </div>
        </div>
    </div>
        </div>


</asp:Content>
