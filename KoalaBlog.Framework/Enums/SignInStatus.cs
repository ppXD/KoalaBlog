using System;

namespace KoalaBlog.Framework.Enums
{
    public enum SignInStatus
    {
        Failure,
        Succeeded,
        NotYetEmailConfirmed,
        LockedOut,
        WrongPassword
    }
}
