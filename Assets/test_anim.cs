using UnityEngine;

public class test_anim : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float y_rot = transform.localRotation.eulerAngles.y;
        transform.localRotation = Quaternion.Euler(0, y_rot + rotationSpeed, 0);

        rb.linearVelocity += Vector3.forward * speed * Time.deltaTime;
    }
}
