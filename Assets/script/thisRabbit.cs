using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class thisRabbit : thispet
{
    GameObject target;
    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public override void Attack()
    {
        target = ItemManager.Iteminstance.foundmob();

      
    }
    bool purch;

    public override void Update()
    {
        base.Update();
        anim.SetBool("Run", Vector3.Distance(transform.position, player.transform.position) > dis);
        if (attackreal && !purch)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.08f);
            
            this.transform.LookAt(target.transform.position);
            if(Vector3.Distance(transform.position, target.transform.position) < dis)
            {
                purch = true;
                anim.SetTrigger("Attack");
            }
        }



      
    }

    public void attacks()
    {
        GetComponent<Collider>().enabled = true;
    }

    public void attackcoloff()
    {
        GetComponent<Collider>().enabled = false;
    }
    public void attackend()
    {
        
        attackreal = false;
        purch = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mob"))
        {
            other.GetComponent<monster_one>().Damage_calculate(damage * damage_per, false, true);
            other.GetComponent<Rigidbody>().AddForce((other.transform.position - this.transform.position).normalized * 800);


        }
    }


}
