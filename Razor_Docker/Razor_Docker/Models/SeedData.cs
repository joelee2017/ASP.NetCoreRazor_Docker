using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Razor_Docker.Models
{
    /// <summary>
    /// 種子數據
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// 初始化數據庫和種子數據
        /// </summary>
        /// <param name="dbcontext"></param>

        public static IApplicationBuilder UseDataInitializer(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {

                var dbcontext = scope.ServiceProvider.GetService<ProductDbContext>();
                System.Console.WriteLine("開始執行遷移數據庫...");
                dbcontext.Database.Migrate();
                System.Console.WriteLine("數據庫遷移完成...");
                if (!dbcontext.Products.Any())
                {
                    System.Console.WriteLine("開始創建種子數據中...");
                    dbcontext.Products.AddRange(
                    new Product("空调", "家用電器", 2750),
                    new Product("電視機", "家用電器", 2448.95m),
                    new Product("洗衣機 ", "家用電器", 1449.50m),
                    new Product("油烟機 ", "家用電器", 3454.95m),
                    new Product("冰箱", "家用電器", 9500),
                    new Product("猪肉 ", "食品", 36),
                    new Product("牛肉 ", "食品", 49.95m),
                    new Product("雞肉 ", "食品", 22),
                    new Product("鴨肉", "食品", 18)
                    );
                    dbcontext.SaveChanges();
                }
                else
                {
                    System.Console.WriteLine("無需創建種子數據...");
                }


            }
            return builder;

        }

    }
}
