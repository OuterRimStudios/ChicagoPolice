using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "IATCollection" + "")]
public class IATCollection : ScriptableObject
{
    public IATKeys keys;
    public List<GameObject> IATobjects;   
}

public enum IATKeys
{
    White,
    Black,
    Safe,
    Dangerous
};
