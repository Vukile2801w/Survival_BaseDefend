using System;
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
    [SerializeField] Vector2 noise_offset;

    [SerializeField] Placement_System placement_system;

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

    private void Start()
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
                    UnityEngine.Random.Range(-grid.cellSize.x / 2, grid.cellSize.x / 2),
                    0,
                    UnityEngine.Random.Range(-grid.cellSize.z / 2, grid.cellSize.z / 2)
                );

                // Konačna pozicija za stablo
                Vector3 new_pos = cellCenter + offset;


                System.Random random = new System.Random();

                // Izaberi nasumičan prefab iz niza
                GameObject treePrefab = trees_prefabs[random.Next(trees_prefabs.Length)];

                float noise = Mathf.PerlinNoise(new_pos.x * scale + noise_offset.x, new_pos.z * scale + noise_offset.y);

                if (noise > (1 - chance_to_spawn))
                {
                    // Dobij ID na osnovu prefaba stabla
                    int ID = placement_system.Get_Selected_Object_Index(treePrefab);

                    if (ID == -1)
                    {
                        Debug.LogError($"Nije pronađen prefab stabla u bazi podataka. Name: {treePrefab.name}");
                        continue;
                    }

                    // Sada pronađi indeks objekta u listi na osnovu ID-a
                    int obj_index = placement_system.database.objectData.FindIndex(data => data.ID == ID);

                    if (obj_index < 0)
                    {
                        Debug.LogError($"Nije pronađen objekat sa ID {ID} u bazi podataka.");
                        continue;
                    }

                    // Postavi objekat na mapu
                    GameObject new_Object = Instantiate(treePrefab, new_pos, Quaternion.identity, new_tree_parent.transform);

                    placement_system.placed_GameObject.Add(new_Object);

                    Vector3Int grid_pos_int = grid.WorldToCell(new_Object.transform.position);

                    placement_system.grid_data.Add_Object_At(
                        grid_pos_int,
                        placement_system.database.objectData[ID].Size,
                        placement_system.database.objectData[ID].ID,
                        Mathf.Max(placement_system.placed_GameObject.Count - 1, 0)
                        );
                }

            }
        }
    }
}
