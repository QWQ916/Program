using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TP
{
    public class Products
    {
        public int id; string name; int count; int price;
        public Products(int id, string name, int count)
        {
            this.id = id; this.name = name; this.count = count;
        }
        public Products(string name, int price, int count)
        {
            this.name = name; this.count = count; this.price = price;
        }

        public string Name { get => name; }
        public int Count { get => count; }
        public int Price { get => price; }
    }
}
