using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class WorldMenu : SerializedMonoBehaviour
{
    [SerializeField] private RectTransform content;
    private void Awake()
    {
        Vector2 pos = content.anchoredPosition;
        if(worldPos.ContainsKey(FieldManager.nowField))
        {
            pos = new Vector2(worldPos[FieldManager.nowField], pos.y);
            content.anchoredPosition = pos;
        }
    }

    private void Start()
    {
        PlayData playData = DataManager.Instance.playData;
        int nowStage = playData.nowStage;

        foreach (KeyValuePair<string, GameObject> pair in fieldBtn)
        {
            int stageOrder = FieldManager.GetFieldOrder(pair.Key);
            pair.Value.SetActive(nowStage + 1 >= stageOrder);
        }

    }

    //private void Update()
    //{
    //    Debug.Log(content.anchoredPosition.x);
    //}

    public void GotoMain()
    {
        FieldManager.nowField = "Home";
        MoveScene.LoadScene("Main");
    }

    [SerializeField] private Dictionary<string, float> worldPos = new Dictionary<string, float>(); 
    [SerializeField] private Dictionary<string, GameObject> fieldBtn = new Dictionary<string, GameObject>();
}
