namespace ReactApp.Server.Entity
{
    public abstract class BaseEntity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
