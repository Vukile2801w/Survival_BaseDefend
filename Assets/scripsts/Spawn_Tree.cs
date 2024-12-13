using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Spawn_Tree : MonoBehaviour
{
    [SerializeField] GameObject[] objects_to_avoid;
    [SerializeField] GameObject[] trees_prefabs; // Prefabi za stabla
    [SerializeField] GameObject new_tree_parent; // Parent objekat za organizaciju

    [SerializeField] Grid grid; // Grid za pozicioniranje

    [SerializeField] Vector3 pos; // Početna globalna pozicija u gridu
    [SerializeField] Vector3Int size_by_cell; // Dimenzije u broju ćelija

    [SerializeField][Range(0, 1)] float chance_to_spawn;
    [SerializeField][Range(0, 1f)] float scale;

    /// <summary>
    /// Vraća sve objekte (kolajdere) koji se nalaze unutar određene ćelije.
    /// </summary>
    /// <param name="cellCenter">Centar ćelije u svetu.</param>
    /// <param name="cellSize">Veličina ćelije.</param>
    /// <returns>Lista kolajdera unutar ćelije.</returns>
    /// <summary>
    /// Vraća niz `Collider` objekata koji se nalaze unutar određene ćelije.
    /// </summary>
    /// <param name="cellCenter">Centar ćelije u svetu.</param>
    /// <param name="cellSize">Veličina ćelije.</param>
    /// <returns>Niz `Collider` objekata unutar ćelije.</returns>
    Collider[] Get_Colliders_In_Cell(Vector3 cellCenter, Vector3 cellSize)
    {
        Vector3 halfCellSize = cellSize / 2; // Polovina veličine ćelije
        return Physics.OverlapBox(cellCenter, halfCellSize, Quaternion.identity);
    }


    /// <summary>
    /// Proverava da li ćelija sadrži objekte koji treba da se ignorišu.
    /// </summary>
    /// <param name="cellCenter">Centar ćelije u svetu.</param>
    /// <param name="cellSize">Veličina ćelije.</param>
    /// <returns>True ako sadrži objekte za ignorisanje, False inače.</returns>
    bool Is_Containing_Ignore_Object(Vector3 cellCenter, Vector3 cellSize)
    {
        Collider[] colliders = Get_Colliders_In_Cell(cellCenter, cellSize);

        foreach (var collider in colliders)
        {
            foreach (var avoidObject in objects_to_avoid)
            {
                if (collider.gameObject == avoidObject)
                {
                    return true; // Nađen je objekat koji treba da se ignoriše
                }
            }
        }
        return false; // Nema objekata koji treba da se ignorišu
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

                // Preskoči ćeliju ako sadrži objekat za ignorisanje
                if (Is_Containing_Ignore_Object(cellCenter, grid.cellSize))
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

                    
                }
            }
        }
    }
}
