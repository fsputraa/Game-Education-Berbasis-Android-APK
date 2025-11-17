using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataSoalPengetahuanUmum
{
    public string soal;
    public string[] opsiJawaban = new string[4];
    public int jawabanBenar;
}

public class SoalPengetahuanUmum : MonoBehaviour
{
    public Text teksSoal;
    public Button tombolA, tombolB, tombolC, tombolD;

    public DataSoalPengetahuanUmum[] daftarSoal;

    private int levelSaatIni;
    private string mapelDipilih;

    private bool sudahMenjawab = false;

    private UIPengetahuanUmum uiPengetahuanUmum;

    void Start()
    {
        mapelDipilih = PlayerPrefs.GetString("mapelDipilih", "PengetahuanUmum");
        levelSaatIni = PlayerPrefs.GetInt("levelDipilih", 1);

        // Cek apakah soal sudah habis
        if (levelSaatIni > daftarSoal.Length)
        {
            KumpulanSuara.instance.Panggil_sfx(5); // ✅ Suara menang
            PlayerPrefs.DeleteKey("HitPoint");     // ✅ Reset nyawa
            PlayerPrefs.DeleteKey("Timer");
            SceneManager.LoadScene("PilihLevel" + mapelDipilih);
            return;
        }

        TampilkanSoal();
        uiPengetahuanUmum = FindObjectOfType<UIPengetahuanUmum>();

        tombolA.onClick.AddListener(() => PeriksaJawaban(0));
        tombolB.onClick.AddListener(() => PeriksaJawaban(1));
        tombolC.onClick.AddListener(() => PeriksaJawaban(2));
        tombolD.onClick.AddListener(() => PeriksaJawaban(3));
    }

    void TampilkanSoal()
    {
        int index = levelSaatIni - 1;

        if (index >= 0 && index < daftarSoal.Length)
        {
            teksSoal.text = daftarSoal[index].soal;
            tombolA.GetComponentInChildren<Text>().text = daftarSoal[index].opsiJawaban[0];
            tombolB.GetComponentInChildren<Text>().text = daftarSoal[index].opsiJawaban[1];
            tombolC.GetComponentInChildren<Text>().text = daftarSoal[index].opsiJawaban[2];
            tombolD.GetComponentInChildren<Text>().text = daftarSoal[index].opsiJawaban[3];
        }
        else
        {
            teksSoal.text = "Soal tidak ditemukan!";
        }
    }

    void PeriksaJawaban(int jawaban)
    {
        if (sudahMenjawab) return;
        sudahMenjawab = true;

        int index = levelSaatIni - 1;

        if (jawaban == daftarSoal[index].jawabanBenar)
        {
            Debug.Log("Jawaban benar!");
            KumpulanSuara.instance.Panggil_sfx(1); // ✅ Jawaban benar

            int levelBerikutnya = levelSaatIni + 1;
            string keyLevelBerikutnya = $"{mapelDipilih}_Level{levelBerikutnya}_Unlocked";

            PlayerPrefs.SetInt("levelDipilih", levelBerikutnya);
            PlayerPrefs.SetInt(keyLevelBerikutnya, 1);
            PlayerPrefs.Save();

            KumpulanSuara.instance.Panggil_sfx(3); // ✅ Next level
            SceneManager.LoadScene("Game" + mapelDipilih);
        }
        else
        {
            Debug.Log("Jawaban salah");
            KumpulanSuara.instance.Panggil_sfx(2); // ✅ Jawaban salah

            if (uiPengetahuanUmum != null)
            {
                uiPengetahuanUmum.KurangiNyawa();

                if (PlayerPrefs.GetInt("HitPoint") <= 0)
                {
                    KumpulanSuara.instance.Panggil_sfx(4); // ✅ Kalah
                    SceneManager.LoadScene("PilihLevel" + mapelDipilih);
                }
                else
                {
                    SceneManager.LoadScene("Game" + mapelDipilih);
                }
            }
        }
    }
}
