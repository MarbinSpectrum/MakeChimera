using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResolution : MonoBehaviour
{
    [SerializeField] private int width = 1920;
    [SerializeField] private int height = 1080;

    private void Awake()
    {
        SetupCamear();
    }

    private void SetupCamear()
    {
        float targetWidthAspect = width * 0.01f;
        float targetHeightAspect = height * 0.01f;

        Camera camera = Camera.main;

        camera.aspect = targetWidthAspect / targetHeightAspect;

        float widthRatio = Screen.width / targetWidthAspect;
        float heightRatio = Screen.height / targetHeightAspect;

        float heightAdd = ((100.0f * widthRatio / heightRatio) - 100.0f) * 0.005f;
        float widthAdd = ((100.0f * heightRatio / widthRatio) - 100.0f) * 0.005f;

        if (heightRatio > widthRatio)
        {
            widthAdd = 0.0f;
        }
        else
        {
            heightAdd = 0.0f;
        }


        camera.rect = new Rect(
            camera.rect.x + Mathf.Abs(widthAdd),
            camera.rect.y + Mathf.Abs(heightAdd),
            camera.rect.width + (widthAdd * 2),
            camera.rect.height + (heightAdd * 2)
            );
    }

    private void OnPreCull()
    {
        Camera camera = Camera.main;

        Rect rect = camera.rect;
        Rect newRect = new Rect(0, 0, 1, 1);
        camera.rect = newRect;
        GL.Clear(true, true, Color.black);
        camera.rect = rect;
    }
}
