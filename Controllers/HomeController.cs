using Microsoft.AspNetCore.Mvc;
using ServerSidePagination.Models;
using System.Diagnostics;

namespace ServerSidePagination.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("home/order")]
        public IActionResult Order()
        {
            ServerSidePaginationContext db = new();
            var order = db.Orders.ToList();

            return View(order);
        }

        [HttpPost]
        [Route("home/orderlist")]
        public IActionResult OrderList(string hi)
        {
            ServerSidePaginationContext db = new();

            var pageSize = Convert.ToInt32(HttpContext.Request.Form["length"].FirstOrDefault() ?? "0");
            var skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var draw = Request.Form["draw"].FirstOrDefault();
            var search = Request.Form["search[value]"].FirstOrDefault();
            //var search = Request.Form("draw").FirstOrDefault();

             List<Order> orders = new();
            int recordsFiltered = 0;

            var countOrder = db.Orders.Count();
            if (search != "")
            {
                try
                {
                    var ordersFiltered = db.Orders.Where(p => p.Id.Contains(search.ToLower()) ||
                        p.Status.Contains(search.ToLower()));
                    
                    
                    orders = ordersFiltered.Skip(skip).Take(pageSize).ToList();
                    recordsFiltered = ordersFiltered.Count();
                }catch(Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
            orders = db.Orders.Skip(skip).Take(pageSize).ToList();
                recordsFiltered = countOrder;

            }




            return Json(new
            {
                draw = draw,
                recordsTotal = countOrder,
                recordsFiltered = recordsFiltered,
                data = orders.Select(c => new
                {
                    id = c.Id,
                    status = c.Status,
                    total = c.Total,
                    createdonutc = c.CreatedOnUtc
                }).ToList()
            });
            }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}