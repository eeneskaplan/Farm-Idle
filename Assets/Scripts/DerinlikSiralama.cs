using UnityEngine;
using UnityEngine.Rendering;

public class DerinlikSiralama : MonoBehaviour
{
    private SpriteRenderer sr;
    private SortingGroup sg;
    public float ayakHizasi = 0f;
    public bool hareketliObje = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sg = GetComponent<SortingGroup>();
        if (hareketliObje == false)
        {
            DerinligiHesapla();
        }
    }


    void Update()
    {
        if (hareketliObje)
        {
            DerinligiHesapla();
        }
    }
    void DerinligiHesapla()
    {
        int sira = (int)(-(transform.position.y + ayakHizasi) * 10);

        if (sg != null)
        {
            sg.sortingOrder = sira;
        }
        else if (sr != null)
        {
            sr.sortingOrder = sira;
        }
    }
}
