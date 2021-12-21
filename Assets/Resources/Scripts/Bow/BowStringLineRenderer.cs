using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowStringLineRenderer : MonoBehaviour
{
    LineRenderer lineRenderer;

    public Transform point1;
    public Transform point2;
    public Transform point3;

    void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();    
        lineRenderer.positionCount = 3;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
    }

    private void Awake()
    {
        PlayerShoot.onBowDraw += EnableLineRenderer;
        PlayerShoot.onShoot += DisableLineRenderer;
    }

    void FixedUpdate()
    {
        lineRenderer.SetPosition(0, new Vector3(point1.transform.position.x, point1.transform.position.y, point1.transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(point3.transform.position.x, point3.transform.position.y, point3.transform.position.z));
        lineRenderer.SetPosition(2, new Vector3(point2.transform.position.x, point2.transform.position.y, point2.transform.position.z));
    }

    private void EnableLineRenderer(float time)
    {
        lineRenderer.enabled = true;
    }

    private void DisableLineRenderer(PlayerShoot playerShoot, Projectile projectileSpawned)
    {
        lineRenderer.enabled = false;
    }

    private void OnDestroy()
    {
        PlayerShoot.onBowDraw -= EnableLineRenderer;
        PlayerShoot.onShoot -= DisableLineRenderer;
    }
}
