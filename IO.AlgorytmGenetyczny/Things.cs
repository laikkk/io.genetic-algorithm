using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.AlgorytmGenetyczny
{
    public class Things
    {
        public static List<Product> products = new List<Product> {
            new Product {
                Name="pocketknife",
                SurvivalPoints=10,
                Weight=1
            },
            new Product {
                Name="beans",
                SurvivalPoints=20,
                Weight=5
            },
            new Product {
                Name="potatoes",
                SurvivalPoints=15,
                Weight=10
            },
            new Product {
                Name="onions",
                SurvivalPoints=2,
                Weight=1
            },
            new Product {
                Name="sleeping bag",
                SurvivalPoints=30,
                Weight=7
            },
            new Product {
                Name="rope",
                SurvivalPoints=10,
                Weight=5
            },
            new Product {
                Name="compass",
                SurvivalPoints=30,
                Weight=1
            }
        };
    }
}

