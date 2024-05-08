using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;
[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/Item", order = int.MaxValue)]
public class Items : ScriptableObject
{
    public List<item> item;
    
}
[System.Serializable]
public class item
{
    public string name;
    public State State;
    public XState xstate;
    public itemtype itemtype;

    public item DeepCopy()
    {
        item copy = new item();
        copy.name = name;
        copy.xstate = xstate.DeepCopy();
        copy.State = State.DeeyCopy();
        copy.itemtype = itemtype;



        return  copy;
    }

}

public enum itemtype
{
    일반=0,
    레어=1,
    유니크=2,
    에픽=3,    
    레전드=4

}


[System.Serializable]
public class XState
{
    public float hp = 1;
    public float speed = 1;
    public float Attack_power = 1;
    public float Shield_power = 1;
    public float critcal_Damage = 1;
    public float critcal_Per = 1;
    public float Attack_speed = 1;
    public float rush_power = 1;
    public float rush_time = 1;
    public float evasion  =1;

    public XState DeepCopy()
    {
        XState copy = new XState();
        copy.hp = hp;
        copy.speed          = speed         ;
        copy.Attack_power   = Attack_power  ;
        copy.Shield_power   = Shield_power  ;
        copy.critcal_Damage = critcal_Damage;
        copy.critcal_Per    = critcal_Per   ;
        copy.Attack_speed   = Attack_speed  ;
        copy.rush_power     = rush_power    ;
        copy.rush_time      = rush_time     ;
        copy.evasion = evasion;

        return copy;


    }


}
