
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Pharmacy_v2.Data;
using Pharmacy_v2.Models;
using Pharmacy_v2.SignalR_Database;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;

namespace Pharmacy_v2.Hubs
{
    public class NewChatHub:Hub
    {
        private readonly AppDbContext context;

        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }



        public NewChatHub(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext _context)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            context = _context;

        }
       [HubMethodName("login")]
       public async Task Login(string name)
       {
           var user = await UserManager.FindByNameAsync(name);
           if (user != null)
           {
               
                    context.UserConnections.Add(new UserConnection()
                    {
                        ConnectionId = Context.ConnectionId,
                        UserId = user.Id
                    });

                    context.SaveChanges();
                    await Clients.Caller.SendAsync("loggedin", user.Id);
                   return; // Exit the method after successful login
               }
           }

           // Login failed, send a message to the client
       
      
    }
}
