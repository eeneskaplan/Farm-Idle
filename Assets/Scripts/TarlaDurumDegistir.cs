using System.Collections;
using UnityEngine;

public class TarlaDurumDegistir : MonoBehaviour
{
    public Collider2D ayakCollider;
    [SerializeField] private float animasyonBeklemeSuresi = 0.5f;
    private bool islemYapiliyorMu = false;
    Animator karakterAnim;

    void Start()
    {
        karakterAnim = GetComponent<Animator>();        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        
        if(other.TryGetComponent(out TarlaDongusu tarla))
        {
            if (other.IsTouching(ayakCollider) != true) return;
            if (islemYapiliyorMu == true) return;
            else if (tarla.mevcutDurum == TarlaDongusu.TarlaDurumu.islak) return;
            else 
            { 
                StartCoroutine(IslemRutini(tarla)); 
            }
        }
        
    }
    IEnumerator IslemRutini(TarlaDongusu tarla)
    {
        islemYapiliyorMu = true;
        if (karakterAnim != null)
        {
            if (tarla.mevcutDurum == TarlaDongusu.TarlaDurumu.bos)
            {
                karakterAnim.SetTrigger("tohumEk");
            }
            if (tarla.mevcutDurum == TarlaDongusu.TarlaDurumu.tohumlu)
            {
                karakterAnim.SetTrigger("islemYap");
            }
        }
            yield return new WaitForSeconds(animasyonBeklemeSuresi);

            int urunMiktari = 0;
            tarla.EtkilesimYap(out urunMiktari);
            if (urunMiktari > 0)
            {
                Debug.Log($"Urun miktarý: {urunMiktari}");
            }
            islemYapiliyorMu = false;
        
    }
    
}
