namespace NRepository.Core.Command
{
    public interface ICommandInterceptors
    {
        IAddCommandInterceptor AddCommandInterceptor { get; }

        IModifyCommandInterceptor ModifyCommandInterceptor { get; }

        IDeleteCommandInterceptor DeleteCommandInterceptor { get; }

        ISaveCommandInterceptor SaveCommandInterceptor { get; }
    }
}