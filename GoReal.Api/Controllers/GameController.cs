using System.Linq;
using GoReal.Common.Interfaces;
using GoReal.Models.Api;
using D = GoReal.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using GoReal.Models.Api.Mappers;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using GoReal.Models.Api.Helpers;
using GoReal.Common.Interfaces.Enumerations;
using System.Net;

namespace GoReal.Api.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    [AuthRequired]
    public class GameController : ControllerBase
    {
        private readonly IRepository<D.Game, GameResult> _gameService;
        private readonly IRepository<D.Rule, RuleResult> _ruleService;
        private readonly IRepository<D.TimeControl, TimeControlResult> _timeControlService;
        private readonly IUserRepository<D.User> _userService;
        private readonly IStoneRepository<D.Stone> _stoneService;

        public GameController(IRepository<D.Game, GameResult> GameService, IRepository<D.Rule, RuleResult> RuleService, IRepository<D.TimeControl, TimeControlResult> TimeControlService, IUserRepository<D.User> UserService, IStoneRepository<D.Stone> StoneService)
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
                return Problem(((int)GameResult.GameNotExist).ToString(), statusCode: (int)HttpStatusCode.NotFound);

            game.Rule = _ruleService.Get(game.Rule.Id);
            game.TimeControl = _timeControlService.Get(game.TimeControl.Id);
            game.BlackPlayer = _userService.Get(game.BlackPlayer.UserId)?.ToClient();
            game.WhitePlayer = _userService.Get(game.WhitePlayer.UserId)?.ToClient();

            bool?[][] stoneMap = JaggedArray.InitalizeStoneMap(game.Size, game.Size);

            List<D.Stone> stones = _stoneService.Get(id).ToList();

            Board board = new Board(stones, game.Size, game.Size, game.BlackPlayer, game.WhitePlayer, game.BlackCapture, game.WhiteCapture, game.KoInfo.ToStone());

            if (!board.IsValid())
                return Problem(((int)GameResult.BoardNotValid).ToString(), statusCode: (int)HttpStatusCode.BadRequest);

            board.SetCaptures(game.BlackPlayer, game.BlackCapture);
            board.SetCaptures(game.WhitePlayer, game.WhiteCapture);
            game.Board = board;

            return Ok(game);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Game game)
        {
            if (_gameService.Create(game.ToDal()))
                return Ok();

            return Problem(((int)GameResult.Failed).ToString(), statusCode: (int)HttpStatusCode.NotFound);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] D.Stone newStone)
        {
            Game game = _gameService.Get(id).ToClient();

            if (game is null)
                return Problem(((int)GameResult.GameNotExist).ToString(), statusCode: (int)HttpStatusCode.NotFound);

            if (!(game.Result is null))
                return Problem(((int)GameResult.GameFinished).ToString(), statusCode: (int)HttpStatusCode.BadRequest);

            game.Rule = _ruleService.Get(game.Rule.Id);
            List<D.Stone> stones = _stoneService.Get(id).ToList();

            Board board = new Board(stones, game.Size, game.Size, game.BlackPlayer, game.WhitePlayer, game.BlackCapture, game.WhiteCapture, game.KoInfo.ToStone());

            if (!board.IsValid())
                return Problem(((int)GameResult.BoardNotValid).ToString(), statusCode: (int)HttpStatusCode.BadRequest);

            board.SetCaptures(game.BlackPlayer, game.BlackCapture);
            board.SetCaptures(game.WhitePlayer, game.WhiteCapture);

            var move = board.MakeMove(newStone, game.Rule.Suicide, game.Rule.Overwrite, game.Rule.Ko);

            switch (move.result)
            {
                case GameResult.ValidMove:
                    MoveResult result = board.Diff(move.board);
                    foreach (D.Stone stone in result.Stones)
                    {
                        if (stone.Color is null) _stoneService.DeleteStone(id, stone);
                        else
                        {
                            if (_stoneService.AddStone(id, stone) == GameResult.PreventOverwrite) 
                                return Problem(((int)GameResult.PreventOverwrite).ToString(), statusCode: (int)HttpStatusCode.BadRequest);
                        }
                    }

                    game.BlackCapture = move.board.GetCaptures(game.BlackPlayer);
                    game.WhiteCapture = move.board.GetCaptures(game.WhitePlayer);
                    game.KoInfo = move.board.KoInfo.ToDal();
                    _gameService.Update(id, game.ToDal());

                    return Ok(result);

                case GameResult.PreventOverwrite:
                    return Problem(((int)GameResult.PreventOverwrite).ToString(), statusCode: (int)HttpStatusCode.BadRequest);

                case GameResult.PreventSuicide:
                    return Problem(((int)GameResult.PreventSuicide).ToString(), statusCode: (int)HttpStatusCode.BadRequest);

                case GameResult.PreventKo:
                    return Problem(((int)GameResult.PreventKo).ToString(), statusCode: (int)HttpStatusCode.BadRequest);

                default:
                    break;
            }

            return Problem(((int)GameResult.Failed).ToString(), statusCode: (int)HttpStatusCode.NotFound);
        }
    }
}
