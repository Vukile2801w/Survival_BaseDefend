using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.scripsts.Player;

public class Player_Shoot : MonoBehaviour
{
    private Player_Controle playerControle;
    private InputAction fire;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Player_Movement playerMovement; // Referenca na Player_Movement
    [SerializeField] private Transform bullet_spawn_point;
    [SerializeField] private float bulletSpeed = 20f;

    public Vector3 shoot_direction = Vector3.forward; // Podrazumevani pravac pucanja

    private void Awake()
    {
        playerControle = new Player_Controle();
    }

    private void OnEnable()
    {
        fire = playerControle.Player.Fire; // Pretpostavka da koristiš Fire akciju
        fire.Enable();
    }

    private void OnDisable()
    {
        fire.Disable();
    }

    void Update()
    {
        // Postavljanje pravca pucanja samo ako je pravac kretanja različit od nule
        if (Player.movement_dir != Vector3.zero)
        {
            shoot_direction = RoundToDiscreteDirections(Player.movement_dir);
        }

        // Provera pucanja
        if (fire.triggered)
        {
            Debug.Log("WHY");
            Shoot();
        }
    }

    private Vector3 RoundToDiscreteDirections(Vector3 direction)
    {
        // Zaokruživanje svake komponente na -1, 0 ili 1
        return new Vector3(
            Mathf.Round(direction.x), // Zaokruži x na -1, 0 ili 1
            0f,                       // Y ostaje 0 jer pucanje nije vertikalno
            Mathf.Round(direction.z)  // Zaokruži z na -1, 0 ili 1
        ).normalized; // Normalizuj rezultat
    }

    private void Shoot()
    {
        // Provera da li je shoot_direction validan (nije nula)
        if (shoot_direction == Vector3.zero)
        {
            Debug.LogWarning("Invalid shoot direction: " + shoot_direction);
            return;
        }

        // Kreiranje metka
        GameObject bullet = Instantiate(bulletPrefab, bullet_spawn_point, true);

        // Postavljanje pravca kretanja metka
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shoot_direction * bulletSpeed; // Koristi diskretni pravac za pucanje
        }

        // Uništavanje metka nakon 5 sekundi
        Destroy(bullet, 5f);
    }

}
