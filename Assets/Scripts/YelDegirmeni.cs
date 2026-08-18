using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YelDegirmeni : MonoBehaviour
{
    [SerializeField] private float urunGitmeHizi = 0.2f;
    Coroutine DegirmenRutini;
    [SerializeField] private GameObject yelDegirmeni;
    public int icindekiBugdaySayisi = 0;
    [SerializeField] private float uretimSuresi = 7f;
    private float uretimSayaci = 0f;
    [SerializeField] private GameObject ekmekPrefab;
    [SerializeField] private Transform cikisNoktasi;
    [SerializeField] private int gerekliBugdaySayisi = 5;
    [SerializeField] private TextMeshProUGUI bugdayUI;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        if (collision.CompareTag("Player") && collision.TryGetComponent<UrunStack>(out UrunStack oyuncuCanta))
        {
            DegirmenRutini = StartCoroutine(BugdayCek(oyuncuCanta));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        if (collision.CompareTag("Player"))
        {
            if (DegirmenRutini != null) 
            {
                StopCoroutine(DegirmenRutini);
            }
        }
    }
    
    IEnumerator BugdayCek(UrunStack canta)
    {
        while (true)
        {
            bool urunVerildiMi = canta.UrunBirak(this);
            if (urunVerildiMi) 
            {
                yield return new WaitForSeconds(urunGitmeHizi); 
            }

            else
            {
                yield break;
            }
            

        }

        
        
    }
    void Update()
    {
        bugdayUI.text = $"Buðday sayýsý {icindekiBugdaySayisi} / {gerekliBugdaySayisi}";
        if (icindekiBugdaySayisi >= gerekliBugdaySayisi)
        {

            uretimSayaci += Time.deltaTime;

            if (uretimSayaci >= uretimSuresi)
            {
                icindekiBugdaySayisi -= gerekliBugdaySayisi;
                Instantiate(ekmekPrefab, cikisNoktasi.position, Quaternion.identity);
                uretimSayaci = 0f;
            }

        }
    }

}
