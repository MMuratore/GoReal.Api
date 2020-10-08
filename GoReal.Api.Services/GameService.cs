using GoReal.Common.Exceptions;
using GoReal.Common.Exceptions.Enumerations;
using GoReal.Api.Models;
using System.Collections.Generic;
using System.Linq;
using Tools.Databases;
using D = GoReal.Dal.Entities;
using GoReal.Common.Interfaces;
using GoReal.Dal.Repository;
using GoReal.Api.Services.Mappers;
using System.Net;

namespace GoReal.Api.Services
{
    public class GameService
    {
        private readonly GameRepository _gameRepository;
        private readonly IRepository<D.Rule> _ruleRepository;
        private readonly IRepository<D.TimeControl> _timeControlRepository;
        private readonly IUserRepository<D.User> _userRepository;
        private readonly IStoneRepository<D.Stone> _stoneRepository;

        public GameService(Connection connection)
        {
            _gameRepository = new GameRepository(connection);
            _ruleRepository = new RuleRepository(connection);
            _timeControlRepository = new TimeControlRepository(connection);
            _userRepository = new UserRepository(connection);
            _stoneRepository = new StoneRepository(connection);
        }

        public List<Game> GetByUserId(int userId)
        {
            List<Game> games = _gameRepository.GetByUserId(userId).Select(x => x.ToClient()).ToList();

            if (games.Count() == 0)
                throw new GameException(GameResult.NoGamePlayed, HttpStatusCode.NotFound, "No Game Played");

            foreach (Game game in games)
            {
                game.Rule = _ruleRepository.Get(game.Rule.Id);
                game.TimeControl = _timeControlRepository.Get(game.TimeControl.Id);
                game.BlackPlayer = _userRepository.Get(game.BlackPlayer.UserId)?.ToClient();
                game.WhitePlayer = _userRepository.Get(game.WhitePlayer.UserId)?.ToClient();
            }

            return games;
        }

        public Game Get(int id)
        {
            Game game = _gameRepository.Get(id)?.ToClient();

            if (game is null)
                throw new GameException(GameResult.GameNotExist, HttpStatusCode.NotFound, "Game do not exist");

            game.Rule = _ruleRepository.Get(game.Rule.Id);
            game.TimeControl = _timeControlRepository.Get(game.TimeControl.Id);
            game.BlackPlayer = _userRepository.Get(game.BlackPlayer.UserId)?.ToClient();
            game.WhitePlayer = _userRepository.Get(game.WhitePlayer.UserId)?.ToClient();

            List<D.Stone> stones = _stoneRepository.Get(id).ToList();

            Board board = new Board(stones, game.Size, game.Size, game.BlackPlayer, game.WhitePlayer, game.BlackCapture, game.WhiteCapture, game.KoInfo.ToStone());

            if (!board.IsValid())
                throw new GameException(GameResult.BoardNotValid, HttpStatusCode.BadRequest, "Board not valid");

            board.SetCaptures(game.BlackPlayer, game.BlackCapture);
            board.SetCaptures(game.WhitePlayer, game.WhiteCapture);
            game.Board = board;

            return game;
        }

        public bool Create(Game entity)
        {
            return _gameRepository.Create(entity.ToDal());
        }

        public MoveResult MakeMove(int gameId, D.Stone newStone)
        {
            MoveResult result = new MoveResult();

            Game game = _gameRepository.Get(gameId).ToClient();

            if (game is null)
                throw new GameException(GameResult.GameNotExist, HttpStatusCode.NotFound, "Game do not exist");

            if (!(game.Result is null))
                throw new GameException(GameResult.GameFinished, HttpStatusCode.BadRequest, "Game already finished");

            result.BlackCapture = game.BlackCapture;
            result.WhiteCapture = game.WhiteCapture;
            result.KoInfo = game.KoInfo.ToStone();

            if (newStone.Color is true && game.WhiteState != true || newStone.Color is false && game.BlackState != true)
                throw new GameException(GameResult.OtherPlayerTurn, HttpStatusCode.BadRequest, "Not your turn");

            game.Rule = _ruleRepository.Get(game.Rule.Id);
            List<D.Stone> stones = _stoneRepository.Get(gameId).ToList();
            Board board = new Board(stones, game.Size, game.Size, game.BlackPlayer, game.WhitePlayer, game.BlackCapture, game.WhiteCapture, game.KoInfo.ToStone());

            if (!board.IsValid())
                throw new GameException(GameResult.BoardNotValid, HttpStatusCode.BadRequest, "Board not valid");

            board.SetCaptures(game.BlackPlayer, game.BlackCapture);
            board.SetCaptures(game.WhitePlayer, game.WhiteCapture);

            if (!board.Has(newStone) || newStone.Color is null)
                throw new GameException(GameResult.InvalidMove, HttpStatusCode.BadRequest, "Invalid move");

            Board move = board.MakeMove(newStone, game.Rule.Suicide, game.Rule.Overwrite, game.Rule.Ko);

            result = board.Diff(move);
            foreach (D.Stone stone in result.Stones)
            {
                _ = stone.Color is null ? _stoneRepository.DeleteStone(gameId, stone) : _stoneRepository.AddStone(gameId, stone);
            }
            result.BlackState = !(game.BlackState is true);
            result.WhiteState = !(game.WhiteState is true);

            game.BlackCapture = move.GetCaptures(game.BlackPlayer);
            game.WhiteCapture = move.GetCaptures(game.WhitePlayer);
            game.BlackState = result.BlackState;
            game.WhiteState = result.WhiteState;
            game.KoInfo = move.KoInfo.ToDal();

            _gameRepository.Update(gameId, game.ToDal());

            return result;
        }

