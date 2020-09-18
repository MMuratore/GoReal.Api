using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Common.Interfaces.Enumerations
{
    public enum GameResult
    {
        Register,
        ValidMove,
        PointNotExist,
        PreventOverwrite,
        PreventKo,
        PreventSuicide,
        GameNotExist,
        GameFinished,
        BoardNotValid,
        OtherPlayerTurn,
        Failed
    }
}
