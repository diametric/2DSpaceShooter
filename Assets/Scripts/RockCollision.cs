using UnityEngine;

public class RockCollision : MonoBehaviour
{
    public int damageDone = 25;

    // Update is called once per frame
    void Update()
    {
        // Map bounds
        if (transform.position.y < -1.5f)
        {
            GameManager.instance.MissedRock();
            Destroy(gameObject);
        }
        else if (transform.position.x < -5.5f || transform.position.x > 5.5)
        {
            // Destroy rocks that go out of bounds on the X axis, not counted as misses.
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            GameManager.instance.TakeDamage(25);

            AudioSource fx = collision.collider.gameObject.GetComponent<AudioSource>();
            if (fx != null)
            {
                if (!fx.isPlaying)
                    fx.Play();
            }

            CameraShake shake = Camera.main.GetComponent<CameraShake>();
            if (shake != null)
                shake.shakeDuration = 0.5f;
        }
    }
}
