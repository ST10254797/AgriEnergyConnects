using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnects.Data;
using AgriEnergyConnects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AgriEnergyConnects.Models.ViewModels;

namespace AgriEnergyConnects.Controllers
{
    [Authorize]
    public class FarmersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FarmersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Farmers
        public async Task<IActionResult> Index()
        {
            var farmers = await _context.Farmers.Include(f => f.User).ToListAsync();
            return View(farmers);
        }

        // GET: Farmers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        // GET: Farmers/Create
        public async Task<IActionResult> Create()
        {
            // Fetch all users from the database asynchronously
            var users = await _userManager.Users.ToListAsync();

            // Filter users by "Farmer" role in-memory
            var usersInFarmerRole = new List<ApplicationUser>();

            foreach (var user in users)
            {
                // Check if the user is in the "Farmer" role
                if (await _userManager.IsInRoleAsync(user, "Farmer"))
                {
                    usersInFarmerRole.Add(user);
                }
            }

            // Create a model and bind the data to the view
            var model = new FarmerCreateViewModel
            {
                Users = usersInFarmerRole
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.Email
                    }).ToList()
            };

            return View(model);
        }

        // POST: Farmers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FarmerCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new Farmer entity with the selected UserId
                var farmer = new Farmer
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Location = model.Location,
                    UserId = model.UserId // Store the selected UserId
                };

                _context.Add(farmer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Re-populate the Users dropdown if the model is not valid
            model.Users = _context.Users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.Email
            }).ToList();

            return View(model);
        }


        // GET: Farmers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", farmer.UserId);
            return View(farmer);
        }

        // POST: Farmers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,PhoneNumber,Email,Location,UserId")] Farmer farmer)
        {
            if (id != farmer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(farmer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmerExists(farmer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", farmer.UserId);
            return View(farmer);
        }

        // GET: Farmers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        // POST: Farmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer != null)
            {
                _context.Farmers.Remove(farmer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FarmerExists(int id)
        {
            return _context.Farmers.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult CreateNewWithAccount()
        {
            // Populate a list of users for the dropdown in case you want to assign a specific user to the farmer
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewWithAccount(FarmerCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Step 1: Create the Identity User
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Step 2: Assign role
                    await _userManager.AddToRoleAsync(user, "Farmer");

                    // Step 3: Create Farmer profile linked to the newly created user
                    var farmer = new Farmer
                    {
                        FullName = model.FullName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        Location = model.Location,
                        UserId = user.Id // Link the UserId to the Farmer
                    };

                    _context.Farmers.Add(farmer);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Farmer and account created successfully.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // In case of errors, we still need to populate the UserId dropdown
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", model.Email);
            return View(model);
        }
    }
}
