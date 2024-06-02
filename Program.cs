// Разработка web-приложения на C# (семинары)
// Урок 2. Работа с данными (CSV + статика), маппинг и кэширование
// Доработайте контроллер, реализовав в нем метод возврата CSV-файла с товарами.
// Доработайте контроллер, реализовав статичный файл со статистикой работы кэш. Сделайте его доступным по ссылке.
// Перенесите строку подключения для работы с базой данных в конфигурационный файл приложения.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly string _connectionString;

        public ProductController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            List<Product> products = GetProductsFromDatabase();

            // Конвертируем список товаров в CSV файл
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("Id,Name,Price");
            foreach (var product in products)
            {
                csv.AppendLine($"{product.Id},{product.Name},{product.Price}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "products.csv");
        }

        private List<Product> GetProductsFromDatabase()
        {
             _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";
        }

        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            // Чтение статичного файла с информацией о кэше
            string cacheStats = System.IO.File.ReadAllText("cache_statistics.txt");
            return Content(cacheStats, "text/plain");
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
