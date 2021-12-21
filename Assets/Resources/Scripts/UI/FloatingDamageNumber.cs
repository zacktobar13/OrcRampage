using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamageNumber : MonoBehaviour
{
    public float movementSpeed;
    public TextMeshPro textMeshPro;

    void Start()
    {
        StartCoroutine("DestroySelf");
        transform.position = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * movementSpeed);
    }

    public IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void SetNumber(string number)
    {
        textMeshPro.SetText(number);
    }
}
