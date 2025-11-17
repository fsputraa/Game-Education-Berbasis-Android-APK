using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorController : MonoBehaviour
{
    public Button[] tombolLevel;
    private string mapelDipilih;

    void Start()
    {
        mapelDipilih = PlayerPrefs.GetString("mapelDipilih", "SeniRupa");

        for (int i = 0; i < tombolLevel.Length; i++)
        {
            int level = i + 1;
            string key = $"{mapelDipilih}_Level{level}_Unlocked";

            // Level 1 harus default terbuka
            int unlocked = PlayerPrefs.GetInt(key, (level == 1) ? 1 : 0);

            tombolLevel[i].interactable = unlocked == 1;
        }
    }

    public void MainkanLevel(int level)
    {
        PlayerPrefs.SetInt("levelDipilih", level);
        SceneManager.LoadScene("Game" + mapelDipilih);
    }
}
