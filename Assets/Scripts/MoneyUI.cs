using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public void Start()
    {
        EconomyManager.instance.ParaDegisikligi += ParaGuncelle;
        ParaGuncelle(EconomyManager.instance.guncelPara);
    }
    public void OnDestroy()
    {
        EconomyManager.instance.ParaDegisikligi -= ParaGuncelle;
    }
    public void ParaGuncelle(int yeniPara)
    {
        moneyText.text = yeniPara.ToString();
    }
}
