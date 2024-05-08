using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boompet : thispet
{   
    public GameObject attack_pre;

    public override void Attack()
    {
        GameObject boom = Instantiate(attack_pre,this.transform.position,Quaternion.identity);
      
        if (boom.GetComponent<Boom>() != null)
        boom.GetComponent<Boom>().damage = damage*damage_per;
        attackreal = false;

    }


}
