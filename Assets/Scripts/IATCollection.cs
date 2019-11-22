using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "IATCollection")]
public class IATCollection : ScriptableObject
{
    public IATKeys key;
    public List<Sprite> IATobjects;       
}

public enum IATKeys
{
    White,
    Black,
    Safe,
    Dangerous
};