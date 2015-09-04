namespace NRepository.Core.Command
{
    using System;
    using System.Collections.Generic;

    public interface IEntityStateWrapper
    {
        object Entity { get; set; }
        State State { get; set; }
        ICommandInterceptor CommandInterceptor { get; set; }
    }
}
