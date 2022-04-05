using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerCloth : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<string, GameObject> data = new Dictionary<string, GameObject>();
    public string nowName;

    public void SetCloth(string itemDataName)
    {
        nowName = itemDataName;
        foreach (KeyValuePair<string, GameObject> pair in data)
            pair.Value.SetActive(false);
        if(data.ContainsKey(itemDataName) == false)
        {
            Debug.LogError($"{itemDataName} 외형 정보가 없다.");
            return;
        }
        data[itemDataName].SetActive(true);
    }
}
