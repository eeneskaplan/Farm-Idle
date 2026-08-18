using UnityEngine;

public class Madenci : MonoBehaviour
{
    MadenSistemi hedefMaden;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // GÜVENLÝK: Eðer maden kýrýlýp görünmez olduysa, kazmayý anýnda býrak!
        if (hedefMaden != null && hedefMaden.GetComponent<Collider2D>().enabled == false)
        {
            hedefMaden = null;
            anim.SetBool("kaziyorMu", false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Maden") && gameObject.CompareTag("Player"))
        {
            hedefMaden = collision.GetComponent<MadenSistemi>();

            if (hedefMaden != null && hedefMaden.GetComponent<Collider2D>().enabled == true)
            {
                anim.SetBool("kaziyorMu", true);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Maden") && gameObject.CompareTag("Player"))
        {
            hedefMaden = null;
            anim.SetBool("kaziyorMu", false);
        }
    }

    public void VurusAni()
    {
        if (hedefMaden != null && hedefMaden.GetComponent<Collider2D>().enabled == true)
        {
            hedefMaden.HasarAl();
        }
    }
}