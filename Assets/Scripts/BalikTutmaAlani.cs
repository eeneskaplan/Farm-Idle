using UnityEngine;

public class BalikTutmaAlani : MonoBehaviour
{
    [Header("Balýk Tutma Ayarlarý")]
    [Tooltip("Bir balýk tutmak için iskelede geçmesi gereken süre (saniye)")]
    [SerializeField] private float tutmaSuresi = 3f;

    [Tooltip("Sudan fýrlayacak olan Balýk Prefab'ý")]
    [SerializeField] private GameObject balikPrefab;

    [Tooltip("Balýðýn denizden çýkacaðý nokta (Empty obje)")]
    [SerializeField] private Transform balikCikisNoktasi;

    [Tooltip("Ýskelede dururken sola bakmasýný istiyorsan TÝKLE, saða bakacaksa kaldýr")]
    [SerializeField] private bool solaBakar = true;

    private float zamanlayici = 0f;
    private bool oyuncuIceride = false;

    private Animator oyuncuAnim;
    private SpriteRenderer oyuncuSR;

    private void LateUpdate()
    {
        
        if (oyuncuIceride)
        {
            zamanlayici += Time.deltaTime;

            if (zamanlayici >= tutmaSuresi)
            {
                BalikYakala();
                zamanlayici = 0f;
            }

            
            if (oyuncuSR != null)
            {
                oyuncuSR.flipX = solaBakar;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oyuncuIceride = true;
            zamanlayici = 0f;

            oyuncuAnim = collision.GetComponent<Animator>();
            oyuncuSR = collision.GetComponent<SpriteRenderer>();

            if (oyuncuAnim != null)
            {
                oyuncuAnim.SetBool("balikTutuyor", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oyuncuIceride = false;
            zamanlayici = 0f;

            if (oyuncuAnim != null)
            {
                oyuncuAnim.SetBool("balikTutuyor", false);
                oyuncuAnim = null;
            }
            oyuncuSR = null;
        }
    }

    private void BalikYakala()
    {
        if (balikPrefab != null && balikCikisNoktasi != null)
        {
            Instantiate(balikPrefab, balikCikisNoktasi.position, Quaternion.identity);
        }
    }
}