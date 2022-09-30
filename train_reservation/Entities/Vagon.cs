namespace Entities

{
    public class Vagon
    {
        public string Ad { get; set; }
        public int Kapasite { get; set; }
        public int DoluKoltukAdedi { get; set; }
        public bool RezervasyonYapılabilir { get => (double)DoluKoltukAdedi / (double)Kapasite < (double)70 / (double)100; }
        public int RezervasyonYapılabilecekKoltukAdedi { get => RezervasyonYapılabilir ? (int)Math.Floor(((double)Kapasite * (double)70 / (double)100) - (double)DoluKoltukAdedi) : 0; }

        public Vagon(string ad, int kapasite, int doluKoltukAdedi)
        {
            Ad = ad;
            Kapasite = kapasite;
            DoluKoltukAdedi = doluKoltukAdedi;
        }
    }
}