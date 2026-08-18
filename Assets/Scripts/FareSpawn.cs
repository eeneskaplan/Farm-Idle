using System;
using System.Collections.Generic;
using UnityEngine;

public class FareSpawn : MonoBehaviour
{
    [SerializeField] private GameObject farePrefab;
    [SerializeField] private Collider2D spawnAlani;
    [SerializeField] private float spawnSuresi = 20f;
    [SerializeField] private int maxFareSay = 5;
    private List<GameObject> canliFareler = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating("SpawnFare", spawnSuresi, spawnSuresi);
    }

    private void Update()
    {
        canliFareler.RemoveAll(f => f == null);
    }

    void SpawnFare()
    {
        if (canliFareler.Count >= maxFareSay) return;

        Bounds sinirlar = spawnAlani.bounds;
        Vector2 rastgeleKonum = Vector2.zero;
        bool noktaBulundu = false;

        
        for (int i = 0; i < 30; i++)
        {
            Vector2 adayNokta = new Vector2(
                UnityEngine.Random.Range(sinirlar.min.x, sinirlar.max.x),
                UnityEngine.Random.Range(sinirlar.min.y, sinirlar.max.y)
            );

            
            if (spawnAlani.OverlapPoint(adayNokta))
            {
                rastgeleKonum = adayNokta;
                noktaBulundu = true;
                break;
            }
        }

        
        if (!noktaBulundu) return;

        GameObject yeniFare = Instantiate(farePrefab, rastgeleKonum, Quaternion.identity);

        FareAI fareKodu = yeniFare.GetComponent<FareAI>();
        if (fareKodu != null) fareKodu.GezinmeAlaniAyarla(spawnAlani);

        MantarAI mantarKodu = yeniFare.GetComponent<MantarAI>();
        if (mantarKodu != null) mantarKodu.DevriyeAlaniniAyarla(spawnAlani);

        IskeletAI iskeletKodu = yeniFare.GetComponent<IskeletAI>();
        if (iskeletKodu != null) iskeletKodu.DevriyeAlaniniAyarla(spawnAlani);

        canliFareler.Add(yeniFare);
    }
}