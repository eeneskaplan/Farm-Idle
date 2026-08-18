using UnityEngine;
using TMPro;
public class WoodUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI odunText;

    
    public void Start()
    {
        EconomyManager.instance.OdunDegisikligi += OdunGuncelle;
        OdunGuncelle(EconomyManager.instance.guncelOdun);
    }

    
    public void OnDestroy()
    {
        EconomyManager.instance.OdunDegisikligi -= OdunGuncelle;
    }

    public void OdunGuncelle(int yeniOdun)
    {
        odunText.text = yeniOdun.ToString();
    }
}
