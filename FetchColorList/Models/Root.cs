using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FetchColorList.Models
{
    public class Root
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<ColorData> data { get; set; }
    }
}
