using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Text;
public class DataManager : Manager
{
    public static DataManager m_instance;

    public static DataManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = Instantiate(Resources.Load("Manager/DataManager") as GameObject).GetComponent<DataManager>();

                LoadPlayData();

                m_instance.name = "DataManager";

                DontDestroyOnLoad(m_instance.gameObject);

                m_instance.loadComplete = true;
            }
            return m_instance;
        }
    }
    public override void CallManager()
    {
        Debug.Log($"{Instance.gameObject.transform.name} Load");
    }

    [System.NonSerialized]
    public PlayData playData;


    public static void SaveData()
    {
        Instance.playData.SaveData();
        string jsonData = Instance.ObjectToJson(Instance.playData);
        var jtc2 = Instance.JsonToOject<PlayData>(jsonData);
        if (!Application.isEditor)
            Instance.CreateJsonFile(Application.persistentDataPath, "PlayData", jsonData);
        else
            Instance.CreateJsonFile(Application.dataPath, "Resources/PlayData", jsonData);
    }

    public static void LoadPlayData()
    {
        string filePath = Application.persistentDataPath + "/PlayData.json";
        if (Application.isEditor)
        {
            filePath = Application.dataPath + "/Resources/PlayData.json";
            if (File.Exists(filePath))
            {
                var jtc = Instance.LoadJsonFile<PlayData>(Application.dataPath, "Resources/PlayData");
                Instance.playData = jtc;
            }
            else
            {
                Instance.playData = new PlayData();
                string jsonData = Instance.ObjectToJson(Instance.playData);
                var jtc = Instance.JsonToOject<PlayData>(jsonData);
                Instance.CreateJsonFile(Application.dataPath, "Resources/PlayData", jsonData);
                jtc = Instance.LoadJsonFile<PlayData>(Application.dataPath, "Resources/PlayData");
                Instance.playData = jtc;
            }
        }
        else
        {
            if (File.Exists(filePath))
            {
                var jtc = Instance.LoadJsonFile<PlayData>(Application.persistentDataPath, "PlayData");
                Instance.playData = jtc;
            }
            else
            {
                Instance.playData = new PlayData();
                string jsonData = Instance.ObjectToJson(Instance.playData);
                var jtc = Instance.JsonToOject<PlayData>(jsonData);
                Instance.CreateJsonFile(Application.persistentDataPath, "PlayData", jsonData);
                jtc = Instance.LoadJsonFile<PlayData>(Application.persistentDataPath, "PlayData");
                Instance.playData = jtc;
            }
        }
        Instance.playData.LoadData();
    }

    #region[JSON °ü¸®]
    string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    T JsonToOject<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }

    [System.Serializable]
    public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField]
        List<TKey> keys;
        [SerializeField]
        List<TValue> values;

        Dictionary<TKey, TValue> target;
        public Dictionary<TKey, TValue> ToDictionary() { return target; }

        public Serialization(Dictionary<TKey, TValue> target)
        {
            this.target = target;
        }

        public void OnBeforeSerialize()
        {
            keys = new List<TKey>(target.Keys);
            values = new List<TValue>(target.Values);
        }

        public void OnAfterDeserialize()
        {
            var count = Mathf.Min(keys.Count, values.Count);
            target = new Dictionary<TKey, TValue>(count);
            for (var i = 0; i < count; ++i)
            {
                target.Add(keys[i], values[i]);
            }
        }
    }

    [System.Serializable]
    public class Serialization<T>
    {
        [SerializeField]
        List<T> target;
        public List<T> ToList() { return target; }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }
    #endregion
}