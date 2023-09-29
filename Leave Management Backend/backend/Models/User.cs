using backend.Enums;

namespace backend.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }

        virtual public ICollection<Request> Requests { get; }
        virtual public ICollection<Resolvement> Resolvements { get; }
    }
}
