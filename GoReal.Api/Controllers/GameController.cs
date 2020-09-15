using System.Linq;
using GoReal.Common.Interfaces;
using GoReal.Models.Api;
using D = GoReal.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using GoReal.Models.Api.Mappers;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;


namespace GoReal.Api.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    [AuthRequired]
    public class GameController : ControllerBase
    {
        private readonly IRepository<D.Game> _gameService;
        private readonly IRepository<D.Rule> _ruleService;
        private readonly IRepository<D.TimeControl> _timeControlService;
        private readonly IUserRepository<D.User> _userService;
        private readonly IStoneRepository<D.Stone> _stoneService;

        public GameController(IRepository<D.Game> GameService, IRepository<D.Rule> RuleService, IRepository<D.TimeControl> TimeControlService, IUserRepository<D.User> UserService, IStoneRepository<D.Stone> StoneService)
        {
            _gameService = GameService;
            _ruleService = RuleService;
            _timeControlService = TimeControlService;
            _userService = UserService;
            _stoneService = StoneService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Game game = _gameService.Get(id)?.ToClient();

            if ( game is null )
                return NotFound();

            game.Rule = _ruleService.Get(game.Rule.Id);
            game.TimeControl = _timeControlService.Get(game.TimeControl.Id);
            game.BlackPlayer = _userService.Get(game.BlackPlayer.UserId)?.ToClient();
            game.WhitePlayer = _userService.Get(game.WhitePlayer.UserId)?.ToClient();

            bool?[][] stoneMap = new bool?[game.Size][];
            for (int i = 0; i < game.Size; i++)
            {
                stoneMap[i] = new bool?[game.Size];
            }

            List<D.Stone> stones = _stoneService.Get(id).ToList();
            foreach (D.Stone stone in stones)
            {
                stoneMap[stone.Column][stone.Row] = stone.Color;
            }

            Board board = new Board(stoneMap);
            board.SetCaptures(true, game.BlackCapture);
            board.SetCaptures(false, game.WhiteCapture);

            if (!board.IsValid())
                return BadRequest();

            game.Board = board;

            return Ok(game);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Game game)
        {
            if (_gameService.Create(game.ToDal()))
                return Ok();

            return NotFound();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] D.Stone newStone)
        {
            Game game = _gameService.Get(id).ToClient();

            if (game is null)
                return NotFound();

            bool?[][] stoneMap = new bool?[game.Size][];
            for (int i = 0; i < game.Size; i++)
            {
                stoneMap[i] = new bool?[game.Size];
            }

            List<D.Stone> stones = _stoneService.Get(id).ToList();
            foreach (D.Stone stone in stones)
            {
                stoneMap[stone.Column][stone.Row] = stone.Color;
            }

            Board board = new Board(stoneMap);

            if(!board.IsValid())
                return BadRequest();

            board.SetCaptures(true, game.BlackCapture);
            board.SetCaptures(false, game.WhiteCapture);
            Board newBoard = board.MakeMove(newStone);

            foreach (D.Stone stone in newBoard.StoneDiff)
            {
                if (stone.Color is null) _stoneService.DeleteStone(id, stone);
                else
                {
                    if (!_stoneService.AddStone(id, stone)) return BadRequest();
                }
            }

            return Ok(newBoard.Diff());
        }
    }
}
