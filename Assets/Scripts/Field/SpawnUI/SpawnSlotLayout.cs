using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnSlotLayout : MonoBehaviour
{
    [SerializeField] private float space;
    [SerializeField] private float speed;

    private void Update()
    {
        List<Transform> list = new List<Transform>();
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform obj = transform.GetChild(i);
            if(obj.gameObject.activeSelf)
                list.Add(obj);
        }

        for (int i = 0; i < list.Count; i++)
        {
            Vector3 targetPos = new Vector3(0, -space * i, 0);

            if (Application.isPlaying)
            {
                float distance = speed * Time.deltaTime;
                Vector3 dic = targetPos - list[i].localPosition;
                dic = dic.normalized;

                if (Vector3.Distance(list[i].localPosition, targetPos) >= speed * Time.deltaTime)
                {
                    list[i].localPosition += dic * distance;
                }
            }
            else
            {
                list[i].localPosition = targetPos;
            }
        }
    }
}
