using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Round")]
public class Round : ScriptableObject
{
    public List<IATKeys> leftKeys;
    public List<IATKeys> rightKeys;
}