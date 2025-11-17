using UnityEngine;
using UnityEngine.SceneManagement;

public class PilihMapelController : MonoBehaviour
{
    public void PilihMapel(string namaMapel)
    {
        PlayerPrefs.SetString("mapelDipilih", namaMapel);
        SceneManager.LoadScene("PilihLevelBahasaIndonesia");
    }
}
