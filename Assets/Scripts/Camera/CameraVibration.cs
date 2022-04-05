using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVibration : MonoBehaviour
{
    public static CameraVibration Instance;
    [SerializeField] private float power;
    [SerializeField] private float time;
    private Vector3 basePos;
    private bool getPosFlag = false;
    private void Awake()
    {
        Instance = this;
    }

    public static void Vibration()
    {
        Instance.StartCoroutine(Instance.Cor_Vibration());
    }

    private Vector2 GetDic(float angle)
    {
        float th = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(th), Mathf.Sin(th));
    }

    public IEnumerator Cor_Vibration()
    {
#if UNITY_ANDROID
        Custom.Vibration.Vibrate(100);
#endif

        if (getPosFlag == false)
        {
            getPosFlag = true;
            basePos = transform.position;
        }
        float p = power;
        float t = time;
        int cnt = (int)(t / Time.deltaTime);
        float v = power / cnt;
        while (t > 0)
        {
            float angle = Random.Range(0, 360);
            transform.position = basePos + (Vector3)(GetDic(angle) * p);

            yield return new WaitForSeconds(Time.deltaTime);
            t -= Time.deltaTime;
            p -= v;
        }
        transform.position = basePos;
    }
}
