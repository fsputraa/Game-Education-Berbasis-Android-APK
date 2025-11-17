using System;

[System.Serializable]
public class DataGamePG
{
    public string Soal;
    public string[] Pilihan = new string[4]; // A, B, C, D
    public int JawabanBenar; // 0 = A, 1 = B, dst
}
