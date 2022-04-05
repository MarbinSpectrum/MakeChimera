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
                //NULL���� ��� �����͸� �Ŵ����� �����´�.
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
    /// : �����츦 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnOffWindow(bool state)
    {
        Instance.body.SetActive(state);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : �����츦 ON OFF
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void MakeFieldWindow(string fieldName)
    {
        OnOffWindow(true);

        FieldManager fieldManager = FieldManager.Instance;
        FieldData fieldData = fieldManager.fieldData[fieldName];

        //�ʵ� �̹���ǥ��
        Sprite sprite = fieldData.fieldSprite;
        Instance.fieldImg.sprite = sprite;

        //�ʵ� �̸� �� ���� ǥ��
        SetFieldData();

        //�ʵ忡 ������ ���� ǥ��
        SetMonsterData();

        FieldManager.nowField = fieldName;

        void SetFieldData()
        {
            //�ʵ� ����ǥ��
            Instance.fieldName.text = fieldData.fieldName;
            Instance.fieldExplain.text = fieldData.fieldExplain;
            Instance.moveSceneName = fieldData.moveSceneName;
        }

        void SetMonsterData()
        {
            //�ʵ忡 ������ ����
            List<string> monsterList = fieldData.spawnMonster;
            int createNum = monsterList.Count - Instance.slotList.Count;

            for (int i = 0; i < createNum; i++)
            {
                //�ʿ� ������ŭ ������ �߰�
                FieldMonsterSlot obj = Instantiate(Instance.monsterSlot);
                obj.transform.parent = Instance.slotContent;
                obj.transform.localScale = new Vector3(1, 1, 1);
                Instance.slotList.Add(obj);
            }

            for (int i = 0; i < Instance.slotList.Count; i++)
            {
                //���Կ� ������ ������ ǥ��
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
    /// : �̵� ��ư�� ó��
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void GotoField()
    {
        //�ʵ� ������ �̵�ó��
        MoveScene.LoadScene(moveSceneName);
    }
}
