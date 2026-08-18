using System.Collections.Generic;
using UnityEngine;

public class HayvanUretim : MonoBehaviour
{
    [Header("Üretim Ayarları")]
    [Tooltip("Üretilecek Süt veya Yumurta prefabi")]
    [SerializeField] private GameObject urunPrefab; 
    
    [Tooltip("Kaç saniyede bir ürün verecek")]
    [SerializeField] private float uretimSuresi = 10f; 
    
    [Tooltip("Hayvanın etrafında en fazla kaç tane ürün birikebilir?")]
    [SerializeField] private int maxYerdekiUrun = 3; 
    
    [Tooltip("Ürünün düşeceği merkez nokta (Genelde hayvanın altı)")]
    [SerializeField] private Transform urunCikisNoktasi; 

    private float zamanlayici = 0f;
    private List<GameObject> yerdekiUrunler = new List<GameObject>();
    private HayvanAI hayvanAIKodu;

    private void Start()
    {
        hayvanAIKodu = GetComponent<HayvanAI>();
    }

    void Update()
    {
        // KİLİT NOKTA: Eğer ürün yerden oyuncunun sırtına (çantasına) geçtiyse, 
        // parent'ı değişmiş demektir. Parent'ı değişenleri listeden çıkarıyoruz ki hayvan tekrar üretsin.
        yerdekiUrunler.RemoveAll(u => u == null || u.transform.parent != null);

        // Eğer yerdeki ürün sayısı limiti doldurmadıysa üretmeye devam et
        if (yerdekiUrunler.Count < maxYerdekiUrun)
        {
            if (hayvanAIKodu != null) hayvanAIKodu.UykuModunuAyarla(false);
            zamanlayici += Time.deltaTime;

            if (zamanlayici >= uretimSuresi)
            {
                Uret();
                zamanlayici = 0f;
            }
        }
        else
        {
            // Limit dolduysa zamanlayıcıyı beklemede tut
            zamanlayici = 0f;
            if (hayvanAIKodu != null) hayvanAIKodu.UykuModunuAyarla(true);
        }
    }

    private void Uret()
    {
        // Ürünler tam üst üste binmesin diye hafif rastgele etrafa saçılsın
        Vector2 rastgeleKonum = new Vector2(
            urunCikisNoktasi.position.x + Random.Range(-0.7f, 0.7f),
            urunCikisNoktasi.position.y + Random.Range(-0.7f, 0.7f)
        );

        GameObject yeniUrun = Instantiate(urunPrefab, rastgeleKonum, Quaternion.identity);
        yerdekiUrunler.Add(yeniUrun);
    }
}
