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
    public class PartidasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartidasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Partidas
        public async Task<IActionResult> Index(string Jogadorselecionado)
        {
            if (Jogadorselecionado == "0" || Jogadorselecionado == null)
            {
                try
                {
                    ViewBag.JOGADORES = new SelectList(_context.Jogadores, "Id", "Nome");
                    var applicationDbContext = _context.Partidas.Include(p => p.Jogador)
                        .Include(p => p.Quizz)
                        .OrderByDescending(p => p.Data);
                    return View(await applicationDbContext.ToListAsync());

                }
                catch (Exception)
                {

                    throw;
                }


                //return NotFound();
            }
            else
            {
                ViewBag.JOGADORES = new SelectList(_context.Jogadores, "Id", "Nome");
                var applicationDbContext = _context.Partidas
                    .Include(p => p.Jogador)
                    .Include(p => p.Quizz).Where(p => p.JogadorId == Convert.ToInt32(Jogadorselecionado))
                    .OrderByDescending(p => p.Data);
                return View(await applicationDbContext.ToListAsync());

            }

        }

        // GET: Partidas
        //public async Task<IActionResult> Index()
        //{
        //    ViewBag.JOGADORES = new SelectList(_context.Jogadores, "Id", "Nome");
        //    var applicationDbContext = _context.Partidas.Include(p => p.Jogador)
        //        .Include(p => p.Quizz)
        //        .OrderByDescending(p => p.Data);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        // GET: Partidas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Partidas == null)
            {
                return NotFound();
            }

            var partida = await _context.Partidas
                .Include(p => p.Jogador)
                .Include(p => p.Quizz)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partida == null)
            {
                return NotFound();
            }

            return View(partida);
        }

        // GET: Partidas/Create
        public IActionResult Create()
        {
            ViewData["JogadorId"] = new SelectList(_context.Jogadores, "Id", "Email");
            ViewData["QuizzId"] = new SelectList(_context.Quizzs, "Id", "Descricao");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DateTime Data, int Pontuacao, int JogadorId, int QuizzId)
        {
            if (ModelState.IsValid)
            {
                var partida = new Partida
                {
                    Data = Data,
                    Pontuacao = Pontuacao,
                    JogadorId = JogadorId,
                    QuizzId = QuizzId
                };

                _context.Add(partida);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            // Se houver erros de validação, retorne a view com os erros
            ViewData["JogadorId"] = new SelectList(_context.Jogadores, "Id", "Email");
            ViewData["QuizzId"] = new SelectList(_context.Quizzs, "Id", "Descricao");
            return View();
        }


        // GET: Partidas/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Partidas == null)
        //    {
        //        return NotFound();
        //    }

        //    var partida = await _context.Partidas.FindAsync(id);
        //    if (partida == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["JogadorId"] = new SelectList(_context.Jogadores, "Id", "Email", partida.JogadorId);
        //    ViewData["QuizzId"] = new SelectList(_context.Quizzs, "Id", "Descricao", partida.QuizzId);
        //    return View(partida);
        //}

        // POST: Partidas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Pontuacao,JogadorId,QuizzId")] Partida partida)
        //{
        //    if (id != partida.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(partida);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PartidaExists(partida.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["JogadorId"] = new SelectList(_context.Jogadores, "Id", "Email", partida.JogadorId);
        //    ViewData["QuizzId"] = new SelectList(_context.Quizzs, "Id", "Descricao", partida.QuizzId);
        //    return View(partida);
        //}

        public IActionResult Edit(int id)
        {
            var partida = _context.Partidas.Find(id);

            if (partida == null)
            {
                return NotFound();
            }

            ViewData["JogadorId"] = new SelectList(_context.Jogadores, "Id", "Email", partida.JogadorId);
            ViewData["QuizzId"] = new SelectList(_context.Quizzs, "Id", "Descricao", partida.QuizzId);

            return View(partida);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DateTime Data, int Pontuacao, int JogadorId, int QuizzId)
        {
            if (id == null)
            {
                return NotFound();
            }
            var partida = new Partida();
            partida.Id = id;
            partida.Data = Data;
            partida.Pontuacao = Pontuacao;
            partida.JogadorId = JogadorId;
            partida.QuizzId = QuizzId;


            if (ModelState.IsValid)
            {
                _context.Update(partida);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewData["JogadorId"] = new SelectList(_context.Jogadores, "Id", "Email", partida.JogadorId);
            ViewData["QuizzId"] = new SelectList(_context.Quizzs, "Id", "Descricao", partida.QuizzId);

            return View(partida);
        }


        // GET: Partidas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Partidas == null)
            {
                return NotFound();
            }

            var partida = await _context.Partidas
                .Include(p => p.Jogador)
                .Include(p => p.Quizz)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partida == null)
            {
                return NotFound();
            }

            return View(partida);
        }

        // POST: Partidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Partidas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Partidas'  is null.");
            }
            var partida = await _context.Partidas.FindAsync(id);
            if (partida != null)
            {
                _context.Partidas.Remove(partida);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartidaExists(int id)
        {
            return (_context.Partidas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
