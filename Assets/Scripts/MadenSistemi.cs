using System.Collections;
using UnityEngine;

public class MadenSistemi : MonoBehaviour
{
    public int maxCan = 3;
    private int mevcutCan;

    public GameObject madenPrefab;

    [SerializeField] private float dogmaSuresi = 30f;

    [SerializeField] private int kazanilacakMaden= 1;

    

    void Start()
    {
        mevcutCan = maxCan;
    }

    public void HasarAl()
    {
        mevcutCan--;
        if (mevcutCan <= 0)
        {
            for(int i = 0; i < kazanilacakMaden; i++)
            {
                float randomX = Random.Range(-0.5f, 0.5f);
                float randomY = Random.Range(-0.5f, 0.5f);
                Vector3 rastgeleKonum = transform.position + new Vector3 (randomX, randomY, 0);
                Instantiate(madenPrefab, rastgeleKonum, Quaternion.identity);
            }
            KirilmaÝslemi();
            StartCoroutine(MadenYenilemeBekleme());

        }
    }

    public void KirilmaÝslemi()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    IEnumerator MadenYenilemeBekleme()
    {
        yield return new WaitForSeconds(dogmaSuresi);

        mevcutCan = maxCan;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

}
