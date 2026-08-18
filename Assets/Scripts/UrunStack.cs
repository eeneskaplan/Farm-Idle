using UnityEngine;

public class UrunStack : MonoBehaviour
{
    [SerializeField] private int kapasite = 7;
    private int mevcutUrunSayisi = 0;
    [SerializeField] private Transform stackNoktasi;
    [SerializeField] private float objeArasiBosluk = 0.5f;
    private ToplanabilirObje.UrunTipi cantadakiTip;

    public bool CantaDoluMu()
    {
        if (mevcutUrunSayisi >= kapasite) 
        {
            return true; 
        }
        else
        {
            return false;
        }
    }
     
    public bool UrunEkle(GameObject gelenUrun)
    {
        if (CantaDoluMu()) return false;

        ToplanabilirObje gelenObjeninScripti = gelenUrun.GetComponent<ToplanabilirObje>();

        if (mevcutUrunSayisi == 0)
        {
            cantadakiTip = gelenObjeninScripti.benimTipim;
        }

        if (mevcutUrunSayisi > 0 && gelenObjeninScripti.benimTipim != cantadakiTip)
        {
            return false;
        }
     
            
            gelenUrun.transform.SetParent(stackNoktasi);
            gelenUrun.transform.localPosition = new Vector3(0, mevcutUrunSayisi * objeArasiBosluk, 0);
            mevcutUrunSayisi++;


            return true;
        
            
    }
    public bool UrunBirak(Transform hedefNokta, ToplanabilirObje.UrunTipi istenenTip)
    {
        if (mevcutUrunSayisi == 0 || cantadakiTip != istenenTip)
        {
            return false;
        }
            mevcutUrunSayisi--;
            Transform enUsttekiUrun=stackNoktasi.transform.GetChild(mevcutUrunSayisi);
            enUsttekiUrun.SetParent(null);
            enUsttekiUrun.GetComponent<ToplanabilirObje>().SatisaGonder(hedefNokta);
            return true;
        }

    public bool UrunBirak(Transform hedefNokta)
    {
        if (mevcutUrunSayisi == 0 )
        {
            return false;
        }
        mevcutUrunSayisi--;
        Transform enUsttekiUrun = stackNoktasi.transform.GetChild(mevcutUrunSayisi);
        enUsttekiUrun.SetParent(null);
        enUsttekiUrun.GetComponent<ToplanabilirObje>().SatisaGonder(hedefNokta);
        return true;
    }

    public bool UrunBirak(YelDegirmeni degirmenMakinesi)
    {
        if (mevcutUrunSayisi == 0 || cantadakiTip != ToplanabilirObje.UrunTipi.bugday)
        {
            return false;
        }
        mevcutUrunSayisi--;
        Transform enUsttekiUrun = stackNoktasi.transform.GetChild(mevcutUrunSayisi);
        enUsttekiUrun.SetParent(null);
        enUsttekiUrun.GetComponent<ToplanabilirObje>().DegirmeneGonder(degirmenMakinesi);
        return true;
    }


    public bool UrunAlabilirMi(ToplanabilirObje.UrunTipi tip)
    {
        if (CantaDoluMu()) return false;
        
        if (mevcutUrunSayisi > 0 && tip != cantadakiTip) return false;
        
        return true;
    }

   

}
