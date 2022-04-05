using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라의 비율에 따라 오브젝트 크기가 바꿔야되는 경우 사용된다.
public class CameraObject : MonoBehaviour
{
    [SerializeField] private float size;

    private void OnEnable()
    {
        SetScale(size);
    }

    //오브젝트 크기 설정
    private void SetScale(float size)
    {
        float scale = (float)1920 / Screen.width;
        transform.localScale = new Vector3(1, 1, 1) * scale * size;
    }
}
