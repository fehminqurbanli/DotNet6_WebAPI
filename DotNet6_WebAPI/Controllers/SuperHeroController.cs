using DotNet6_WebAPI.Data;
using DotNet6_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet6_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private static List<SuperHero> heroes = new List<SuperHero>
            {
                new SuperHero{Id=1,
                    Name="Spider Man",
                    FirstName="Parker",
                    LastName="Peter",
                    Place="New york"
                },
                new SuperHero{Id=2,
                    Name="Ironman",
                    FirstName="Tony",
                    LastName="Stark",
                    Place="Island"
                }
            };
        private readonly DataContext _dataContext;

        public SuperHeroController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            return Ok(await _dataContext.SuperHeroes.ToListAsync());

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetById(int id)
        {
            var hero = await _dataContext.SuperHeroes.FirstOrDefaultAsync(x=>x.Id==id);
            if (hero == null)
            {
                return BadRequest("Hero not found.");
            }
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            _dataContext.SuperHeroes.Add(hero);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }


        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero hero)
        {
            var updatedHero = await _dataContext.SuperHeroes.FirstOrDefaultAsync(x => x.Id == hero.Id);
            if (updatedHero == null)
            {
                return BadRequest("Hero not found.");
            }

            updatedHero.Name = hero.Name;
            updatedHero.FirstName = hero.FirstName;
            updatedHero.LastName=hero.LastName;
            updatedHero.Place=hero.Place;

            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }


        [HttpDelete]
        public async Task<ActionResult<SuperHero>> Delete(int id)
        {
            var deletedHero =await _dataContext.SuperHeroes.FirstOrDefaultAsync(x=>x.Id==id);
            if (deletedHero==null)
            {
                return BadRequest("Hero not found.");
            }
            _dataContext.SuperHeroes.Remove(deletedHero);
            await _dataContext.SaveChangesAsync();
            return Ok(deletedHero);
        }

    }
}
