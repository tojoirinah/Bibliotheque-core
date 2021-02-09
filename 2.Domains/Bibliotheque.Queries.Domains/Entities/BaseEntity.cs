namespace Bibliotheque.Queries.Domains.Entities
{
    public class BaseEntity<T> where T : struct
    {
        public T Id { get; set; }
    }
}
