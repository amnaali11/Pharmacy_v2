using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy_v2.Data;

namespace Pharmacy_v2.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly AppDbContext context;

        public ChatController(AppDbContext _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            return View(context.conversation.ToList());
        }
   


    }
}
