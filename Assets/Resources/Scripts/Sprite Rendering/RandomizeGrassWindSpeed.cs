using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeGrassWindSpeed : MonoBehaviour
{
    Material shaderMaterial;

    void Start()
    {
        shaderMaterial = transform.Find("Sprite").GetComponent<SpriteRenderer>().material;
        StartCoroutine("StartAnimating");
    }

    IEnumerator StartAnimating()
    {
        yield return new WaitForSeconds(Random.Range(0f, .25f));
        shaderMaterial.SetFloat("_GrassSpeed", Random.Range(1f, 2f));
    } 
}
