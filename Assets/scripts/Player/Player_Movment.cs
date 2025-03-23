using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using Assets.scripsts.Player;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private Player_Controle player_controle;
    private InputAction move;
    private InputAction look;

    [SerializeField] private Camera cam;

    [SerializeField] private Player self;

    [SerializeField] private float speed = 5f; // Brzina kretanja

    // Dodaj ovo svojstvo u klasu Player_Movement


    [SerializeField] Player player;


    private void Awake()
    {
        player_controle = new Player_Controle();

        Player.rb = rb;
        Player.camera = cam;

        Player.movement_dir = Vector3.zero;

    }

    private void OnEnable()
    {
        move = player_controle.Player.Move;
        move.Enable();


        look = player_controle.Player.Look;
        look.Enable();
    }


    private void OnDisable()
    {
        move.Disable();
        look.Disable();
    }

    void FixedUpdate()
    {
        // Čitanje pravca kretanja
        Vector2 input = move.ReadValue<Vector2>();

        // Dobijanje pravca kamere
        Vector3 cameraForward = Get_Camera_Forward(cam);
        Vector3 cameraRight = Get_Camera_Right(cam);

        // Računanje pravca kretanja
        Player.movement_dir = (cameraRight * input.x + cameraForward * input.y).normalized * speed;

        // Postavljanje brzine pomoću Rigidbody-a
        Vector3 newVelocity = new Vector3(Player.movement_dir.x, Player.rb.velocity.y, Player.movement_dir.z);
        Player.rb.velocity = newVelocity;

        // Okretanje igrača
        Look_At();


        // Testiraj trenutni pravac
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


    private void Look_At()
    {
        Vector3 direction = Player.rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            Player.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            Player.rb.angularVelocity = Vector3.zero;
    }

    

}
