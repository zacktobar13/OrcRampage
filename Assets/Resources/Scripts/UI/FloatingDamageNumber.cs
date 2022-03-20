using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamageNumber : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI shadowText;
    Animator anim;
    float movementSpeed;
    int horizontalDirection;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = Random.Range(.9f, 1.1f);
        int[] horiztonalDirections = new int[2] { -1, 1 };
        int[] horizontalSpeeds = new int[] { -4, -3, 3, 4 };
        movementSpeed = Utility.Choose(horizontalSpeeds);
        horizontalDirection = Utility.Choose(horiztonalDirections);
        transform.position = new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-2f, 2f), transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * movementSpeed * Time.fixedDeltaTime * horizontalDirection);
    }

    public void Anim_DestroySelf()
    {
        Destroy(gameObject);
    }

    public void SetNumber(string number)
    {
        text.SetText(number);
        shadowText.SetText(number);
    }
}
