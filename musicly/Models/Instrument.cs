using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace musicly.Models
{
    public class Instrument
    {
        #region PROPS
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }

        [ForeignKey("InstrumentType")]
        public int TypeID { get; set; }
        public InstrumentType InstrumentType { get; set; }

        public ICollection<InstrumentOrder> InstrumentOrders { get; set; }
        #endregion

    }
}
