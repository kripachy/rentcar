<%@ Page Title="" Language="C#" MasterPageFile="~/view/admin/usermaster.master" AutoEventWireup="true"
    CodeFile="CarDetails.aspx.cs" Inherits="WebApplication1.view.admin.CarDetails" %>

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

            .color-swatch-link {
                display: block;
                padding: 4px;
                border: 2px solid transparent;
                border-radius: 50%;
                transition: all 0.2s ease;
            }

            .color-swatch-link.active {
                border-color: #dc3545;
                box-shadow: 0 0 5px rgba(220, 53, 69, 0.5);
            }

            .color-swatch-item {
                width: 32px;
                height: 32px;
                border: 1px solid #ddd;
                border-radius: 50%;
            }
        </style>

        <div class="container mt-4">
            <div class="mb-4">
                <a href="~/view/admin/carlistt.aspx" runat="server" class="btn btn-outline-danger">
                    <i class="fas fa-arrow-left"></i> Вернуться к списку машин
                </a>
            </div>

            <h1 class="text-center mb-4 text-danger">
                <asp:Label ID="lblCarName" runat="server"></asp:Label>
            </h1>

            <div class="row">
                <!-- Карусель слева -->
                <div class="col-md-7">
                    <div class="carousel-container">
                        <div id="carCarousel" class="carousel slide mb-4" data-bs-ride="carousel">
                            <div class="carousel-indicators">
                                <asp:Repeater ID="rptIndicators" runat="server">
                                    <ItemTemplate>
                                        <button type="button" data-bs-target="#carCarousel"
                                            data-bs-slide-to='<%# Container.ItemIndex %>'
                                            class='<%# Container.ItemIndex == 0 ? "active" : "" %>'
                                            aria-current='<%# Container.ItemIndex == 0 ? "true" : "false" %>'
                                            aria-label='Slide <%# Container.ItemIndex + 1 %>'></button>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="carousel-inner rounded">
                                <asp:Repeater ID="rptImages" runat="server">
                                    <ItemTemplate>
                                        <div class='carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>'>
                                            <img src='<%# Container.DataItem %>' class="d-block w-100 rounded"
                                                alt="Car Image" style="object-fit: cover; height: 450px;" />
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <button class="carousel-control-prev" type="button" data-bs-target="#carCarousel"
                                data-bs-slide="prev">
                                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                            </button>
                            <button class="carousel-control-next" type="button" data-bs-target="#carCarousel"
                                data-bs-slide="next">
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
                                <asp:Literal ID="litSpecs" runat="server"></asp:Literal>
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
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"
                                            TextMode="DateTimeLocal"></asp:TextBox>
                                    </div>
                                    <div class="mb-3">
                                        <label for="txtEndDate" class="form-label">Дата и время конца аренды:</label>
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control"
                                            TextMode="DateTimeLocal"></asp:TextBox>
                                    </div>

                                </div>
                                <div class="col-md-4 text-center">
                                    <div class="price-tag mb-3">
                                        <h4 class="text-danger mb-0">Цена за день: <asp:Label ID="lblPrice"
                                                runat="server" CssClass="font-weight-bold" style="white-space: nowrap;">
                                            </asp:Label> $</h4>
                                    </div>
                                    <div class="color-info mb-3">
                                        <h5 class="mb-2">Выберите цвет:</h5>
                                        <div class="d-flex justify-content-center gap-2 flex-wrap">
                                            <asp:Repeater ID="rptOtherColors" runat="server">
                                                <ItemTemplate>
                                                    <a href='<%# "CarDetails.aspx?plate=" + Eval("CPlateNum") %>'
                                                        class='color-swatch-link <%# Eval("CPlateNum").ToString() == hfCarPlate.Value ? "active" : "" %>'
                                                        title='<%# Eval("Color") %>'>
                                                        <div class="color-swatch-item"
                                                            style='<%# "background-color:" + Eval("Color") %>'>
                                                        </div>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div class="mt-2 text-muted small">
                                            Текущий: <asp:Label ID="lblColorName" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="btnRent" runat="server" Text="Арендовать"
                                        CssClass="btn btn-danger btn-lg w-100" OnClick="btnRent_Click" />
                                </div>
                            </div>
                            <div class="mt-3">
                                <p class="text-muted text-center">
                                    После аренды на вашу почту придет примерный договор. Вам нужно будет подъехать к
                                    нам, чтобы подписать его и забрать машину.
                                </p>
                                <p class="text-muted text-center mt-2">
                                    Пожалуйста, не забудьте взять с собой:
                                </p>
                                <ul class="list-unstyled text-center text-muted">
                                    <li><i class="fas fa-id-card me-1"></i> Паспорт</li>
                                    <li><i class="fas fa-address-card me-1"></i> Водительское удостоверение</li>
                                </ul>
                            </div>
                            <asp:Label ID="lblRentalMessage" runat="server" CssClass="d-block text-center mt-2"
                                Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hfCarPlate" runat="server" />
        <asp:HiddenField ID="hfCarName" runat="server" />

        <script>
            document.addEventListener('DOMContentLoaded', function () {
                const now = new Date();
                now.setSeconds(0, 0);
                const minDateTime = now.toISOString().slice(0, 16);

                const startDateInput = document.getElementById('<%= txtStartDate.ClientID %>');
                const endDateInput = document.getElementById('<%= txtEndDate.ClientID %>');
                const btnRent = document.getElementById('<%= btnRent.ClientID %>');
                const lblPrice = document.getElementById('<%= lblPrice.ClientID %>');
                const hfCarName = document.getElementById('<%= hfCarName.ClientID %>');

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
                    btnRent.addEventListener('click', function (e) {
                        if (!startDateInput.value || !endDateInput.value) {
                            e.preventDefault();
                            alert('Пожалуйста, выберите даты начала и конца аренды');
                            return false;
                        }

                        const startDate = new Date(startDateInput.value);
                        const endDate = new Date(endDateInput.value);
                        const rentalDuration = endDate - startDate;
                        const days = Math.ceil(rentalDuration / (1000 * 60 * 60 * 24));
                        const pricePerDay = parseFloat(lblPrice.textContent);
                        const totalPrice = days * pricePerDay;
                        const carName = hfCarName ? hfCarName.value : "Unknown";

                        const message = `Вы уверены, что хотите арендовать ${carName}?\n\n` +
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

                // Update end date when start date changes
                if (startDateInput && endDateInput) {
                    startDateInput.addEventListener('change', function () {
                        if (startDateInput.value) {
                            const startDate = new Date(startDateInput.value);
                            const endDate = new Date(startDate.getTime() + 24 * 60 * 60 * 1000);
                            endDateInput.value = endDate.toISOString().slice(0, 16);
                            endDateInput.min = endDate.toISOString().slice(0, 16);
                        } else {
                            endDateInput.value = '';
                            endDateInput.min = minDateTime;
                        }
                    });
                }

                // Initialize carousel
                var carousel = new bootstrap.Carousel(document.getElementById('carCarousel'), {
                    interval: 5000,
                    wrap: true
                });
            });
        </script>
    </asp:Content>