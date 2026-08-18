using UnityEngine;

public class GorevOku : MonoBehaviour
{
    public static GorevOku instance;
    public Transform hedef;
    public float yaricap = 1.5f;

    private SpriteRenderer sr;
    private bool hedefBulunduMu = false;

    private void Awake()
    {
        instance = this;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (hedef != null) sr.enabled = true;
        else sr.enabled = false;
    }

    private void Update()
    {
        if (hedef == null || hedefBulunduMu)
        {
            sr.enabled = false;
            return;
        }

        Transform oyuncu = transform.parent;
        if (oyuncu != null)
        {
            float uzaklik = Vector2.Distance(oyuncu.position, hedef.position);

            if (uzaklik < 3f)
            {
                hedefBulunduMu = true;
                sr.enabled = false;
                return;
            }

            sr.enabled = true;
            Vector2 yon = (hedef.position - oyuncu.position).normalized;
            transform.position = (Vector2)oyuncu.position + (yon * yaricap);

            float aci = Mathf.Atan2(yon.y, yon.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, aci - 90f);
        }
    }

    public void HedefBelirle(Transform yeniHedef)
    {
        hedef = yeniHedef;
        hedefBulunduMu = false;
    }
}