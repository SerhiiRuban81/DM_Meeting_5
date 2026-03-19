using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_Meeting_5.Models
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public virtual ICollection<City> Cities { get; set; } = new List<City>();
    }
}
