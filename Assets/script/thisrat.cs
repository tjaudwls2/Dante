using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thisrat : thispet
{
    GameObject target;

    public override void Attack()
    {
        target = ItemManager.Iteminstance.foundmob();
        GetComponent<Collider>().enabled = true;
    }


     public override void Update()
    {
        base.Update();
      
        if (attackreal)
        {

            this.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.08f);
            this.transform.LookAt(target.transform.position);



        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Mob"))
        {
            other.GetComponent<monster_one>().Damage_calculate(damage*damage_per,false,true);
            GetComponent<Collider>().enabled = false;
            attackreal = false;
        }
    }



}
