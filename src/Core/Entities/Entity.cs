using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Entities
{
    public interface IEntity
    {
    }

    public class Entity : IEntity
    {
    }

    public static class EntityExtensions
    {
        public static IReadOnlyList<Guid> Ids<E>(this IEnumerable<E> entities, Func<E, Guid> selector)
            where E : class, IEntity
        {
            return new List<Guid>(entities.Select(selector).Distinct());
        }
        public static IReadOnlyList<Guid> Ids<E>(this IEnumerable<E> entities, Func<E, Guid?> selector)
            where E : class, IEntity
        {
            IEnumerable<Guid> ids = entities.Select(selector).Where(item => item != null).Select(item => item.Value);

            return new List<Guid>(ids.Distinct());
        }
    }
}
