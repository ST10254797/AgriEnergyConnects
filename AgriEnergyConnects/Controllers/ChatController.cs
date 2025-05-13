using AgriEnergyConnects.Data;
using AgriEnergyConnects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnects.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _context.ChatMessages
                .Include(m => m.User)
                .OrderByDescending(m => m.SentAt)
                .Take(50)
                .ToListAsync();

            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Send(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return RedirectToAction("Index");

            var user = await _userManager.GetUserAsync(User);
            var newMessage = new ChatMessage
            {
                Message = message,
                UserId = user.Id,
                SentAt = DateTime.UtcNow
            };

            _context.ChatMessages.Add(newMessage);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
