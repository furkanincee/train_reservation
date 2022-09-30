using Entities;
using Microsoft.AspNetCore.Mvc;
using train_reservation.Models.Rezervasyon;

namespace train_reservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezervasyonController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("talep")]
        public IActionResult RezervasyonTalebi(RezervasyonRequest rezervasyon)
        {
            RezervasyonResponse response = new RezervasyonResponse();
            List<Vagon> yerOlanVagonlar = rezervasyon.Tren.Vagonlar.Where(x => x.RezervasyonYapılabilir == true).ToList();

            // Hiçbir vagonda yer yoksa
            if (yerOlanVagonlar.Count == 0)
            {
                response.RezervasyonYapılabilir = false;
                return Ok(response);
            }

            int toplamBosKoltuk = 0;
            foreach (Vagon vagon in yerOlanVagonlar)
            {
                toplamBosKoltuk += (vagon.RezervasyonYapılabilecekKoltukAdedi);
            }

            // Trendeki toplam boş koltuk sayısı rezervasyon yapılacak kişi sayısını karşılıyorsa
            if (toplamBosKoltuk >= rezervasyon.RezervasyonYapilacakKisiSayisi)
            {

                // Rezervasyon yapılacak kişi sayısı kadar boş koltuğu olan vagon yoksa
                if (yerOlanVagonlar.Where(x => x.RezervasyonYapılabilecekKoltukAdedi >= rezervasyon.RezervasyonYapilacakKisiSayisi).Count() == 0)
                {
                    // Farklı vagonlara yerleştirilebilir ise
                    if (rezervasyon.KisilerFarkliVagonlaraYerlestirilebilir != false)
                    {


                        Vagon vagon;
                        while (rezervasyon.RezervasyonYapilacakKisiSayisi > 0)
                        {
                            // Kişileri olabildiğince aynı vagona yerleştirmeye çalışır
                            Yerlesim yerlesim = new Yerlesim();
                            vagon = yerOlanVagonlar.First(x => x.RezervasyonYapılabilecekKoltukAdedi == yerOlanVagonlar.Max(y => y.RezervasyonYapılabilecekKoltukAdedi));
                            yerlesim.KisiSayisi = vagon.RezervasyonYapılabilecekKoltukAdedi < rezervasyon.RezervasyonYapilacakKisiSayisi ? vagon.RezervasyonYapılabilecekKoltukAdedi : rezervasyon.RezervasyonYapilacakKisiSayisi;
                            yerlesim.VagonAdi = vagon.Ad;
                            response.YerlesimAyrinti.Add(yerlesim);
                            rezervasyon.RezervasyonYapilacakKisiSayisi -= yerlesim.KisiSayisi;
                            // yerOlanVagonlar.First(x => x.Ad == vagon.Ad).DoluKoltukAdedi += rezervasyon.RezervasyonYapilacakKisiSayisi;
                            vagon.DoluKoltukAdedi += yerlesim.KisiSayisi;
                        }

                        response.RezervasyonYapılabilir = true;
                        return Ok(response);

                    }
                    else // Farklı vagonlara yerleştirilemez ise
                    {
                        response.RezervasyonYapılabilir = false;
                        return Ok(response);
                    }
                }
                else // Rezervasyon yapılacak kişi sayısı kadar boş koltuğu olan vagon varsa o vagona yerleştirilir
                {
                    Yerlesim yerlesim = new Yerlesim();

                    Vagon vagon = yerOlanVagonlar.FirstOrDefault(x => x.RezervasyonYapılabilecekKoltukAdedi >= rezervasyon.RezervasyonYapilacakKisiSayisi);

                    yerlesim.KisiSayisi = rezervasyon.RezervasyonYapilacakKisiSayisi;
                    yerlesim.VagonAdi = vagon.Ad;
                    response.YerlesimAyrinti.Add(yerlesim);
                    response.RezervasyonYapılabilir = true;
                    vagon.DoluKoltukAdedi += rezervasyon.RezervasyonYapilacakKisiSayisi;
                    return Ok(response);

                }
            }

            response.RezervasyonYapılabilir = false;
            return Ok(response);
        }
    }
}
