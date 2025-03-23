#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy_Manger))]
public class Enemy_Manger_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        // Crtamo default Inspector
        DrawDefaultInspector();
        // Pribavimo referencu na ciljnu skriptu
        Enemy_Manger enemyManager = (Enemy_Manger)target;

        // Dodajemo prazan prostor radi preglednosti
        EditorGUILayout.Space();

        // Dodajemo dugme u Inspector
        if (GUILayout.Button("Spawn Wave"))
        {
            // Pozivamo željenu funkciju
            enemyManager.Spawn_Wave();

            // Ako želiš da se scena osveži (npr. da se vide promene boje) pozovi:
            EditorUtility.SetDirty(enemyManager);
        }
        if (GUILayout.Button("Test Spawn Wave"))
        {
            // Pozivamo željenu funkciju
            enemyManager.Test_Spawn_Wave();

            // Ako želiš da se scena osveži (npr. da se vide promene boje) pozovi:
            EditorUtility.SetDirty(enemyManager);
        }
    }
}
#endif