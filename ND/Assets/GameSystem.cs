using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data
{
    public static int DataLevel, DataScore, DataWaktu, DataDarah;
}

public class GameSystem : MonoBehaviour
{
    int MaxLevel = 6;

    public static GameSystem instance;

    [Header("Data Permainan")]
    public bool GameAktif;
    public bool GameSelesai;
    public int Target, DataSaatIni;

    [Header("Komponen UI")]
    public Text Teks_Level;
    public Text Teks_Waktu, Teks_Score;
    public RectTransform UI_Darah;

    [Header("Obj UI")]
    public GameObject Gui_Pause;
    public GameObject Gui_Transisi;

    [Space]
    public bool SistemAcak;

    [System.Serializable]
    public class DataGame
    {
        public string Nama;
        public Sprite Gambar;
    }

    [Header("Settingan Standar")]
    public DataGame[] DataPermainan;

    [Space]
    public Obj_TempatDrop[] Drop_Tempat;
    public Obj_Drag[] Drag_obj;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameAktif = false;
        GameSelesai = false;
        ResetData();
        AcakSoal();
        Target = Drop_Tempat.Length;

        DataSaatIni = 0;
        GameAktif = true;
    }

    void ResetData()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game0")
        {
            Data.DataWaktu = 90 * 10;
            Data.DataScore = 0;
            Data.DataDarah = 5;
            Data.DataLevel = 0;
        }
    }

    [HideInInspector] public List<int> _AcakSoal = new List<int>();
    [HideInInspector] public List<int> _AcakPos = new List<int>();
    int rand;
    int rand2;

    public void AcakSoal()
    {
        _AcakSoal.Clear();
        _AcakPos.Clear();

        _AcakSoal = new List<int>(new int[Drag_obj.Length]);

        for (int i = 0; i < _AcakSoal.Count; i++)
        {
            rand = Random.Range(1, DataPermainan.Length);
            while (_AcakSoal.Contains(rand))
                rand = Random.Range(1, DataPermainan.Length);

            _AcakSoal[i] = rand;

            Drag_obj[i].ID = rand - 1;
            Drag_obj[i].Teks.text = DataPermainan[rand - 1].Nama;
        }

        _AcakPos = new List<int>(new int[Drop_Tempat.Length]);

        for (int i = 0; i < _AcakPos.Count; i++)
        {
            rand2 = Random.Range(1, _AcakSoal.Count + 1);
            while (_AcakPos.Contains(rand2))
                rand2 = Random.Range(1, _AcakSoal.Count + 1);

            _AcakPos[i] = rand2;

            Drop_Tempat[i].Drop.ID = _AcakSoal[rand2 - 1] - 1;
            Drop_Tempat[i].Gambar.sprite = DataPermainan[Drop_Tempat[i].Drop.ID].Gambar;
        }
    }

    float s;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AcakSoal();

        if (GameAktif && !GameSelesai)
        {
            if (Data.DataWaktu > 0)
            {
                s += Time.deltaTime;
                if (s >= 1)
                {
                    Data.DataWaktu--;
                    s = 0;
                }
            }
        }

        SetInfoUI();

        // ✅ FIX: Gabungin kondisi kalah biar suara & transisi nggak dobel
        if ((Data.DataWaktu <= 0 || Data.DataDarah <= 0) && !GameSelesai)
        {
            GameAktif = false;
            GameSelesai = true;

            Gui_Transisi.GetComponent<UI_Control>().Btn_Pindah("GameSelesai");
            KumpulanSuara.instance.Panggil_sfx(4);
        }

        // ✅ Lolos semua soal
        if (DataSaatIni >= Target)
        {
            GameSelesai = true;
            GameAktif = false;

            if (Data.DataLevel < (MaxLevel - 1))
            {
                Data.DataLevel++;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Game" + Data.DataLevel);
                KumpulanSuara.instance.Panggil_sfx(3);
            }
            else
            {
                Gui_Transisi.GetComponent<UI_Control>().Btn_Pindah("GameSelesai");
                KumpulanSuara.instance.Panggil_sfx(5);
            }
        }
    }

    public void SetInfoUI()
    {
        Teks_Level.text = (Data.DataLevel + 1).ToString();

        int Menit = Mathf.FloorToInt(Data.DataWaktu / 60);
        int Detik = Mathf.FloorToInt(Data.DataWaktu % 60);
        Teks_Waktu.text = Menit.ToString("00") + " : " + Detik.ToString("00");

        Teks_Score.text = Data.DataScore.ToString();

        UI_Darah.sizeDelta = new Vector2(50f * Data.DataDarah, 50f);
    }

    public void Btn_Pause(bool pause)
    {
        if (pause)
        {
            GameAktif = false;
            Gui_Pause.SetActive(true);
        }
        else
        {
            GameAktif = true;
            Gui_Pause.SetActive(false);
        }
    }
}
