using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_Enemy_tri : MonoBehaviour
{
    public float damage;
    public bool notadd,startdes;
//    public float dealtime, dealtimemax = 1;
    public float destime = 0;
    public bool rushend_des;
    Player player;
    private void Start()
    {
        if (startdes)
        {
            Destroy(gameObject,destime);
        }
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (rushend_des)
        {
            if (player.rush_Check!=rush_check.rush)
                Destroy(gameObject);

        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Mob"))
        {
            if (!notadd)
            {
                other.gameObject.GetComponent<monster_one>().Damage_calculate(damage, false);
                other.GetComponent<Rigidbody>().AddForce((other.transform.position - this.transform.position).normalized * 300);
            }
           
        }
    }

   // private void OnTriggerStay(Collider other)
   // {
   //     if (other.gameObject.CompareTag("Mob"))
   //     {
   //         if (notadd)
   //         {
   //            
   //             other.gameObject.GetComponent<monster_one>().Damage_calculate(damage, false,true);
   //             dealtime += Time.deltaTime;
   //             if (dealtime > dealtimemax)
   //             {
   //                 dealtime = 0;
   //
   //             }
   //         }
   //         
   //     }
   // }
}
