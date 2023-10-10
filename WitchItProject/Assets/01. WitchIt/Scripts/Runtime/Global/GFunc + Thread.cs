using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;


public static partial class GFunc
{
    public static Coroutine KillCoroutine(this Coroutine routine, MonoBehaviour manager_, float time_)
    {
        DoKillCoroutine(routine, manager_, time_);
        return routine;
    }       // KillCoroutine()

    private static IEnumerator DoKillCoroutine(Coroutine routine, MonoBehaviour manager_, float time_)
    {
        while (0 < time_)
        {
            time_ -= Time.deltaTime;
            yield return routine;
        }

        // ���⼭ ���� ����Ʈ���� ����� ��.
        ThreadManager.instance.RemoveRoutine(routine);
        // �� ������ �ν��Ͻ� ų
        Debug.LogFormat("routine null or default ? : {0}", GFunc.IsCoroutineDead(routine));

        manager_.StopCoroutine(routine);
        routine = default;
        Debug.LogFormat("routine null or default ? : {0}", GFunc.IsCoroutineDead(routine));

        //yield return 0.1f;
        //Debug.LogFormat("is real dead Coroutine?? -> {0}", IsCoroutineDead(routine));
    }       // DoKillCoroutine()

    public static  void KillAllCoroutuine(this List<Coroutine> routines_, MonoBehaviour manager_)
    {
        foreach(Coroutine routine in routines_)
        {
            manager_.StopCoroutine(routine);
        }
    }       // KillAllCoroutuine()

    public static bool IsCoroutineDead(this Coroutine routine)
    {
        if(routine == default || routine == null)
        {
            return true;
        }

        return false;
    }

    //public static IEnumerator KillAllCoroutine(this List<IEnumerator> routines)
    //{
    //    if(routines.Count < 1)
    //    {
    //        return default(IEnumerator);
    //    }
    //    else
    //    {
    //        foreach(IEnumerator routine in routines)
    //        {
    //            return routine;
    //        }

    //    }
    //}
}

