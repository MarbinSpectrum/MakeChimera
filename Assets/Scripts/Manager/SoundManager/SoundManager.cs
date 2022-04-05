using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SoundEvent
{
    NO_Event,
    Awake,
    OnEnable,
    OnDisable,
    Start,
}
public class SoundManager : Manager
{
    private static SoundManager m_instance;
    public static SoundManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                //NULL값인 경우 데이터를 가져온다.
                GameObject managerObj = Instantiate(Resources.Load("Manager/SoundManager") as GameObject);
                m_instance = managerObj.GetComponent<SoundManager>();

                //사운드 출력객체 추가
                GameObject seAudio = new GameObject("SE");
                m_instance.seSource = seAudio.AddComponent<AudioSource>();
                seAudio.transform.parent = m_instance.transform;

                GameObject bgmAudio = new GameObject("BGM");
                m_instance.bgmSource = bgmAudio.AddComponent<AudioSource>();
                bgmAudio.transform.parent = m_instance.transform;

                m_instance.name = "SoundManager";

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

    [SerializeField] private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

    [SerializeField] [Range(0, 1)] private float seVolume;
    [SerializeField] [Range(0, 1)] private float bgmVolume;

    private Dictionary<string, AudioSource> seList = new Dictionary<string, AudioSource>();
    private AudioSource seSource;

    private Dictionary<string, AudioSource> bgmList = new Dictionary<string, AudioSource>();
    private AudioSource bgmSource;

    //현재 배경음 이벤트
    private IEnumerator corBGMEvent;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static float GetBGM_Volume()
    {
        return Instance.bgmVolume;
    }
    public static void SetBGM_Volume(float value)
    {
        Instance.bgmVolume = value;
        foreach (KeyValuePair<string,AudioSource> audio in Instance.bgmList)
            audio.Value.volume = value;
    }

    public static float GetSE_Volume()
    {
        return Instance.seVolume;
    }
    public static void SetSE_Volume(float value)
    {
        Instance.seSource.volume = value;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 효과음 출력 메소드
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void PlaySE(string seName)
    {
        Instance.playSE(seName);
    }
    private void playSE(string seName)
    {
        if (sounds.ContainsKey(seName) == false)
        {
            //KEY에 해당하는 사운드가 없다.
            //사운드를 추가하거나 KEY값이 잘못榮募 이야기 
            Debug.LogError(seName + "에 해당하는 사운드가 없습니다.");
            return;
        }

        AudioClip seData = sounds[seName];
        seSource.volume = seVolume;
        seSource.PlayOneShot(seData);
    }
    public static void PlaySE_IsKey(string seName, string key)
    {
        Instance.playSE_IsKey(seName, key);
    }
    public void playSE_IsKey(string seName, string key)
    {
        AudioSource audioSource = null;
        if (seList.ContainsKey(key))
            audioSource = seList[key];
        else
            audioSource = Instantiate(seSource);
        audioSource.name = key;
        audioSource.transform.parent = transform;
        seList[key] = audioSource;

        AudioClip seData = sounds[seName];
        audioSource.clip = seData;
        audioSource.Play();
    }

    public static void StopSE_IsKey(string key)
    {
        Instance.stopSE_IsKey(key);
    }
    public void stopSE_IsKey(string key)
    {
        AudioSource audioSource = null;
        if (seList.ContainsKey(key))
        {
            audioSource = seList[key];
            audioSource.Stop();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : key에 해당하는 배경음을 실행시킨다.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void SetBGM_isKey(string bgmName, string key)
    {
        Instance.setBGM_isKey(bgmName, key);
    }
    private void setBGM_isKey(string bgmName, string key)
    {
        AudioSource audioSource = null;
        if (bgmList.ContainsKey(key))
            audioSource = bgmList[key];
        else
            audioSource = Instantiate(bgmSource);
        audioSource.name = key;
        audioSource.transform.parent = transform;
        bgmList[key] = audioSource;

        AudioClip seData = sounds[bgmName];
        audioSource.clip = seData;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : key에 해당하는 배경음을 실행시킨다.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void PlayBGM_isKey(string bgmName, string key)
    {
        Instance.playBGM_isKey(bgmName, key);
    }
    private void playBGM_isKey(string bgmName, string key)
    {
        setBGM_isKey(bgmName, key);

        playBGM_isKey(key);
    }
    public static void PlayBGM_isKey(string key)
    {
        Instance.playBGM_isKey(key);
    }
    private void playBGM_isKey(string key)
    {
        if (bgmList.ContainsKey(key))
        {
            bgmList[key].Play();
 
        }
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 하나의 배경음만 들리도록 slot의 배경음으로 changeTime 시간동안 바뀌어진다.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void OnlyPlayBGM(string key, float changeTime)
    {
        Instance.onlyPlayBGM(key, changeTime);
    }
    private void onlyPlayBGM(string key, float changeTime)
    {
        if (bgmList.ContainsKey(key) == false)
        {
            return;
        }

        if (corBGMEvent != null)
        {
            //전에 실행중인 코루틴을 제거
            StopCoroutine(corBGMEvent);
        }

        corBGMEvent = ChangeBGM(key, changeTime);
        StartCoroutine(corBGMEvent);
    }

    IEnumerator ChangeBGM(string key, float changeTime)
    {
        if (changeTime > 0)
        {
            //배경음 변화량
            float v = bgmVolume / (float)(changeTime * 1000);

            for (float f = changeTime; f > 0; f -= 0.001f)
            {
                foreach (KeyValuePair<string, AudioSource> audio in Instance.bgmList)
                {
                    float volume = audio.Value.volume;
                    if (audio.Key == key)
                    {
                        volume = Mathf.Min(volume + v, bgmVolume);
                        audio.Value.volume = volume;
                    }
                    else
                    {
                        volume = Mathf.Max(volume - v, 0);
                        audio.Value.volume = volume;
                    }
                }
                yield return new WaitForSeconds(0.001f);
            }
        }

        //slot의 배경음을 제외하면 모두 volume이 0이 된다.
        foreach (KeyValuePair<string, AudioSource> audio in Instance.bgmList)
        {
            float volume = audio.Value.volume;
            if (audio.Key == key)
            {
                audio.Value.volume = bgmVolume;
            }
            else
            {
                audio.Value.volume = 0;
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : slot에 해당하는 배경음을 멈춘다.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void StopBGM(string key)
    {
        Instance.stopBGM(key);
    }
    private void stopBGM(string key)
    {
        if (bgmList.ContainsKey(key) == false)
        {
            return;
        }
        bgmList[key].Stop();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : 모든 배경음을 멈춘다.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void StopAllBGM()
    {
        Instance.stopAllBGM();
    }
    private void stopAllBGM()
    {
        foreach (KeyValuePair<string, AudioSource> audio in Instance.bgmList)
        {
            audio.Value.Stop();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// : slot에 해당하는 배경음의 루프 여부를 설정한다.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void Setloop(string key, bool state)
    {
        Instance.setloop(key, state);
    }

    private void setloop(string key, bool state)
    {
        if (bgmList.ContainsKey(key) == false)
        {
            return;
        }
        bgmList[key].loop = state;
    }
}