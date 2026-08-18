using System.Collections;
using UnityEngine;

public class MantarAI : MonoBehaviour
{
    [SerializeField] private float devriyeHizi = 1.5f;
    [SerializeField] private float takipHizi = 3f;
    private Collider2D gezinmeAlani;

    [SerializeField] private float takipMesafesi = 5f;
    [SerializeField] private float patlamaMesafesi = 1.2f;
    [SerializeField] private float patlamaSuresi = 1f;
    [SerializeField] private GameObject patlamaEfekti;

    private Transform oyuncu;
    private Animator anim;
    private SpriteRenderer sr;

    private Vector2 devriyeNoktasi;
    private bool patliyorMu = false;
    private float beklemeSayaci = 0f;
    

    public enum MantarDurumu { Devriye,Bekleme,Takip,Patlama}
    public MantarDurumu mevcutDurum = MantarDurumu.Devriye;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) oyuncu = p.transform;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }

    
    void Update()
    {
        
        if (patliyorMu) return;
        if (oyuncu == null) return;

        float oyuncuMesafe = Vector2.Distance(transform.position, oyuncu.position);

        switch (mevcutDurum)
        {
            case MantarDurumu.Devriye:
                DevriyeAt(oyuncuMesafe);
                break;
            case MantarDurumu.Bekleme:
                Bekle(oyuncuMesafe);
                break;
            case MantarDurumu.Takip:
                Kovala(oyuncuMesafe);
                break;
        }
    }

    private void DevriyeAt(float oyuncuMesafe)
    {
        if (oyuncuMesafe <= takipMesafesi)
        {
            mevcutDurum = MantarDurumu.Takip;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, devriyeNoktasi, devriyeHizi * Time.deltaTime);
        YonuAyarla(devriyeNoktasi);

        if (anim != null) anim.SetBool("yuruyor", true);
        if (Vector2.Distance(transform.position, devriyeNoktasi) < 0.2f)
        {
            mevcutDurum = MantarDurumu.Bekleme;
            beklemeSayaci = Random.Range(1f, 3f);
            if (anim != null) anim.SetBool("yuruyor", false);
        }
    }

    private void Bekle(float oyuncuMesafe)
    {
        if (oyuncuMesafe <= takipMesafesi)
        {
            mevcutDurum = MantarDurumu.Takip;
            return;
        }
        beklemeSayaci -= Time.deltaTime;
        if (beklemeSayaci <= 0)
        {
            YeniDevriyeNoktasi();
            mevcutDurum = MantarDurumu.Devriye;
        }

    }

    private void Kovala(float oyuncuMesafe)
    {
        if (oyuncuMesafe > takipMesafesi+2f)
        {
            mevcutDurum = MantarDurumu.Devriye;
            YeniDevriyeNoktasi();
            return;
        }

        if (oyuncuMesafe <= patlamaMesafesi)
        {
            StartCoroutine(PatlamaRutini());
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, oyuncu.position, takipHizi * Time.deltaTime);
        YonuAyarla(oyuncu.position);
        if (anim != null) anim.SetBool("yuruyor", true);

    }

    private System.Collections.IEnumerator PatlamaRutini()
    {
        patliyorMu = true;
        mevcutDurum = MantarDurumu.Patlama;
        if (anim != null)
        {
            anim.SetBool("yuruyor", false);
            anim.SetTrigger("patliyor");
        }
        yield return new WaitForSeconds(patlamaSuresi);

        if (patlamaEfekti != null)
        {
            Instantiate(patlamaEfekti, transform.position, Quaternion.identity);
        }

        float sonMesafe = Vector2.Distance(transform.position, oyuncu.position);

        if (sonMesafe <= patlamaMesafesi + 1f)
        {
            KarakterCan oyuncuCan = oyuncu.GetComponent<KarakterCan>();
            if (oyuncuCan != null)
            {
                oyuncuCan.HasarAl(20f);
            }
        }
        Destroy(gameObject);
    }

    private void YeniDevriyeNoktasi()
    {
        if (gezinmeAlani == null) return;

        bool bulundu = false;
        int deneme = 0;

        while(!bulundu && deneme < 20)
        {
            Vector2 aday = (Vector2)transform.position + (Random.insideUnitCircle * 5f);
            if (gezinmeAlani.OverlapPoint(aday))
            {
                devriyeNoktasi = aday;
                bulundu = true;
            }
            deneme++;
        }
    }

    private void YonuAyarla(Vector2 hedef)
    {
        
        Vector2 yon = (hedef - (Vector2)transform.position).normalized;

        
        if (anim != null)
        {
            anim.SetFloat("MoveX", yon.x);
            anim.SetFloat("MoveY", yon.y);
        }
        
        if (Mathf.Abs(yon.x) > Mathf.Abs(yon.y)) 
        {
            if (yon.x > 0) sr.flipX = false; 
            else sr.flipX = true; 
        }
    }

    public void DevriyeAlaniniAyarla(Collider2D alan)
    {
        gezinmeAlani = alan;
        YeniDevriyeNoktasi();   
    }

}
