using UnityEngine;

public class Ok : MonoBehaviour
{

    [SerializeField] private float ucmaHizi = 15f;
    [SerializeField] private int hasar = 30;

    private Transform hedef;
    

    public void HedefiBelirle(Transform yeniHedef)
    {
        hedef = yeniHedef;
    }

    
    void Update()
    {
        if (hedef == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, hedef.position, ucmaHizi * Time.deltaTime);
        Vector2 yon = (hedef.position - transform.position).normalized;
        float aci = Mathf.Atan2(yon.y, yon.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, aci);

        if (Vector2.Distance(transform.position, hedef.position) < 0.2f)
        {
            DusmanSaglik dusman = hedef.GetComponent<DusmanSaglik>();
            if (dusman != null)
            {
                dusman.HasarAl(hasar);
            }
            Destroy(gameObject);
        }
    }
}
