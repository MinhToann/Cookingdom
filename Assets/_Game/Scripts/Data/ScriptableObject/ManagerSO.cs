using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Manager SO")]
public class ManagerSO : ScriptableObject
{
    [field: SerializeField] public List<ObjectSO> listObjects = new List<ObjectSO>();
}
