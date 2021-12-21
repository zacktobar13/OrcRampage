using UnityEngine;
using UnityEngine.UI;

public class DestructibleObject : MonoBehaviour
{
    public float health;
    public int maxHealth;
    public Image healthbar;
    public GameObject healthUI;
    public GameObject[] itemsToDrop;

    private GameObject floatingDamageNumber;
    private GameObject critDamageNumber;

    void Awake()
    {
        health = maxHealth;
    }

    protected void Start()
    {
        floatingDamageNumber = StaticResources.floatingDamageNumber;
        critDamageNumber = StaticResources.critDamageNumber;
    }

    public void ApplyDamage(DamageInfo damageInfo)
    {
        if (!enabled)
            return;

        health -= damageInfo.damageAmount;
        healthUI.SetActive(true);

        if ( healthbar != null )
        {
            healthbar.fillAmount = health / maxHealth;
        }

        if (health <= 0)
        {
            Death();
        }

        // Spawn floating damage numbers for crits and non-crits.
        if (damageInfo.criticalHit)
        {
            GameObject damageNumber = Instantiate(critDamageNumber, new Vector3(transform.position.x, transform.position.y + 7f, transform.position.z), Quaternion.identity);
            damageNumber.SendMessage("SetNumber", damageInfo.damageAmount.ToString());
        }
        else
        {
            GameObject damageNumber = Instantiate(floatingDamageNumber, new Vector3(transform.position.x, transform.position.y + 7f, transform.position.z), Quaternion.identity);
            damageNumber.SendMessage("SetNumber", damageInfo.damageAmount.ToString());
        }
    }

    public void ApplyDamageAsPercentage(float percentage)
    {
        health *= percentage;
    }

    public void DropItems()
    {
        foreach(GameObject item in itemsToDrop)
        {
            Instantiate(item, new Vector2(transform.position.x + Random.Range(1f, 2f), transform.position.y + Random.Range(1f, 2f)), transform.rotation);
        }
    }

    public virtual void Death()
    {
        DropItems();
        Destroy(gameObject);
    }
}
