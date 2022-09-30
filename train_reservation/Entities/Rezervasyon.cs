namespace Entities
{
    public class Rezervasyon
    {
        public int RezervasyonId { get; set; }
        public Tren Tren { get; set; }
        public int RezervasyonYapilacakKisiSayisi { get; set; }
        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }

    }
}
