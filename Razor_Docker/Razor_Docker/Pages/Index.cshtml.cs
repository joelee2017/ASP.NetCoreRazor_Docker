using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Razor_Docker.Models;
using System.Collections.Generic;
using System.Linq;

namespace Razor_Docker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IProductRepository _repository;
        private readonly IConfiguration _config;
        public string Message { get; set; }
        public List<Product> Products { get; set; }

        /// <summary>
        /// 服務器的名稱
        /// </summary>
        public string Hostname { get; set; }
        public string DBHOST { get; set; }
        public string DBPORT { get; set; }
        public string DBPASSWORD { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IProductRepository repository, IConfiguration config)
        {
            _logger = logger;
            _repository = repository;
            _config = config;
        }
        public void OnGet()
        {
            Message = _config["MESSAGE"] ?? "深入浅出 ASP.NET Core 與 Docker";

            Products = _repository.Products.ToList();


            Hostname = _config["HOSTNAME"];
            DBHOST = _config["DBHOST"] ?? "localhost";
            DBPORT = _config["DBPORT"] ?? "3306";
            DBPASSWORD = _config["DBPASSWORD"] ?? "bb123456";
        }
    }


}
