using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektSklepLogikaKCK.Models;

namespace ProjektSklepLogikaKCK.DTOs
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
    }
}