        public MoveResult Pass(int gameId, int userId)
        {
            MoveResult result = new MoveResult();
            Game game = _gameRepository.Get(gameId).ToClient();

            if (game is null)
                throw new GameException(GameResult.GameNotExist, HttpStatusCode.NotFound, "Game do not exist");

            if (!(game.Result is null))
                throw new GameException(GameResult.GameFinished, HttpStatusCode.BadRequest, "Game already finished");

            result.BlackCapture = game.BlackCapture;
            result.WhiteCapture = game.WhiteCapture;
            result.KoInfo = game.KoInfo.ToStone();

            result = ModifyPlayerState(result, game, userId);

            game.BlackState = result.BlackState;
            game.WhiteState = result.WhiteState;
            game.Result = result.Result;

            _gameRepository.Update(gameId, game.ToDal());

            return result;
        }

        public MoveResult Resign(int gameId, int userId)
        {
            MoveResult result = new MoveResult();
            Game game = _gameRepository.Get(gameId).ToClient();

            if (game is null)
                throw new GameException(GameResult.GameNotExist, HttpStatusCode.NotFound, "Game do not exist");

            if (!(game.Result is null))
                throw new GameException(GameResult.GameFinished, HttpStatusCode.BadRequest, "Game already finished");

            result.BlackCapture = game.BlackCapture;
            result.WhiteCapture = game.WhiteCapture;
            result.KoInfo = game.KoInfo.ToStone();

            if (game.BlackPlayer.UserId == userId)
            {
                if (!(game.BlackState == true))
                    throw new GameException(GameResult.OtherPlayerTurn, HttpStatusCode.BadRequest, "Not your turn");
                result.Result = "W+R";
            }
                
            else if(game.WhitePlayer.UserId == userId)
            {
                if (!(game.WhiteState == true))
                    throw new GameException(GameResult.OtherPlayerTurn, HttpStatusCode.BadRequest, "Not your turn");
                result.Result = "B+R";
            }
            else
                throw new GameException(GameResult.NotParticipate, HttpStatusCode.BadRequest, "User don't participate");

            game.Result = result.Result;
            game.BlackState = null;
            game.WhiteState = null;
            _gameRepository.Update(gameId, game.ToDal());

            return result;
        }

        private MoveResult ModifyPlayerState(MoveResult result, Game game, int userId)
        {
            if (game.BlackPlayer.UserId == userId)
            {
                if (!(game.BlackState == true))
                    throw new GameException(GameResult.OtherPlayerTurn, HttpStatusCode.BadRequest, "Not your turn");

                game.BlackState = null;
                if (game.BlackState == null && game.WhiteState == null)
                {
                    //TODO: implement end of game
                    result.Result = "TODO";
                }
                else
                {
                    result.BlackState = game.BlackState is null ? null : !game.BlackState;
                    result.WhiteState = game.WhiteState is null ? null : !game.WhiteState;
                }
            }

            else if (game.WhitePlayer.UserId == userId)
            {
                if (!(game.WhiteState == true))
                    throw new GameException(GameResult.OtherPlayerTurn, HttpStatusCode.BadRequest, "Not your turn");
                game.WhiteState = null;
                if (game.BlackState == null && game.WhiteState == null)
                {
                    //TODO: implement end of game
                    result.Result = "TODO";
                }
                else
                {
                    result.BlackState = game.BlackState is null ? null : !game.BlackState;
                    result.WhiteState = game.WhiteState is null ? null : !game.WhiteState;
                }
            }
            else
                throw new GameException(GameResult.NotParticipate, HttpStatusCode.BadRequest, "User don't participate");

            return result;
        }
    }
}
