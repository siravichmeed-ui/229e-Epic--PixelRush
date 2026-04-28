using UnityEngine;

[System.Serializable]
public class PatternData
{
    public string name;

    [Header("ตำแหน่ง Y ที่จะ spawn")]
    public float[] spawnHeights;

    [Header("เลือก obstacle ได้")]
    public GameObject[] obstacles;

    [Header("หน่วงเวลาระหว่าง spawn")]
    public float delay = 0.5f;
}