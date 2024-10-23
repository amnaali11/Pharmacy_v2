using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Pharmacy.HttpServices;
using Pharmacy_v2.Models;
using Pharmacy.HttpServices;
using Pharmacy_v2.Repos.Repo_Interfaces;
using Pharmacy_v2.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Pharmacy.Controllers
{
    public class BagController : Controller
    {
        private readonly IBagRepository BagRepository;
        private readonly IOrderRepository OrderRepository;
        private readonly UserManager<ApplicationUser> _userManeger;
        private readonly IHttpServiceAsyncAwait _service;

        public BagController(IBagRepository BagRepo, IOrderRepository orderRepo, UserManager<ApplicationUser> userManager, IHttpServiceAsyncAwait service)
        {
            BagRepository=BagRepo;
            OrderRepository=orderRepo;
            _userManeger  = userManager;
            _service = service;
        }
        public async Task<IActionResult> Index(string? id) //id=user_id
        {

            ApplicationUser? User1 = await _userManeger.FindByNameAsync(User.Identity.Name);
            if (User != null)
            {
                if (_userManeger.IsInRoleAsync(User1, "Locked").Result)
                {
                    return View("Locked");
                }
            }
            Bag? bag = BagRepository.GetByUserId(id);
            if(bag != null)
            {
                List<Order>? list = OrderRepository.GetOrdersInBag(bag.Id);
                bag.Orders = list;
                if (list.Count() == 0)
                {
                    return View("EmptyCart");
                }
            }

            return View("Index",bag);
        }
        public JsonResult AddOrder(Order order, string User_Id)  //id=user_id
        {
            Bag bag = BagRepository.GetByUserId(User_Id);
            order.BagId =bag.Id;
            Order order1 = OrderRepository.GetOrdersInBag(bag.Id).Where(x=>x.Name==order.Name).FirstOrDefault();
            if (order1==null)OrderRepository.Insert(order);
            OrderRepository.Save();
           			
            return Json(new { success =true, message = "Item added to cart" });
        }

        public ActionResult DeleteOrder(int id)      //id=order_id
        {
            Order order = OrderRepository.GetById(id);
            Bag bag = BagRepository.GetById(order.BagId);
            OrderRepository.Delete(order);
            OrderRepository.Save();
            return RedirectToAction("Index", "Bag", new { id = bag.UserId });

        }
        public async Task<IActionResult> Checkout(string id)
        {
            int Total_cost = 0;
            Bag bag = BagRepository.GetByUserId(id);
            if(bag != null)
            {
                List<Order> list = OrderRepository.GetOrdersInBag(bag.Id);
                foreach (var item in list)
                {
                    Total_cost += item.Quantity * (int)item.Cost;
                }
            }
          
            ViewData["Total_cost"] = Total_cost;
            //ViewBag.Total_cost = Total_cost;
            return View("CheckOut");
        }

        public async Task<IActionResult> Confirmation(Data data)
        {

            var Tokenobj = await _service.GetToken();
            var objwithID = await _service.OrderRegistation(Tokenobj, data.Cost);
            var IframToken = await _service.PaymentKey(Tokenobj, objwithID, data);
            //return Redirect("https://accept.paymob.com/api/acceptance/iframes/875210?payment_token=" + IframToken);
            // Include the return URL as a query parameter (assuming Paymob accepts it)
            string Url = $"https://accept.paymob.com/api/acceptance/iframes/875210?payment_token={IframToken}";
            return Redirect(Url);

            //ViewBag.PaymentUrl = $"https://accept.paymob.com/api/acceptance/iframes/875210?payment_token={IframToken}";
            //return View();

        }

        public IActionResult UpdateQuantity(int Id, int Quantity)  //id=>order_id
        {
            Order order = OrderRepository.GetById(Id);
            // Update the quantity
            order.Quantity = Quantity;
            OrderRepository.Save();
            return Json(new { success = true});

        }

    }
}
