using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Works.Auditing;
using Works.Domain.Entities;
using Works.Domain.Entities.Auditing;

namespace Works.Paradigma.Entities
{
    public class Post : AuditedEntity<int>, ISoftDelete, IMayHaveTenant
    {
        [Required]
        public virtual Blog Blog { get; set; }

        public virtual string Title { get; set; }

        public virtual string Body { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual int? TenantId { get; set; }

        public Post()
        {
           // Id = Guid.NewGuid();
        }

        public Post(Blog blog, string title, string body)
            : this()
        {
            Blog = blog;
            Title = title;
            Body = body;
        }
    }
}
