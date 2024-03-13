using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Quota Data", menuName = "Quota")]

public class QuotaData : ScriptableObject
{
    public int quotaRemain = 100;
    public float quotaLevel = 1;
    public int daysLeft = 3;
}
