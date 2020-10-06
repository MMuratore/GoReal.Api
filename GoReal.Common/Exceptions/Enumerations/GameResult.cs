using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Common.Exceptions.Enumerations
{
    public enum GameResult
    {
        InvalidMove,
        PointNotExist,
        PreventOverwrite,
        PreventKo,
        PreventSuicide,
        GameNotExist,
        GameFinished,
        BoardNotValid,
        OtherPlayerTurn,
        NotParticipate,
        NoGamePlayed
    }
}
