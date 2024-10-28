using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSklepLogikaKCK.Models
{
    public class Client
    {
        public string name, surname, mail;
        public bool wantNetto = false;
        public ShoppingCart cart = new ShoppingCart();


        public Client(string name, string surname, string mail)
        {
            this.name = name;
            this.surname = surname;
            this.mail = mail;
        }
    }
}
