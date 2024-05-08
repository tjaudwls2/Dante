using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_rush : monster_one
{
    float arrowtime;
    public float arrowtime_Max;
    GameObject trail;
    public override void AttackReady()
    {
        rb.isKinematic = false;
        arrow.SetActive(true);
        StartCoroutine("Arrow_Ready");
        gameObject.layer = LayerMask.NameToLayer("rush");
    }

    IEnumerator Arrow_Ready()
    {
        while (arrowtime<=arrowtime_Max)
        {
            
            yield return new WaitForSeconds(0.01f);
            arrowtime += 0.01f;
            arrow.transform.localScale = new Vector3(1, 1, arrowtime);
            nav.enabled = false;
            if(arrowtime <= (arrowtime_Max*0.6f))
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), 0.1f);

        }
        if(arrowtime >= arrowtime_Max)
        {
            Attack();
            arrowtime = 0;
            arrow.transform.localScale = new Vector3(1, 1, arrowtime);
            arrow.SetActive(false);
        }


    }
    public void Attack()
    {
        if (!die)
        {
            attacking = true;
            StopCoroutine("Arrow_Ready");
           // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.transform.position - transform.position), 0.1f);
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 2000);
            rb.drag = 3;
            Invoke("attackstop", 0.5f);
        }
    }

    public void attackstop()
    {
        if (!die)
        {
            rb.drag = 0;
            attacking = false;
            rb.isKinematic = true;
            attack = false;
            gameObject.layer = LayerMask.NameToLayer("mob");
            attackcool = 0;
        }
    }

}
