using backend.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Request : BaseEntity
    {
        [ForeignKey("UserId")]
        virtual public User? User { get; set; }

        public int? UserId { get; set; }
        virtual public Resolvement? Resolvement { get; set; }
        public int? ResolvementId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public string Comments { get; set; }



    }
}
