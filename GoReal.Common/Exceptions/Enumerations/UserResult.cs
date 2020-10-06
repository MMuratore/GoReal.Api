using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Common.Exceptions.Enumerations
{
    public enum UserResult
    {
        NotFound,
        GoTagNotUnique,
        EmailNotUnique,
        Ban,
        Inactive
    }
}
