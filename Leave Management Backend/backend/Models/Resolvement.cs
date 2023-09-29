using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Resolvement : BaseEntity
    {
        public int? RequestId { get; set; }
        virtual public Request? Request { get; set; }
        public int? AdminId { get; set; }
        [ForeignKey("AdminId")]
        virtual public User? Admin { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }
}
