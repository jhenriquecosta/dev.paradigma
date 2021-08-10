using System;
using System.Collections.Generic;
using System.Text;
using Works.Auditing;
using Works.Domain.Entities;

namespace Works.Paradigma.Entities
{
    [Audited]
    public class Comment : Entity
    {
        public virtual Post Post { get; set; }

        public virtual string Content { get; set; }
    }
}
