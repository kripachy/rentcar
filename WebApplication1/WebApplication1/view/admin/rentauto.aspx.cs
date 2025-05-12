using System;
using System.Web.UI;
using System.Collections.Generic;

namespace WebApplication1.view.admin
{
    public partial class rentauto : System.Web.UI.Page
    {
        // Словарь для хранения моделей и изображений автомобилей
        private Dictionary<string, string> brandModelMap = new Dictionary<string, string>()
        {
            {"Ford", "Mustang S550"},
            {"Chevrolet", "Camaro"},
            {"Lamborghini", "Huracan"},
            {"Jaguar", "XJ"}
        };

        private Dictionary<string, string> brandImageMap = new Dictionary<string, string>()
        {
            {"Ford", "../../assets/images/cars/Ford_red.png"},
            {"Chevrolet", "../../assets/images/cars/Chevrolet_red.png"},
            {"Lamborghini", "../../assets/images/cars/Lamborghini_red.png"},
            {"Jaguar", "../../assets/images/cars/Jaguar_red.png"}
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblModel.Text = "Выберите марку";
            }
        }

        // Обработчик события при выборе марки
        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedBrand = ddlBrand.SelectedValue;

            if (brandModelMap.ContainsKey(selectedBrand))
            {
                lblModel.Text = brandModelMap[selectedBrand];
                UpdateCarImage();
            }
            else
            {
                lblModel.Text = "Модель недоступна";
                carImage.ImageUrl = "~/assets/images/default-car.png";
            }
        }

        // Обработчик события при выборе цвета
        protected void ddlColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCarImage();
        }

        // Метод для обновления изображения автомобиля
        private void UpdateCarImage()
        {
            string selectedBrand = ddlBrand.SelectedValue;
            string selectedColor = ddlColor.SelectedValue;
            string imageUrl = "~/assets/images/default-car.png";

            if (brandImageMap.ContainsKey(selectedBrand))
            {
                imageUrl = brandImageMap[selectedBrand];
                imageUrl = imageUrl.Replace("_red", "_" + selectedColor); // Меняем цвет в URL
            }

            carImage.ImageUrl = imageUrl;
        }

        // Обработчик аренды
        protected void btnRent_Click(object sender, EventArgs e)
        {
            string brand = ddlBrand.SelectedValue;
            string model = lblModel.Text;
            string color = ddlColor.SelectedValue;
            string startDate = txtStartDate.Text;
            string endDate = txtEndDate.Text;

            // Проверяем заполненность полей
            if (string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                carDetails.InnerText = "Заполните все поля!";
                return;
            }

            // Формируем документ аренды
            carDetails.InnerText = $"Вы выбрали автомобиль {brand} {model}, цвет {color}. " +
                                    $"Дата аренды: с {startDate} по {endDate}.";
        }
    }
}
