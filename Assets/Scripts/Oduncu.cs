using UnityEngine;

public class Oduncu : MonoBehaviour
{
    AgacSistemi hedefAgac;
    Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Agac") && gameObject.CompareTag("Player"))
        {
            Debug.Log("1- Karakter aðacýn dibine girdi!");
            hedefAgac = collision.GetComponent<AgacSistemi>();

            if (hedefAgac == null)
            {
                Debug.LogWarning("DÝKKAT: Aðaca çarptýk ama üstünde AgacSistemi scripti yok!");
            }

            anim.SetBool("KesiyorMu", true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Agac") && gameObject.CompareTag("Player"))
        {
            hedefAgac = null;
            anim.SetBool("KesiyorMu", false);
        }
    }
    public void VurusAni()
    {
        Debug.Log("2- Animasyon Event'i (Ýðne) baþarýyla çalýþtý!");

        if (hedefAgac != null)
        {
            Debug.Log("3- Aðaca hasar veriliyor!");
            hedefAgac.HasarAl();
        }
    }

}
