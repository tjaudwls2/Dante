using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class State 
{

    public float  hp=0;
    public float  speed=0;
    public float  Attack_power=0;
    public float  Shield_power=0;
    public float  critcal_Damage = 0;
    public float  critcal_Per = 0;
    public float  Attack_speed = 0;
    public float  rush_power = 0;
    public float  rush_time = 0;
    public float  evasion = 0;

    public State DeeyCopy()
    {
        State copy = new State(); 

        copy.hp = hp;
        copy.speed = speed;
        copy.Attack_power = Attack_power;
        copy.Shield_power = Shield_power;
        copy.critcal_Damage = critcal_Damage;
        copy.critcal_Per = critcal_Per;
        copy.Attack_speed = Attack_speed;
        copy.rush_power = rush_power;
        copy.rush_time = rush_time;
        copy.evasion = evasion;

        return copy;
    }


}
