using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thispet : MonoBehaviour
{

    public float damage;
    public float damage_per=1;
    public float attackcool,attackcool_max;
    public bool attackreal;
    public float dis;
    public Player player;
    public bool ifnotpower;
    private void Start()
    {
        player = GameManager.Instance.player.GetComponent<Player>();


    }

 

    public virtual void Update()
    {

        if (!ifnotpower)
        {
            if (attackcool > attackcool_max)
            {

                if (GameManager.Instance.RCC == GameManager.room_clear_check.battle)
                {
                    attackreal = true;
                    attackcool = 0;
                    Attack();

                }

            }
        }
        if (!attackreal)
            {
                attackcool += Time.deltaTime;
                if (Vector3.Distance(transform.position, player.transform.position) > dis)
                    this.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.08f);
                this.transform.LookAt(player.transform.position);
            }

       
    }
    public virtual void Attack()
    {

    }



}
