using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSklepLogikaKCK.ZStare
{
    public class Category
    {
        public string name;
        public int vat;

        public Category(string name, int vat)
        {
            this.name = name;
            this.vat = vat;
        }
    }
}
