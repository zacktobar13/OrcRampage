using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    public int projectileDamage;
    public GameObject hitEffect;
    public Rigidbody2D rigidBody;

    public float movementSpeed;
    public Vector2 movementDirection;
    Vector2 target;

    float hitEffectOffset = 0.5f;

    void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + movementDirection * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string colliderTag = collision.gameObject.tag;
        if (colliderTag != "Enemy" && colliderTag != "GatherableResource" && colliderTag != "Projectile")
        {
            DamageInfo damageInfo = new DamageInfo( 10, false, movementDirection );
            collision.gameObject.SendMessage( "ApplyDamage", damageInfo );
            Vector2 offset = (transform.position - collision.transform.position) * hitEffectOffset;
            Destroy(Instantiate(hitEffect, ((Vector2)collision.transform.position) + offset, collision.transform.rotation), 2f);
            Destroy(gameObject);
        }
    }
}
