using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Obj_Drag : MonoBehaviour
{
    [HideInInspector] public Vector2 SavePosisi;
    [HideInInspector] public bool IsDiAtasObj;

    Transform SaveObj;

    public int ID;
    public Text Teks;

    [Space]
    public UnityEvent OnDragBenar;

    void Start()
    {
        SavePosisi = transform.position;
    }

    void Update()
    {

    }

    private void OnMouseDown()
    {
        KumpulanSuara.instance.Panggil_sfx(0);
    }

    private void OnMouseUp()
    {
        if (IsDiAtasObj)
        {
            int ID_TempatDrop = SaveObj.GetComponent<Tempat_Drop>().ID;

            if (ID == ID_TempatDrop)
            {
                transform.SetParent(SaveObj);
                transform.localPosition = Vector3.zero;
                transform.localScale = new Vector2(1f, 1f);

                SaveObj.GetComponent<SpriteRenderer>().enabled = false;

                SaveObj.GetComponent<Rigidbody2D>().simulated = false;
                SaveObj.GetComponent<BoxCollider2D>().enabled = false;

                gameObject.GetComponent<BoxCollider2D>().enabled = false;

                OnDragBenar.Invoke();

                GameSystem.instance.DataSaatIni++;
                Data.DataScore += 50;

                KumpulanSuara.instance.Panggil_sfx(1);
            }
            else
            {
                transform.position = SavePosisi;

                Data.DataDarah--;
                Data.DataScore = Mathf.Max(0, Data.DataScore - 30); // Skor -30 jika salah, tapi tidak boleh < 0

                KumpulanSuara.instance.Panggil_sfx(2);
            }
        }
        else
        {
            transform.position = SavePosisi;
        }
    }

    private void OnMouseDrag()
    {
        if (!GameSystem.instance.GameSelesai)
        {
            Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Pos;
        }
    }

    private void OnTriggerStay2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Drop"))
        {
            IsDiAtasObj = true;
            SaveObj = trig.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Drop"))
        {
            IsDiAtasObj = false;
        }
    }
}
