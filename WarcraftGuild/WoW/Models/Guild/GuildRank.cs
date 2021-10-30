using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarcraftGuild.WoW.Models
{
    public class GuildRank
    {
        public int Rank { get; set; }
        public string Name { get; set; }
        public bool IsStaff { get; set; }
    }
}
