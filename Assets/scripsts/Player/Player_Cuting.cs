using UnityEngine.InputSystem;
using UnityEngine;

public class Player_Cuting : MonoBehaviour
{
    private Player_Controle player_controle;
    private InputAction use;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxDistance = 5f; // Maksimalna daljina za sečenje

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
                    if (hitObject.CompareTag("Tree")) // Sigurnija provera taga
                    {
                        User_Tree tree = hitObject.GetComponent<User_Tree>();
                        if (tree != null)
                        {
                            tree.Cut(10); // Poziv metode Cut
                        }
                        else
                        {
                            Debug.LogWarning("User_Tree component not found on " + hitObject.name);
                        }
                    }
                }
                else
                {
                    Debug.Log($"Object is too far: {distance:F2} units away.");
                }
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.red); // Produžena linija ako nema hit-a
            }
        }
    }
}
