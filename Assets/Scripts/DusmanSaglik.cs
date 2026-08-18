using UnityEngine;
using System.Collections;

public class DusmanSaglik : MonoBehaviour
{
    [SerializeField] private int maxCan = 100;

    
    [SerializeField] private bool olumAnimasyonuVarMi = false;

    private int mevcutCan;
    private SpriteRenderer sr;
    private Animator anim;

    public bool olduMu = false;

    void Start()
    {
        mevcutCan = maxCan;
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void HasarAl(int miktar)
    {
        if (olduMu) return;

        mevcutCan -= miktar;
        StartCoroutine(HasarEfekti());

        if (mevcutCan <= 0)
        {
            StartCoroutine(OlumRutini());
        }
    }

    private IEnumerator OlumRutini()
    {
        olduMu = true;

        
        if (olumAnimasyonuVarMi && anim != null)
        {
            anim.SetTrigger("olum");
            yield return new WaitForSeconds(1f);
        }

        
        Destroy(gameObject);
    }

    private IEnumerator HasarEfekti()
    {
        if (sr != null)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            sr.color = Color.white;
        }
    }
}