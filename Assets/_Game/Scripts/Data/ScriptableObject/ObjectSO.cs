using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Object Data")]
public class ObjectSO : ScriptableObject
{
    [field: SerializeField] public PoolType poolType;
    [field: SerializeField] public ObjectType objectType;
    [field: SerializeField] public bool hasShell;
}
