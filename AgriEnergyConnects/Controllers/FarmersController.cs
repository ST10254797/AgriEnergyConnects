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
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Get all users with "Farmer" role
            var farmers = await _userManager.GetUsersInRoleAsync("Farmer");

            var model = new FarmerCreateViewModel
            {
                Users = farmers.Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.Email
                }).ToList()
            };

            return View(model);
        }


        // POST: Farmers/Create
        [HttpPost]
        public async Task<IActionResult> Create(string UserId, string PhoneNumber, string Location)
        {
            try
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    ModelState.AddModelError("", "Please select a farmer");
                    return View(new FarmerCreateViewModel
                    {
                        Users = await GetUsersList()
                    });
                }

                var user = await _userManager.FindByIdAsync(UserId);
                var farmer = new Farmer
                {
                    UserId = UserId,
                    FullName = $"{user.Firstname} {user.Lastname}",
                    Email = user.Email,
                    PhoneNumber = PhoneNumber,
                    Location = Location
                };

                _context.Add(farmer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(new FarmerCreateViewModel
                {
                    Users = await GetUsersList()
                });
            }
        }
        private async Task<List<SelectListItem>> GetUsersList()
        {
            var farmers = await _userManager.GetUsersInRoleAsync("Farmer");
            return farmers.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.Email
            }).ToList();
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

        // GET: Farmers/CreateNewWithAccount
        public async Task<IActionResult> CreateNewWithAccount()
        {
            var users = await _userManager.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.Email
                })
                .ToListAsync();

            return View(new FarmerCreateViewModel
            {
                Users = users,
                PhoneNumber = "000-000-0000",  // Set default values
                Location = "Unknown Location"
            });
        }

        // POST: Farmers/CreateNewWithAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewWithAccount(FarmerCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var farmer = new Farmer
                    {
                        UserId = model.UserId,
                        FullName = model.FullName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Location = model.Location
                    };

                    _context.Farmers.Add(farmer);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Farmer profile created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error saving farmer: {ex.Message}");
            }

            // Repopulate users if validation fails
            model.Users = await _userManager.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.Email
                })
                .ToListAsync();

            return View(model);
        }
        // GET: Farmers/Profile
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);
            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(f => f.UserId == userId);

            if (farmer == null)
            {
                return NotFound();
            }

            var viewModel = new FarmerProfileViewModel
            {
                FullName = farmer.FullName,
                PhoneNumber = farmer.PhoneNumber,
                Email = farmer.Email,
                Location = farmer.Location
            };

            return View(viewModel);
        }

        // POST: Farmers/UpdateProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(FarmerProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var farmer = await _context.Farmers
                    .FirstOrDefaultAsync(f => f.UserId == User.Identity.Name);

                if (farmer == null)
                {
                    return NotFound();
                }

                // Update phone number and location
                farmer.PhoneNumber = model.PhoneNumber;
                farmer.Location = model.Location;

                // Save changes to the database
                _context.Farmers.Update(farmer);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction("Profile");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Farmer"))
            {
                return NotFound();
            }

            return Json(new
            {
                fullName = $"{user.Firstname} {user.Lastname}",
                email = user.Email
            });
        }

    }

}

