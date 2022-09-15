
using EntityFramework.Dtos;
using EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public CharacterController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> Get(int userId)
        {
            var characters = await _dataContext.Characters.Where(x => x.UserId == userId).Include(x=>x.Weapon).ToListAsync();

            return Ok(characters);
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> Create(CreateCharacterDto character)
        {
            var user = await _dataContext.Users.FindAsync(character.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var newCharacter = new Character
            {
                Name = character.Name,
                RpgClass = character.RpgClass,
                User = user
            };
            _dataContext.Characters.Add(newCharacter);
            await _dataContext.SaveChangesAsync();
            return await Get(character.UserId);
        }

    }
}
