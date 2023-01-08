using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Transform spriteTransform;
    int projectileDamage;
    float radius;
    float warningDuration;
    float lerpEndTime;
    float lerpStartTime;

    void FixedUpdate()
    {
        if (Time.time > lerpEndTime) {
            DealDamage();
            Destroy(gameObject);
        }
        float scaleInterpolation = Mathf.Lerp(1, radius, (Time.time - lerpStartTime) / (lerpEndTime - lerpStartTime));
        spriteTransform.localScale = new Vector3(scaleInterpolation, scaleInterpolation, spriteTransform.localScale.z);
    }

    public void SetProjectileInfo(int damage, float endRadius, float warningDuration) {
        projectileDamage = damage;
        radius = endRadius;
        this.warningDuration = warningDuration;
        lerpEndTime = Time.time + warningDuration;
        lerpStartTime = Time.time;
        Debug.Assert(radius >= 1);
    }

    public void DealDamage() {
        Collider2D[] objectsHit = Physics2D.OverlapCircleAll(transform.position, radius * (transform.localScale.x / 2));
        foreach (Collider2D collider in objectsHit) {
            PlayerHealth playerHealth;
            if (collider.TryGetComponent(out playerHealth))
            {
                DamageInfo damageInfo = new DamageInfo(projectileDamage, false);
                playerHealth.ApplyDamage(damageInfo);
                continue;
            }
            MapClutter mapClutter;
            if (collider.TryGetComponent(out mapClutter))
            {
                DamageInfo damageInfo = new DamageInfo(projectileDamage, false);
                mapClutter.ApplyDamage(damageInfo);
                continue;
            }
        }
    }
}
