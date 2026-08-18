using UnityEngine;

public class TarlaDongusu : MonoBehaviour
{
    public enum TarlaDurumu
    {
        bos,
        tohumlu,
        islak,
        buyuyor,
        toplanmayaHazir
    }

    [Header("Fýrlatýlacak Ekin Prefabi")]
    [SerializeField] private GameObject ekinPrefab;

    [Header("Mevcut Tarla Durumu")]
    public TarlaDurumu mevcutDurum=TarlaDurumu.bos;

    [SerializeField] private SpriteRenderer toprakSr;
    [SerializeField] private SpriteRenderer ekinSr;

    [Header("Toprak Spritelarý")]
    [SerializeField] private Sprite spriteIslakToprak;
    [SerializeField] private Sprite spriteKuruToprak;

    [Header("Ekin Spritelarý")]
    [SerializeField] private Sprite spriteTohumlu;
    [SerializeField] private Sprite spriteCicek;
    [SerializeField] private Sprite spriteToplanabilir;

    [SerializeField] private float yariBuyumeSuresi=8f;
    [SerializeField] private float TamBuyumeSuresi = 10f;
    [SerializeField] private float sayac = 0f;

    public void SpriteGuncelle()
    {
        switch (mevcutDurum)
        {
            case TarlaDurumu.bos:
                toprakSr.sprite = spriteKuruToprak;
                ekinSr.sprite = null;
                break;
            case TarlaDurumu.tohumlu:
                toprakSr.sprite = spriteKuruToprak;
                ekinSr.sprite = spriteTohumlu;
                break;
            case TarlaDurumu.islak:
                toprakSr.sprite = spriteIslakToprak;
                break;
            case TarlaDurumu.buyuyor:
                toprakSr.sprite = spriteIslakToprak;
                ekinSr.sprite = spriteCicek;
                break;
            case TarlaDurumu.toplanmayaHazir:
                toprakSr.sprite = spriteIslakToprak;
                ekinSr.sprite = spriteToplanabilir;
                break;
        }
    }
    void Start()
    {
        if (toprakSr == null) toprakSr = GetComponent<SpriteRenderer>();    
        SpriteGuncelle();
    }

 
    void Update()
    {
        if(mevcutDurum == TarlaDurumu.islak)
        {
            sayac += Time.deltaTime;
            if (sayac >= yariBuyumeSuresi)
            {
                mevcutDurum = TarlaDurumu.buyuyor;
                sayac = 0f;
                SpriteGuncelle();
            }
        }
        else if (mevcutDurum == TarlaDurumu.buyuyor)
        {
            sayac += Time.deltaTime;
            if (sayac >= TamBuyumeSuresi)
            {
                mevcutDurum = TarlaDurumu.toplanmayaHazir;
                sayac = 0f;
                SpriteGuncelle();
            }
        }
    }

    public bool EtkilesimYap(out int kazanilanUrun)
    {
        kazanilanUrun = 0;
        switch (mevcutDurum)
        {
            case TarlaDurumu.bos:
                mevcutDurum = TarlaDurumu.tohumlu;
                SpriteGuncelle();
                return true;
            case TarlaDurumu.tohumlu:
                mevcutDurum = TarlaDurumu.islak;
                SpriteGuncelle();
                return true;
            case TarlaDurumu.toplanmayaHazir:
                kazanilanUrun += 1;
                for (int i = 0; i < kazanilanUrun; i++)
                {
                    float rastgeleX = Random.Range(-2f, 2f);
                    float rastgeleY = Random.Range(-2f, 2f);
                    Vector3 rastgeleKonum = transform.position + new Vector3(rastgeleX, rastgeleY, 0);

                    Instantiate(ekinPrefab, rastgeleKonum, Quaternion.identity);
                }
                mevcutDurum = TarlaDurumu.bos;
                SpriteGuncelle();
                return true;
            case TarlaDurumu.islak:
            case TarlaDurumu.buyuyor:
            default:
                return false;
        }
        
    }

    public void FareYedi()
    {
        if(mevcutDurum!=TarlaDurumu.bos && mevcutDurum != TarlaDurumu.tohumlu)
        {
            mevcutDurum = TarlaDurumu.bos;
            sayac = 0f;
            SpriteGuncelle();
        }
    }

    }
