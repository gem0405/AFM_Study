using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AFM_T.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class AFM_Product
    {
        public class ProductMetadata
        {
            [Required]
            [Display(Name = "產品編號")]
            public string ProductID { get; set; }
            [Required]
            [Display(Name ="尺寸")]
            public string Size { get; set; }

        }
    }
}