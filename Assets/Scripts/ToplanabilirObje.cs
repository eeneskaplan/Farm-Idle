using UnityEngine;

public class ToplanabilirObje : MonoBehaviour
{

    public enum UrunTipi
    {
        bugday,
        ekmek,
        sut,
        yumurta,
        balik,
        altin,
        zumrut,
        elmas
    }
    public enum ObjeNereyeUcuyor
    {
        player,
        satisNoktasi,
        degirmen
    }
    private ObjeNereyeUcuyor mevcutDurum;
    [SerializeField] private float ucusHizi=15f;
    [SerializeField] private Transform mevcutHedef;
    private YelDegirmeni hedefDegirmen;
    private bool toplaniyorMu = false;
    [SerializeField] private UrunStack oyuncuCanta;
    [SerializeField] private int urunDegeri = 5;
    [SerializeField] private bool cantayaGiremez = false;
    public UrunTipi benimTipim;
    void Start()
    {
        mevcutHedef = GameObject.FindWithTag("Player").transform;
        oyuncuCanta = mevcutHedef.GetComponent<UrunStack>();
        
        // Obje yere ilk düştüğünde (birinin çantasında değilse) havaya fırlama efekti yap
        if (transform.parent == null)
        {
            StartCoroutine(FirlamaAnimasyonu());
        }
    }
    
    private System.Collections.IEnumerator FirlamaAnimasyonu()
    {
        Vector2 baslangic = transform.position;
        Vector2 hedef = baslangic + new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
        
        float gecenSure = 0f;
        float sure = 0.4f; // 0.4 saniye havada kalır
        float yukseklik = 1.5f; // Zıplama yüksekliği

        while (gecenSure < sure && toplaniyorMu == false)
        {
            gecenSure += Time.deltaTime;
            float t = gecenSure / sure;
            
            Vector2 yatay = Vector2.Lerp(baslangic, hedef, t);
            float sicrama = Mathf.Sin(t * Mathf.PI) * yukseklik;
            
            transform.position = new Vector3(yatay.x, yatay.y + sicrama, transform.position.z);
            yield return null;
        }
    }

   
    void Update()
    {
        if (toplaniyorMu == true)
        {
            float adimBoyu = ucusHizi * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, mevcutHedef.position, adimBoyu);
            if (Vector2.Distance(transform.position, mevcutHedef.position) < 0.2f)
            {
                if (mevcutDurum == ObjeNereyeUcuyor.player)
                {
                    if (cantayaGiremez == true)
                    {
                        EconomyManager.instance. OdunEkle(1);
                        Destroy(gameObject);
                    }

                    else
                    {
                        bool eklendiMi = oyuncuCanta.UrunEkle(gameObject);
                        if (eklendiMi)
                        {
                            GetComponent<Collider2D>().enabled = false;
                            this.enabled = false;


                        }
                        else
                        {
                            toplaniyorMu = false;
                        }
                    }
                        
                }
                if (mevcutDurum == ObjeNereyeUcuyor.satisNoktasi)
                {
                    toplaniyorMu = true;
                    EconomyManager.instance.ParaEkle(urunDegeri);
                    Destroy(gameObject);
                }
                if (mevcutDurum == ObjeNereyeUcuyor.degirmen)
                {
                    hedefDegirmen.icindekiBugdaySayisi++;
                    Destroy(gameObject);
                }
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            if (oyuncuCanta.UrunAlabilirMi(benimTipim))
            {
                toplaniyorMu = true;
            }
        }
        
    }

    public void SatisaGonder(Transform gemiSatis)
    {
        mevcutHedef = gemiSatis;
        mevcutDurum = ObjeNereyeUcuyor.satisNoktasi;
        this.enabled = true;
    }

    public void DegirmeneGonder(YelDegirmeni degirmenScripti)
    {
        hedefDegirmen = degirmenScripti;
        mevcutHedef = degirmenScripti.transform;
        mevcutDurum = ObjeNereyeUcuyor.degirmen;
        this.enabled = true;

    }
}
