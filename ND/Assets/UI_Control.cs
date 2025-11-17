using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Control : MonoBehaviour
{
    public bool IsTransisi, IsTidakPerlu;

    string SaveNamaScene;
    bool isPindahTriggered = false; // Untuk mencegah dobel pindah

    [Header("Tambahan untuk tombol keluar")]
    public GameObject Canvas_Keluar;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (IsTransisi && IsTidakPerlu)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPilihLevelClicked()
    {
        SceneManager.LoadScene("PilihMataPelajaran");
    }

    

    public void btn_suara(int id)
    {
        KumpulanSuara.instance.Panggil_sfx(0);
    }

    public void Btn_Pindah(string nama)
    {
        SaveNamaScene = nama;
        Debug.Log("Scene yang disimpan untuk pindah: " + SaveNamaScene);
        if (animator != null) animator.Play("end");
        else pindah(); // Fallback kalau animator null
    }

    public void Btn_Restart()
    {
        Debug.Log("Restart seluruh game, kembali ke game0");

        Data.DataLevel = 0;
        Data.DataScore = 0;
        Data.DataWaktu = 90 * 10;
        Data.DataDarah = 5;

        SceneManager.LoadScene("game0"); // INSTANT restart
    }

    public void Btn_MainMenu()
    {
        SaveNamaScene = "MainMenu";
        Debug.Log("Main Menu dipanggil");
        if (animator != null) animator.Play("end");
        else pindah(); // Kalau animator rusak
    }

    public void Btn_Keluar()
    {
        if (Canvas_Keluar != null)
        {
            Canvas_Keluar.SetActive(true);
        }
    }

    public void Btn_KeluarGame()
    {
        Debug.Log("Keluar game dipanggil");
        Application.Quit();
    }

    // Dipanggil dari event animasi
    public void pindah()
    {
        if (isPindahTriggered) return; // Cegah dobel pindah
        isPindahTriggered = true;

        if (string.IsNullOrEmpty(SaveNamaScene))
        {
            Debug.LogWarning("pindah() dipanggil tapi SaveNamaScene belum diset, dilewati.");
            return;
        }

        Debug.Log("Pindah ke scene: " + SaveNamaScene);
        SceneManager.LoadScene(SaveNamaScene);
    }
}
