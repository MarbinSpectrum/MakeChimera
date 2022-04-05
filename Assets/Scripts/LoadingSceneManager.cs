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
                        loadText.text = "������ �ε���";
                    }
                    break;
                case "MonsterManager":
                    {
                        loadText.text = "���� �ε���";
                    }
                    break;
                case "FieldManager":
                    {
                        loadText.text = "�ʵ� �ε���";
                    }
                    break;
                case "DataManager":
                    {
                        loadText.text = "������ �ε���";
                    }
                    break;
                case "PlayerManager":
                    {
                        loadText.text = "�÷��̾� �ε���";
                    }
                    break;
                case "PeddlerManager":
                    {
                        loadText.text = "���� �ε���";
                    }
                    break;
                case "SoundManager":
                    {
                        loadText.text = "���� �ε���";
                    }
                    break;
                case "UpgradeManager":
                    {
                        loadText.text = "��ȭ���� �ε���";
                    }
                    break;
            }
            manager.CallManager();
            yield return new WaitWhile(() => { return manager.loadComplete; });
            yield return new WaitForSeconds(0.3f);
        }
        loadText.text = "UI �ε���";
        foreach (UI_base ui in uis)
        {
            ui.CallUI();
            yield return new WaitWhile(() => { return ui.loadComplete; });
        }
        yield return new WaitForSeconds(1);

        loadText.text = "���ӽ���";

        yield return new WaitForSeconds(1);

        PlayData playData = DataManager.Instance.playData;
        MoveScene.LoadScene(inGameScene);


        if (playData.firstPlay == false)
            MoveScene.LoadScene(inGameScene);
        else
            MoveScene.LoadScene(introScene);
    } 
}