using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerSavas : MonoBehaviour
{

    [Header("Saldýrý Ayarlarý")]
    [SerializeField] private GameObject okPrefab;
    [SerializeField] private Transform okCikisNoktasi;
    [SerializeField] private float saldiriAraligi = 0.5f;

    [Header("Cooldown Ayarlarý")]
    [SerializeField] private int maxOk = 3;
    [SerializeField] private float dolumSuresi = 2f;
    [SerializeField] private Image dolumBari;

    private int mevcutOkSayisi;
    private bool dolumYapiyorMu = false;
    private float zamanlayici = 0f;

    private List<DusmanSaglik> menzildekiDusmanlar = new List<DusmanSaglik>();
    private Animator anim;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponentInParent<SpriteRenderer>();
        anim = GetComponentInParent<Animator>();
        mevcutOkSayisi = maxOk;
        if (dolumBari != null) dolumBari.fillAmount = 1f;
    }

    
    void Update()
    {
        menzildekiDusmanlar.RemoveAll(d => d == null);
        if (dolumYapiyorMu) return;
        zamanlayici += Time.deltaTime;
        if(zamanlayici>=saldiriAraligi && menzildekiDusmanlar.Count > 0 && mevcutOkSayisi > 0)
        {
            Saldir(EnYakinDusmaniBul());
            zamanlayici = 0f;
        }
    }

    private void Saldir(DusmanSaglik hedef)
    {
        if (hedef == null) return;

        mevcutOkSayisi--;
        OkBariniGuncelle();

        Vector2 yon = (hedef.transform.position - transform.position).normalized;
        AnimasyonuTetikle(yon);

        GameObject yeniOk = Instantiate(okPrefab, okCikisNoktasi.position, Quaternion.identity);
        yeniOk.GetComponent<Ok>().HedefiBelirle(hedef.transform);

        if (mevcutOkSayisi <= 0)
        {
            StartCoroutine(OkDoldurmaRutini());
        }
    }

    private void AnimasyonuTetikle(Vector2 yon)
    {
        if (Mathf.Abs(yon.x) > Mathf.Abs(yon.y))
        {
            if (yon.x < 0)
            {
                // SOLA atýþ yapýlýyorsa
                anim.SetFloat("MoveX", -1);
                anim.SetFloat("MoveY", 0);
                sr.flipX = true; // Yüzünü sola çevir (sað animasyonunu ters oynatýr)
            }
            else
            {
                // SAÐA atýþ yapýlýyorsa
                anim.SetFloat("MoveX", 1);
                anim.SetFloat("MoveY", 0);
                sr.flipX = false; // Düzelt
            }
        }
        else
        {
            anim.SetFloat("MoveY", Mathf.Round(yon.y));
            anim.SetFloat("MoveX", 0);
            sr.flipX = false;
        }

        anim.SetTrigger("Saldiri");

    }

    private System.Collections.IEnumerator OkDoldurmaRutini()
    {
        dolumYapiyorMu = true;
        float gecenSure = 0f;
        
        while (gecenSure < dolumSuresi)
        {
            gecenSure += Time.deltaTime;

            if (dolumBari != null)
            {
                dolumBari.fillAmount = gecenSure / dolumSuresi;
            }
            yield return null;
        }

        mevcutOkSayisi = maxOk;
        if (dolumBari != null) dolumBari.fillAmount = 1f;
        dolumYapiyorMu = false;
    }

    private DusmanSaglik EnYakinDusmaniBul()
    {
        DusmanSaglik enYakin = null;
        float enKisaMesafe = Mathf.Infinity;

        foreach(DusmanSaglik dusman in menzildekiDusmanlar)
        {
            if (dusman == null) continue;

            float mesafe = Vector2.Distance(transform.position, dusman.transform.position);
            if (mesafe < enKisaMesafe)
            {
                enKisaMesafe = mesafe;
                enYakin = dusman;
            }
        }
        return enYakin;
    }

    private void OkBariniGuncelle()
    {
        if (dolumBari != null)
        {
            dolumBari.fillAmount = (float)mevcutOkSayisi / maxOk;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DusmanSaglik dusman = collision.GetComponent<DusmanSaglik>();
        if (dusman != null && !menzildekiDusmanlar.Contains(dusman))
        {
            menzildekiDusmanlar.Add(dusman);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        DusmanSaglik dusman = collision.GetComponent<DusmanSaglik>();
        if (dusman != null && menzildekiDusmanlar.Contains(dusman))
        {
            menzildekiDusmanlar.Remove(dusman);
        }
    }

}
