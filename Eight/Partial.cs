using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eight
{
    public partial class Product
    {
        public string NDescription => Description == null ? "" : Description;
        public decimal Cost => ProductMaterial.Sum(x => x.Material.Cost);
        public string ImagePath => Image == null ? Environment.CurrentDirectory + @"\products\picture.png" : Environment.CurrentDirectory + Image;
        //{
        //    get
        //    {
        //        if (Image == null)
        //        {
        //            return Environment.CurrentDirectory + @"\products\picture.png";
        //        }
        //        else
        //        {
        //            return Environment.CurrentDirectory + Image;
        //        }
        //    }
        //}
    }
}
