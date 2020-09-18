using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Api;
using GoReal.Models.Api.DataTransfertObject;
using GoReal.Models.Api.Exceptions;
using GoReal.Models.Services;
using GoReal.Services.Api.Mappers;
using System.Collections.Generic;
using System.Linq;
using Tools.Databases;
using D = GoReal.Models.Entities;

namespace GoReal.Services.Api
{
    public class GameService
    {
        private readonly IRepository<D.Game, GameResult> _gameRepository;
        private readonly IRepository<D.Rule, RuleResult> _ruleRepository;
        private readonly IRepository<D.TimeControl, TimeControlResult> _timeControlRepository;
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

        public Game Get(int id)
        {
            Game game = _gameRepository.Get(id)?.ToClient();

            if (game is null)
                throw new GameException(GameResult.GameNotExist, "Game do not exist");

            game.Rule = _ruleRepository.Get(game.Rule.Id);
            game.TimeControl = _timeControlRepository.Get(game.TimeControl.Id);
            game.BlackPlayer = _userRepository.Get(game.BlackPlayer.UserId)?.ToClient();
            game.WhitePlayer = _userRepository.Get(game.WhitePlayer.UserId)?.ToClient();

            List<D.Stone> stones = _stoneRepository.Get(id).ToList();

            Board board = new Board(stones, game.Size, game.Size, game.BlackPlayer, game.WhitePlayer, game.BlackCapture, game.WhiteCapture, game.KoInfo.ToStone());

            if (!board.IsValid())
                throw new GameException(GameResult.BoardNotValid, "Board not valid");

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
                throw new GameException(GameResult.GameNotExist, "Game do not exist");

            if (!(game.Result is null))
                throw new GameException(GameResult.GameFinished, "Game already finished");


            if (newStone.Color is null && newStone.Row == -1 && newStone.Column == -1)
            {
                result.Result = game.BlackState is true ? "W+R" : "B+R";
                game.Result = result.Result;
                _gameRepository.Update(gameId, game.ToDal());
                return result;
            }

            if (newStone.Color is null)
            {
                game.BlackState = game.BlackState is true ? null : game.BlackState;
                game.WhiteState = game.WhiteState is true ? null : game.WhiteState;
                if (game.BlackState == null && game.WhiteState == null)
                {
                    //TODO: implement end of game
                    game.Result = "TODO";
                    _gameRepository.Update(gameId, game.ToDal());
                    return result;
                }
                else
                {
                    game.BlackState = game.BlackState is null ? null : !game.BlackState;
                    game.WhiteState = game.WhiteState is null ? null : !game.WhiteState;
                    _gameRepository.Update(gameId, game.ToDal());
                    return result;
                }
            }

            if (newStone.Color is true && game.WhiteState != true || newStone.Color is false && game.BlackState != true)
                throw new GameException(GameResult.OtherPlayerTurn, "Not your turn");

            game.Rule = _ruleRepository.Get(game.Rule.Id);
            List<D.Stone> stones = _stoneRepository.Get(gameId).ToList();
            Board board = new Board(stones, game.Size, game.Size, game.BlackPlayer, game.WhitePlayer, game.BlackCapture, game.WhiteCapture, game.KoInfo.ToStone());

            if (!board.IsValid())
                throw new GameException(GameResult.BoardNotValid, "Board not valid");

            board.SetCaptures(game.BlackPlayer, game.BlackCapture);
            board.SetCaptures(game.WhitePlayer, game.WhiteCapture);

            Board move = board.MakeMove(newStone, game.Rule.Suicide, game.Rule.Overwrite, game.Rule.Ko);

            result = board.Diff(move);
            foreach (D.Stone stone in result.Stones)
            {
                if (stone.Color is null) _stoneRepository.DeleteStone(gameId, stone);
                else
                {
                    if (_stoneRepository.AddStone(gameId, stone) == GameResult.PreventOverwrite)
                        throw new GameException(GameResult.PreventOverwrite, "Prevent Overwrite");
                }
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
    }
}
