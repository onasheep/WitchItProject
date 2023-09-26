using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ThreadManager : MonoBehaviour
{
    public static ThreadManager instance;

    private Dictionary<string, IEnumerator> routineDic;
    
    public static List<IEnumerator> routines;

    private const string SET_TIMER = "SetTimer";

    private float skillTimer = default;
    
    private void Awake()
    {
        if(instance == null || instance == default)
        {
            instance = this;
            
            Init();
        }       // if : instance가 비어있는 경우 채워주고, 초기화 필요 O
        else
        {
            return;
        }       // else : instance가 이미 존재하기 떄문에 , 초기화 필요 X
    }

    private void Init()
    {
        routines = new List<IEnumerator>();
        skillTimer = 0f;
    }



    public void Update()
    {
        Debug.LogFormat("{0}",routines.Count);
    }

    public void DoRoutine(UnityAction action_, float time_)
    {

        IEnumerator routine = SetTimer(action_, time_);
        routines.Add(routine);
        StartCoroutine(routine);

    }

    private IEnumerator SetTimer(UnityAction action_, float time_)
    {

        Debug.Log("SetTimer?");
        while (0 <= time_)
        {
            time_ -= Time.deltaTime;
            yield return null;
        }
        action_();

    }

    private IEnumerator LateCallFunc(UnityAction action, float delay)
    {
        while (true)
        {
            yield return null;
        }
    }

    //private static IEnumerator KillTime(this IEnumerator co_, float time)
    //{

    //    return co_;
    //}
    // Start is called before the first frame update

}
