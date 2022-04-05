using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ī�޶��� ������ ���� ������Ʈ ũ�Ⱑ �ٲ�ߵǴ� ��� ���ȴ�.
public class CameraObject : MonoBehaviour
{
    [SerializeField] private float size;

    private void OnEnable()
    {
        SetScale(size);
    }

    //������Ʈ ũ�� ����
    private void SetScale(float size)
    {
        float scale = (float)1920 / Screen.width;
        transform.localScale = new Vector3(1, 1, 1) * scale * size;
    }
}
