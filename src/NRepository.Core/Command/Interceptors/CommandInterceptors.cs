namespace NRepository.Core.Command
{
    using NRepository.Core.Utilities;

    public class CommandInterceptors : ICommandInterceptors
    {
        public CommandInterceptors(ISaveCommandInterceptor saveCommandInterceptor)
            : this()
        {
            Check.NotNull(saveCommandInterceptor, "saveCommandInterceptor");

            SaveCommandInterceptor = saveCommandInterceptor;
        }

        public CommandInterceptors(IAddCommandInterceptor addCommandInterceptor)
            : this()
        {
            Check.NotNull(addCommandInterceptor, "addCommandInterceptor");

            AddCommandInterceptor = addCommandInterceptor;
        }

        public CommandInterceptors(IModifyCommandInterceptor modifyCommandInterceptor)
            : this()
        {
            Check.NotNull(modifyCommandInterceptor, "modifyCommandInterceptor");

            ModifyCommandInterceptor = modifyCommandInterceptor;
        }

        public CommandInterceptors(IDeleteCommandInterceptor deleteCommandInterceptor)
            : this()
        {
            Check.NotNull(deleteCommandInterceptor, "deleteCommandInterceptor");

            DeleteCommandInterceptor = deleteCommandInterceptor;
        }

        public CommandInterceptors()
        {
            AddCommandInterceptor = new DefaultAddCommandInterceptor();
            ModifyCommandInterceptor = new DefaultModifyCommandInterceptor();
            DeleteCommandInterceptor = new DefaultDeleteCommandInterceptor();
            SaveCommandInterceptor = new DefaultSaveCommandInterceptor();
        }

        public IAddCommandInterceptor AddCommandInterceptor
        {
            get;
            set;
        }

        public IModifyCommandInterceptor ModifyCommandInterceptor
        {
            get;
            set;
        }

        public IDeleteCommandInterceptor DeleteCommandInterceptor
        {
            get;
            set;
        }

        public ISaveCommandInterceptor SaveCommandInterceptor
        {
            get;
            set;
        }
    }
}