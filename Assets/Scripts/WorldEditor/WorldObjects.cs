using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Objects List", menuName = "ScriptableObjects/World Objects List", order = 1)]
public class WorldObjects : ScriptableObject
{
    public string listDescription = "";
    public List<WorldObject> objectList;
}
