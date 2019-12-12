using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "IATCollection")]
public class IATCollection : ScriptableObject
{
    public IATKey key;
    public List<Sprite> IATobjects;       
}

public enum IATKey
{
    White,
    Black,
    Good,
    Bad,
    Bird,
    Mammal,
    None
};