using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizLib_Model.Models
{
    [Table("tbl_Category")]
   public class Category
    {
        [Key]
        public int Category_Id { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}
