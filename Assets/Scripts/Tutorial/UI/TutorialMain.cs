using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TutorialMain : SerializedMonoBehaviour
{
    public static TutorialMain Instance;
    [SerializeField] private Dictionary<UI, GameObject> Tutorial_Show = new Dictionary<UI, GameObject>();
    [SerializeField] private Dictionary<UI, GameObject> Tutorial_actFlag = new Dictionary<UI, GameObject>();
    [SerializeField] private Dictionary<UI, GameObject> UI_BtnSelect = new Dictionary<UI, GameObject>();
    [SerializeField] private Dictionary<UI, Transform> UI_Btn = new Dictionary<UI, Transform>();
    private Dictionary<UI, bool> nowTutorial = new Dictionary<UI, bool>();

    private void Awake()
    {
        Instance = this;
    }

    UI[] TutorialOrder = new UI[] { UI.Craft, UI.Decompose, UI.Equipment, UI.Reinforce, UI.Field };

    private void Start()
    {
        PlayData playData = DataManager.Instance.playData;

        if(playData.firstPlay)
        {
            //처음 실행하면 튜토리얼 시작
            nowTutorial[UI.Craft] = false;
            nowTutorial[UI.Decompose] = false;
            nowTutorial[UI.Equipment] = false;
            nowTutorial[UI.Reinforce] = false;
            nowTutorial[UI.Field] = false;

            StartCoroutine(StartTutorial());
        }
        else
        {
            //아니면 실행안함
            gameObject.SetActive(false);
        }

        IEnumerator StartTutorial()
        {
            foreach(UI ui in TutorialOrder)
            {
                if(ui == UI.Equipment)
                {
                    ItemManager itemManager = ItemManager.Instance;
                    EquipmentData sampleData = (EquipmentData)itemManager.equipment_Items["늑대 머리"];
                    ItemClass sampleItem = sampleData.MakeItem(true);
                }

                transform.SetAsLastSibling();
                Tutorial_Show[ui].SetActive(true);

                yield return new WaitWhile(() => { return !Tutorial_actFlag[ui].activeSelf; });
                UI_BtnSelect[ui].SetActive(true);
                UI_Btn[ui].transform.SetAsLastSibling();
                nowTutorial[ui] = true;

                yield return new WaitWhile(() => { return nowTutorial[ui]; });
            }
        }
    }

    public void CraftBtnClick()
    {
        Tutorial_Show[UI.Craft].SetActive(false);
        UI_BtnSelect[UI.Craft].SetActive(false);
    }

    public static void CraftTutorialEnd()
    {
        Instance.nowTutorial[UI.Craft] = false;
    }

    public void DecomposeBtnClick()
    {
        Tutorial_Show[UI.Decompose].SetActive(false);
        UI_BtnSelect[UI.Decompose].SetActive(false);
    }

    public static void DecomposeTutorialEnd()
    {
        Instance.nowTutorial[UI.Decompose] = false;
    }

    public void EquipmentBtnClick()
    {
        Tutorial_Show[UI.Equipment].SetActive(false);
        UI_BtnSelect[UI.Equipment].SetActive(false);
    }

    public static void EquipmentTutorialEnd()
    {
        Instance.nowTutorial[UI.Equipment] = false;
    }

    public void ReinforceBtnClick()
    {
        Tutorial_Show[UI.Reinforce].SetActive(false);
        UI_BtnSelect[UI.Reinforce].SetActive(false);
    }

    public static void ReinforceTutorialEnd()
    {
        Instance.nowTutorial[UI.Reinforce] = false;
    }

    public static void FieldTutorialEnd()
    {
        PlayData playData = DataManager.Instance.playData;
        playData.firstPlay = false;

        DataManager.SaveData();
    }
}
