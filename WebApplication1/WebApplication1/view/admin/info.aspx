<%@ Page Title="О нас" Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true" CodeBehind="info.aspx.cs" Inherits="WebApplication1.view.admin.info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-12">
                <div class="card shadow mb-4">
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
                                    <i class="fas fa-car fa-3x mb-3 text-danger"></i>
                                    <h5>Премиальный автопарк</h5>
                                    <p>Только лучшие модели от ведущих производителей</p>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="text-center">
                                    <i class="fas fa-shield-alt fa-3x mb-3 text-danger"></i>
                                    <h5>Безопасность</h5>
                                    <p>Все автомобили проходят регулярное техническое обслуживание</p>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="text-center">
                                    <i class="fas fa-headset fa-3x mb-3 text-danger"></i>
                                    <h5>Поддержка 24/7</h5>
                                    <p>Наша служба поддержки всегда готова помочь вам</p>
                                </div>
                            </div>
                        </div>

                        <div class="row mb-4">
                            <div class="col-12">
                                <h4 class="text-center mb-3">Почему выбирают нас?</h4>
                                <ul class="list-unstyled">
                                    <li class="mb-2"><i class="fas fa-check text-danger me-2"></i> Большой выбор премиальных автомобилей</li>
                                    <li class="mb-2"><i class="fas fa-check text-danger me-2"></i> Гибкая система скидок и бонусов</li>
                                    <li class="mb-2"><i class="fas fa-check text-danger me-2"></i> Простая и быстрая процедура аренды</li>
                                    <li class="mb-2"><i class="fas fa-check text-danger me-2"></i> Профессиональная команда специалистов</li>
                                    <li class="mb-2"><i class="fas fa-check text-danger me-2"></i> Конкурентные цены на рынке</li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card shadow">
                    <div class="card-body">
                        <h3 class="text-center mb-4">Наши контакты</h3>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="contact-info">
                                    <h4 class="mb-3">Свяжитесь с нами</h4>
                                    <ul class="list-unstyled">
                                        <li class="mb-3">
                                            <i class="fas fa-map-marker-alt text-danger me-2"></i>
                                            <strong>Адрес:</strong> ул. Уручская, 21в, Минск, Беларусь
                                        </li>
                                        <li class="mb-3">
                                            <i class="fas fa-phone text-danger me-2"></i>
                                            <strong>Телефон:</strong> +375 (29) 123-45-67
                                        </li>
                                        <li class="mb-3">
                                            <i class="fas fa-envelope text-danger me-2"></i>
                                            <strong>Email:</strong> info@wheeldeal.by
                                        </li>
                                        <li class="mb-3">
                                            <i class="fas fa-clock text-danger me-2"></i>
                                            <strong>Режим работы:</strong><br />
                                            Пн-Пт: 9:00 - 20:00<br />
                                            Сб-Вс: 10:00 - 18:00
                                        </li>
                                    </ul>
                                    <div class="social-links mt-4">
                                        <h5 class="mb-3">Мы в социальных сетях:</h5>
                                        <a href="#" class="btn btn-outline-danger me-2"><i class="fab fa-instagram text-danger"></i> Instagram</a>
                                        <a href="#" class="btn btn-outline-danger me-2"><i class="fab fa-facebook text-danger"></i> Facebook</a>
                                        <a href="#" class="btn btn-outline-danger"><i class="fab fa-telegram text-danger"></i> Telegram</a>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="map-container" style="height: 400px;">
                                    <iframe 
                                        src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2350.692510629614!2d27.6783!3d53.9433!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x46dbcf0c9c0c3c3f%3A0x0!2z0KPQstC40YHRgtCw0L3QsCDQo9GA0YPQv9C-0YfQutCwLCAyMdCy!5e0!3m2!1sru!2sby!4v1647881234567!5m2!1sru!2sby" 
                                        width="100%" 
                                        height="100%" 
                                        style="border:0;" 
                                        allowfullscreen="" 
                                        loading="lazy"
                                        referrerpolicy="no-referrer-when-downgrade">
                                    </iframe>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

  
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css">
</asp:Content>
