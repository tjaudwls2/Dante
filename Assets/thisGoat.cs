using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thisGoat : thispet
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
        if (Vector3.Distance(transform.position, player.transform.position) > dis)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.08f);
            this.transform.LookAt(player.transform.position);
        }
        anim.SetBool("Run", Vector3.Distance(transform.position, player.transform.position) > dis);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack_Object"))
        {
            transform.LookAt(other.transform.position);
            Destroy(other.gameObject);


        }
    }
}
