using Entities;

namespace train_reservation.Models.Rezervasyon
{
    public class RezervasyonResponse
    {
        public bool RezervasyonYapılabilir { get; set; }
        public List<Yerlesim> YerlesimAyrinti { get; set; }
        public RezervasyonResponse()
        {
            YerlesimAyrinti = new List<Yerlesim>();
        }
    }
}
