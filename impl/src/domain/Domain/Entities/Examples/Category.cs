using Works.Auditing;
using Works.Domain.Entities;

namespace Works.Paradigma.Entities
{
    public class Category : Entity
    {
        [Audited]
        public virtual string Name { get; set; }
    }
}
