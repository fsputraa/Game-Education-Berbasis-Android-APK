using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIPilihanGanda : MonoBehaviour
{
    public Text teksLevel, teksWaktu, teksScore;
    public RectTransform UI_Darah;
    public GameObject guiPause;
    public GameObject guiTransisi;
    public GameObject CanvasKeluar; // ✅ Untuk tombol keluar

    float timer;

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (Data.DataWaktu > 0)
            {
                timer += Time.deltaTime;
                if (timer >= 1)
                {
                    Data.DataWaktu--;
                    timer = 0;
                }
            }
            else
            {
                SceneManager.LoadScene("GameSelesai");
            }

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        teksLevel.text = (Data.DataLevel + 1).ToString();
        teksScore.text = Data.DataScore.ToString();

        int menit = Mathf.FloorToInt(Data.DataWaktu / 60);
        int detik = Mathf.FloorToInt(Data.DataWaktu % 60);
        teksWaktu.text = menit.ToString("00") + " : " + detik.ToString("00");

        UI_Darah.sizeDelta = new Vector2(50f * Data.DataDarah, 50f);
    }

    public void BtnPause(bool pause)
    {
        if (pause)
        {
            guiPause.SetActive(true);

            Animator anim = guiPause.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("end");
            }

            StartCoroutine(DelayPause());
        }
        else
        {
            StopAllCoroutines();
            Time.timeScale = 1;
            guiPause.SetActive(false);
        }
    }

    IEnumerator DelayPause()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 0;
    }

    public void BtnRestart()
    {
        Time.timeScale = 1;

        Data.DataWaktu = 90 * 10;
        Data.DataScore = 0;
        Data.DataDarah = 5;
        Data.DataLevel = 0;

        SceneManager.LoadScene("Game0");
    }

    public void BtnMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    // ✅ Fungsi tombol keluar
    public void BtnKeluar()
    {
        Time.timeScale = 0; // pause gameplay

        if (CanvasKeluar != null)
        {
            CanvasKeluar.SetActive(true);

            Animator anim = CanvasKeluar.GetComponent<Animator>();
            if (anim != null)
            {
                anim.Play("start", 0, 0); // animasi start di-reset ke awal
            }
        }
    }
}
