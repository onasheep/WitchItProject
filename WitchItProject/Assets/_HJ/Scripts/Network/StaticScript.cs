using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Serialization<T>
{
    public Serialization(List<T> _target) => target = _target;
    public List<T> target;
}



[System.Serializable]
public class PlayerInfo
{
    public PlayerInfo(string _nickName, int _actorNum, int _killDeath, double _lifeTime, bool _isDie)
    {
        nickName = _nickName;
        actorNum = _actorNum;
        killDeath = _killDeath;
        lifeTime = _lifeTime;
        isDie = _isDie;
    }

    public string nickName;
    public int actorNum;
    public int killDeath;
    public double lifeTime;
    public bool isDie;
}