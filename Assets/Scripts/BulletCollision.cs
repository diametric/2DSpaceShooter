using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y > 9.4)
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

            AudioSource fx = collider.gameObject.GetComponent<AudioSource>();
            if (fx != null)
            {
                fx.Play();
            }

            Animator explosion = collider.gameObject.GetComponent<Animator>();
            if (explosion != null)
            {
                // Play the attached animator and then destroy it.
                if (collider.gameObject.name.Contains("Better"))
                {
                    explosion.Play("BetterRockExplosion");
                }
                else if (collider.gameObject.name.Contains("Small"))
                {
                    explosion.Play("SmallRockExplosion");
                }
                Destroy(collider.gameObject, 0.5f);
            }
            else
            {
                Destroy(collider.gameObject);
            }
            GameManager.instance.DestroyRock();
        }

    }
}
