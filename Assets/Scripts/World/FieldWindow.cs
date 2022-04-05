using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using TMPro;

public class FieldWindow : UI_base
{
    private static FieldWindow m_instance;
    public static FieldWindow Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL값인 경우 데이터를 매니저를 가져온다.
                GameObject managerObj = Instantiate(Resources.Load("UI/FieldWindow") as GameObject);
                m_instance = managerObj.GetComponent<FieldWindow>();

                m_instance.name = "FieldWindow";

                DontDestroyOnLoad(m_instance.gameObject);

                m_instance.loadComplete = true;
            }
            return m_instance;
        }
    }

    public override void CallUI()
    {
        Debug.Log($"{Instance.gameObject.transform.name} Load");
    }

    [SerializeField] private GameObject body;
    [SerializeField] private Image fieldImg;
    [SerializeField] private TextMeshProUGUI fieldName;
    [SerializeField] private TextMeshProUGUI fieldExplain;

    [SerializeField] private Transform slotContent;
    [SerializeField] private FieldMonsterSlot monsterSlot;
    private List<FieldMonsterSlot> slotList = new List<FieldMonsterSlot>();
    private string moveSceneName;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 윈도우를 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffWindow(bool state)
    {
        Instance.body.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 윈도우를 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void MakeFieldWindow(string fieldName)
    {
        OnOffWindow(true);

        FieldManager fieldManager = FieldManager.Instance;
        FieldData fieldData = fieldManager.fieldData[fieldName];

        //필드 이미지표시
        Sprite sprite = fieldData.fieldSprite;
        Instance.fieldImg.sprite = sprite;

        //필드 이름 및 정보 표시
        SetFieldData();

        //필드에 나오는 몬스터 표시
        SetMonsterData();

        FieldManager.nowField = fieldName;

        void SetFieldData()
        {
            //필드 정보표시
            Instance.fieldName.text = fieldData.fieldName;
            Instance.fieldExplain.text = fieldData.fieldExplain;
            Instance.moveSceneName = fieldData.moveSceneName;
        }

        void SetMonsterData()
        {
            //필드에 나오는 몬스터
            List<string> monsterList = fieldData.spawnMonster;
            int createNum = monsterList.Count - Instance.slotList.Count;

            for (int i = 0; i < createNum; i++)
            {
                //필요 갯수만큼 슬롯을 추가
                FieldMonsterSlot obj = Instantiate(Instance.monsterSlot);
                obj.transform.parent = Instance.slotContent;
                obj.transform.localScale = new Vector3(1, 1, 1);
                Instance.slotList.Add(obj);
            }

            for (int i = 0; i < Instance.slotList.Count; i++)
            {
                //슬롯에 아이템 정보를 표시
                if (i >= monsterList.Count)
                    Instance.slotList[i].gameObject.SetActive(false);
                else
                {
                    string monsterName = monsterList[i];
                    Instance.slotList[i].SetIcon(fieldName, monsterName);
                    Instance.slotList[i].gameObject.SetActive(true);
                }
            }
        }
    }

    

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 이동 버튼시 처리
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void GotoField()
    {
        //필드 씬으로 이동처리
        MoveScene.LoadScene(moveSceneName);
    }
}
