using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_Meeting_5.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Title { get; set; } = default!;

        public int StudioId { get; set; }

        public virtual Studio Studio { get; set; } = default!;

        public GameStyle GameStyle { get; set; }
    }

    public enum GameStyle
    {
        SinglePerson,
        MultiPerson
    }
}
