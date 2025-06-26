using System;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask groundLayer;

    private Player_Controle player_controle;
    private InputAction buildMenu, cancell;


    public event Action onPlace;
    public event Action<int> onBuildStarted;





    public Vector3 mousePos { private set; get; }
    public bool isBuilding { private set; get; } = false;

    [HideInInspector]
    public int objectID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player_controle = new Player_Controle();
        buildMenu = player_controle.Building.Building;
        cancell = player_controle.Building.Cancell;
    }

    void OnEnable()
    {
        player_controle.Enable();
    }

    void OnDisable()
    {
        player_controle.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        if (isBuilding)
        {
            BuildingUpdate();
        }
        else
        {
            if (buildMenu.WasPressedThisFrame())
            {
                Build(1);
            }
        }

    }


    public void Build(int id)
    {
        isBuilding = true;
        objectID = id;
        onBuildStarted?.Invoke(id); 

        new Log(true, "[InputHandler] ").log($"Started building object with ID: {id}");
    }

    Vector3? GetMousePos()
    {
        
        Ray screenToRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(screenToRay, out hit, float.MaxValue, groundLayer))
        {
            return hit.point;
        }

        return null;
    }

    void BuildingUpdate()
    {

        mousePos = GetMousePos() ?? mousePos;

        if (mousePos == Vector3.positiveInfinity) return;
        
        if (cancell.WasPressedThisFrame())
        {
            isBuilding = false;
        }

    }
}
