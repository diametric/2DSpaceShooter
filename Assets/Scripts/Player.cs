using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float fireSpeed = 0.5f;
    public float fireVelocity = 300f;

    public Transform firePoint;

    public GameObject ammo;
    public GameObject bomb;

    private Rigidbody2D rb;
    private float lastFire = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float td = Time.realtimeSinceStartup;

        if (Input.GetButton("Fire1"))
            if (td - lastFire > fireSpeed)
            {
                //FireWeapon();
                FireTripleSpread();
                lastFire = td;
            }

        if (Input.GetMouseButtonDown(1))
            FireBomb();
    }

    private void FixedUpdate()
    {
        float hMovement = Input.GetAxisRaw("Horizontal") * Time.fixedDeltaTime * speed;
        float vMovement = Input.GetAxisRaw("Vertical") * Time.fixedDeltaTime * speed;

        
        // Constraint player movement
        Vector2 newPos = rb.position + new Vector2(hMovement, vMovement);
        newPos.x = Mathf.Clamp(newPos.x, -4.7f, 4.7f);
        newPos.y = Mathf.Clamp(newPos.y, 0, 2);

        rb.MovePosition(newPos);
    }

    void FireBomb()
    {
        GameObject obj = Instantiate(bomb, firePoint.position, Quaternion.identity);
    }

    void FireTripleSpread()
    {
        FireWeapon(new Vector3(5f, 1), 15);
        FireWeapon(Vector3.zero, 1);
        FireWeapon(new Vector3(-5f, 1), -15);
    }

    void FireWeapon(Vector3 offset, float rotation = 0)
    {
        GameObject obj = Instantiate(ammo, firePoint.position, Quaternion.identity);

        var bulletRb = obj.GetComponent<Rigidbody2D>();

        bulletRb.velocity = Quaternion.Euler(0, 0, rotation) * Vector2.up * fireVelocity;
    }
}
