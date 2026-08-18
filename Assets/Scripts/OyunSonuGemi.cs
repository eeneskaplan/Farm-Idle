using System.Collections;
using UnityEngine;

public class OyunSonuGemi : MonoBehaviour
{
    [Header("Hareket Edecek Gemi")]
    public Transform gemiObjesi; 
    public float gemiHizi = 3f; 

    [Header("Oyun Sonu Paneli")]
    public GameObject kazanmaPaneli; 


    private void OnEnable()
    {
        SaveSystem.DeleteAll();
        StartCoroutine(GemiKacisRutini());
    }

    private IEnumerator GemiKacisRutini()
    {
        float gecenSure = 0f;
        while (gecenSure < 3.5f)
        {
            if (gemiObjesi != null)
            {
                gemiObjesi.Translate(Vector2.right * gemiHizi * Time.deltaTime);
            }
            gecenSure += Time.deltaTime;
            yield return null;
        }

        SaveSystem.DeleteAll();
        if (kazanmaPaneli != null) kazanmaPaneli.SetActive(true);

        Time.timeScale = 0f;
    }
}
