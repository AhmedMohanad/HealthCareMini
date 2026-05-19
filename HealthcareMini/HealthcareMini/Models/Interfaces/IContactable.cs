using HealthcareMini.Models.Objects;

namespace HealthcareMini.Models.Interfaces
   
{
    public interface IContactable
    {

        // this interface will be implemented by any thing who has contact details
        public ContentDetails ContactDetails { get; set; }

    }
}
