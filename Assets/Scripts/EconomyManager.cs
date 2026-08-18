using UnityEngine;
using System;
public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;
    public int guncelPara;
    public int guncelOdun;
    public Action<int> OdunDegisikligi;
    public Action<int> ParaDegisikligi;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SaveSystem.Init();
            guncelPara = SaveSystem.GetInt("Save_Para", guncelPara);
            guncelOdun = SaveSystem.GetInt("Save_Odun", guncelOdun);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }


    public void ParaEkle(int eklenecekMiktar)
    {
        guncelPara += eklenecekMiktar;
        SaveSystem.SetInt("Save_Para", guncelPara);
        SaveSystem.Save();
        if (ParaDegisikligi != null)
        {
            ParaDegisikligi.Invoke(guncelPara);
        }
    }
    public void OdunEkle(int odunMiktar)
    {
        guncelOdun += odunMiktar;
        SaveSystem.SetInt("Save_Odun", guncelOdun);
        SaveSystem.Save();
        if (OdunDegisikligi != null)
        {
            OdunDegisikligi.Invoke(guncelOdun);
        }
    }

    public bool ParaYeterliMi(int miktar)
    {
        return guncelPara >= miktar;
    }

    public bool OdunYeterliMi(int miktar)
    {
        return guncelOdun >= miktar;
    }

    public void ParaHarca(int miktar)
    {
        if (ParaYeterliMi(miktar))
        {
            guncelPara -= miktar;
            SaveSystem.SetInt("Save_Para", guncelPara);
            if (ParaDegisikligi != null) ParaDegisikligi.Invoke(guncelPara);
        }
    }

    public void OdunHarca(int miktar)
    {
        if (OdunYeterliMi(miktar))
        {
            guncelOdun -= miktar;
            SaveSystem.SetInt("Save_Odun", guncelOdun);
            if (OdunDegisikligi != null) OdunDegisikligi.Invoke(guncelOdun);
        }
    }
}
