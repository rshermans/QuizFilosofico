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
    public class JogadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JogadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jogadores
        public async Task<IActionResult> Index()
        {
          
              return _context.Jogadores != null ? 
                          View(await _context.Jogadores.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Jogadores'  is null.");
            
        }

        // GET: Jogadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Jogadores == null)
            {
                return NotFound();
            }

            var jogador = await _context.Jogadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jogador == null)
            {
                return NotFound();
            }

            return View(jogador);
        }

        // GET: Jogadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jogadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Senha,Administrador,Estado")] Jogador jogador)
        {
            
            if (ModelState.IsValid)
            {
                if (jogador.Administrador != true || jogador.Estado != false)
                {
                    jogador.Administrador = false;
                    jogador.Estado = true;
                }
                _context.Add(jogador);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Login");
            }
            return View(jogador);
        }


        // GET: Jogadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Jogadores == null)
            {
                return NotFound();
            }

            var jogador = await _context.Jogadores.FindAsync(id);
            if (jogador == null)
            {
                return NotFound();
            }
            return View(jogador);
        }

        // POST: Jogadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Senha,Administrador,Estado")] Jogador jogador)
        {
            if (id != jogador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jogador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JogadorExists(jogador.Id))
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
            return View(jogador);
        }

        // GET: Jogadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Jogadores == null)
            {
                return NotFound();
            }

            var jogador = await _context.Jogadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jogador == null)
            {
                return NotFound();
            }

            return View(jogador);
        }

        // POST: Jogadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Jogadores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Jogadores'  is null.");
            }
            var jogador = await _context.Jogadores.FindAsync(id);
            if (jogador != null)
            {
                _context.Jogadores.Remove(jogador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JogadorExists(int id)
        {
          return (_context.Jogadores?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //Metodo para implementar o Referer (referência para pagina anterior
        public IActionResult Back([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext.Request;

            if (request.Headers.ContainsKey("Referer"))
            {
                var refererUrl = request.Headers["Referer"].ToString();
                return Redirect(refererUrl);
            }

            // Caso o cabeçalho "Referer" não esteja presente, redirecione para uma ação específica
            return RedirectToAction("Index", "Home");
        }

    }
}
