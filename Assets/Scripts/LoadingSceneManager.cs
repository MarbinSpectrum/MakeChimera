using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneManager : MonoBehaviour 
{
    [SerializeField] private string inGameScene = "Main";
    [SerializeField] private string introScene = "Main";
    [SerializeField] private TextMeshProUGUI loadText;
    [SerializeField] private List<Manager> managers = new List<Manager>();
    [SerializeField] private List<UI_base> uis = new List<UI_base>();
    private void Start() 
    { 
        StartCoroutine(LoadData()); 
    } 

    IEnumerator LoadData()
    { 
        foreach (Manager manager in managers)
        {
            string managerName = manager.transform.name;

            switch(managerName)
            {
                case "ItemManager":
                    {
                        loadText.text = "아이템 로딩중";
                    }
                    break;
                case "MonsterManager":
                    {
                        loadText.text = "몬스터 로딩중";
                    }
                    break;
                case "FieldManager":
                    {
                        loadText.text = "필드 로딩중";
                    }
                    break;
                case "DataManager":
                    {
                        loadText.text = "데이터 로딩중";
                    }
                    break;
                case "PlayerManager":
                    {
                        loadText.text = "플레이어 로딩중";
                    }
                    break;
                case "PeddlerManager":
                    {
                        loadText.text = "상점 로딩중";
                    }
                    break;
                case "SoundManager":
                    {
                        loadText.text = "사운드 로딩중";
                    }
                    break;
                case "UpgradeManager":
                    {
                        loadText.text = "강화정보 로딩중";
                    }
                    break;
            }
            manager.CallManager();
            yield return new WaitWhile(() => { return manager.loadComplete; });
            yield return new WaitForSeconds(0.3f);
        }
        loadText.text = "UI 로딩중";
        foreach (UI_base ui in uis)
        {
            ui.CallUI();
            yield return new WaitWhile(() => { return ui.loadComplete; });
        }
        yield return new WaitForSeconds(1);

        loadText.text = "게임시작";

        yield return new WaitForSeconds(1);

        PlayData playData = DataManager.Instance.playData;
        MoveScene.LoadScene(inGameScene);


        if (playData.firstPlay == false)
            MoveScene.LoadScene(inGameScene);
        else
            MoveScene.LoadScene(introScene);
    } 
}