using D = GoReal.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Cors;
using System.Net;
using GoReal.Api.Services;
using GoReal.Api.Models.DataTransfertObject;
using GoReal.Common.Exceptions.Enumerations;
using GoReal.Common.Exceptions;
using GoReal.Api.Models;

namespace GoReal.Api.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    [AuthRequired]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService GameService)
        {
            _gameService = GameService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Game game = new Game();

            try
            {
                game = _gameService.Get(id);
            }
            catch (GameException gameException)
            {
                return gameException.Result switch
                {
                    GameResult.GameNotExist => Problem(((int)GameResult.GameNotExist).ToString(), statusCode: (int)HttpStatusCode.NotFound),
                    GameResult.BoardNotValid => Problem(((int)GameResult.BoardNotValid).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    _ => NotFound(),
                };
            }
            return Ok(game);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Game game)
        {
            if (!_gameService.Create(game)) return BadRequest();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] D.Stone newStone)
        {
            MoveResult result = new MoveResult();

            try
            {
                result = _gameService.MakeMove(id, newStone);
            }
            catch (GameException gameException)
            {
                return gameException.Result switch
                {
                    GameResult.GameNotExist => Problem(((int)GameResult.GameNotExist).ToString(), statusCode: (int)HttpStatusCode.NotFound),
                    GameResult.GameFinished => Problem(((int)GameResult.GameFinished).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    GameResult.OtherPlayerTurn => Problem(((int)GameResult.OtherPlayerTurn).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    GameResult.BoardNotValid => Problem(((int)GameResult.BoardNotValid).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    GameResult.PreventOverwrite => Problem(((int)GameResult.PreventOverwrite).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    GameResult.PreventSuicide => Problem(((int)GameResult.PreventSuicide).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    GameResult.PreventKo => Problem(((int)GameResult.PreventKo).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    _ => NotFound(),
                };
            }
            return Ok(result);
        }
    }
}
