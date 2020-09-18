using System.Linq;
using GoReal.Common.Interfaces;
using GoReal.Models.Api;
using D = GoReal.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using GoReal.Common.Interfaces.Enumerations;
using System.Net;
using System;
using GoReal.Services.Api;
using GoReal.Models.Api.Exceptions;
using GoReal.Models.Api.DataTransfertObject;

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
                return gameException.GameResult switch
                {
                    GameResult.GameNotExist => Problem(((int)GameResult.GameNotExist).ToString(), statusCode: (int)HttpStatusCode.NotFound),
                    GameResult.BoardNotValid => Problem(((int)GameResult.BoardNotValid).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    _ => Problem(((int)GameResult.Failed).ToString(), statusCode: (int)HttpStatusCode.NotFound),
                };
            }
            return Ok(game);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Game game)
        {
            if (_gameService.Create(game))
                return Ok();

            return Problem(((int)GameResult.Failed).ToString(), statusCode: (int)HttpStatusCode.NotFound);
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
                return gameException.GameResult switch
                {
                    GameResult.GameNotExist => Problem(((int)GameResult.GameNotExist).ToString(), statusCode: (int)HttpStatusCode.NotFound),
                    GameResult.GameFinished => Problem(((int)GameResult.GameFinished).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    GameResult.OtherPlayerTurn => Problem(((int)GameResult.OtherPlayerTurn).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    GameResult.BoardNotValid => Problem(((int)GameResult.BoardNotValid).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    GameResult.PreventOverwrite => Problem(((int)GameResult.PreventOverwrite).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    GameResult.PreventSuicide => Problem(((int)GameResult.PreventSuicide).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    GameResult.PreventKo => Problem(((int)GameResult.PreventKo).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    _ => Problem(((int)GameResult.Failed).ToString(), statusCode: (int)HttpStatusCode.NotFound),
                };
            }
            return Ok(result);
        }
    }
}
