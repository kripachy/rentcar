<%@ Page Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true" CodeBehind="rentauto.aspx.cs" Inherits="WebApplication1.view.admin.rentauto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        body { font-family: 'Segoe UI', sans-serif; }
        .card-custom { border-radius: 16px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }
        .car-image-wrapper {
            position: relative;
            margin: 20px 0;
            min-height: 400px;
            display: flex;
            align-items: center;
            justify-content: center;
            overflow: hidden;
            background: #f8f9fa;
            border-radius: 12px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        
        .carousel-container {
            position: relative;
            width: 100%;
            height: 400px;
            overflow: hidden;
        }
        
        .carousel-slide {
            position: absolute;
            width: 100%;
            height: 100%;
            opacity: 0;
            transition: opacity 0.5s ease-in-out;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        
        .carousel-slide.active {
            opacity: 1;
            z-index: 1;
        }
        
        .carousel-image {
            max-width: 100%;
            max-height: 100%;
            object-fit: contain;
            border-radius: 8px;
        }
        
        .default-image {
            position: absolute;
            width: 100%;
            height: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 2;
            background: #fff;
        }
        
        .nav-button {
            position: absolute;
            top: 50%;
            transform: translateY(-50%);
            background: rgba(220, 53, 69, 0.8);
            color: white;
            border: none;
            width: 40px;
            height: 40px;
            border-radius: 50%;
            cursor: pointer;
            transition: all 0.3s ease;
            display: none;
            z-index: 10;
            font-size: 20px;
            line-height: 1;
            padding: 0;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        
        .nav-button:hover {
            background: rgba(220, 53, 69, 1);
            transform: translateY(-50%) scale(1.1);
        }
        
        .nav-button.prev {
            left: 15px;
        }
        
        .nav-button.next {
            right: 15px;
        }

        .carousel-indicators {
            position: absolute;
            bottom: 15px;
            left: 50%;
            transform: translateX(-50%);
            display: flex;
            gap: 8px;
            z-index: 10;
        }

        .carousel-indicator {
            width: 10px;
            height: 10px;
            border-radius: 50%;
            background: rgba(255, 255, 255, 0.5);
            cursor: pointer;
            transition: all 0.3s ease;
        }

        .carousel-indicator.active {
            background: #dc3545;
            transform: scale(1.2);
        }

        .color-circles-container {
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
            margin-top: 10px;
            padding: 10px;
        }
        
        .color-circle {
            width: 32px;
            height: 32px;
            border-radius: 50%;
            border: 2px solid #ccc;
            cursor: pointer;
            transition: all 0.3s ease;
            opacity: 0.3;
            margin: 5px;
            position: relative;
            background-color: var(--color);
            padding: 0;
        }
        
        .color-circle:hover {
            transform: scale(1.1);
            box-shadow: 0 0 8px rgba(220, 53, 69, 0.7);
        }
        
        .color-circle.selected {
            opacity: 1;
            border-width: 3px;
            border-color: #dc3545;
            box-shadow: 0 0 8px rgba(220, 53, 69, 0.7);
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" AsyncPostBackTimeout="90">
        <Scripts>
            <asp:ScriptReference Name="MicrosoftAjax.js" />
            <asp:ScriptReference Name="MicrosoftAjaxWebForms.js" />
        </Scripts>
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnPrevImage" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnNextImage" EventName="Click" />
        </Triggers>
        <ContentTemplate>

            <div class="card-custom p-4 my-5">
                <div class="row align-items-center">
                    <div class="col-lg-6">
                        <h3>Выберите параметры</h3>

                        <div class="form-group mb-3">
                            <label class="form-label">Марка:</label>
                            <asp:DropDownList ID="ddlBrand" runat="server" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged" 
                                CssClass="form-select">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group mb-3">
                            <label class="form-label">Модель:</label>
                            <asp:Label ID="lblModel" runat="server" CssClass="form-control-plaintext fw-bold fs-5"></asp:Label>
                        </div>

                        <div class="form-group mb-3">
                            <label class="form-label">Цена за аренду:</label>
                            <asp:Label ID="lblPrice" runat="server" CssClass="form-control-plaintext text-danger fw-bold fs-4"></asp:Label>
                        </div>

                        <div class="form-group mb-3">
                            <label class="form-label">Цвет:</label>
                            <asp:DropDownList ID="ddlColor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlColor_SelectedIndexChanged" CssClass="form-select"></asp:DropDownList>
                        </div>

                        <asp:HiddenField ID="hfSelectedColor" runat="server" />
                        
                        <div class="form-group mb-3">
                            <asp:Button ID="btnRent" runat="server" Text="Арендовать" 
                                CssClass="btn btn-danger mt-3" OnClick="btnRent_Click" />
                        </div>
                    </div>

                    <div class="col-lg-6">
                        <div class="car-image-wrapper">
                            <div class="carousel-container">
                                <div id="carouselSlides" class="carousel-slides">
                                    <!-- Слайды будут добавлены динамически -->
                                </div>
                                <div class="carousel-indicators" id="carouselIndicators">
                                    <!-- Индикаторы будут добавлены динамически -->
                                </div>
                            </div>
                            <asp:Button ID="btnPrevImage" runat="server" Text="❮" CssClass="nav-button prev" OnClick="btnPrevImage_Click" />
                            <asp:Button ID="btnNextImage" runat="server" Text="❯" CssClass="nav-button next" OnClick="btnNextImage_Click" />
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        // Глобальные переменные для хранения состояния
        let currentSlide = 0;
        let slides = [];
        let indicators = [];
        let isProcessingColorChange = false;
        let pendingColorChange = null;

        // Функция для безопасного выполнения асинхронных операций
        function safeAsyncOperation(operation) {
            return new Promise((resolve, reject) => {
                try {
                    const result = operation();
                    if (result instanceof Promise) {
                        result.then(resolve).catch(reject);
                    } else {
                        resolve(result);
                    }
                } catch (error) {
                    reject(error);
                }
            });
        }

        function initializeCarousel(imageUrls) {
            try {
                const carouselSlides = document.getElementById('carouselSlides');
                const carouselIndicators = document.getElementById('carouselIndicators');
                
                if (!carouselSlides || !carouselIndicators) {
                    console.error('Не найдены элементы карусели');
                    return;
                }
                
                // Очищаем существующие слайды и индикаторы
                carouselSlides.innerHTML = '';
                carouselIndicators.innerHTML = '';
                slides = [];
                indicators = [];
                
                // Создаем слайды
                imageUrls.forEach((url, index) => {
                    const slide = document.createElement('div');
                    slide.className = 'carousel-slide';
                    const img = document.createElement('img');
                    img.src = url;
                    img.className = 'carousel-image';
                    img.alt = `Car image ${index + 1}`;
                    slide.appendChild(img);
                    carouselSlides.appendChild(slide);
                    slides.push(slide);
                    
                    // Создаем индикатор
                    const indicator = document.createElement('div');
                    indicator.className = 'carousel-indicator';
                    indicator.addEventListener('click', () => goToSlide(index));
                    carouselIndicators.appendChild(indicator);
                    indicators.push(indicator);
                });
                
                // Показываем первый слайд
                if (slides.length > 0) {
                    showSlide(0);
                }
            } catch (error) {
                console.error('Ошибка при инициализации карусели:', error);
            }
        }

        function showSlide(index) {
            // Скрываем все слайды
            slides.forEach(slide => {
                slide.classList.remove('active');
            });
            
            // Убираем активный класс со всех индикаторов
            indicators.forEach(indicator => {
                indicator.classList.remove('active');
            });
            
            // Показываем выбранный слайд
            if (slides[index]) {
                slides[index].classList.add('active');
                indicators[index].classList.add('active');
                currentSlide = index;
            }
        }

        function goToSlide(index) {
            showSlide(index);
        }

        function nextSlide() {
            const nextIndex = (currentSlide + 1) % slides.length;
            showSlide(nextIndex);
        }

        function prevSlide() {
            const prevIndex = (currentSlide - 1 + slides.length) % slides.length;
            showSlide(prevIndex);
        }

        // Добавляем обработчики для кнопок навигации
        document.getElementById('<%= btnPrevImage.ClientID %>').addEventListener('click', function(e) {
            e.preventDefault();
            prevSlide();
        });

        document.getElementById('<%= btnNextImage.ClientID %>').addEventListener('click', function(e) {
            e.preventDefault();
            nextSlide();
        });

        // Инициализация при загрузке страницы
        document.addEventListener('DOMContentLoaded', function() {
            console.log('Страница загружена');
            
            // Проверяем, есть ли выбранный бренд
            const brandSelect = document.getElementById('<%= ddlBrand.ClientID %>');
            if (brandSelect && brandSelect.value) {
                console.log('Выбранный бренд при загрузке:', brandSelect.value);
                // Если есть, обновляем доступные цвета
                const availableColors = [];
                document.querySelectorAll('.color-circle.selected').forEach(circle => {
                    availableColors.push(circle.getAttribute('data-color'));
                });
                updateAvailableColors(availableColors);
            }
        });

        // Добавляем обработчик для завершения асинхронного обновления
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function(sender, args) {
            if (args.get_error()) {
                console.error('Ошибка при обновлении страницы:', args.get_error().message);
                args.set_errorHandled(true);
            } else {
                console.log('Страница успешно обновлена');
                // Переинициализируем обработчики после обновления
                initializeCarousel([]);
            }
        });
    </script>

</asp:Content>
