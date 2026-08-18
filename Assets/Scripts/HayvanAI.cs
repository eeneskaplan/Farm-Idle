using UnityEngine;

public class HayvanAI : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [SerializeField] private float yurumeHizi = 1f;
    [Tooltip("Hayvanýn dýþýna çýkamayacaðý çit bölgesi")]
    [SerializeField] private Collider2D gezinmeAlani;

    [Tooltip("Ýnek gibi 4 yönlü ise TÝKLE. Tavuk gibi sadece Sað/Sol ise TÝKÝ KALDIR!")]
    [SerializeField] private bool dortYonluMu = true;

    private Animator anim;
    private SpriteRenderer sr;

    private Vector2 rastgeleNokta;
    private bool hedefeGidiyorMu = false;
    private float beklemeSayaci = 0f;
    private float yurumeSayaci = 0f;

    private bool uyuyorMu = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        beklemeSayaci = Random.Range(1f, 3f);
    }

    private void FixedUpdate()
    {
        if (gezinmeAlani == null) return;

        if (uyuyorMu) return;

        if (hedefeGidiyorMu)
        {
            yurumeSayaci += Time.fixedDeltaTime;

            transform.position = Vector2.MoveTowards(transform.position, rastgeleNokta, yurumeHizi * Time.fixedDeltaTime);
            float mesafe = Vector2.Distance(transform.position, rastgeleNokta);

            if (mesafe < 0.1f || yurumeSayaci > 5f)
            {
                hedefeGidiyorMu = false;
                beklemeSayaci = Random.Range(2f, 5f);

                if (anim != null)
                {
                    anim.SetBool("yuruyor", false);
                    anim.SetBool("yemekYiyor", true);
                }
            }
            else
            {
                if (anim != null) anim.SetBool("yuruyor", true);

                Vector2 yon = (rastgeleNokta - (Vector2)transform.position).normalized;

                // ---- ÞALTER BURADA DEVREYE GÝRÝYOR ----
                if (dortYonluMu)
                {
                    // ÝNEK ÝÇÝN (4 YÖNLÜ BLEND TREE)
                    if (Mathf.Abs(yon.x) > Mathf.Abs(yon.y))
                    {
                        anim.SetFloat("MoveX", Mathf.Round(yon.x));
                        anim.SetFloat("MoveY", 0);

                        if (yon.x > 0) sr.flipX = true; // Saða dön
                        else sr.flipX = false;          // Sola dön
                    }
                    else
                    {
                        anim.SetFloat("MoveX", 0);
                        anim.SetFloat("MoveY", Mathf.Round(yon.y));
                        sr.flipX = false;
                    }
                }
                else
                {
                    // TAVUK ÝÇÝN (SADECE SAÐ/SOL FLIP X)
                    // Yukarý/Aþaðý gitse bile en son baktýðý yöne (sað/sol) bakmaya devam eder
                    if (Mathf.Abs(yon.x) > 0.01f)
                    {
                        if (yon.x > 0) sr.flipX = true; // Saða dön
                        else sr.flipX = false;          // Sola dön
                    }
                }
            }
        }
        else
        {
            beklemeSayaci -= Time.fixedDeltaTime;

            if (beklemeSayaci <= 0)
            {
                if (anim != null) anim.SetBool("yemekYiyor", false);
                YeniNoktaBelirle();
            }
        }
    }

    private void YeniNoktaBelirle()
    {
        bool gecerliNoktaBulundu = false;
        int guvenlikSayaci = 0;

        while (!gecerliNoktaBulundu && guvenlikSayaci < 20)
        {
            Vector2 adayNokta = (Vector2)transform.position + (Random.insideUnitCircle * 4f);

            if (gezinmeAlani.OverlapPoint(adayNokta))
            {
                rastgeleNokta = adayNokta;
                hedefeGidiyorMu = true;
                gecerliNoktaBulundu = true;
                yurumeSayaci = 0f;
            }
            guvenlikSayaci++;
        }
    }

    public void UykuModunuAyarla(bool uyusunMu)
    {
        uyuyorMu = uyusunMu;
        if (anim != null)
        {
            anim.SetBool("uyuyor", uyusunMu);

            if (uyusunMu)
            {
                anim.SetBool("yuruyor", false);
                anim.SetBool("yemekYiyor", false);
                hedefeGidiyorMu = false;
            }
        }
    }
}