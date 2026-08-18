using UnityEngine;
using System.Collections;

public class IskeletAI : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float devriyeHizi = 1f;
    [SerializeField] private float takipHizi = 2.5f;
    private Collider2D gezinmeAlani;

    [Header("Savaþ Ayarlarý")]
    [SerializeField] private float takipMesafesi = 6f;
    [SerializeField] private float saldiriMesafesi = 2f;
    [SerializeField] private float dashHizi = 8f;
    [SerializeField] private float hasarMiktari = 30f;

    private Transform oyuncu;
    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb; // FÝZÝK MOTORU EKLENDÝ

    private Vector2 devriyeNoktasi;
    private bool saldiriyorMu = false;
    private float beklemeSayaci = 0f;
    private DusmanSaglik saglikKodu;

    public enum IskeletDurumu { Devriye, Bekleme, Takip, SaldiriHazirlik, Dash }
    public IskeletDurumu mevcutDurum = IskeletDurumu.Devriye;

    private Vector2 dashHedefi;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        saglikKodu = GetComponent<DusmanSaglik>();
        rb = GetComponent<Rigidbody2D>();

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) oyuncu = p.transform;

        // Duvardan geçmemesi için koda zorunlu fizik ayarlarý (Dynamic) ekledik
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Saða sola devrilmesin
        }
    }

    // Fizik iþlemleri olduðu için Update yerine FixedUpdate kullanýyoruz
    void FixedUpdate()
    {
        if (saglikKodu != null && saglikKodu.olduMu) return;
        if (saldiriyorMu) return;
        if (oyuncu == null) return;

        float oyuncuyaMesafe = Vector2.Distance(transform.position, oyuncu.position);

        switch (mevcutDurum)
        {
            case IskeletDurumu.Devriye:
                DevriyeAt(oyuncuyaMesafe);
                break;
            case IskeletDurumu.Bekleme:
                Bekle(oyuncuyaMesafe);
                break;
            case IskeletDurumu.Takip:
                Kovala(oyuncuyaMesafe);
                break;
        }
    }

    private void DevriyeAt(float oyuncuyaMesafe)
    {
        if (oyuncuyaMesafe <= takipMesafesi)
        {
            mevcutDurum = IskeletDurumu.Takip;
            return;
        }

        // Duvardan geçmemesi için Transform yerine Rigidbody (rb) ile ilerliyoruz
        Vector2 yeniPos = Vector2.MoveTowards(rb.position, devriyeNoktasi, devriyeHizi * Time.fixedDeltaTime);
        rb.MovePosition(yeniPos);

        YonuAyarla(devriyeNoktasi);
        if (anim != null) anim.SetBool("yuruyor", true);

        if (Vector2.Distance(rb.position, devriyeNoktasi) < 0.2f)
        {
            mevcutDurum = IskeletDurumu.Bekleme;
            beklemeSayaci = Random.Range(0.5f, 0.8f);
            if (anim != null) anim.SetBool("yuruyor", false);
        }
    }

    private void Bekle(float oyuncuyaMesafe)
    {
        if (oyuncuyaMesafe <= takipMesafesi)
        {
            mevcutDurum = IskeletDurumu.Takip;
            return;
        }

        beklemeSayaci -= Time.fixedDeltaTime;
        if (beklemeSayaci <= 0)
        {
            YeniDevriyeNoktasi();
            mevcutDurum = IskeletDurumu.Devriye;
        }
    }

    private void Kovala(float oyuncuyaMesafe)
    {
        if (oyuncuyaMesafe > takipMesafesi + 2f)
        {
            mevcutDurum = IskeletDurumu.Devriye;
            YeniDevriyeNoktasi();
            return;
        }

        if (oyuncuyaMesafe <= saldiriMesafesi)
        {
            StartCoroutine(SaldiriRutini());
            return;
        }

        Vector2 yeniPos = Vector2.MoveTowards(rb.position, oyuncu.position, takipHizi * Time.fixedDeltaTime);
        rb.MovePosition(yeniPos);

        YonuAyarla(oyuncu.position);
        if (anim != null) anim.SetBool("yuruyor", true);
    }

    private IEnumerator SaldiriRutini()
    {
        saldiriyorMu = true;
        mevcutDurum = IskeletDurumu.SaldiriHazirlik;
        if (anim != null) anim.SetBool("yuruyor", false);

        YonuAyarla(oyuncu.position);
        dashHedefi = oyuncu.position;

        yield return new WaitForSeconds(0.5f);

        mevcutDurum = IskeletDurumu.Dash;
        if (anim != null) anim.SetTrigger("saldiriyor");

        float dashSuresi = 0f;
        while (Vector2.Distance(rb.position, dashHedefi) > 0.1f && dashSuresi < 0.5f)
        {
            Vector2 yeniPos = Vector2.MoveTowards(rb.position, dashHedefi, dashHizi * Time.fixedDeltaTime);
            rb.MovePosition(yeniPos);
            dashSuresi += Time.fixedDeltaTime;

            if (Vector2.Distance(rb.position, oyuncu.position) < 1f)
            {
                KarakterCan oyuncuCan = oyuncu.GetComponent<KarakterCan>();
                if (oyuncuCan != null) oyuncuCan.HasarAl(hasarMiktari);
                break;
            }
            // Fizik motoruna uyumlu olmasý için FixedUpdate gecikmesi kullanýyoruz
            yield return new WaitForFixedUpdate();
        }

        if (anim != null) anim.SetBool("yuruyor", false);
        yield return new WaitForSeconds(1f);

        saldiriyorMu = false;
        mevcutDurum = IskeletDurumu.Takip;
    }

    private void YeniDevriyeNoktasi()
    {
        if (gezinmeAlani == null) return;

        bool bulundu = false;
        int deneme = 0;
        while (!bulundu && deneme < 20)
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
        Vector2 yon = (hedef - rb.position).normalized;

        if (anim != null)
        {
            anim.SetFloat("MoveX", yon.x);
            anim.SetFloat("MoveY", yon.y);
        }

        if (yon.x < 0) sr.flipX = true;
        else sr.flipX = false;
    }

    public void DevriyeAlaniniAyarla(Collider2D alan)
    {
        gezinmeAlani = alan;
        YeniDevriyeNoktasi();
    }
}