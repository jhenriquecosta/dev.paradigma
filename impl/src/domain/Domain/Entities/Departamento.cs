using System.Collections.Generic;
using Works.DataAccess.Attributes;
using Works.Domain.Entities;
using Works.Domain.Entities.Attributes;

namespace Works.Paradigma.Domain.Entities
{



    [MapTable(Table = "departamento",
                 Schema = WorksParadigmaSettings.Schema.Default,
                 UseSchema = WorksParadigmaSettings.Schema.Use)]
    public class Departamento : Entity
    {
         
        [ModelField(LookupField =true)]
        public virtual string Nome { get; set; }

         
        public virtual IList<Pessoa> Pessoas { get; set; }
        public override string ToString()
        {
            return Nome;
        }
    }

   
    
}
