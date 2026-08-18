using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject anaMenuPaneli;
    public GameObject oyunIciPausePaneli;

    [Header("Ses Ayarları")]
    public AudioSource arkaplanMuzigi;
    public Slider sesSlider;

    private void Start()
    {
        AnaMenuyuAc();

        if (sesSlider != null && arkaplanMuzigi != null)
        {
            sesSlider.value = arkaplanMuzigi.volume;
            sesSlider.onValueChanged.AddListener(SesAyarla);
        }
    }

    public void OyunaBasla()
    {
        anaMenuPaneli.SetActive(false);
        oyunIciPausePaneli.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OyundanCik()
    {
        SaveSystem.Save();
        Application.Quit();
    }

    public void OyunuDurdur()
    {
        SaveSystem.Save();
        oyunIciPausePaneli.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OyunaDevamEt()
    {
        oyunIciPausePaneli.SetActive(false);
        Time.timeScale = 1f;
    }

    public void AnaMenuyeDon()
    {
        SaveSystem.Save();
        oyunIciPausePaneli.SetActive(false);
        AnaMenuyuAc();
    }

    private void AnaMenuyuAc()
    {
        anaMenuPaneli.SetActive(true);
        oyunIciPausePaneli.SetActive(false);
        Time.timeScale = 0f;
    }
    public void OyunuSifirla()
    {
        SaveSystem.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SesAyarla(float yeniSesSeviyesi)
    {
        if (arkaplanMuzigi != null)
        {
            arkaplanMuzigi.volume = yeniSesSeviyesi;
        }
    }
}
