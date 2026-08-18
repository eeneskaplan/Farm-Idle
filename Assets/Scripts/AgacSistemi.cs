using UnityEngine;
using System.Collections;

public class AgacSistemi : MonoBehaviour
{
    public int maxCan = 3;
    private int mevcutCan;
    public GameObject odunPrefab;
    [SerializeField] private float dogmaSuresi=30f;
    [SerializeField] private int kazanilacakOdun = 2;

    public Sprite kesilmemisAgacSprite;
    public Sprite kesilmisAgacSprite;

    void Start()
    {
        mevcutCan = maxCan;
        GetComponent<SpriteRenderer>().sprite = kesilmemisAgacSprite;
    }

    public void HasarAl()
    {
        mevcutCan--;
        if (mevcutCan <= 0)
        {
            for(int i = 0; i < kazanilacakOdun; i++)
            {
                float randomX = Random.Range(-0.5f, 0.5f);
                float randomY = Random.Range(-0.5f, 0.5f);
                Vector3 rastgeleKonum = transform.position + new Vector3(randomX, randomY, 0);
                Instantiate(odunPrefab, rastgeleKonum, Quaternion.identity);
            }
            
            KutugeDonus();
            StartCoroutine(AgacBuyumeBekleme());
        }

    }

    public void KutugeDonus()
    {
        GetComponent<SpriteRenderer>().sprite = kesilmisAgacSprite;
        GetComponent <Collider2D>().enabled = false;
    }

    IEnumerator AgacBuyumeBekleme()
    {
        yield return new WaitForSeconds(dogmaSuresi);

        mevcutCan = maxCan;
        GetComponent<SpriteRenderer>().sprite = kesilmemisAgacSprite;
        GetComponent<Collider2D>().enabled = true;
    }

}
