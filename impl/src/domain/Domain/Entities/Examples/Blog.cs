using System;
using System.Collections.Generic;
using System.Text;
using Works.Auditing;
using Works.DataAccess.Attributes;
using Works.Domain.Entities;
using Works.Domain.Entities.Auditing;

namespace Works.Paradigma.Entities
{

    [Audited]
    public class Blog : AggregateRoot, IHasCreationTime
    {
        [DisableAuditing]
        public virtual string Name { get; set; }

        public virtual string Url { get; protected set; }

        public virtual DateTime? CreationTime { get; set; }

        public virtual BlogEx More { get; set; }
        
        public virtual ICollection<Post> Posts { get; set; }
      
        public Blog()
        {

        }

        public Blog(string name, string url, string bloggerName)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            Name = name;
            Url = url;
            More = new BlogEx { BloggerName = bloggerName };
        }

        public virtual void ChangeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var oldUrl = Url;
            Url = url;
        }
    }

    [Component]
    public class BlogEx
    {
        public virtual string BloggerName { get; set; }
    }

    
}
