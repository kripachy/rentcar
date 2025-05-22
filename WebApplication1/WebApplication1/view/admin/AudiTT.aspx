<%@ Page Title="Audi TT" Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true" CodeBehind="AudiTT.aspx.cs" Inherits="WebApplication1.view.admin.AudiTT" ContentType="text/html" ResponseEncoding="utf-8" %>

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
        .carousel-container:not(:hover) .carousel-control-prev,
        .carousel-container:not(:hover) .carousel-control-next {
            opacity: 0 !important;
            visibility: hidden !important;
            pointer-events: none !important;
        }
        .color-swatch {
            width: 35px;
            height: 35px;
            border-radius: 50%;
            margin: 0 auto;
            border: 1px solid #ddd;
            position: relative;
        }
        .color-option {
            cursor: pointer;
            padding: 5px;
            border-radius: 5px;
            margin: 2px;
            transition: all 0.3s ease;
            text-align: center;
        }
        .color-option:hover {
            background-color: #f8f9fa;
        }
        .color-option.active {
            border: 2px solid #dc3545;
        }
        .color-option.unavailable {
            opacity: 0.7;
        }
        .color-option.unavailable .color-name {
            color: #dc3545;
            font-weight: bold;
        }
        .color-option.unavailable .availability-status {
            color: #dc3545;
            font-size: 11px;
            font-weight: bold;
            margin-top: 2px;
        }
        .color-name {
            font-size: 12px;
            margin-top: 2px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }
    </style>

    <div class="container mt-4">
        <div class="mb-4">
            <a href="~/view/admin/carlistt.aspx" runat="server" class="btn btn-outline-danger">
                <i class="fas fa-arrow-left"></i> Вернуться к списку машин
            </a>
        </div>

        <h1 class="text-center mb-4 text-danger">Audi TT 2020</h1>
        
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
                            <!-- Images will be dynamically loaded based on selected color -->
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
                            <li class="list-group-item"><strong>Двигатель:</strong> 2.0 TFSI, 4-цилиндровый, турбированный</li>
                            <li class="list-group-item"><strong>Мощность:</strong> 197 л.с. (40 TFSI) / 245 л.с. (45 TFSI)</li>
                            <li class="list-group-item"><strong>Крутящий момент:</strong> 320–370 Н·м</li>
                            <li class="list-group-item"><strong>Привод:</strong> Передний (40 TFSI) / Полный (quattro для 45 TFSI)</li>
                            <li class="list-group-item"><strong>Коробка передач:</strong> 7-ступенчатая роботизированная (S tronic)</li>
                            <li class="list-group-item"><strong>Разгон (0-100 км/ч):</strong> 6.6 сек (40 TFSI) / 5.1 сек (45 TFSI quattro)</li>
                            <li class="list-group-item"><strong>Максимальная скорость:</strong> 250 км/ч (ограничена)</li>
                            <li class="list-group-item"><strong>Габариты:</strong> 4190 x 1832 x 1350 мм</li>
                            <li class="list-group-item"><strong>Колёсная база:</strong> 2505 мм</li>
                            <li class="list-group-item"><strong>Масса:</strong> 1340–1530 кг</li>
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
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="txtStartDate" class="form-label">Дата и время начала аренды:</label>
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="DateTimeLocal"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <label for="txtEndDate" class="form-label">Дата и время конца аренды:</label>
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="DateTimeLocal"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-4 text-center">
                                <div class="price-tag mb-3">
                                    <h4 class="text-danger mb-0">Цена за день: <asp:Label ID="lblPrice" runat="server" CssClass="font-weight-bold" style="white-space: nowrap;"></asp:Label> $</h4>
                                </div>
                                <div class="color-selection mb-3">
                                    <h5 class="mb-3">Выберите цвет:</h5>
                                    <div class="row justify-content-center">
                                        <div class="col-4">
                                            <div class="color-option" data-color="Black" data-available="true">
                                                <div class="color-swatch" style="background-color: #000000;"></div>
                                                <p class="color-name">Черный</p>
                                                <p class="availability-status"></p>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="color-option" data-color="Blue" data-available="true">
                                                <div class="color-swatch" style="background-color: #0000FF;"></div>
                                                <p class="color-name">Синий</p>
                                                <p class="availability-status"></p>
                                            </div>
                                        </div>
                                        <div class="col-4">
                                            <div class="color-option" data-color="Orange" data-available="true">
                                                <div class="color-swatch" style="background-color: #FFA500;"></div>
                                                <p class="color-name">Оранжевый</p>
                                                <p class="availability-status"></p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:HiddenField ID="hdnSelectedColor" runat="server" Value="Black" />
                                <asp:Button ID="btnRent" runat="server" Text="Арендовать" CssClass="btn btn-danger btn-lg w-100" OnClick="btnRent_Click" />
                            </div>
                        </div>
                        <div class="mt-3">
                            <p class="text-muted text-center">
                                После аренды на вашу почту придет примерный договор. Вам нужно будет подъехать к нам, чтобы подписать его и забрать машину.
                            </p>
                            <p class="text-muted text-center mt-2">
                                Пожалуйста, не забудьте взять с собой:
                            </p>
                            <ul class="list-unstyled text-center text-muted">
                                <li><i class="fas fa-id-card me-1"></i> Паспорт</li>
                                <li><i class="fas fa-address-card me-1"></i> Водительское удостоверение</li>
                            </ul>
                        </div>
                        <asp:Label ID="lblRentalMessage" runat="server" CssClass="d-block text-center mt-2" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        document.addEventListener('DOMContentLoaded', function() {
            const now = new Date();
            now.setSeconds(0, 0);
            const minDateTime = now.toISOString().slice(0, 16);

            const startDateInput = document.getElementById('<%= txtStartDate.ClientID %>');
            const endDateInput = document.getElementById('<%= txtEndDate.ClientID %>');
            const btnRent = document.getElementById('<%= btnRent.ClientID %>');
            const hdnSelectedColor = document.getElementById('<%= hdnSelectedColor.ClientID %>');
            const carouselInner = document.querySelector('.carousel-inner');
            const carouselIndicators = document.querySelector('.carousel-indicators');
            let selectedColor = hdnSelectedColor.value;

            // Set only minimum dates, not values
            if (startDateInput) {
                startDateInput.min = minDateTime;
            }
            if (endDateInput) {
                const endDate = new Date(now.getTime() + 24 * 60 * 60 * 1000);
                endDateInput.min = endDate.toISOString().slice(0, 16);
            }

            // Add click handler for rent button
            if (btnRent) {
                btnRent.addEventListener('click', function(e) {
                    if (!startDateInput.value || !endDateInput.value) {
                        e.preventDefault();
                        alert('Пожалуйста, выберите даты начала и конца аренды');
                        return false;
                    }

                    const startDate = new Date(startDateInput.value);
                    const endDate = new Date(endDateInput.value);
                    const rentalDuration = endDate - startDate;
                    const days = Math.ceil(rentalDuration / (1000 * 60 * 60 * 24));
                    const pricePerDay = parseFloat(document.getElementById('<%= lblPrice.ClientID %>').textContent);
                    const totalPrice = days * pricePerDay;

                    const colorNames = {
                        'Black': 'Черный',
                        'Blue': 'Синий',
                        'Orange': 'Оранжевый'
                    };

                    const selectedColorName = colorNames[selectedColor];
                    const message = `Вы уверены, что хотите арендовать Audi TT ${selectedColorName} цвета?\n\n` +
                                  `Период аренды: ${startDate.toLocaleString()} - ${endDate.toLocaleString()}\n` +
                                  `Количество дней: ${days}\n` +
                                  `Стоимость за день: $${pricePerDay}\n` +
                                  `Общая стоимость: $${totalPrice}\n\n` +
                                  `После подтверждения на вашу почту придет договор аренды.`;

                    if (!confirm(message)) {
                        e.preventDefault();
                        return false;
                    }
                });
            }

            // Image sets for each color
            const imageSets = {
                'Black': [
                    '../../colorcars/Audi TT/black/1.jpg',
                    '../../colorcars/Audi TT/black/2.jpeg'
                ],
                'Blue': [
                    '../../colorcars/Audi TT/blue/1.jpg',
                    '../../colorcars/Audi TT/blue/2.jpg',
                    '../../colorcars/Audi TT/blue/3.jpg'
                ],
                'Orange': [
                    '../../colorcars/Audi TT/orange/1.jpg',
                    '../../colorcars/Audi TT/orange/2.jpg',
                    '../../colorcars/Audi TT/orange/3.jpg'
                ]
            };

            // Update carousel with selected color images
            function updateCarousel(color) {
                const images = imageSets[color];
                carouselInner.innerHTML = '';
                carouselIndicators.innerHTML = '';

                images.forEach((src, index) => {
                    // Add indicator
                    const indicator = document.createElement('button');
                    indicator.type = 'button';
                    indicator.setAttribute('data-bs-target', '#carCarousel');
                    indicator.setAttribute('data-bs-slide-to', index.toString());
                    if (index === 0) {
                        indicator.classList.add('active');
                        indicator.setAttribute('aria-current', 'true');
                    }
                    indicator.setAttribute('aria-label', `Slide ${index + 1}`);
                    carouselIndicators.appendChild(indicator);

                    // Add slide
                    const slide = document.createElement('div');
                    slide.className = `carousel-item ${index === 0 ? 'active' : ''}`;
                    slide.innerHTML = `
                        <img src="${src}" class="d-block w-100 rounded" alt="Audi TT ${color} ${index + 1}" 
                             style="object-fit: cover; height: 450px;">
                    `;
                    carouselInner.appendChild(slide);
                });

                // Reinitialize carousel
                const carousel = new bootstrap.Carousel(document.getElementById('carCarousel'), {
                    interval: 5000,
                    wrap: true
                });
            }

            // Color selection
            const colorOptions = document.querySelectorAll('.color-option');
            colorOptions.forEach(option => {
                option.addEventListener('click', function() {
                    const color = this.dataset.color;
                    colorOptions.forEach(opt => opt.classList.remove('active'));
                    this.classList.add('active');
                    selectedColor = color;
                    hdnSelectedColor.value = color;
                    updateCarousel(color);
                    checkAvailability(color);
                });
            });

            // Check color availability
            function checkAvailability(color) {
                fetch(`CheckColorAvailability.aspx?brand=Audi&model=TT&color=${color}`)
                    .then(response => response.json())
                    .then(data => {
                        const option = document.querySelector(`.color-option[data-color="${color}"]`);
                        const statusElement = option.querySelector('.availability-status');
                        
                        if (data.available) {
                            option.classList.remove('unavailable');
                            statusElement.textContent = '';
                            if (btnRent) {
                                btnRent.disabled = false;
                                btnRent.title = '';
                            }
                        } else {
                            option.classList.add('unavailable');
                            statusElement.textContent = 'Нет в наличии';
                            statusElement.style.color = '#dc3545';
                            statusElement.style.fontWeight = 'bold';
                            if (btnRent && option.classList.contains('active')) {
                                btnRent.disabled = true;
                                btnRent.title = 'Этот цвет автомобиля недоступен для аренды';
                            }
                        }
                    })
                    .catch((error) => {
                        console.error('Error checking availability:', error);
                    });
            }

            // Initialize
            updateCarousel(selectedColor);
            checkAvailability(selectedColor);
        });
    </script>
</asp:Content> 