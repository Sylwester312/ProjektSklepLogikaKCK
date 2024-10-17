using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSklepLogikaKCK
{
    public class Client
    {
        string name, surname, mail;
        private ShoppingCart cart;
        
        public Client(string name, string surname, string mail)
        {
            this.name = name;
            this.surname = surname;
            this.mail = mail;
        }


    }
}
