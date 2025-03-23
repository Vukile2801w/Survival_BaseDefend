using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
#endif
using System.IO;



[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Enemy_Manger : MonoBehaviour
{
    
    [SerializeField] Spawn_Area[] spawn_areas;

    [SerializeField] TextMeshProUGUI timer_text;


    [SerializeField] float Group_Spawn_Radius = 1f;
    [SerializeField] int Enemy_per_group = 5;
    [SerializeField] GameObject Enemy_prefab;
    [SerializeField] float time_between_groups = 30f;
    private float lastSpawnTime = 0f;

    [SerializeField] bool debug = false;

    Vector3 point = new Vector3();


    public IEnumerator WaitFor(float time) { yield return new WaitForSecondsRealtime(time); Spawn_Wave(); }

    private void Start()
    {
        
    }

    public void Test_Spawn_Wave()
    {
        for (int i = 0; i < 1000; i++)
        {
            Spawn_Wave();
        }
    }

    public void Spawn_Wave()
    {
        Debug.Log("Spawning wave");


        int area_index = UnityEngine.Random.Range(0, spawn_areas.Length);
        Spawn_Area area = spawn_areas[area_index];




        // Point of spawn of Group
        float randomX = UnityEngine.Random.Range(area.center.x - area.size.x / 2, area.center.x + area.size.x / 2);
        float randomY = area.center.y + 1.14f; 
        float randomZ = UnityEngine.Random.Range(area.center.z - area.size.z / 2, area.center.z + area.size.z / 2);
        point = new Vector3(randomX, randomY, randomZ);

        Group_Spawn_Area group_spawn_area = new Group_Spawn_Area(point, Group_Spawn_Radius);
        

        // Point of spawn of Individual's

        Vector3[] Enemy_Points = new Vector3[Enemy_per_group];
        for (int i = 0; i < Enemy_Points.Length; i++)
        {
            Vector2 random_point = GetRandomPointInCircle(Group_Spawn_Radius);
            Enemy_Points[i] = new Vector3(random_point.x + point.x, point.y, random_point.y + point.z);
        }

        for (int i = 0; i < Enemy_Points.Length; i++)
        {
            GameObject enemy = Instantiate(Enemy_prefab, Enemy_Points[i], Quaternion.identity);
            enemy.transform.parent = transform.Find("Enemies").transform;
        }



        // Debug
        if (debug)
        {
            transform.Find("Debug_Point").gameObject.SetActive(true);

            SavePointToFile(point);
            transform.Find("Debug_Point").position = point;

            // Area of spawn
            area.display_color = Color.green;
            StartCoroutine(color_reset(area, 1f));
        }
        else
        {
            transform.Find("Debug_Point").gameObject.SetActive(false);

        }
    }

    IEnumerator color_reset(Spawn_Area area, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        area.display_color = area.debug_color;
    }

    void Update()
    {

        timer_text.text = (time_between_groups - lastSpawnTime).ToString("F2");

        // Check if enough time has passed since the last spawn
        if (lastSpawnTime > time_between_groups)
        {
            Spawn_Wave();
            lastSpawnTime = 0;  // Update the last spawn time
            time_between_groups -= 1;
            Enemy_per_group += 1;
            
            if (time_between_groups < 5) time_between_groups = 5;
            if (Enemy_per_group > 25) time_between_groups = 25;

        }
        else
        {
            lastSpawnTime += Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;




        foreach (Spawn_Area area in spawn_areas)
        {


            Gizmos.color = area.display_color;
            Gizmos.DrawWireCube(area.center, area.size);

        }
    }



    void SavePointToFile(Vector3 point)
    {
        string folderPath = Application.dataPath + "/scripts/Enemy";
        string filePath = folderPath + "/spawn_points.txt";

        // Proveri da li direktorijum postoji, ako ne - kreiraj ga
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string x = point.x.ToString().Replace(",", ".");
        string z = point.z.ToString().Replace(",", ".");


        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"{x},{z}");
        }

        Debug.Log($"Point saved: {point} to {filePath}");
    }

    Vector2 GetRandomPointInCircle(float radius)
    {
        float theta = UnityEngine.Random.Range(0f, 2f * Mathf.PI); // Nasumični ugao
        float r = Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f)) * radius; // Nasumični radijus

        float x = r * Mathf.Cos(theta);
        float y = r * Mathf.Sin(theta);

        return new Vector2(x, y);
    }
}

[Serializable]
class Spawn_Area
{
    public Vector3 center;
    public Vector3 size;

    public Color debug_color;
    [HideInInspector]
    public Color display_color;


    public Spawn_Area(Vector3 center, Vector3 size, Color color)
    {
        debug_color = color;
        this.center = center;
        this.size = size;

    }

    public Spawn_Area(Spawn_Area spawn_Area)
    {
        debug_color = spawn_Area.debug_color;
        this.center = spawn_Area.center;
        this.size = spawn_Area.size;

    }
}

class Group_Spawn_Area
{
    public Vector3 center;
    public float radius;

    public Group_Spawn_Area(Vector3 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }
}