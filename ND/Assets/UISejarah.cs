using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UISejarah : MonoBehaviour
{
    public Text teksLevel;
    public Text teksTimer;
    public Image[] iconNyawa;

    public RectTransform UI_Darah;

    public GameObject panelPause;
    public GameObject panelKeluar;

    private int levelSaatIni;
    private float waktuTersisa;
    private int hitPoint;

    private bool permainanAktif = true;

    void Start()
    {
        levelSaatIni = PlayerPrefs.GetInt("levelDipilih", 1);
        waktuTersisa = 60f;

        if (!PlayerPrefs.HasKey("HitPoint") || PlayerPrefs.GetInt("HitPoint") <= 0)
        {
            hitPoint = 3;
            PlayerPrefs.SetInt("HitPoint", hitPoint);
        }
        else
        {
            hitPoint = PlayerPrefs.GetInt("HitPoint");
        }

        UpdateUI();
        UpdateNyawaUI();
        UpdateDarahBar();

        if (panelPause != null) panelPause.SetActive(false);
        if (panelKeluar != null) panelKeluar.SetActive(false);
    }

    void Update()
    {
        if (!permainanAktif) return;

        waktuTersisa -= Time.deltaTime;
        teksTimer.text = Mathf.CeilToInt(waktuTersisa).ToString();

        if (waktuTersisa <= 0)
        {
            permainanAktif = false;
            KumpulanSuara.instance.Panggil_sfx(4); // Suara kalah
            Invoke("KembaliKePilihLevel", 1.0f);
        }
    }

    public void TambahLevel()
    {
        levelSaatIni++;
        PlayerPrefs.SetInt("levelDipilih", levelSaatIni);
        waktuTersisa = 60f;
        PlayerPrefs.SetFloat("Timer", waktuTersisa);
        UpdateUI();
    }

    public void KurangiNyawa()
    {
        if (!permainanAktif) return;

        if (hitPoint > 0)
        {
            hitPoint--;
            PlayerPrefs.SetInt("HitPoint", hitPoint);
            UpdateNyawaUI();
            UpdateDarahBar();
        }

        if (hitPoint <= 0)
        {
            permainanAktif = false;
            KumpulanSuara.instance.Panggil_sfx(4); // Suara kalah
            Invoke("KembaliKePilihLevel", 1.0f);
        }
    }

    void UpdateUI()
    {
        teksLevel.text = levelSaatIni.ToString();
        teksTimer.text = Mathf.CeilToInt(waktuTersisa).ToString();
    }

    void UpdateNyawaUI()
    {
        for (int i = 0; i < iconNyawa.Length; i++)
        {
            iconNyawa[i].enabled = i < hitPoint;
        }
    }

    void UpdateDarahBar()
    {
        if (UI_Darah != null)
        {
            UI_Darah.sizeDelta = new Vector2(50f * hitPoint, UI_Darah.sizeDelta.y);
        }
    }

    void KembaliKePilihLevel()
    {
        PlayerPrefs.DeleteKey("HitPoint");
        PlayerPrefs.DeleteKey("Timer");
        SceneManager.LoadScene("PilihLevelSejarah");
    }

    public void TombolPause(bool pause)
    {
        permainanAktif = !pause;
        if (panelPause != null) panelPause.SetActive(pause);
    }

    public void TombolLanjut()  
    {
        TombolPause(false);
    }

    public void TombolMainMenu()
    {
        PlayerPrefs.DeleteKey("Timer");
        SceneManager.LoadScene("PilihLevelSejarah");
    }

    public void TombolKeluar()
    {
        if (panelKeluar != null)
        {
            panelKeluar.SetActive(true);
        }
    }

    public void KonfirmasiKeluar_Ya()
    {
        Application.Quit();
    }

    public void KonfirmasiKeluar_Batal()
    {
        if (panelKeluar != null) panelKeluar.SetActive(false);
        if (panelPause != null) panelPause.SetActive(true);
    }
}
