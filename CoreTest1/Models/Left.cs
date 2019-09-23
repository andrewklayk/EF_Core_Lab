using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CoreTest1.Models
{
    public class Left
    {
        public int ID { get; set; }
        public int PartID { get; set; }
        public int StockID { get; set; }
        [DataType(DataType.Date)]
        [CustomDateRange]
        [Display(Name = "Дата")]
        public DateTime ArrDate { get; set; }
        [Display(Name = "Кількість")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Display(Name = "Найменування")]
        public Part Part { get; set; }
        [Display(Name = "Склад")]
        public Stock Stock { get; set; }
    }

    public class CustomDateRangeAttribute : RangeAttribute
    {
        public CustomDateRangeAttribute() : base(typeof(DateTime), "1/1/1989", DateTime.Now.ToShortDateString())
        { }
    }
}
