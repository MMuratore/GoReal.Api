using System;

namespace GoReal.Dal.Entities
{
    [Flags]
    public enum Role
    {
        None = 0,
        Viewer = 1,
        Player = 2,
        Moderator = 4,
        Administrator = 8,
        SuperAdministrator = 16
    }
}
