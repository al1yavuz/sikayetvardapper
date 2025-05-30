using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sikayetvar.Data;
using sikayetvar.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace sikayetvar.Controllers
{
    [Authorize]
    public class ComplaintsController : Controller
    {
        private readonly AppDbContext _context;

        public ComplaintsController(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var complaints = await _context.Complaints.Include(c => c.User).ToListAsync();
            return View(complaints);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Complaint complaint)
        {
            if (ModelState.IsValid)
            {
                complaint.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(complaint);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(complaint);
        }
    }
}
