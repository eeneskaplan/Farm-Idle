using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KilitAcmaAlani : MonoBehaviour
{
    [SerializeField] private int istenenPara = 50;
    [SerializeField] private int istenenOdun = 0;
    [SerializeField] private GameObject[] acilacakObjeler;
    [SerializeField] private GameObject[] kapanacakObjeler;

    public GameObject sonrakiKilitAcmaAlani; // Görev oku için public yaptık

    [SerializeField] private float odemeHizi = 0.05f;
    Coroutine OdemeRutini;

    [SerializeField] private Image paraBari;
    [SerializeField] private TextMeshProUGUI paraMetni;
    [SerializeField] private Image odunBari;
    [SerializeField] private TextMeshProUGUI odunMetni;
    private float baslangicPara;
    private float baslangicOdun;
    private string kayitAnahtari;

    private void Awake()
    {
        kayitAnahtari = "Kilit_" + gameObject.name;
        baslangicPara = istenenPara;
        baslangicOdun = istenenOdun;

        if (SaveSystem.HasKey(kayitAnahtari + "_KalanPara"))
        {
            istenenPara = SaveSystem.GetInt(kayitAnahtari + "_KalanPara", istenenPara);
        }
        if (SaveSystem.HasKey(kayitAnahtari + "_KalanOdun"))
        {
            istenenOdun = SaveSystem.GetInt(kayitAnahtari + "_KalanOdun", istenenOdun);
        }

        ArayuzuGuncelle();
    }

    private void Start()
    {
        if (SaveSystem.GetInt(kayitAnahtari + "_Acildi", 0) == 1)
        {
            AlaniAc(true);
            return;
        }

        ArayuzuGuncelle();
    }

    private void ArayuzuGuncelle()
    {
        if (paraMetni != null) paraMetni.text = istenenPara.ToString();
        if (paraBari != null && baslangicPara > 0) paraBari.fillAmount = istenenPara / baslangicPara;

        if (odunMetni != null) odunMetni.text = istenenOdun.ToString();
        if (odunBari != null && baslangicOdun > 0) odunBari.fillAmount = istenenOdun / baslangicOdun;
    }

    private void AlaniAc(bool kayitYukleniyor)
    {
        foreach (GameObject obje in acilacakObjeler)
        {
            if (obje != null) obje.SetActive(true);
        }
        foreach (GameObject obje in kapanacakObjeler)
        {
            if (obje != null) obje.SetActive(false);
        }

        if (sonrakiKilitAcmaAlani != null && GorevOku.instance != null)
        {
            bool sonrakiZatenAcik = SaveSystem.GetInt("Kilit_" + sonrakiKilitAcmaAlani.name + "_Acildi", 0) == 1;
            if (!kayitYukleniyor || !sonrakiZatenAcik)
            {
                GorevOku.instance.HedefBelirle(sonrakiKilitAcmaAlani.transform);
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        if (SaveSystem.GetInt("Kilit_" + gameObject.name + "_Acildi", 0) == 1) return;
        if (collision.CompareTag("Player"))
        {
            OdemeRutini = StartCoroutine(OdemeYap());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        if (collision.CompareTag("Player"))
        {
            if (OdemeRutini != null) StopCoroutine(OdemeRutini);
        }
    }

    IEnumerator OdemeYap()
    {
        int paraCekmeHizi = Mathf.Max(1, (int)(baslangicPara / 40f));
        int odunCekmeHizi = Mathf.Max(1, (int)(baslangicOdun / 40f));

        while (true)
        {
            int buTurOdenenPara = 0;
            while (istenenPara > 0 && EconomyManager.instance.ParaYeterliMi(1) && buTurOdenenPara < paraCekmeHizi)
            {
                EconomyManager.instance.ParaHarca(1);
                istenenPara--;
                buTurOdenenPara++;
            }

            if (buTurOdenenPara > 0)
            {
                if (paraMetni != null) paraMetni.text = istenenPara.ToString();
                if (paraBari != null) paraBari.fillAmount = istenenPara / baslangicPara;
                SaveSystem.SetInt(kayitAnahtari + "_KalanPara", istenenPara);
            }

            int buTurOdenenOdun = 0;
            while (istenenOdun > 0 && EconomyManager.instance.OdunYeterliMi(1) && buTurOdenenOdun < odunCekmeHizi)
            {
                EconomyManager.instance.OdunHarca(1);
                istenenOdun--;
                buTurOdenenOdun++;
            }

            if (buTurOdenenOdun > 0)
            {
                if (odunMetni != null) odunMetni.text = istenenOdun.ToString();
                if (odunBari != null) odunBari.fillAmount = istenenOdun / baslangicOdun;
                SaveSystem.SetInt(kayitAnahtari + "_KalanOdun", istenenOdun);
            }

            if (buTurOdenenPara > 0 || buTurOdenenOdun > 0)
            {
                SaveSystem.Save();
            }

            if (istenenPara <= 0 && istenenOdun <= 0)
            {
                SaveSystem.SetInt(kayitAnahtari + "_Acildi", 1);
                SaveSystem.Save();
                AlaniAc(false);
                yield break;
            }

            yield return new WaitForSeconds(odemeHizi);
        }
    }
}
