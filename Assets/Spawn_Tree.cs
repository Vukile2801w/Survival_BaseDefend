using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Spawn_Tree : MonoBehaviour
{
    [SerializeField] GameObject[] keep_away;
    [SerializeField] GameObject[] trees_prefabs; // Prefabi za stabla
    [SerializeField] GameObject new_tree_parent; // Parent objekat za organizaciju

    [SerializeField] Grid grid; // Grid za pozicioniranje

    [SerializeField] Vector3 pos; // Početna globalna pozicija u gridu
    [SerializeField] Vector3Int size_by_cell; // Dimenzije u broju ćelija

    [SerializeField][Range(0, 1)] float chance_to_spawn;
    [SerializeField][Range(0, 1f)] float scale;


    bool Is_Full(Vector3Int pos)
    {
        for (int i = 0; i < keep_away.Length; i++)
        {
            Vector3Int object_pos = grid.WorldToCell(keep_away[i].transform.position);
            if (object_pos == pos) return true;
        }
        return false;
    }


    private void Awake()
    {
        // Konvertuj početnu globalnu poziciju u grid indeks
        Vector3Int startCell = grid.WorldToCell(pos);

        for (int x = 0; x < size_by_cell.x; x++)
        {
            for (int z = 0; z < size_by_cell.z; z++)
            {
                // Računaj trenutnu poziciju ćelije u grid indeksima
                Vector3Int cellPosition = new Vector3Int(
                    startCell.x + x,
                    startCell.y,
                    startCell.z + z
                );

                // Dobij centar ćelije iz grida
                Vector3 cellCenter = grid.GetCellCenterWorld(cellPosition);

                if (Is_Full(cellPosition))
                {
                    continue;
                }

                // Nasumični offset unutar ćelije
                Vector3 offset = new Vector3(
                    Random.Range(-grid.cellSize.x / 2, grid.cellSize.x / 2),
                    0,
                    Random.Range(-grid.cellSize.z / 2, grid.cellSize.z / 2)
                );

                // Konačna pozicija za stablo
                Vector3 new_pos = cellCenter + offset;

                // Izaberi nasumičan prefab iz niza
                GameObject treePrefab = trees_prefabs[Random.Range(0, trees_prefabs.Length)];


                float noise = Mathf.PerlinNoise(new_pos.x * scale, new_pos.z * scale);

                if (noise > (1 - chance_to_spawn))
                {
                    // Instanciraj stablo
                    Instantiate(treePrefab, new_pos, Quaternion.identity, new_tree_parent.transform);

                    Debug.Log($"New Tree at {new_pos}");
                }
            }
        }
    }
}
