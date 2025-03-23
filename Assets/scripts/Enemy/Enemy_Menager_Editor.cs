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
            // Pozivamo �eljenu funkciju
            enemyManager.Spawn_Wave();

            // Ako �eli� da se scena osve�i (npr. da se vide promene boje) pozovi:
            EditorUtility.SetDirty(enemyManager);
        }
        if (GUILayout.Button("Test Spawn Wave"))
        {
            // Pozivamo �eljenu funkciju
            enemyManager.Test_Spawn_Wave();

            // Ako �eli� da se scena osve�i (npr. da se vide promene boje) pozovi:
            EditorUtility.SetDirty(enemyManager);
        }
    }
}
#endif