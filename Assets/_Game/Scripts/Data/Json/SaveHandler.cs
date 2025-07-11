using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] private string fileName = "GameLevels.json";

    private Dictionary<string, GameObject> prefabDictionary;
    public void OnInit()
    {

    }
    public static void OnSave()
    {

    }
    public static void OnLoad()
    {

    }
}
[System.Serializable]
public class MapInfo
{
    public string objName;
    public Vector3 position;
    public Vector3 rotation;
}
[System.Serializable]
public class MapData
{
    public int levelID;
    public string key;
    public List<MapInfo> listMap = new List<MapInfo>();
}