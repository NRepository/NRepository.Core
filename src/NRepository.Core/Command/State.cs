namespace NRepository.Core.Command
{
    using System;
    using System.Collections.Generic;

    public enum State
    {
        Unchanged,
        Add,
        Modify,
        Delete,
    }
}
