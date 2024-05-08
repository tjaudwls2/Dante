using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class thismonkey : thispet
{


    public override void Attack()
    {
        target = ItemManager.Iteminstance.foundmob();
        anim.SetTrigger("Attack");
        anim.SetBool("Attacking",true);
        this.GetComponent<Collider>().enabled = true;
        rid.AddForce((target.transform.position - this.transform.position).normalized * 1000);
        Invoke("rushend", 3);
        transform.rotation = Quaternion.LookRotation(new Vector3((rid.velocity).normalized.x, 0, (rid.velocity).normalized.z));
    }

    GameObject target;
    Animator anim;
    Rigidbody rid; 
    private void Awake()
    {rid= GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        anim.SetBool("Run", Vector3.Distance(transform.position, player.transform.position) > dis);

    }

    public void rushend()
    {
        anim.SetBool("Attacking", false);
        this.GetComponent<Collider>().enabled = false;
        attackreal = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;  
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mob"))
        {
            collision.gameObject.GetComponent<monster_one>().Damage_calculate(damage * damage_per, false, true);
            transform.rotation = Quaternion.LookRotation(new Vector3((rid.velocity).normalized.x, 0, (rid.velocity).normalized.z));
            rid.AddForce((rid.velocity).normalized * 500);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.rotation = Quaternion.LookRotation(new Vector3((rid.velocity).normalized.x, 0, (rid.velocity).normalized.z));
            rid.AddForce((rid.velocity).normalized * 500);
        }
    }
}
