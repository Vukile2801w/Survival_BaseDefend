using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private Player_Controle player_controle;
    private InputAction move;

    [SerializeField] private Camera cam;
    [SerializeField] private float speed = 5f; // Brzina kretanja
    [SerializeField] private LayerMask layerMask; // Mask for raycasting

    // Dodaj ovo svojstvo u klasu Player_Movement




    private void Awake()
    {
        player_controle = new Player_Controle();


    }

    private void OnEnable()
    {


        move = player_controle.Player.Move;
        move.Enable();


    }


    private void OnDisable()
    {
        

        move.Disable();
    }

    void FixedUpdate()
    {
        // Čitanje pravca kretanja
        Vector2 input = move.ReadValue<Vector2>();

        // Dobijanje pravca kamere
        Vector3 cameraForward = Get_Camera_Forward(cam);
        Vector3 cameraRight = Get_Camera_Right(cam);

        // Računanje pravca kretanja
        Vector3 movement_dir = (cameraRight * input.x + cameraForward * input.y).normalized * speed;

        // Postavljanje brzine pomoću Rigidbody-a
        Vector3 newVelocity = new Vector3(movement_dir.x, rb.linearVelocity.y, movement_dir.z);
        rb.linearVelocity = newVelocity;

        // Okretanje igrača
        Look_At(movement_dir);


    }


    private Vector3 Get_Camera_Forward(Camera player_Camera)
    {
        Vector3 forward = player_Camera.transform.forward;
        forward.y = 0; // Ignoriramo visinu kamere
        return forward.normalized;
    }

    private Vector3 Get_Camera_Right(Camera player_Camera)
    {
        Vector3 right = player_Camera.transform.right;
        right.y = 0; // Ignoriramo visinu kamere
        return right.normalized;
    }

    
    


    public void Look_At(Vector3 move_dir)
    {
        if (move_dir == Vector3.zero)
            return; // Ako nema kretanja, ne radimo ništa

        move_dir.y = 0; // Ignoriramo visinu

        transform.LookAt(transform.position + move_dir);

    }

    

}
