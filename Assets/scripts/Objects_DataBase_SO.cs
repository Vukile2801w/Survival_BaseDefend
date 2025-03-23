using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




[CreateAssetMenu(fileName = "ObjectsDataBase", menuName = "ScriptableObjects/ObjectsDataBase")]
public class Objects_DataBase_SO : ScriptableObject
{
    public List<Object_Data> objectData;
}


[Serializable]
public class Object_Data
{
    [field: SerializeField]
    public string Name { get; private set; }
    
    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;


    [field: SerializeField]
    public GameObject prefab { get; private set; }

}
