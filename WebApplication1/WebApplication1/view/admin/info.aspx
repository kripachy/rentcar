<%@ Page Title="О нас" Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true" CodeBehind="info.aspx.cs" Inherits="WebApplication1.view.admin.info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        * {
            box-sizing: border-box;
        }

        .info-page-wrapper {
            padding: 0;
            background-color: #fff;
            overflow-x: hidden;
        }

        .hero-section {
            background: #000;
            padding: 60px 0 50px;
            text-align: center;
            position: relative;
            overflow: hidden;
        }

        .hero-section::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: linear-gradient(135deg, rgba(0, 188, 212, 0.05) 0%, transparent 100%);
            pointer-events: none;
        }

        .hero-section h1 {
            color: #00bcd4;
            font-size: 4rem;
            font-weight: 800;
            margin-bottom: 25px;
            letter-spacing: -1px;
            position: relative;
            z-index: 1;
        }

        .hero-section p {
            color: #fff;
            font-size: 1.4rem;
            max-width: 750px;
            margin: 0 auto;
            line-height: 1.6;
            opacity: 0.9;
            position: relative;
            z-index: 1;
        }

        .content-section {
            margin-bottom: 50px;
            padding: 40px 15px 0;
        }

        .section-title {
            color: #000;
            font-size: 2.5rem;
            font-weight: 700;
            text-align: center;
            margin-bottom: 40px;
            position: relative;
            padding-bottom: 20px;
            letter-spacing: -0.5px;
        }

        .section-title::after {
            content: '';
            position: absolute;
            bottom: 0;
            left: 50%;
            transform: translateX(-50%);
            width: 100px;
            height: 5px;
            background: linear-gradient(90deg, transparent, #00bcd4, transparent);
        }

        .info-cards-wrapper {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(450px, 1fr));
            gap: 30px;
            margin-bottom: 50px;
            margin-top: 20px;
        }

        .info-card {
            background: #fff;
            border: 1px solid #e8e8e8;
            border-radius: 16px;
            padding: 35px 30px;
            height: 100%;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            position: relative;
            overflow: hidden;
            margin-bottom: 20px;
        }

        .info-card::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 4px;
            background: #00bcd4;
            transform: scaleX(0);
            transform-origin: left;
            transition: transform 0.4s ease;
        }

        .info-card:hover {
            border-color: #00bcd4;
            box-shadow: 0 8px 25px rgba(0, 188, 212, 0.15);
            transform: translateY(-5px);
        }

        .info-card:hover::before {
            transform: scaleX(1);
        }

        .info-card-icon {
            color: #00bcd4;
            font-size: 3rem;
            margin-bottom: 20px;
            display: block;
            transition: transform 0.3s ease;
        }

        .info-card:hover .info-card-icon {
            transform: rotate(5deg);
        }

        .info-card h3 {
            color: #000;
            font-size: 1.7rem;
            font-weight: 700;
            margin-bottom: 20px;
            letter-spacing: -0.3px;
        }

        .info-card p {
            color: #555;
            font-size: 1.1rem;
            line-height: 1.8;
            margin: 0;
        }

        .features-section {
            background: linear-gradient(to bottom, #fafafa 0%, #fff 100%);
            padding: 50px 30px;
            border-radius: 20px;
            margin: 40px 0;
        }

        .features-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 25px;
            margin-top: 30px;
        }

        .feature-item {
            background: #fff !important;
            border: 2px solid #000 !important;
            border-radius: 16px;
            padding: 35px 25px;
            text-align: center;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            position: relative;
            overflow: hidden;
            margin-bottom: 20px;
            cursor: pointer;
        }

        .feature-item:hover {
            background: #00bcd4 !important;
            border-color: #00bcd4 !important;
            transform: translateY(-8px);
            box-shadow: 0 12px 35px rgba(0, 188, 212, 0.2);
            z-index: 10;
        }

        .feature-item:hover .feature-icon,
        .feature-item:hover h4,
        .feature-item:hover p {
            color: #000 !important;
        }

        .feature-icon {
            color: #00bcd4 !important;
            font-size: 3.5rem;
            margin-bottom: 20px;
            transition: all 0.3s ease;
            display: inline-block;
        }

        .feature-item h4 {
            color: #000 !important;
            font-size: 1.4rem;
            font-weight: 700;
            margin-bottom: 18px;
            transition: all 0.4s ease;
            letter-spacing: -0.2px;
        }

        .feature-item p {
            color: #444 !important;
            font-size: 1.05rem;
            margin: 0;
            transition: all 0.4s ease;
            line-height: 1.6;
        }

        .benefits-section {
            background: #fff;
            border: 2px solid #000;
            border-radius: 20px;
            padding: 40px 35px;
            margin: 50px 0;
            position: relative;
        }

        .benefits-section::before {
            content: '';
            position: absolute;
            top: -2px;
            left: -2px;
            right: -2px;
            bottom: -2px;
            background: linear-gradient(45deg, #00bcd4, transparent, #00bcd4);
            border-radius: 20px;
            z-index: -1;
            opacity: 0;
            transition: opacity 0.4s ease;
        }

        .benefits-section:hover::before {
            opacity: 0.1;
        }

        .benefits-list {
            list-style: none;
            padding: 0;
            margin: 0;
        }

        .benefits-list li {
            padding: 18px 0;
            border-bottom: 1px solid #e8e8e8;
            color: #000;
            font-size: 1.1rem;
            transition: all 0.3s ease;
            display: flex;
            align-items: center;
            position: relative;
        }

        .benefits-list li:last-child {
            border-bottom: none;
        }

        .benefits-list li::before {
            content: '';
            position: absolute;
            left: 0;
            top: 50%;
            transform: translateY(-50%);
            width: 0;
            height: 2px;
            background: #00bcd4;
            transition: width 0.3s ease;
        }

        .benefits-list li:hover {
            padding-left: 20px;
            color: #00bcd4;
        }

        .benefits-list li:hover::before {
            width: 15px;
        }

        .benefits-list i {
            color: #00bcd4;
            font-size: 1.4rem;
            margin-right: 25px;
            min-width: 30px;
            transition: transform 0.3s ease;
        }

        .benefits-list li:hover i {
            transform: scale(1.1);
        }

        .contacts-section {
            background: #000;
            border-radius: 24px;
            padding: 50px 35px;
            margin: 50px 0 40px;
            position: relative;
            overflow: hidden;
        }

        .contacts-section::before {
            content: '';
            position: absolute;
            top: 0;
            right: 0;
            width: 300px;
            height: 300px;
            background: radial-gradient(circle, rgba(0, 188, 212, 0.1) 0%, transparent 70%);
            border-radius: 50%;
        }

        .contacts-section h2 {
            color: #00bcd4;
            font-size: 2.5rem;
            font-weight: 800;
            text-align: center;
            margin-bottom: 40px;
            position: relative;
            z-index: 1;
            letter-spacing: -1px;
        }

        .contact-card {
            background: #fff;
            border-radius: 20px;
            padding: 35px 30px;
            height: 100%;
            box-shadow: 0 8px 30px rgba(0, 0, 0, 0.1);
            transition: transform 0.3s ease;
            margin-bottom: 20px;
        }

        .contact-card:hover {
            transform: translateY(-3px);
        }

        .contact-card h4 {
            color: #000;
            font-size: 1.7rem;
            font-weight: 700;
            margin-bottom: 35px;
            letter-spacing: -0.3px;
        }

        .contact-item {
            padding: 15px 0;
            border-bottom: 1px solid #f0f0f0;
            display: flex;
            align-items: flex-start;
            transition: all 0.3s ease;
        }

        .contact-item:last-child {
            border-bottom: none;
        }

        .contact-item:hover {
            padding-left: 10px;
        }

        .contact-item i {
            color: #00bcd4;
            font-size: 1.5rem;
            margin-right: 20px;
            margin-top: 5px;
            min-width: 30px;
            transition: transform 0.3s ease;
        }

        .contact-item:hover i {
            transform: scale(1.1);
        }

        .contact-item strong {
            color: #000;
            margin-right: 10px;
            font-weight: 600;
        }

        .contact-item span {
            color: #444;
            line-height: 1.6;
        }

        .social-buttons {
            margin-top: 30px;
            padding-top: 25px;
            border-top: 2px solid #f0f0f0;
        }

        .social-buttons h5 {
            color: #000;
            font-size: 1.3rem;
            margin-bottom: 25px;
            font-weight: 600;
        }

        .btn-social {
            display: inline-block;
            padding: 14px 28px;
            margin: 8px 8px 8px 0;
            border: 2px solid #000;
            border-radius: 10px;
            color: #000;
            text-decoration: none;
            font-weight: 600;
            font-size: 0.95rem;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            background: transparent;
            position: relative;
            overflow: hidden;
        }

        .btn-social::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: #00bcd4;
            transition: left 0.3s ease;
            z-index: -1;
        }

        .btn-social:hover {
            border-color: #00bcd4;
            color: #000;
            transform: translateY(-3px);
            box-shadow: 0 8px 20px rgba(0, 188, 212, 0.3);
        }

        .btn-social:hover::before {
            left: 0;
        }

        .btn-social i {
            margin-right: 10px;
        }

        .map-wrapper {
            border-radius: 20px;
            overflow: hidden;
            border: 3px solid #00bcd4;
            height: 350px !important;
            min-height: 350px !important;
            max-height: 350px !important;
            box-shadow: 0 10px 35px rgba(0, 188, 212, 0.15);
            transition: all 0.3s ease;
            position: relative;
        }

        #yandex-map {
            width: 100% !important;
            height: 100% !important;
            min-height: 350px !important;
        }

        .map-wrapper::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            border: 2px solid #00bcd4;
            border-radius: 20px;
            opacity: 0;
            transition: opacity 0.3s ease;
            pointer-events: none;
        }

        .map-wrapper:hover {
            box-shadow: 0 15px 45px rgba(0, 188, 212, 0.25);
            transform: translateY(-3px);
        }

        .map-wrapper:hover::before {
            opacity: 0.5;
        }

        .map-wrapper iframe {
            width: 100%;
            height: 100%;
            border: 0;
        }

        .footer {
            background-color: #000 !important;
            color: #fff !important;
            padding: 20px 0 !important;
            margin-top: 50px !important;
            width: 100% !important;
        }

        .footer p {
            color: #fff !important;
            margin: 0 !important;
        }

        @media (max-width: 992px) {
            .info-cards-wrapper {
                grid-template-columns: 1fr;
            }
        }

        @media (max-width: 768px) {
            .hero-section {
                padding: 60px 0 50px;
            }

            .hero-section h1 {
                font-size: 2.5rem;
            }

            .hero-section p {
                font-size: 1.1rem;
            }

            .section-title {
                font-size: 2rem;
            }

            .features-grid {
                grid-template-columns: 1fr;
                gap: 25px;
            }

            .features-section {
                padding: 50px 25px;
            }

            .benefits-section {
                padding: 40px 30px;
            }

            .contacts-section {
                padding: 50px 30px;
            }

            .contact-card {
                padding: 35px 30px;
                margin-bottom: 30px;
            }

            .info-card {
                padding: 40px 30px;
            }
        }
    </style>

    <div class="info-page-wrapper">
        <div class="hero-section">
            <div class="container">
                <h1>О компании WheelDeal</h1>
                <p>Ваш надежный партнер в мире премиальной аренды автомобилей</p>
            </div>
        </div>

        <div class="container">
            <div class="content-section">
                <div class="info-cards-wrapper">
                    <div class="info-card">
                        <i class="fas fa-history info-card-icon"></i>
                        <h3>Наша история</h3>
                        <p>WheelDeal начала свой путь с мечты сделать премиальную мобильность доступной каждому. С момента основания мы постоянно расширяем автопарк, добавляя только проверенные временем бренды и модели. Наш опыт и внимание к деталям позволяют предлагать клиентам не просто автомобиль, а незабываемые впечатления от вождения.</p>
                    </div>
                    <div class="info-card">
                        <i class="fas fa-bullseye info-card-icon"></i>
                        <h3>Наша миссия</h3>
                        <p>Мы стремимся сделать аренду премиальных автомобилей доступной и удобной для каждого клиента, обеспечивая высочайший уровень сервиса и безопасность.</p>
                    </div>
                </div>
            </div>

            <div class="features-section">
                <h2 class="section-title">Наши преимущества</h2>
                <div class="features-grid">
                    <div class="feature-item">
                        <i class="fas fa-car feature-icon"></i>
                        <h4>Премиальный автопарк</h4>
                        <p>Только лучшие модели от ведущих производителей</p>
                    </div>
                    <div class="feature-item">
                        <i class="fas fa-shield-alt feature-icon"></i>
                        <h4>Безопасность</h4>
                        <p>Все автомобили проходят регулярное техническое обслуживание</p>
                    </div>
                    <div class="feature-item">
                        <i class="fas fa-headset feature-icon"></i>
                        <h4>Поддержка 24/7</h4>
                        <p>Наша служба поддержки всегда готова помочь вам</p>
                    </div>
                </div>
            </div>

            <div class="benefits-section">
                <h2 class="section-title">Почему выбирают нас?</h2>
                <ul class="benefits-list">
                    <li><i class="fas fa-check-circle"></i> Большой выбор премиальных автомобилей</li>
                    <li><i class="fas fa-check-circle"></i> Гибкая система скидок и бонусов</li>
                    <li><i class="fas fa-check-circle"></i> Простая и быстрая процедура аренды</li>
                    <li><i class="fas fa-check-circle"></i> Профессиональная команда специалистов</li>
                    <li><i class="fas fa-check-circle"></i> Конкурентные цены на рынке</li>
                </ul>
            </div>

            <div class="contacts-section">
                <h2>Наши контакты</h2>
                <div class="row">
                    <div class="col-md-6 mb-4">
                        <div class="contact-card">
                            <h4>Свяжитесь с нами</h4>
                            <div class="contact-item">
                                <i class="fas fa-map-marker-alt"></i>
                                <div>
                                    <strong>Адрес:</strong>
                                    <span>ул. Уручская, 21, Минск, Беларусь</span>
                                </div>
                            </div>
                            <div class="contact-item">
                                <i class="fas fa-phone"></i>
                                <div>
                                    <strong>Телефон:</strong>
                                    <span>+375 (29) 123-45-67</span>
                                </div>
                            </div>
                            <div class="contact-item">
                                <i class="fas fa-envelope"></i>
                                <div>
                                    <strong>Email:</strong>
                                    <span>info@wheeldeal.by</span>
                                </div>
                            </div>
                            <div class="contact-item">
                                <i class="fas fa-clock"></i>
                                <div>
                                    <strong>Режим работы:</strong><br />
                                    <span>Пн-Пт: 9:00 - 20:00<br />Сб-Вс: 10:00 - 18:00</span>
                                </div>
                            </div>
                            <div class="social-buttons">
                                <h5>Мы в социальных сетях:</h5>
                                <a href="#" class="btn-social"><i class="fab fa-instagram"></i> Instagram</a>
                                <a href="#" class="btn-social"><i class="fab fa-facebook"></i> Facebook</a>
                                <a href="#" class="btn-social"><i class="fab fa-telegram"></i> Telegram</a>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="map-wrapper">
                            <iframe src="https://www.google.com/maps?q=53.955490,27.691550&hl=ru&z=16&output=embed" 
                                    width="100%" 
                                    height="100%" 
                                    frameborder="0" 
                                    style="border:0;" 
                                    allowfullscreen="" 
                                    loading="lazy" 
                                    referrerpolicy="no-referrer-when-downgrade"></iframe>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css">
</asp:Content>
