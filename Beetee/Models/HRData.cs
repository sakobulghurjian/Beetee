using System.ComponentModel.DataAnnotations;

namespace Beetee.Models
{
    public class HRData
    {
        [Key]
        public int ID { get; set; }

        public int EmployeeID { get; set; }

        public string SocialSecurityNumber { get; set; }

        public int Salery { get; set; }
    }
}
