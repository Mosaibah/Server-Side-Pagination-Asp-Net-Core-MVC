using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerSidePagination.Models;
using ServerSidePagination.Models.ViewModel;
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
        public async Task<IActionResult> Order()
        {
            ServerSidePaginationContext db = new();

            var statusRaw = await db.Orders.Select(c => c.Status).Distinct().ToListAsync();
            List<SelectListItem> status = statusRaw.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a,
                    Value = a,
                    Selected = false
                };
            });

            ViewBag.StatusList = status;

            return View();
        }

        [HttpPost]
        [Route("home/orderlist")]
        public async Task<IActionResult> OrderList(SearchVM model)
        {
            ServerSidePaginationContext db = new();

            var pageSize = Convert.ToInt32(HttpContext.Request.Form["length"].FirstOrDefault() ?? "0");
            var skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var draw = Request.Form["draw"].FirstOrDefault();

            List<Order> orders = new();
            int recordsFiltered = 0;

            var countOrder = await  db.Orders.CountAsync();
            var ordersFiltered = db.Orders.AsQueryable();
                
            if(model.Id is not null)
            {
            ordersFiltered = ordersFiltered.Where(c => c.Id == model.Id);
            }

            //if(model.Status is not null)
            //{
            //    ordersFiltered = ordersFiltered.Where()
            //}
                    
            orders = await ordersFiltered.Skip(skip).Take(pageSize).ToListAsync();
            recordsFiltered = await ordersFiltered.CountAsync();
               


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