using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public TextAsset assetSoal;

    private string[] soal;
    private string[,] soalBag;

    int indexSoal;
    int maxSoal;
    bool ambilSoal;
    char kunciJ;

    public Text tSoal, tOpsiA, tOpsiB, tOpsiC, tOpsiD;

    // Start is called before the first frame update
    void Start()
    {
        soal = assetSoal.ToString().Split('#');

        soalBag = new string[soal.Length, 6];
        maxSoal = soal.Length;
        OlahSoal();

        ambilSoal = true;
        TampilkanSoal();

        print(soalBag[1,3]);
    }

    private void OlahSoal()
    {
        for (int i = 0; i < soal.Length; i++)
        {
            string[] tempSoal = soal[i].Split('+');

            // 🔍 Validasi: Pastikan format soal terdiri dari 6 bagian
            if (tempSoal.Length != 6)
            {
                Debug.LogWarning($"⚠️ Soal ke-{i} formatnya salah! Kolom: {tempSoal.Length} → Cek baris ini:\n{soal[i]}");
                continue; // Lewati soal ini
            }

            for (int j = 0; j < tempSoal.Length; j++)
            {
                soalBag[i, j] = tempSoal[j];
            }
        }
    }


    private void TampilkanSoal()
    {
        if(indexSoal < maxSoal)
        {
            if (ambilSoal) 
            {
                tSoal.text = soalBag[indexSoal, 0];
                tOpsiA.text = soalBag[indexSoal, 1];
                tOpsiB.text = soalBag[indexSoal, 2];
                tOpsiC.text = soalBag[indexSoal, 3];
                tOpsiD.text = soalBag[indexSoal, 4];
                kunciJ = soalBag[indexSoal, 5][0];

                ambilSoal = false;
            }
        }
    }

    public void Opsi(string opsiHuruf)
    {
        CheckJawaban(opsiHuruf[0]);
        indexSoal++;
        ambilSoal = true;
        TampilkanSoal();
    }

    private void CheckJawaban(char huruf)
    {
        if (huruf.Equals(kunciJ))
        {
            print("Benar!");
        }
        else
        {
            print("Salah");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
