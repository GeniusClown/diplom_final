using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportik.Classes
{
    class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public int PointId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Code { get; set; }
        public int Cost { get; set; }
        public int Discount { get; set; }
    }
}
