using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class Player : MonoBehaviour
{
    public enum rush_check
    {
        idle,
        aiming, //조준중일때
        rush


    }

    public State Playerstate;
    public float rush_power;
    public float rush_time;
    public GameObject  Arrow;
    public Rigidbody playerrid;
    rush_check rush_Check= rush_check.idle;
    Vector3 startpos, endpos;
    private void Start()
    {

        playerrid = this.GetComponent<Rigidbody>();
    }


    private void Update()
    {
        

        switch (rush_Check)
        {
            case rush_check.idle:

                Move();
                if (Input.GetMouseButtonDown(0))
                {
                    Time.timeScale = 0;
                    startpos = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
                    Arrow.transform.rotation = Quaternion.LookRotation(new Vector3((startpos - endpos).normalized.x, 0, (startpos - endpos).normalized.z));
                    Arrow.gameObject.SetActive(true);
                    playerrid.drag = 0;
                    rush_Check= rush_check.aiming;
                }
                break;
            case rush_check.aiming:


                Inputclick();

                break;


            case rush_check.rush:

               



                break;
        }
        
    }
    public void Inputclick()
    {



      
        if (Input.GetMouseButton(0))
        {
            endpos = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
            Arrow.transform.rotation = Quaternion.LookRotation(new Vector3((startpos - endpos).normalized.x, 0, (startpos - endpos).normalized.z));
        }
        if (Input.GetMouseButtonUp(0))
        {
            endpos = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
            playerrid.AddForce((startpos - endpos).normalized * rush_power);
            
            Arrow.gameObject.SetActive(false);
            rush_Check = rush_check.rush;
            Time.timeScale = 1;
            StartCoroutine("stops");
        }
    }

    IEnumerator stops()
    {
        yield return new WaitForSeconds(rush_time);
        playerrid.drag = 5;
        yield return new WaitForSeconds(1);
        playerrid.velocity = Vector3.zero;
        rush_Check = rush_check.idle;
    }



    public void Move()
    {
        float ho = Input.GetAxisRaw("Horizontal");
        float vo = Input.GetAxisRaw("Vertical");

        playerrid.velocity = new Vector3(ho,0,vo)* Playerstate.speed;
    }
}

