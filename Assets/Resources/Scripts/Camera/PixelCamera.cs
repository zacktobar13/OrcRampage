﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PixelCamera : MonoBehaviour
{
    public int w = 720;
    private int h;

    public Camera cam;

    protected void Start()
    {
        cam = GetComponent<Camera>();
    }
    void Update()
    {

        float ratio = ((float)cam.pixelHeight / (float)cam.pixelWidth);
        h = Mathf.RoundToInt(w * ratio);

    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture buffer = RenderTexture.GetTemporary(w, h, -1);
        buffer.filterMode = FilterMode.Point;
        Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}