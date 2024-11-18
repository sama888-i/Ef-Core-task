using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFcore.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Price { get; set; } = null!;
        

    }
}
