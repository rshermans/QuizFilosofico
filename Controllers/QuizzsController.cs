using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizFilosofico.Data;
using QuizFilosofico.Models;

namespace QuizFilosofico.Controllers
{
    public class QuizzsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizzsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Quizzs
        public async Task<IActionResult> Index()
        {
              return _context.Quizzs != null ? 
                          View(await _context.Quizzs.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Quizzs'  is null.");
        }

        // GET: Quizzs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Quizzs == null)
            {
                return NotFound();
            }

            var quizz = await _context.Quizzs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quizz == null)
            {
                return NotFound();
            }

            return View(quizz);
        }

        // GET: Quizzs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quizzs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Tema,ImgCaminho")] Quizz quizz)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quizz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quizz);
        }

        // GET: Quizzs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Quizzs == null)
            {
                return NotFound();
            }

            var quizz = await _context.Quizzs.FindAsync(id);
            if (quizz == null)
            {
                return NotFound();
            }
            return View(quizz);
        }

        // POST: Quizzs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Tema,ImgCaminho")] Quizz quizz)
        {
            if (id != quizz.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quizz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizzExists(quizz.Id))
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
            return View(quizz);
        }

        // GET: Quizzs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Quizzs == null)
            {
                return NotFound();
            }

            var quizz = await _context.Quizzs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quizz == null)
            {
                return NotFound();
            }

            return View(quizz);
        }

        // POST: Quizzs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Quizzs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Quizzs'  is null.");
            }
            var quizz = await _context.Quizzs.FindAsync(id);
            if (quizz != null)
            {
                _context.Quizzs.Remove(quizz);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizzExists(int id)
        {
          return (_context.Quizzs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
