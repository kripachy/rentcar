<%@ Page Title="Aston Martin Vanquish" Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true" CodeBehind="AstonMartinVanquish.aspx.cs" Inherits="WebApplication1.view.admin.AstonMartinVanquish" ContentType="text/html" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .carousel-container {
            position: relative;
        }
        .carousel-control-prev,
        .carousel-control-next {
            width: 60px;
            height: 60px;
            background: rgba(220, 53, 69, 0.8) !important;
            border-radius: 50%;
            top: 50%;
            transform: translateY(-50%);
            opacity: 0 !important;
            transition: all 0.3s ease;
            pointer-events: none;
            visibility: hidden;
        }
        .carousel-container:hover .carousel-control-prev,
        .carousel-container:hover .carousel-control-next {
            opacity: 1 !important;
            pointer-events: auto;
            visibility: visible;
        }
        .carousel-control-prev {
            left: 20px;
        }
        .carousel-control-next {
            right: 20px;
        }
        .carousel-control-prev-icon,
        .carousel-control-next-icon {
            width: 30px;
            height: 30px;
            filter: brightness(0) invert(1);
        }
        .carousel-control-prev:hover,
        .carousel-control-next:hover {
            background: rgba(220, 53, 69, 1) !important;
            box-shadow: 0 0 15px rgba(220, 53, 69, 0.5);
        }
        /* Принудительно скрываем стрелки, когда курсор не над каруселью */
        .carousel-container:not(:hover) .carousel-control-prev,
        .carousel-container:not(:hover) .carousel-control-next {
            opacity: 0 !important;
            visibility: hidden !important;
            pointer-events: none !important;
        }
    </style>

    <div class="container mt-4">
        <div class="mb-4">
            <a href="~/view/admin/carlistt.aspx" runat="server" class="btn btn-outline-danger">
                <i class="fas fa-arrow-left"></i> Вернуться к списку машин
            </a>
        </div>

        <h1 class="text-center mb-4 text-danger">Aston Martin Vanquish</h1>
        
        <div class="row">
            <!-- Карусель слева -->
            <div class="col-md-7">
                <div class="carousel-container">
                    <div id="carCarousel" class="carousel slide mb-4" data-bs-ride="carousel">
                        <div class="carousel-indicators">
                            <button type="button" data-bs-target="#carCarousel" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                            <button type="button" data-bs-target="#carCarousel" data-bs-slide-to="1" aria-label="Slide 2"></button>
                            <button type="button" data-bs-target="#carCarousel" data-bs-slide-to="2" aria-label="Slide 3"></button>
                        </div>
                        <div class="carousel-inner rounded">
                            <div class="carousel-item active">
                                <img src="../../colorcars/Aston Martin Vanquish/white/1.jpg" class="d-block w-100 rounded" alt="Aston Martin Vanquish White 1" style="object-fit: cover; height: 450px;">
                            </div>
                            <div class="carousel-item">
                                <img src="../../colorcars/Aston Martin Vanquish/white/2.jpg" class="d-block w-100 rounded" alt="Aston Martin Vanquish White 2" style="object-fit: cover; height: 450px;">
                            </div>
                            <div class="carousel-item">
                                <img src="../../colorcars/Aston Martin Vanquish/white/3.jpg" class="d-block w-100 rounded" alt="Aston Martin Vanquish White 3" style="object-fit: cover; height: 450px;">
                            </div>
                        </div>
                        <button class="carousel-control-prev" type="button" data-bs-target="#carCarousel" data-bs-slide="prev">
                            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                        </button>
                        <button class="carousel-control-next" type="button" data-bs-target="#carCarousel" data-bs-slide="next">
                            <span class="carousel-control-next-icon" aria-hidden="true"></span>
                        </button>
                    </div>
                </div>
            </div>

            <!-- Характеристики справа -->
            <div class="col-md-5">
                <div class="card border-danger h-100">
                    <div class="card-header bg-danger text-white">
                        <h3 class="card-title mb-0">Технические характеристики</h3>
                    </div>
                    <div class="card-body">
                        <ul class="list-group list-group-flush">
                            <li class="list-group-item"><strong>Двигатель:</strong> 5.9-литровый V12 (5935 см³), серия AM11</li>
                            <li class="list-group-item"><strong>Коробка передач:</strong> 6-ступенчатый "автомат" Touchtronic 2 (ZF)</li>
                            <li class="list-group-item"><strong>Мощность:</strong> 573 л.с. (421 кВт) при 6750 об/мин</li>
                            <li class="list-group-item"><strong>Крутящий момент:</strong> 620 Н·м при 5500 об/мин</li>
                            <li class="list-group-item"><strong>Разгон (0-100 км/ч):</strong> 4.1 сек</li>
                            <li class="list-group-item"><strong>Максимальная скорость:</strong> 295 км/ч</li>
                            <li class="list-group-item"><strong>Привод:</strong> Задний</li>
                            <li class="list-group-item"><strong>Масса:</strong> 1739 кг</li>
                            <li class="list-group-item"><strong>Габариты:</strong> 4720 x 2067 x 1294 мм</li>
                            <li class="list-group-item"><strong>Колёсная база:</strong> 2740 мм</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>

        <!-- Информация об аренде внизу -->
        <div class="row mt-4">
            <div class="col-12">
                <div class="card border-danger">
                    <div class="card-header bg-danger text-white">
                        <h3 class="card-title mb-0">Информация об аренде</h3>
                    </div>
                    <div class="card-body">
                        <div class="row align-items-center">
                            <div class="col-md-8">
                                <div class="price-tag">
                                    <h4 class="text-danger mb-0">Цена за день: $<asp:Label ID="lblPrice" runat="server" CssClass="font-weight-bold" style="white-space: nowrap;"></asp:Label></h4>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="color-info">
                                    <h5 class="mb-2">Доступный цвет:</h5>
                                    <div class="d-flex align-items-center">
                                        <div class="color-swatch" style="background-color: white; width: 50px; height: 50px; border: 1px solid #ddd;"></div>
                                        <span class="ml-2">Белый</span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnRent" runat="server" Text="Арендовать" CssClass="btn btn-danger btn-lg w-100" OnClick="btnRent_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        document.addEventListener('DOMContentLoaded', function() {
            var carousel = new bootstrap.Carousel(document.getElementById('carCarousel'), {
                interval: 5000,
                wrap: true
            });
        });
    </script>
</asp:Content> 