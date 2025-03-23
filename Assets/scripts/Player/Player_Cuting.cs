using UnityEngine.InputSystem;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Unity")]
public class Player_Cuting : MonoBehaviour
{
    private Player_Controle player_controle;
    private InputAction use;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxDistance = 5f; // Maksimalna daljina za sečenje
    [SerializeField] private float damage = 5f; // Maksimalna daljina za sečenje

    [SerializeField] private Player_Inventory inventory;


    [Header("Debug")]
    [SerializeField] bool debug;
    [SerializeField] private Mesh circle;

    private void Awake()
    {
        player_controle = new Player_Controle();
        use = player_controle.Player.Use;
    }

    // Update is called once per frame
    void Update()
    {
        // Provera da li je dugme pritisnuto
        bool isPressed = use.triggered || Mouse.current.rightButton.wasPressedThisFrame;

        if (isPressed)
        {
            // Uzimanje pozicije miša
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            // Izvođenje Raycast-a
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green); // Ispravan kraj linije

                GameObject hitObject = hit.collider.gameObject;

                // Provera udaljenosti
                float distance = Vector3.Distance(transform.position, hit.point);
                if (distance <= maxDistance)
                {
                    if (!hitObject.CompareTag("Tree")) // Sigurnija provera taga
                    {
                        return;
                    }
                    User_Tree tree = hitObject.GetComponent<User_Tree>();

                    if (tree == null)
                    {
                        Debug.LogWarning("User_Tree component not found on " + hitObject.name);
                        return;
                    }


                    tree.Cut(damage, inventory);
                    
                }
            }
                
        }
    }

    private void OnDrawGizmos()
    {
        if (circle == null) return; // Provera da li je mesh postavljen
        if (!debug) return;

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
