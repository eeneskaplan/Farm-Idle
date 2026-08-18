using UnityEngine;

public class FareAI : MonoBehaviour
{
    [SerializeField] private float fareHiz = 1.5f;
    [SerializeField] private float yemeSuresi = 2f;
    [SerializeField] private TarlaDongusu hedefTarla;
    [SerializeField] private Collider2D gezinmeAlani; 
    
    private Animator anim;
    private SpriteRenderer sr; 

    private Vector2 rastgeleNokta;
    private bool gezinmeNoktasiVarMi = false;
    private bool yemekYiyorMu = false;
    private float gezinmeSayaci = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        
        // Fiziği kapatıp sadece trigger mantığına geçiyoruz (duvarlara fiziksel çarpıp takılmaması için)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; 
        }
        
        // Eski A* bileşenlerini kodla temizliyoruz (eski basit mantığa dönüyoruz)
        var aiPath = GetComponent("AIPath");
        if (aiPath != null) Destroy(aiPath);
        var seeker = GetComponent("Seeker");
        if (seeker != null) Destroy(seeker);
        
        InvokeRepeating("HedefBul", 0f, 0.5f); 
    }

    private void FixedUpdate()
    {
        if (yemekYiyorMu == true) return; 

        if (hedefTarla != null)
        {
            gezinmeNoktasiVarMi = false;
            
            // Hedefe basitçe düz bir çizgi halinde git
            transform.position = Vector2.MoveTowards(transform.position, hedefTarla.transform.position, fareHiz * Time.fixedDeltaTime);
            
            if (anim != null) anim.SetBool("yuruyor", true);
            
            // Yönünü ayarla
            Vector2 yon = (hedefTarla.transform.position - transform.position).normalized;
            if (yon.sqrMagnitude > 0.01f)
            {
                sr.flipX = (yon.x < 0);
            }
        }
        else
        {
            if (gezinmeNoktasiVarMi == false && gezinmeAlani != null)
            {
                bool gecerliNoktaBulundu = false;
                int guvenlikSayaci = 0; 
                
                while (!gecerliNoktaBulundu && guvenlikSayaci < 20)
                {
                    Vector2 adayNokta = (Vector2)transform.position + (Random.insideUnitCircle * 5f);
                    
                    if (gezinmeAlani.OverlapPoint(adayNokta))
                    {
                        rastgeleNokta = adayNokta;
                        gezinmeNoktasiVarMi = true;
                        gecerliNoktaBulundu = true;
                        gezinmeSayaci = 0f;
                    }
                    guvenlikSayaci++;
                }
            }

            if (gezinmeNoktasiVarMi)
            {
                gezinmeSayaci += Time.fixedDeltaTime;
                
                transform.position = Vector2.MoveTowards(transform.position, rastgeleNokta, fareHiz * Time.fixedDeltaTime);
                
                float mesafe = Vector2.Distance(transform.position, rastgeleNokta);
                
                if (mesafe < 0.1f || gezinmeSayaci > 5f)
                {
                    gezinmeNoktasiVarMi = false;
                    if (anim != null) anim.SetBool("yuruyor", false);
                }
                else
                {
                    if (anim != null) anim.SetBool("yuruyor", true);
                    Vector2 yon = (rastgeleNokta - (Vector2)transform.position).normalized;
                    if (yon.sqrMagnitude > 0.01f)
                    {
                        sr.flipX = (yon.x < 0);
                    }
                }
            }
            else
            {
                if (anim != null) anim.SetBool("yuruyor", false);
            }
        }
    }

    public void HedefBul()
    {
        if (hedefTarla != null && hedefTarla.mevcutDurum != TarlaDongusu.TarlaDurumu.bos && hedefTarla.mevcutDurum != TarlaDongusu.TarlaDurumu.tohumlu)
        {
            return;
        }

        float enKisaMesafe = Mathf.Infinity;
        TarlaDongusu enYakinTarla = null;

        
        TarlaDongusu[] tumTarlalar = FindObjectsByType<TarlaDongusu>(FindObjectsSortMode.None);

        foreach (TarlaDongusu tarlaSuanki in tumTarlalar)
        {
            if (tarlaSuanki.mevcutDurum != TarlaDongusu.TarlaDurumu.bos && tarlaSuanki.mevcutDurum != TarlaDongusu.TarlaDurumu.tohumlu)
            {
                
                if (gezinmeAlani != null && !gezinmeAlani.OverlapPoint(tarlaSuanki.transform.position))
                {
                    continue; 
                }

                float aradakiMesafe = Vector2.Distance(transform.position, tarlaSuanki.transform.position);
                if (aradakiMesafe < enKisaMesafe)
                {
                    enKisaMesafe = aradakiMesafe;
                    enYakinTarla = tarlaSuanki;
                }
            }
        }
        hedefTarla = enYakinTarla;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TarlaDongusu tarla))
        {
            if (tarla == hedefTarla && yemekYiyorMu == false)
            {
                StartCoroutine(YemekYemeRutini(tarla));
            }
        }
    }

    private System.Collections.IEnumerator YemekYemeRutini(TarlaDongusu tarla)
    {
        yemekYiyorMu = true;
        
        if (anim != null) anim.SetBool("yuruyor", false);
        if (anim != null) anim.SetBool("yemekYiyor", true);
        
        yield return new WaitForSeconds(yemeSuresi); 
        
        tarla.FareYedi();
        hedefTarla = null; 
        
        if (anim != null) anim.SetBool("yemekYiyor", false);
        yemekYiyorMu = false;
    }

    public void GezinmeAlaniAyarla(Collider2D alan)
    {
        gezinmeAlani = alan;
    }
}