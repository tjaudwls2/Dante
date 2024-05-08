using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thisRooster : thispet
{
    GameObject target;
    Animator anim;
    float dealtime, dealtimemax = 1;
    private void Awake()
    {

        anim = GetComponent<Animator>(); 
    
    }
    public override void Attack()
    {
        target = ItemManager.Iteminstance.foundmob();
        GetComponent<Collider>().enabled = true;
    }


    public override void Update()
    {
        base.Update();
        anim.SetBool("Run", Vector3.Distance(transform.position, player.transform.position) > dis);

        if (attackreal)
        {

            this.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.08f);
            this.transform.LookAt(target.transform.position);

            if (target.GetComponent<monster_one>().die)
            {
                attackreal = false;
                dealtime = 0;
                GetComponent<Collider>().enabled = false;

            }

        }



    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target)
        {

            dealtime += Time.deltaTime;
            if (dealtime > dealtimemax)
            {
                other.GetComponent<monster_one>().Damage_calculate(damage * damage_per, false, true,true);
                dealtime = 0;

            }



       
        }
    }

}
