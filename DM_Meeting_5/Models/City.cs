using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_Meeting_5.Models
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public int CountryId { get; set; }

        public Country Country { get; set; } = default!;

        public ICollection<Studio> Studios { get; set; } = new List<Studio>();
    }
}
