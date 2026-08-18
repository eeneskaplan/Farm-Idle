using UnityEngine;
using System.Collections;

public class KapiIsinlanma : MonoBehaviour
{
    [Header("Iþýnlanma Ayarlarý")]
    public Transform hedefNokta;
    public float isinlanmaGecikmesi = 0f;
    private bool islemDevamEdiyor = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Debug.Log("KAPIYA DEÐEN OBJE: " + collision.gameObject.name);

        if (collision.CompareTag("Player") && !islemDevamEdiyor)
        {
            if (hedefNokta != null)
            {
                StartCoroutine(IsinlanmaRutini(collision.gameObject));
            }
            else
            {
                Debug.LogWarning("DÝKKAT: Gidilecek hedef nokta boþ býrakýlmýþ!");
            }
        }
    }

    private IEnumerator IsinlanmaRutini(GameObject oyuncu)
    {
        islemDevamEdiyor = true;

        if (isinlanmaGecikmesi > 0)
        {
            yield return new WaitForSeconds(isinlanmaGecikmesi);
        }

        
        Rigidbody2D rb = oyuncu.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.position = hedefNokta.position;
        }
        oyuncu.transform.position = hedefNokta.position; 

        yield return new WaitForSeconds(1f); 
        islemDevamEdiyor = false;
    }
}