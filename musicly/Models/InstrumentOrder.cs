using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace musicly.Models
{
    public class InstrumentOrder
    {
        #region PROPS
        public int InstrumentOrderID { get; set; }

        [ForeignKey("Order")]
        [Column(Order = 1)]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey("Instrument")]
        [Column(Order = 2)]
        public int InstrumentId { get; set; }
        public Instrument Instrument { get; set; }
        public int Quantity { get; set; }
        #endregion

    }
}
