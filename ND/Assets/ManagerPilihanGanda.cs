using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ManagerPilihanGanda : MonoBehaviour
{
    public Text textSoal;
    public Button[] tombolJawaban;
    public DataGamePG[] DaftarSoal;

    private List<int> indexSoalTersisa = new List<int>();
    private int indexSoalSekarang;

    private string[] pilihanTeracak;
    private int jawabanBenarTeracak;

    void Start()
    {
        for (int i = 0; i < DaftarSoal.Length; i++)
        {
            indexSoalTersisa.Add(i);
        }

        AmbilSoalAcak();
        TampilkanSoal();
    }

    void AmbilSoalAcak()
    {
        if (indexSoalTersisa.Count == 0)
        {
            KumpulanSuara.instance.Panggil_sfx(5);
            SceneManager.LoadScene("GameSelesai");
            return;
        }

        int randomIndex = Random.Range(0, indexSoalTersisa.Count);
        indexSoalSekarang = indexSoalTersisa[randomIndex];
        indexSoalTersisa.RemoveAt(randomIndex);

        AcakOpsi();
    }

    void AcakOpsi()
    {
        var soal = DaftarSoal[indexSoalSekarang];
        List<string> opsi = new List<string>(soal.Pilihan);
        string jawabanBenarTeks = soal.Pilihan[soal.JawabanBenar];

        pilihanTeracak = new string[opsi.Count];
        List<int> indeksTersisa = new List<int> { 0, 1, 2, 3 };

        for (int i = 0; i < pilihanTeracak.Length; i++)
        {
            int acakIndex = Random.Range(0, opsi.Count);
            pilihanTeracak[i] = opsi[acakIndex];

            if (pilihanTeracak[i] == jawabanBenarTeks)
            {
                jawabanBenarTeracak = i;
            }

            opsi.RemoveAt(acakIndex);
        }
    }

    void TampilkanSoal()
    {
        var soal = DaftarSoal[indexSoalSekarang];
        textSoal.text = soal.Soal;

        for (int i = 0; i < tombolJawaban.Length; i++)
        {
            tombolJawaban[i].GetComponentInChildren<Text>().text = pilihanTeracak[i];
            int pilihanIndex = i;

            tombolJawaban[i].onClick.RemoveAllListeners();
            tombolJawaban[i].onClick.AddListener(() => CekJawaban(pilihanIndex));
        }
    }

    void CekJawaban(int jawaban)
    {
        KumpulanSuara.instance.Panggil_sfx(0);

        if (jawaban == jawabanBenarTeracak)
        {
            Data.DataScore += 200;
            Data.DataLevel++;
            KumpulanSuara.instance.Panggil_sfx(3);

            AmbilSoalAcak();
            TampilkanSoal();
        }
        else
        {
            Data.DataScore = Mathf.Max(0, Data.DataScore - 30);
            Data.DataDarah--;

            if (Data.DataDarah <= 0)
            {
                KumpulanSuara.instance.Panggil_sfx(4);
                SceneManager.LoadScene("GameSelesai");
            }
            else
            {
                KumpulanSuara.instance.Panggil_sfx(2);
                TampilkanSoal(); // tampilkan ulang dengan opsi yang sama
            }
        }
    }
}
