<%@ Page Title="О нас" Language="C#" MasterPageFile="~/view/user/UserMaster.Master" AutoEventWireup="true" CodeBehind="info.aspx.cs" Inherits="WebApplication1.view.user.info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-12">
                <div class="card shadow">
                    <div class="card-body">
                        <h2 class="text-center mb-4">О компании WheelDeal</h2>
                        
                        <div class="row mb-4">
                            <div class="col-md-6">
                                <h4>Наша история</h4>
                                <p>Компания WheelDeal была основана в 2021 году с целью предоставления качественных услуг по аренде премиальных автомобилей. За это время мы стали надежным партнером для тысяч клиентов, которые ценят комфорт и качество.</p>
                            </div>
                            <div class="col-md-6">
                                <h4>Наша миссия</h4>
                                <p>Мы стремимся сделать аренду премиальных автомобилей доступной и удобной для каждого клиента, обеспечивая высочайший уровень сервиса и безопасность.</p>
                            </div>
                        </div>

                        <div class="row mb-4">
                            <div class="col-md-4">
                                <div class="text-center">
                                    <i class="fas fa-car fa-3x mb-3 text-primary"></i>
                                    <h5>Премиальный автопарк</h5>
                                    <p>Только лучшие модели от ведущих производителей</p>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="text-center">
                                    <i class="fas fa-shield-alt fa-3x mb-3 text-primary"></i>
                                    <h5>Безопасность</h5>
                                    <p>Все автомобили проходят регулярное техническое обслуживание</p>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="text-center">
                                    <i class="fas fa-headset fa-3x mb-3 text-primary"></i>
                                    <h5>Поддержка 24/7</h5>
                                    <p>Наша служба поддержки всегда готова помочь вам</p>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-12">
                                <h4 class="text-center mb-3">Почему выбирают нас?</h4>
                                <ul class="list-unstyled">
                                    <li class="mb-2"><i class="fas fa-check text-success me-2"></i> Большой выбор премиальных автомобилей</li>
                                    <li class="mb-2"><i class="fas fa-check text-success me-2"></i> Гибкая система скидок и бонусов</li>
                                    <li class="mb-2"><i class="fas fa-check text-success me-2"></i> Простая и быстрая процедура аренды</li>
                                    <li class="mb-2"><i class="fas fa-check text-success me-2"></i> Профессиональная команда специалистов</li>
                                    <li class="mb-2"><i class="fas fa-check text-success me-2"></i> Конкурентные цены на рынке</li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 