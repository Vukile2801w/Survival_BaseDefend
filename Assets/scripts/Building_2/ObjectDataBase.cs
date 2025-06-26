using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDataBase", menuName = "ScriptableObjects/ObjectDataBase")]
public class ObjectDataBase : ScriptableObject
{
    public List<BuildingObject> objects;

    public BuildingObject GetObjectByID(int id)
    {
        return objects.FirstOrDefault(obj => obj.ID == id);
    }
}



[Serializable]
public class BuildingObject
{
    [field: SerializeField]
    public string name { private set; get; }

    [field: SerializeField]
    public GameObject prefab { private set; get; }

    [field: SerializeField]
    public Vector2Int size { private set; get; }


    [field: SerializeField]
    public int ID { private set; get; }

    public BuildingObject(string name, GameObject prefab, Vector2Int size, int id)
    {
        this.name = name;
        this.prefab = prefab;
        this.size = size;
        this.ID = id;
    }
}
