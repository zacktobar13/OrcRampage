/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowReleaseBar : MonoBehaviour
{
    public GameObject slider;
    public GameObject criticalReloadGameObject;
    public RectTransform start;
    public RectTransform finish;
    public RectTransform criticalReloadField;
    public PlayerAttack playerShoot;

    float sliderDistanceTotal;
    float sliderCurrentPosition;
    public float sliderPercentToFinish;
    float sliderSpeedMultiplier;

    float criticalFieldStart;
    float criticalFieldEnd;
    float criticalFieldSize;

    float timeToFinish;
    float time;
    bool startMovement;
    [HideInInspector] public bool canCriticalFire = false;

    private void Awake()
    {
        PlayerAttack.onBowDraw += StartBowDraw;
        PlayerAttack.onAttack += OnShoot;
    }

    private void OnDestroy()
    {
        PlayerAttack.onBowDraw -= StartBowDraw;
        PlayerAttack.onAttack -= OnShoot;
    }

    private void OnEnable()
    {
        sliderDistanceTotal = Vector2.Distance(finish.transform.localPosition, start.transform.localPosition);
        slider.transform.localPosition = start.localPosition;
        time = 0f;
        sliderSpeedMultiplier = 1f;
        canCriticalFire = false;
        criticalReloadGameObject.SetActive(true);

        // Calculate size of critical field based off of current weapon and apply it to our critical field.
        criticalFieldStart = playerShoot.criticalFieldStart * sliderDistanceTotal;
        criticalFieldEnd = playerShoot.criticalFieldEnd * sliderDistanceTotal;
        criticalFieldSize = criticalFieldEnd - criticalFieldStart;
        criticalReloadField.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, criticalFieldSize);
        criticalReloadField.anchoredPosition = new Vector2((criticalFieldStart + criticalFieldEnd) / 2, criticalReloadField.localPosition.y);
    }

    void Update()
    {
        if (startMovement)
        {
            time += (Time.deltaTime / timeToFinish) * sliderSpeedMultiplier;
            slider.transform.localPosition = Vector3.Lerp(start.transform.localPosition, finish.transform.localPosition, time);
            sliderCurrentPosition = Vector2.Distance(slider.transform.localPosition, finish.transform.localPosition);
            sliderPercentToFinish = (sliderDistanceTotal - sliderCurrentPosition) / sliderDistanceTotal;

            canCriticalFire = sliderPercentToFinish >= playerShoot.criticalFieldStart && sliderPercentToFinish <= playerShoot.criticalFieldEnd;

            if (slider.transform.localPosition == finish.localPosition)
            {
                startMovement = false;
            }
        }
    }

    public void StartBowDraw(float time)
    {
        timeToFinish = time;
        startMovement = true;
    }

    public void OnShoot(PlayerAttack playerShoot, Projectile projectile)
    {
        slider.transform.localPosition = start.localPosition;
        time = 0f;
        gameObject.SetActive(false);
    }

    public void OnCriticalShot()
    {
        slider.transform.localPosition = start.localPosition;
        time = 0f;
        gameObject.SetActive(false);
    }
}*/
