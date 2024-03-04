using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

[CreateAssetMenu(fileName = "New Score Data", menuName = "Score")]
public class ScoreData : ScriptableObject
{
    public int score;
    public int itemsCollected;
}
