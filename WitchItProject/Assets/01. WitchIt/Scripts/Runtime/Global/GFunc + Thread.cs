using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public static partial class GFunc
{
    public static IEnumerator KillCoroutine(this IEnumerator routine, float time_)
    {
        while(0 < time_)
        {
            time_ -= Time.deltaTime;
        }
        return routine;
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

