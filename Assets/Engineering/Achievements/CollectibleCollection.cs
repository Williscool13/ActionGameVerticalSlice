using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DataContainers/CharmCollection")]
public class CollectiblesCollection : ScriptableObject
{
    public bool[] charmCollection = new bool[9];

    public void Reset() {
        for (int i=0; i<charmCollection.Length; i++) {
            charmCollection[i] = false;
        }
    }
}
