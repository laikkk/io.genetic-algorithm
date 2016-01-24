using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using UI_Genetic_Algorythm.Models;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.XPath;

namespace UI_Genetic_Algorythm
{
    public class Things
    {
        private const string FileName = "items.json";
        private const string DirectoryName = @"Items\";

        private static List<Product> _products;
        public static List<Product> Products
        {
            get
            {
                if (_products == null)
                {
                    string json = File.ReadAllText(PathToItemsJsonFile());
                    _products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json).ToList();
                }
                return _products;
            }
            set { _products = value; }
        }

        public void AddProductIfNoExistis(Product product)
        {
            if (!Products.Any(chromosom => chromosom.Name == product.Name))
            {
                Products.Add(product);
                saveProductToJSON();
            }
        }

        public void RemoveProduct(Product product)
        {
            Products.Remove(product);
            saveProductToJSON();
        }

        private void saveProductToJSON()
        {
            var json = JsonConvert.SerializeObject(Products);
            File.WriteAllText(PathToItemsJsonFile(), json);
        }

        private static String PathToItemsJsonFile() => Path.Combine(Environment.CurrentDirectory, DirectoryName, FileName);

        //public static List<Product> Products = new List<Product> {
        //    new Product {
        //        Name="pocketknife",
        //        SurvivalPoints=10,
        //        Weight=1
        //    },
        //    new Product {
        //        Name="beans",
        //        SurvivalPoints=20,
        //        Weight=5
        //    },
        //    new Product {
        //        Name="potatoes",
        //        SurvivalPoints=15,
        //        Weight=10
        //    },
        //    new Product {
        //        Name="onions",
        //        SurvivalPoints=2,
        //        Weight=1
        //    },
        //    new Product {
        //        Name="sleeping bag",
        //        SurvivalPoints=30,
        //        Weight=7
        //    },
        //    new Product {
        //        Name="rope",
        //        SurvivalPoints=10,
        //        Weight=5
        //    },
        //    new Product {
        //        Name="compass",
        //        SurvivalPoints=30,
        //        Weight=1
        //    }
        //};
    }
}

