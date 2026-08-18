using System.Collections;
using UnityEngine;

public class SatisAlani : MonoBehaviour
{
    [SerializeField] private float satisHizi = 0.2f;
    Coroutine satisRutini;
    [SerializeField] private GameObject gemiObjesi;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent<UrunStack>(out UrunStack oyuncuCanta ))
        {
            satisRutini = StartCoroutine(SatisYap(oyuncuCanta));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (satisRutini != null)
            {
                StopCoroutine(satisRutini);
            }
            
        }
    }

    IEnumerator SatisYap(UrunStack canta)
    {
        while (true)
        {
            bool urunVerildiMi = canta.UrunBirak(gemiObjesi.transform);
            if (urunVerildiMi)
            {
                yield return new WaitForSeconds(satisHizi);
            }
            else
            {
                yield break;
            }
        }
    }
   
}
