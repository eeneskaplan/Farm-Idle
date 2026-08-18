using UnityEngine;
using UnityEngine.InputSystem;
    
public class PlayerMovement : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sr;
    [SerializeField] private float moveSpeed = 8f; //karakterin hızı
    [SerializeField] private Rigidbody2D playerRigidbody; //rigidbody degiskeni olusturuyoruz
    [SerializeField] private JoystickControl joystickYon;
    private Vector2 klavyeYonu; //hareket yönünü tutacak vector2 değişkeni

    private Vector2 sonKaydedilenKonum;
    private float konumKayitSayaci = 0f;
    
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();//scriptin atalı olduğu objedeki rigidbody componentini referans atıyoruz

        if (SaveSystem.GetInt("Save_KonumVar", 0) == 1)
        {
            float kayitliX = SaveSystem.GetFloat("Save_PlayerX", transform.position.x);
            float kayitliY = SaveSystem.GetFloat("Save_PlayerY", transform.position.y);
            transform.position = new Vector3(kayitliX, kayitliY, transform.position.z);
            if (playerRigidbody != null)
            {
                playerRigidbody.position = new Vector2(kayitliX, kayitliY);
            }
        }

        sonKaydedilenKonum = transform.position;
    }

    public void KonumuKaydet()
    {
        SaveSystem.SetFloat("Save_PlayerX", transform.position.x);
        SaveSystem.SetFloat("Save_PlayerY", transform.position.y);
        SaveSystem.SetInt("Save_KonumVar", 1);
        SaveSystem.Save();
        sonKaydedilenKonum = transform.position;
    }
    
    void OnMove(InputValue value)
    {
        klavyeYonu = value.Get<Vector2>(); //hareket yönünü alıyoruz
    }

 
    void FixedUpdate()
    {
        Vector2 yon = joystickYon.hareketYonu;
        if (yon == Vector2.zero)
        {
            yon = klavyeYonu;
        }

        playerRigidbody.linearVelocity = yon * moveSpeed;

        if (yon != Vector2.zero)
        {
            if (Mathf.Abs(yon.x) > Mathf.Abs(yon.y))
            {
                anim.SetFloat("MoveX", Mathf.Round(yon.x));
                anim.SetFloat("MoveY", 0);
            }
            else if (Mathf.Abs(yon.y) > Mathf.Abs(yon.x))
            {
                anim.SetFloat("MoveY", Mathf.Round(yon.y));
                anim.SetFloat("MoveX", 0);
            }
            anim.SetFloat("MoveSpeed", yon.sqrMagnitude);

            konumKayitSayaci += Time.fixedDeltaTime;
            if (konumKayitSayaci >= 1.5f)
            {
                konumKayitSayaci = 0f;
                KonumuKaydet();
            }
        }
        else
        {
            anim.SetFloat("MoveX", 0);
            anim.SetFloat("MoveY", 0);

            if (Vector2.Distance(transform.position, sonKaydedilenKonum) > 0.05f)
            {
                konumKayitSayaci = 0f;
                KonumuKaydet();
            }
        }
        
    }
}
