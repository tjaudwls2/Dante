using DamageNumbersPro;
using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class monster_one : MonoBehaviour
{
    public DamageNumber dn,cridn;
    public State mobstate;
    public GameObject Player;
   public  NavMeshAgent nav;
    Player playerscipct;
    public Rigidbody rb;
    public float hp;
    public bool die,attack,attacking, hit;
    public float attackcool;
    public GameObject arrow;
    public float Attack_dis;
    public bool confusion,blood,slow,Ice;
    public float contime, contime_Max,bloodtime,bloodtime_Max;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Player = GameManager.Instance.player;
        nav = GetComponent<NavMeshAgent>();
        nav.speed = mobstate.speed;
        playerscipct = Player.GetComponent<Player>();
        contime_Max = 10;
        bloodtime_Max = 10;
        dn = GameManager.Instance.dn;
        cridn  = GameManager.Instance.cridn;
        arrow = transform.Find("Arrow").gameObject;
        attackcool = Random.Range(0,mobstate.Attack_speed-1);
    }
   public  Vector3 target;
    [HideInInspector]
    public float slowtime,slowtime_Max=1;
    [HideInInspector]
    public float icetime;
    float icetime_Max = 10;
    float slow_per=1,ice_per=1;

    // Update is called once per frame
   public virtual void Update()
    {
         nav.speed = mobstate.speed * slow_per* ice_per;
        if (slow)
        {
            slow_per = 0.3f;
             slowtime += Time.deltaTime;
            if(slowtime > slowtime_Max)
            {
                nav.speed = mobstate.speed;
                slow_per = 1f;
                slowtime = 0;
                slow = false;
            }
        }
        if(Ice)
        {
          
            ice_per = 0.5f;
            icetime += Time.deltaTime;
            if (icetime > icetime_Max)
            {
                nav.speed = mobstate.speed;
                ice_per = 1f;
                icetime = 0;
                Ice = false;
            }


        }

        if (blood)
        {
            hp -= Time.deltaTime*(mobstate.hp*0.02f);
            bloodtime += Time.deltaTime;
            if (bloodtime > bloodtime_Max)
            {
                bloodtime = 0;
                blood = false;
                GameManager.Instance.ColorChage(gameObject, Color.white);
            }
        }


        if (!confusion)
        {
            target = Player.transform.position;
        }
        else
        {
            target = transform.position + (transform.position - Player.transform.position);

           contime += Time.deltaTime;
            if (contime > contime_Max)
            {
                contime = 0;
                confusion = false;
                GameManager.Instance.ColorChage(gameObject, Color.white);
            }
        }



      //  Debug.Log(Vector3.Distance(Player.transform.position, this.transform.position));
        if (!die&&!attack&&!hit)
        {
            if (nav.enabled)
            {
                nav.SetDestination(target);
                rb.velocity = Vector3.zero;

            }
            else
            {
                if (Time.timeScale > 0)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), 0.1f);
            }
            nav.enabled = (playerscipct.rush_Check != rush_check.rush);
            rb.isKinematic = (playerscipct.rush_Check != rush_check.rush);

            attackcool += Time.deltaTime;

            if(attackcool > mobstate.Attack_speed && Vector3.Distance(target, this.transform.position) <= Attack_dis )
            {
             
                attack = true;
                AttackReady();
            }

        }
        if (hit)
        {
            nav.enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }


    }

    public virtual void AttackReady()
    {
     
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerscipct.Damage_ca(this.gameObject);
            if (playerscipct.rush_Check == rush_check.rush)
                Damage_calculate(playerscipct.Playerstate.Attack_power);
        }
    }
  float dealtime, dealtimemax = 1;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Stay"))
        {
            
            


                dealtime += Time.deltaTime;
                if (dealtime > dealtimemax)
            {
                Damage_calculate(other.GetComponent<attack_Enemy_tri>().damage, false, true,true);
                dealtime = 0;

                }
            

        }
    }

    public void hitend()
    {
        hit = false;
    }

    public void Damage_calculate(float damage,bool playerhit=true,bool notshake = false,bool stay = false)//데미지 계산
    {
        hit = !stay;
   
        Invoke("hitend", 3f);

        float criper = Random.value;
        if (criper <= (playerscipct.Playerstate.critcal_Per*0.01f))//치명타
        {
            damage *= (playerscipct.Playerstate.critcal_Damage*0.01f);
            cridn.Spawn(transform.position + new Vector3(0, 2, 0), damage);
            GameManager.Instance.UE_cri.Invoke();
        }
        else
        {
            dn.Spawn(transform.position + new Vector3(0, 2, 0), damage);
        }
        hp -= damage;
        Instantiate(GameManager.Instance.hiteff, this.transform.position, this.transform.rotation);
        if (playerhit) 
        GameManager.Instance.UE_hitEnemy.Invoke(this.gameObject);
        if(!notshake)
        GameManager.Instance.mP.PlayFeedbacks();

        if (hp <= 0) Die();



    }
    public void Die()
    {
        nav.enabled = false;
        die = true;

        GetComponent<Collider>().enabled = false;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;

        rb.velocity = Vector3.zero;
        rb.AddTorque(new Vector3(50, 0, 50) * 5000);
        rb.AddForce(((transform.position - playerscipct.gameObject.transform.position).normalized + new Vector3(0,1,0)) * 1300);
        GameManager.Instance.mobs.Remove(this.gameObject);
        if(GameManager.Instance.mobs.Count == 0) {
            GameManager.Instance.RCC = GameManager.room_clear_check.clear;
        }
        GameManager.Instance.UE_Kill.Invoke();
        Destroy(this.gameObject, 1);


    }


}
