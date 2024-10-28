using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSklepLogikaKCK.ZStare
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

        public override string ToString()
        {
            return name + " " + surname + " " + mail;
        }


    }
}
