using UnityEngine.InputSystem;
using UnityEngine;
using System;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Player_Cuting : MonoBehaviour
{
    [SerializeField] private Animation_Menager animator;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxDistance = 5f; // Maksimalna daljina za sečenje
    [SerializeField] private float damage = 5f; // Maksimalna daljina za sečenje
    [SerializeField] private LayerMask treeLayerMask; // Layer mask za drveće

    [SerializeField] private Player_Inventory inventory;

    private Player_Controle player_controle;
    private InputAction cut;

    [Header("Debug")]
    [SerializeField] private DebugOption debugOptions = DebugOption.None;

    [SerializeField] private Mesh circle;

    private void Awake()
    {
        player_controle = new Player_Controle();
        cut = player_controle.Player.Cut;
    }

    private void OnEnable()
    {
        cut.Enable();
    }
    private void OnDisable()
    {
        cut.Disable();
    }

    private User_Tree? TryCut()
    {
        Collider[] trees = Physics.OverlapSphere(
            position: transform.position,
            radius: maxDistance,
            layerMask: treeLayerMask
        );

        Log("Found " + trees.Length + " trees in range", this);

        Vector3 forward = transform.forward;
        float maxAngle = 45f;

        float closestAngle = maxAngle + 1f;
        float closestDistance = float.MaxValue;

        Collider closestTree = null;

        foreach (var tree in trees)
        {
            Vector3 directionToTree = (tree.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(forward, directionToTree);

            float angledif = Mathf.Abs(angle - closestAngle);
            if (angledif < 5)
            {
                float distanceToTree = Vector3.Distance(transform.position, tree.transform.position);
                if (distanceToTree < closestDistance)
                {
                    closestDistance = distanceToTree;
                    closestTree = tree;
                }
            }

            else if (angle <= maxAngle && angle < closestAngle)
            {
                
                closestAngle = angle;
                closestTree = tree;
            }
        }

        Log("Closest tree angle: " + closestAngle, this);

        return closestTree?.GetComponent<User_Tree>();
    }


    // Update is called once per frame
    void Update()
    {
        Log("Updateing", this);

        bool can_cut = cut.triggered && !animator.IsPlayingAnimation("Axe_Cut", "Cut").isAllCorrect;
        Log("Can cut: " + can_cut, this);
        if (can_cut)
        {
            Log("Cutting", this);

            animator.Play("Cut");

            User_Tree tree = TryCut();
            if (tree == null) return;

            tree.Cut(damage, inventory);


        }
    }

    private void OnDrawGizmos()
    {
        if (debugOptions.HasFlag(DebugOption.ShowRay))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Ray(
                origin: transform.position + new Vector3(0, 1, 0),
                direction: transform.forward * maxDistance
                    ));
        }


        if (circle != null && debugOptions.HasFlag(DebugOption.ShowCircle))
        {
            Gizmos.color = Color.green;
            // Iscrtavanje kruga na horizontalnoj ravni
            Gizmos.DrawWireMesh(
                mesh: circle,
                position: new Vector3(transform.position.x, 0.14f, transform.position.z),
                rotation: Quaternion.identity,
                scale: new Vector3(maxDistance * 2, 0, maxDistance * 2) // Skaliranje samo u ravni x i z
            );
        }
    }



    [NonSerialized] private static readonly string logPrefix = "[Cut Debug] ";
    public void Log(string message, UnityEngine.Object context = null)
    {
        if (debugOptions.HasFlag(DebugOption.LogMessages))
        {
            Debug.Log(logPrefix + message, context);
        }
    }


}


[System.Flags]
public enum DebugOption
{
    None = 0,
    ShowRay = 1 << 0,
    ShowCircle = 1 << 1,
    LogMessages = 1 << 2
    // Dodaj još po potrebi
}
