using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public static BackGround Instance;
    [SerializeField] private List<Animation> animation;

    private void Awake()
    {
        Instance = this;
    }

    public static void RunAni()
    {
        if (Instance.animation == null)
            return;
        Instance.animation.ForEach(x => x.Play());
        Instance.animation.ForEach(x =>
        {
            foreach (AnimationState state in x)
            {
                state.speed = 1;
            }
        });
    }

    public static void StopAni()
    {
        if (Instance.animation == null)
            return;
        Instance.animation.ForEach(x =>
        {
            foreach (AnimationState state in x)
            {
                state.speed = 0;
            }
        });
    }

}
