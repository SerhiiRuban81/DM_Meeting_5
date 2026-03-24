using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_Meeting_5.Models.DTOs
{
    public class GameFullInfo
    {
        public int Id { get; set; }

        public string Title { get; set; } = default!;

        public GameStyle GameStyle { get; set; }

        public string Studio { get; set; } = default!;
    }
}
