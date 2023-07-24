using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hacks.Models;
using Hacks.Data;

namespace Test2.Controllers
{
    public class PlayerController : Controller
    {
        private readonly HacksContext _context;

        public PlayerController(HacksContext context)
        {
            _context = context;
        }

        // GET: Player
        public async Task<IActionResult> Index()
        {


            List<Player> queryplayers = new List<Player>();
            queryplayers = await (from p in _context.Players
                                  join t in _context.Teams
                                  on p.TeamId equals t.TeamId
                                  select new Player { Id = p.Id, FirstName = p.FirstName, Surname = p.Surname, Dob = p.Dob, CurrentTeamString = t.TeamName }).ToListAsync();


            return View(queryplayers);

            //var b = _context.Players
            //.FromSql($"SELECT p.*, t.TeamName FROM PLAYER p INNER JOIN TEAM t ON p.teamid = t.teamid where firstname = 'Hayden'");



            //return _context.Players != null ? 
            //              View(await _context.Players.ToListAsync()) :
            //              Problem("Entity set 'HacksContext.Players'  is null.");
        }


        //public List<Player> GetPlayers()
        //{
        //    List<Player> queryplayers = new List<Player>();
        //    queryplayers = (from p in _context.Players
        //                                 join t in _context.Teams
        //                                 on p.TeamId equals t.TeamId
        //                                 select new Player { Id = p.Id, FirstName = p.FirstName, Surname = p.Surname, Dob = p.Dob, CurrentTeamString = t.TeamName }).ToList();

        //    return queryplayers;

        //}



        // GET: Player/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Players == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Player/Create
        public IActionResult Create()
        {
            var vm = new Player();
            vm.teams = GetTeams();


            return View(vm);
        }

        // POST: Player/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,Surname,Dob,TeamId")] Player player)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(player);
        }

        // GET: Player/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Players == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        // POST: Player/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,Surname,Dob,TeamId")] Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
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
            return View(player);
        }

        // GET: Player/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Players == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Player/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Players == null)
            {
                return Problem("Entity set 'HacksContext.Players'  is null.");
            }
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                _context.Players.Remove(player);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return (_context.Players?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public List<SelectListItem> GetTeams()
        {
            List<SelectListItem> query = new List<SelectListItem>();
            query = (from team in _context.Teams
                     orderby team.TeamName
                     select new SelectListItem { Value = team.TeamId.ToString(), Text = team.TeamName })
                     .ToList();

            return query;
        }
    }
}
