using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TestCRUD.Data;
using TestCRUD.Models;
using TestCRUD.Models.Domain;

namespace TestCRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly TestCRUDDbContext testCRUDDbContext;

        public HomeController(TestCRUDDbContext testCRUDDbContext)
        {
            this.testCRUDDbContext = testCRUDDbContext;
        }
        [HttpGet]
        public IActionResult Index(string numberFilter, DateTime? startDate, DateTime? endDate, int? providerIdFilter)
        {
            IQueryable<Order> query = testCRUDDbContext.Orders
                .Include(o => o.Provider)
                .Include(o => o.OrderItems);

            if (!string.IsNullOrEmpty(numberFilter))
            {
                query = query.Where(o => o.Number.Contains(numberFilter));
            }

            if (startDate.HasValue)
            {
                query = query.Where(o => o.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.Date <= endDate.Value);
            }

            if (providerIdFilter.HasValue)
            {
                query = query.Where(o => o.ProviderId == providerIdFilter.Value);
            }

           

            List<Order> orders = query.ToList();

            return View(orders);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Providers = testCRUDDbContext.Providers.ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddOrderViewModel addOrderRequest)
        {
            var order = new Order()
            {
                Number = addOrderRequest.Number,
                Date = addOrderRequest.Date,
                Provider = addOrderRequest.Provider,
                ProviderId = addOrderRequest.ProviderId,
                OrderItems = new List<OrderItem>()
            };

            order.OrderItems.Add(new OrderItem
            {
                Name = addOrderRequest.OrderItems[0].Name,
                Quantity = addOrderRequest.OrderItems[0].Quantity,
                Unit = addOrderRequest.OrderItems[0].Unit
            });

            await testCRUDDbContext.Orders.AddAsync(order);
            await testCRUDDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(int Id)
        {
            var order = await testCRUDDbContext.Orders
                              .Include(o => o.OrderItems) 
                              .FirstOrDefaultAsync(o => o.Id == Id);

            ViewBag.Providers = testCRUDDbContext.Providers.ToList();

            if (order != null)
            {
                var viewModel = new UpdateOrderViewModel()
                {
                    Id = order.Id,
                    Number = order.Number,
                    Date = order.Date,
                    Provider = order.Provider,
                    ProviderId = order.ProviderId,
                    OrderItems = new List<OrderItem>() 
                };

                foreach (var orderItem in order.OrderItems)
                {
                    var orderItemViewModel = new OrderItem()
                    {
                        Name = orderItem.Name,
                        Quantity = orderItem.Quantity,
                        Unit = orderItem.Unit
                    };

                    viewModel.OrderItems.Add(orderItem);
                }

                return View("View", viewModel);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateOrderViewModel viewModel)
        {
            var order = await testCRUDDbContext.Orders
                              .Include(o => o.OrderItems)
                              .FirstOrDefaultAsync(o => o.Id == viewModel.Id);

            if (order != null)
            {
                order.Number = viewModel.Number;
                order.Date = viewModel.Date;
                order.Provider = viewModel.Provider;
                order.ProviderId = viewModel.ProviderId;

                foreach (var viewModelItem in viewModel.OrderItems)
                {
                    var existingOrderItem = order.OrderItems.FirstOrDefault(oi => oi.Id == viewModelItem.Id);

                    if (existingOrderItem != null)
                    {
                        existingOrderItem.Name = viewModelItem.Name;
                        existingOrderItem.Quantity = viewModelItem.Quantity;
                        existingOrderItem.Unit = viewModelItem.Unit;
                    }
                }

                await testCRUDDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> Delete(UpdateOrderViewModel viewModel)
        {
            var order = await testCRUDDbContext.Orders.FindAsync(viewModel.Id);

            if(viewModel != null)
            {
                testCRUDDbContext.Orders.Remove(order);
                await testCRUDDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}