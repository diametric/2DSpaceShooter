using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bomb : MonoBehaviour
{
    public float bombVelocity = 10f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.up * bombVelocity;
    }

    void Update()
    {
        if (transform.position.y > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collider = collision.GetComponent<CircleCollider2D>();
        if (collider == null)
            return;

        if (collider.tag == "Rock")
        {
            Destroy(gameObject);
            collider.enabled = false;

            // Flash the Canvas and destroy every rock on screen.
            GameManager.instance.BombFlash();
            GameManager.instance.SlowMotion();

            AudioSource fx = collision.gameObject.GetComponent<AudioSource>();
            if (fx != null)
            {
                fx.Play();
            }

            var rocks = GameObject.FindGameObjectsWithTag("Rock");
            foreach (var rock in rocks)
            {

                Animator explosion = rock.gameObject.GetComponent<Animator>();
                if (explosion != null)
                {
                    // Play the attached animator and then destroy it.
                    if (rock.gameObject.name.Contains("Better"))
                    {
                        explosion.Play("BetterRockExplosion");
                    } else if (rock.gameObject.name.Contains("Small"))
                    {
                        explosion.Play("SmallRockExplosion");
                    }
                    Destroy(rock.gameObject, 0.3f);
                }
                else
                {
                    Destroy(rock.gameObject);
                }
                GameManager.instance.DestroyRock();
            }

        }


    }
}
