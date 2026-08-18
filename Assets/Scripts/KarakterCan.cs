using UnityEngine;
using UnityEngine.UI;
public class KarakterCan : MonoBehaviour
{
    [SerializeField] private float maxCan = 100f;
    private float mevcutCan;

    [SerializeField] private Image canBarImage;

    [SerializeField] private Transform evDogmaNoktasi;

    private bool olduMu = false;

    void Start()
    {
        mevcutCan = maxCan;
        CanBariniGuncelle();
    }

    public void HasarAl(float hasarMiktari)
    {
        mevcutCan -= hasarMiktari;

        if (mevcutCan <= 0)
        {
            mevcutCan = 0;
            BayilVeEveDon();
        }


        CanBariniGuncelle();

        StartCoroutine(HasarEfekti());

      
    }

    private void CanBariniGuncelle()
    {
        if (canBarImage != null)
        {
            canBarImage.fillAmount = mevcutCan / maxCan;
        }
    }

    private void BayilVeEveDon()
    {
        if (EconomyManager.instance != null)
        {
            int kesilecekPara = Mathf.CeilToInt(EconomyManager.instance.guncelPara * 0.1f);
            int kesilecekOdun = Mathf.CeilToInt(EconomyManager.instance.guncelOdun * 0.1f);

            EconomyManager.instance.ParaHarca(kesilecekPara);
            EconomyManager.instance.OdunHarca(kesilecekOdun);
        }

        mevcutCan = maxCan;
        CanBariniGuncelle();

        if (evDogmaNoktasi != null)
        {
            transform.position = evDogmaNoktasi.position;
        }

    }

    private System.Collections.IEnumerator HasarEfekti()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            sr.color = Color.white;
        }
    }


}
