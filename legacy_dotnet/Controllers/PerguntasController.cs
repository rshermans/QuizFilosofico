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
    public class PerguntasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerguntasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Perguntas
        
        public IActionResult Index(int? TEMAS)
        {
            if (TEMAS == 0 || TEMAS == null)
            {
                try
                {
                    var perguntas = _context.Perguntas.Include(p => p.Quizz);
                    ViewBag.TEMAS = new SelectList(_context.Quizzs, "Id", "Tema");
                    return View( perguntas.ToList());
                }
                catch (Exception)
                {

                    throw;
                } 
                
                
                //return NotFound();
            }
            else
            {
                var perguntas = _context.Perguntas
                    .Where(p => p.QuizzId == TEMAS)
                    .Include(p => p.Quizz);
                ViewBag.TEMAS = new SelectList(_context.Quizzs, "Id","Tema");
                return View(perguntas.ToList());
            }

        }
       
        // GET: Perguntas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Perguntas == null)
            {
                return NotFound();
            }

            var pergunta = await _context.Perguntas
                .Include(p => p.Quizz)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pergunta == null)
            {
                return NotFound();
            }

            return View(pergunta);
        }

        // GET: Perguntas/Create
        public IActionResult Create()
        {
            ViewData["QuizzId"] = new SelectList(_context.Quizzs, "Id", "Tema");
            return View();
        }

        // POST: Perguntas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Enunciado,Nivel,QuizzId")] Pergunta pergunta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pergunta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuizzId"] = new SelectList(_context.Quizzs, "Id", "Tema", pergunta.QuizzId);
            return View(pergunta);
        }

        // GET: Perguntas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Perguntas == null)
            {
                return NotFound();
            }

            var pergunta = await _context.Perguntas.FindAsync(id);
            if (pergunta == null)
            {
                return NotFound();
            }
            ViewData["QuizzId"] = new SelectList(_context.Quizzs, "Id", "Tema", pergunta.QuizzId);
            return View(pergunta);
        }

        // POST: Perguntas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Enunciado,Nivel,QuizzId")] Pergunta pergunta)
        {
            if (id != pergunta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pergunta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerguntaExists(pergunta.Id))
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
            ViewData["QuizzId"] = new SelectList(_context.Quizzs, "Id", "Tema", pergunta.QuizzId);
            return View(pergunta);
        }

        // GET: Perguntas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Perguntas == null)
            {
                return NotFound();
            }

            var pergunta = await _context.Perguntas
                .Include(p => p.Quizz)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pergunta == null)
            {
                return NotFound();
            }

            return View(pergunta);
        }

        // POST: Perguntas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Perguntas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Perguntas'  is null.");
            }
            var pergunta = await _context.Perguntas.FindAsync(id);
            if (pergunta != null)
            {
                _context.Perguntas.Remove(pergunta);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerguntaExists(int id)
        {
          return (_context.Perguntas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
