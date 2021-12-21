using UnityEngine;

public class ClutterRemains : MonoBehaviour
{
    public float verticalForce;
    public float horizontalForce;
    public float spinForce;
    Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(Vector2.right * Random.Range(-verticalForce, verticalForce));
        rigidBody.AddForce(Vector2.up * Random.Range(-horizontalForce, horizontalForce));
        rigidBody.AddTorque(Random.Range(-spinForce, spinForce));
    }
}
