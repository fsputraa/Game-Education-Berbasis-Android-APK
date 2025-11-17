using UnityEngine;

public class PlayerPrefsReset : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("🔥 Semua PlayerPrefs sudah direset!");
    }
}
