using Works.DataAccess.Attributes;
using Works.Domain.Entities;

namespace Works.Paradigma.Domain.Entities
{



    [MapTable(Table = "pessoa",
                 Schema = WorksParadigmaSettings.Schema.Default,
                 UseSchema = WorksParadigmaSettings.Schema.Use)]
    public class Pessoa : Entity
    {
         
        public virtual string Nome { get; set; }
        public virtual decimal Salario { get; set; }
        public virtual Departamento Departamento { get; set; }

        public override string ToString()
        {
            return Nome;
        }
    }

   
    
}
