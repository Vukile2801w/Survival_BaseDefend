using UnityEngine.InputSystem;
using UnityEngine;

public class Player_Cuting : MonoBehaviour
{
    private Player_Controle player_controle;
    private InputAction use;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxDistance = 5f; // Maksimalna daljina za sečenje
    [SerializeField] private Player_Inventory inventory;

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


                    tree.Cut(10, inventory);
                    
                }
            }
                
        }
    }
}
