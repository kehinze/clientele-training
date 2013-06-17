namespace AsbaBank.Core.Queries
{
    public interface GenericQuery
    {
        
    }

    public interface IQuery<TResult> : GenericQuery
    {
    }
}