using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelesai : MonoBehaviour
{
    public Text Teks_Score, Teks_TotalScore;

    public void Start()
    {
        // Ambil high score sebelumnya (jika belum ada, pakai 0)
        int highScoreSebelumnya = PlayerPrefs.GetInt("score", 0);

        // Update high score jika skor baru lebih tinggi
        if (Data.DataScore > highScoreSebelumnya)
        {
            PlayerPrefs.SetInt("score", Data.DataScore);
            PlayerPrefs.Save(); // simpan langsung ke device
        }

        // Tampilkan skor akhir dan high score ke UI
        Teks_Score.text = Data.DataScore.ToString();
        Teks_TotalScore.text = PlayerPrefs.GetInt("score").ToString();
    }
}
