using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Build_Input_Manager : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private Vector3 last_position;

    [SerializeField] private LayerMask placement_layer_mask;

    private InputAction mousePositionAction;

    public event Action onClicked, onExit;

    [SerializeField] private GameObject Build_Menu;
    [SerializeField] private Placement_System placement_system;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (placement_system.selected_Object_index == -1)
            {
                // Paljenje ili gašenje Build Menu-a
                Build_Menu.SetActive(!Build_Menu.activeSelf);
            }
            else
            {
                // Izlazak iz trenutnog režima
                onExit?.Invoke();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            onExit?.Invoke();
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Obrađivanje klika miša
            onClicked?.Invoke();
        }
    }



    public bool Is_Pointer_Over_UI() => EventSystem.current.IsPointerOverGameObject();


    private void Awake()
    {
        // Kreiraj novu Input Action za praćenje pozicije miša
        mousePositionAction = new InputAction("Mouse Position", InputActionType.Value, "<Mouse>/position");
        mousePositionAction.Enable();
    }

    public Vector3 Get_Selected_Map_Position()
    {
        Vector2 mousePos = mousePositionAction.ReadValue<Vector2>();
        Vector3 screenPos = new Vector3(mousePos.x, mousePos.y, m_camera.nearClipPlane);

        Ray ray = m_camera.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, placement_layer_mask))
        {
            last_position = hit.point;
        }

        return last_position;
    }

    private void OnDestroy()
    {
        // Onemogući akciju kada se objekat uništi
        mousePositionAction.Disable();
    }

}
