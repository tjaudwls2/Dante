using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class thisPig : thispet
{
    GameObject target;
    Animator anim; Rigidbody rid;
    private void Awake()
    {
        rid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rushtimeMax = 5;
    }
    public override void Attack()
    {
        target = ItemManager.Iteminstance.foundmob_long();
   GetComponent<Collider>().enabled = true;

    }

    float rushtime, rushtimeMax;
    public override void Update()
    {
        base.Update();
        anim.SetBool("Run", Vector3.Distance(transform.position, player.transform.position) > dis);
        if (attackreal)
        {
            rushtime += Time.deltaTime;
            rid.AddForce((target.transform.position - this.transform.position).normalized * 10);
            this.transform.LookAt(target.transform.position);
            if(rushtime > rushtimeMax)
            {
                rushtime = 0;
                attackreal = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mob"))
        {
            other.GetComponent<monster_one>().Damage_calculate(damage * damage_per, false, true);
            if (other.gameObject == target)
            {
                rid.velocity = Vector3.zero;
                attackreal = false;
                rushtime = 0;
                GetComponent<Collider>().enabled = false;
            }
        }
    }



}
