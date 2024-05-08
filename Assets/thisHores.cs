using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thisHores : thispet
{
    Animator anim; 
    private void Awake()
    {

        anim = GetComponent<Animator>();
        ifnotpower = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        anim.SetBool("Run", Vector3.Distance(transform.position, player.transform.position) > dis);

    }
}
