using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Preview_System : MonoBehaviour
{
    [SerializeField] private float preview_offset_y = 0.05f;

    [SerializeField] private GameObject cell_indicator;
    private GameObject preview_object;

    [SerializeField] private Material preview_material_prefab;
    private Material preview_material_instance;

    [SerializeField] private Renderer[] m_renderer;


    private void Start()
    {
        preview_material_instance = new Material(preview_material_prefab);
        cell_indicator.SetActive(false);
        m_renderer = cell_indicator.GetComponentsInChildren<Renderer>();
    }

    public void Start_Preview(GameObject prefab, Vector2Int size)
    {
        preview_object = Instantiate(prefab);
        PreparePeview(preview_object);
        Prepare_Cursor(size);

        cell_indicator.SetActive(true);
    }

    private void Prepare_Cursor(Vector2Int size)
    {
        if (size.x <= 0 || size.y <= 0)
        {
            Debug.LogWarning("Invalid size for cursor.");
            return;
        }

        cell_indicator.transform.localScale = new Vector3(size.x, 1, size.y);
        Change_Mat_Text_Scale(size);
    }

    private void Set_Tag_For_All_Children(GameObject _object, string tag)
    {
        if (string.IsNullOrEmpty(tag))
        {
            Debug.LogWarning("Tag cannot be null or empty.");
            return;
        }

        _object.tag = tag;

        for (int i = 0; i < _object.transform.childCount; i++)
        {
            GameObject child = _object.transform.GetChild(i).gameObject;
            Set_Tag_For_All_Children(child, tag);
        }
    }



    private void Turn_Off_Colider_For_All_Children(GameObject _object)
    {
        BoxCollider collider = _object.GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        for (int i = 0; i < _object.transform.childCount; i++)
        {
            GameObject child = _object.transform.GetChild(i).gameObject;
            Turn_Off_Colider_For_All_Children(child);
        }
    }


    private void PreparePeview(GameObject preview_object)
    {
        Renderer[] renderers = preview_object.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = preview_material_instance;
            }

            renderer.materials = materials;
        }


        Set_Tag_For_All_Children(preview_object, "Preview Object");
        Turn_Off_Colider_For_All_Children(preview_object);



        Debug.Log(preview_object.tag);
    }



    public void Stop_Preview()
    {
        cell_indicator.SetActive(false);

        if (preview_object != null)
        {
            Destroy(preview_object);
        }
    }


    public void UpdatePosition(Vector3 pos, bool valid)
    {
        Move_Perview(pos);
        Move_Cursor(pos);
        Apply_Feedback(valid);
    }

    private void Apply_Feedback(bool valid)
    {
       Color c = valid ? Color.green : Color.red;
        Change_Mat_Color(c);
        c.a = 0.5f;
        preview_material_instance.color = c;
    }

    private void Move_Cursor(Vector3 pos)
    {
        cell_indicator.transform.position = new Vector3(pos.x, 0.15f, pos.z);
    }

    private void Move_Perview(Vector3 pos)
    {
        preview_object.transform.position = new Vector3(pos.x, pos.y + preview_offset_y, pos.z);

    }

    void Change_Mat_Text_Scale(Vector2Int size)
    {
        foreach (var item in m_renderer)
        {
            item.material.mainTextureScale = new Vector2(size.x, size.y);
        }
    }
    void Change_Mat_Color(Color mat_color)
    {
        foreach (var item in m_renderer)
        {
            item.material.color = mat_color;
        }
    }


}
