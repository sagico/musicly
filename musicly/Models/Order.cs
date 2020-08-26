using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace musicly.Models
{
    public class Order
    {
        #region PROPS
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User{ get; set; }

        public ICollection<InstrumentOrder> InstrumentOrders { get; set; }
        #endregion
    }
}
