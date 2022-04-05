using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEffect : MonoBehaviour
{
    public static BoomEffect Instance;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 startAngleRange;
    [SerializeField] private EventSE getSE;

    private void Awake()
    {
        Instance = this;
    }

    private List<BoomItem> items = new List<BoomItem>();
    [SerializeField] private BoomItem boomItem;

    public static void PlayEffect(List<string> items, Vector3 pos)
    {
        Instance.transform.position = pos;

        Instance.StartCoroutine(BoomItems(items));

        IEnumerator BoomItems(List<string> items)
        {
            for (int i = 0; i < Instance.items.Count; i++)
                Instance.items[i].Stop();

            Instance.items.ForEach(x => 
            {
                x.gameObject.SetActive(false); x.transform.localPosition = new Vector3(Instance.offset.x, Instance.offset.y, 0);
            });

            float startAngle = Random.Range(Instance.startAngleRange.x, Instance.startAngleRange.y);
            float angle = startAngle;
            for (int i = 0; i < items.Count; i++)
            {
                if (Instance.items.Count <= i)
                {
                    BoomItem temp = Instantiate(Instance.boomItem);
                    temp.transform.parent = Instance.transform;
                    temp.transform.localPosition = new Vector3(Instance.offset.x, Instance.offset.y, 0);
                    Instance.items.Add(temp);
                }
                Instance.items[i].gameObject.SetActive(true);
                Instance.items[i].SetSprtie(items[i]);
                Instance.items[i].AddForce(Random.Range(8, 15), angle);
                float addAngle = (items.Count - 1) == 0 ? 90 :
                    (180 - 2 * Random.Range(Instance.startAngleRange.x, Instance.startAngleRange.y)) / (items.Count - 1);
                angle += addAngle;
            }

            yield return new WaitForSeconds(1f);

            bool getItem = Instance.items.Count > 0;
            for (int i = 0; i < Instance.items.Count; i++)
                Instance.items[i].GetItem();

            yield return new WaitForSeconds(0.2f);

            if (getItem)
                Instance.getSE.RunEvent();
        }
    }
}
