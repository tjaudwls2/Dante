using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static Player;
public enum rush_check
{
    idle,
    aiming, //조준중일때
    rush


}
public class Player : MonoBehaviour
{
    public FieldOfView Fov;
    public Image HpGage,staminaGage;
    public PhysicMaterial bouns;
    public State Playerstate,startPS;





    public Animator anim;
    public float hp;
    public GameObject  Arrow,Arrowpos,Arrowtwo,Arrowtwopos,trail;
    public Rigidbody playerrid;
    [HideInInspector]
    public rush_check rush_Check= rush_check.idle;
    Vector3 startpos, endpos;
    public Volume volume;
    public  bool hit;
    public float hittime;
    float hittime_max = 0.5f;
    public float timeslow=0.6f;
    public float attack_cool;
    bool attack_check;
    [HideInInspector]
    public bool firstRoomin;
    float angle = 1;
    private void Start()
    {

        playerrid = this.GetComponent<Rigidbody>();
        hp = Playerstate.hp;
        attack_cool = 100;
        Arrowpos = Arrow.transform.GetChild(0).GetChild(0).gameObject;
        Arrowtwopos = Arrowtwo.transform.GetChild(0).GetChild(0).gameObject;

        startPS.hp = Playerstate.hp;
        startPS.speed = Playerstate.speed;
        startPS.Attack_power = Playerstate.Attack_power;
        startPS.Shield_power= Playerstate.Shield_power;
        startPS.critcal_Damage = Playerstate.critcal_Damage;
        startPS.critcal_Per = Playerstate.critcal_Per;
        startPS.Attack_speed = Playerstate.Attack_speed;
        startPS.rush_power =  Playerstate.rush_power;
        startPS.rush_time = Playerstate.rush_time;
        startPS.evasion = Playerstate.evasion;
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    public List<GameObject> mobs;
    public  bool joystick;
    public void hitout()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    public void black_whiteoff()
    {
        ColorAdjustments CA;
        Vignette VN;
        if (volume.profile.TryGet(out CA))
        {
            DOTween.Kill(CA);
            CA.saturation.value = 0;

       }
        if (volume.profile.TryGet(out VN))
        {
            DOTween.Kill(VN);
            VN.intensity.value = 0;
          //  DOTween.To(() => VN.intensity.value, x => VN.intensity.value = x, VNV, 0.4f).SetEase(Ease.InQuad).SetUpdate(true);
        }
    }
    public void black_white(float BWV,float VNV)
    {
        ColorAdjustments CA;
        Vignette VN;
        if (volume.profile.TryGet(out CA))
        {

            DOTween.To(() => CA.saturation.value, x => CA.saturation.value = x, BWV, 0.4f).SetEase(Ease.InQuad).SetUpdate(true);
        }
        if (volume.profile.TryGet(out VN))
        {
            DOTween.To(() => VN.intensity.value, x => VN.intensity.value = x, VNV, 0.4f).SetEase(Ease.InQuad).SetUpdate(true);
        }
    }
    private void Update()
    {
        hittime += Time.deltaTime;
        if(hittime >= hittime_max&&hit)
        {
            hit = false;
            Invoke("hitout", 0.5f);
        }
        HpGage.fillAmount = hp / Playerstate.hp;
        attack_cool += Time.deltaTime;
        if(attack_cool >= Playerstate.Attack_speed)
        {
            attack_check = true;
        }
        else
            attack_check = false;
        staminaGage.fillAmount = attack_cool/ Playerstate.Attack_speed;

        switch (rush_Check)
        {
            case rush_check.idle:

                if (!hit)
                {
                    Move();
                    if (attack_check)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                       
                            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeslow, 1f).SetEase(Ease.InQuad).SetUpdate(true);
                            black_white(-50,0.3f);
                            //  Time.timeScale = angle;


                            rush_Check = rush_check.aiming;
                            startpos = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
                            Arrow.transform.rotation = Quaternion.LookRotation(new Vector3((startpos - endpos).normalized.x, 0, (startpos - endpos).normalized.z));
                            Arrow.gameObject.SetActive(true);
                           
                           
                            mobs.Clear();
                            foreach (GameObject mob in GameManager.Instance.mobs)
                            {
                                mobs.Add(mob);
                            }
                            joystick = false;
                            attack_check = false;
            
                                       playerrid.velocity = Vector3.zero;
                            Arrow.transform.localScale = new Vector3(1, 1, 0);
                            input_check = 0;
                           
                        }
                       // int x = Input.GetAxisRaw("Mouse X") >= 0.1f ? 1 : (Input.GetAxisRaw("Mouse X") <= -0.1f ? -1 : 0);
                       // int y = Input.GetAxisRaw("Mouse Y") >= 0.1f ? 1 : (Input.GetAxisRaw("Mouse Y") <= -0.1f ? -1 : 0);
                        //   Debug.Log(Input.GetAxisRaw("Fire1"));
                        GetComponent<CapsuleCollider>().material = null;
                        if (Input.GetAxisRaw("Fire1") == 1)
                        {
                                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeslow, 1f).SetEase(Ease.InQuad).SetUpdate(true);
                                Arrowtwo.transform.rotation = Quaternion.LookRotation(new Vector3(Input.GetAxis("Mouse Y"), 0, Input.GetAxis("Mouse X")) + Quaternion.Euler(0, 90, 0).eulerAngles);
                            black_white(-50,0.3f);
                            Arrowtwo.gameObject.SetActive(true);
                            
                            playerrid.velocity = Vector3.zero;
                                rush_Check = rush_check.aiming;
                                mobs.Clear();
                                foreach (GameObject mob in GameManager.Instance.mobs)
                                {
                                    mobs.Add(mob);
                                }
                                joystick = true;
                                attack_check = false;
                                 playerrid.velocity = Vector3.zero;
                            Arrow.transform.localScale = new Vector3(1, 1, 0);
                                input_check = 0;
                        
                        }
                    }
                }

                break;
            case rush_check.aiming:
                GetComponent<CapsuleCollider>().material = bouns;
                if (!joystick)
                Inputclick();
              else
                  joyInputclick();
                
                    break;


            case rush_check.rush:


                Time.timeScale = 1;


                break;
        }
        
    }
    public float input_check;
    public void joyInputclick()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player");

        if (Input.GetAxisRaw("Fire1") == 0)
        {
            if (!hit)
            {
                anim.SetBool("Attack", true);
                GameManager.Instance.UE_attack.Invoke();
                playerrid.drag = 0;
                playerrid.AddForce((Arrowtwopos.transform.position - this.transform.position).normalized * Playerstate.rush_power * input_check);
                black_white(0,0);
                joystick = false;
                Arrowtwo.gameObject.SetActive(false);
                rush_Check = rush_check.rush;
                Time.timeScale = 1;
                attack_cool = 0;
                trail.SetActive(true);
                StartCoroutine("stops");
                
            }
        }
        else
        {


            Arrowtwo.transform.rotation = Quaternion.LookRotation(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
            this.transform.rotation = Quaternion.LookRotation(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
            //  transform.Rotate(0, -90, 0);

            input_check += Time.unscaledDeltaTime * 2;
            if (input_check > 1)
            {
                input_check = 1;
            }
            Arrowtwo.transform.localScale = new Vector3(1, 1, input_check);
        }
    }
    public void Inputclick()
    {


        this.gameObject.layer = LayerMask.NameToLayer("Player");

        if (Input.GetMouseButton(0))
        {
            endpos = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
            Arrow.transform.rotation = Quaternion.LookRotation(new Vector3((startpos - endpos).normalized.x, 0, (startpos - endpos).normalized.z));
            this.transform.rotation = Quaternion.LookRotation(new Vector3((startpos - endpos).normalized.x, 0, (startpos - endpos).normalized.z));

            input_check += Time.unscaledDeltaTime*2 ;
            if (input_check > 1)
            {
                input_check = 1;
            }
            //transform.Rotate(0, -90, 0);
            Arrow.transform.localScale = new Vector3(1, 1, input_check);

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!hit)
            {
                anim.SetBool("Attack",true);
                GameManager.Instance.UE_attack.Invoke();
                playerrid.drag = 0;
                black_white(0,0);
                endpos = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
                playerrid.AddForce((Arrowpos.transform.position - this.transform.position).normalized * Playerstate.rush_power * input_check);
                trail.SetActive(true);
                attack_cool = 0;
                Arrow.gameObject.SetActive(false);
                rush_Check = rush_check.rush;
                Time.timeScale = 1;
                StartCoroutine("stops");
            }
        }



    }

    IEnumerator stops()
    {
        yield return new WaitForSeconds(Playerstate.rush_time);
        playerrid.drag = 5;
        anim.SetBool("Attack", false);
 
        yield return new WaitForSeconds(1);
        GameManager.Instance.UE_attack_End.Invoke();
        playerrid.velocity = Vector3.zero;
        rush_Check = rush_check.idle;
        trail.SetActive(false);

        if (firstRoomin)
        {   firstRoomin = false;
            GameManager.Instance.UE_First.Invoke();
        } 
    }

    public float stoptime;

    public void Move()
    {
        float ho = Input.GetAxisRaw("Horizontal");
        float vo = Input.GetAxisRaw("Vertical");
        Vector3 movedir = new Vector3(ho, 0, vo);
        playerrid.velocity = movedir * Playerstate.speed;
       

        if (!(ho == 0 && vo == 0))
        {
            stoptime = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movedir), Time.deltaTime * 50);
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
           
            stoptime += Time.deltaTime;
            if(stoptime >= 10)
            {
                GameManager.Instance.UE_Stop.Invoke();
                stoptime = 0;
            }
        }

      //  Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
      //
      //  Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
      //
      //  float rayLength;
      //
      //  if (GroupPlane.Raycast(cameraRay, out rayLength))
      //  {
      //
      //      Vector3 pointTolook = cameraRay.GetPoint(rayLength);
      //
      //      transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
      //
      //  }
      //  this.transform.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mob"))
        {
            if(rush_Check == rush_check.rush)
            {
                Vector3 targetpos= Vector3.zero;
           
                    targetpos = playerrid.velocity.normalized;
              
                

                transform.rotation = Quaternion.LookRotation(new Vector3(targetpos.x, 0, targetpos.z));
                playerrid.AddForce(targetpos * Playerstate.rush_power);
            }
        

        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (rush_Check == rush_check.rush)
            {
                Vector3 targetpos = Vector3.zero;
                // Fov.FindVisibleTargets();
                if (ItemManager.Iteminstance.foundmob() != null)//Fov.visibleTargets.Count > 0)
                {
                    playerrid.velocity = Vector3.zero;
                    targetpos = (ItemManager.Iteminstance.foundmob().transform.position - transform.position).normalized;
                    
                }
                else
                    targetpos = playerrid.velocity.normalized;



                transform.rotation = Quaternion.LookRotation(new Vector3(targetpos.x, 0, targetpos.z));
                playerrid.AddForce(targetpos * Playerstate.rush_power);
            }
        }
        if (collision.gameObject.CompareTag("Mob") && rush_Check == rush_check.idle)
        {
         
        }
    
    
    
    }
   
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack_Object"))
        {
            Damage_ca(other.gameObject);
        }
    }
    public void Damage_ca(GameObject damage)
    {
        switch (rush_Check)
        {
            case rush_check.idle:



                if (!hit)
                {
                    float Rans = Random.value * 100;
                    if (Rans > Playerstate.evasion)
                    {
                        anim.SetTrigger("Hit");
                        gameObject.layer = LayerMask.NameToLayer("hit");
                        hit = true;
                        hp -= damage.gameObject.GetComponent<Damage>()!=null ? damage.gameObject.GetComponent<Damage>().damage : damage.gameObject.GetComponent<monster_one>().mobstate.Attack_power;
                        
                        GameManager.Instance.playermp.PlayFeedbacks();
                        hittime = 0;
                        // playerrid.drag = 5;
                        playerrid.AddForce((this.transform.position - damage.gameObject.transform.position).normalized * 3000f);
                        black_whiteoff();
                        GameManager.Instance.UE_hit.Invoke();
                    }
                    else
                    {
                        GameManager.Instance.evdn.Spawn(this.transform.position,"Nope");
                    }
                }

                break;
            case rush_check.aiming:
                if (!hit)
                {
                    float Rans = Random.value * 100;
                    if (Rans > Playerstate.evasion)
                    {
                        anim.SetTrigger("Hit");
                        gameObject.layer = LayerMask.NameToLayer("hit");
                        hit = true;
                        hp -= damage.gameObject.GetComponent<Damage>() != null ? damage.gameObject.GetComponent<Damage>().damage : damage.gameObject.GetComponent<monster_one>().mobstate.Attack_power;
                        //  playerrid.drag = 5;
                     
                        GameManager.Instance.playermp.PlayFeedbacks();
                        hittime = 0;
                        playerrid.AddForce((this.transform.position - damage.gameObject.transform.position).normalized * 3000f);
                        joystick = false;
                        Arrowtwo.gameObject.SetActive(false);
                        Arrow.gameObject.SetActive(false);
                        rush_Check = rush_check.idle;
                        DOTween.KillAll(this);
                        black_whiteoff();
                        Time.timeScale = 1;
                        GameManager.Instance.UE_hit.Invoke();
                    }
                    else
                    {
                        GameManager.Instance.evdn.Spawn(this.transform.position, "Nope");
                    }
                }

                break;
            case rush_check.rush:

                // if (!hit)
                // {
                //     gameObject.layer = LayerMask.NameToLayer("hit");
                //     hit = true;
                //     hp -= damage.gameObject.GetComponent<Damage>()!=null ? damage.gameObject.GetComponent<Damage>().damage : damage.gameObject.GetComponent<monster_one>().mobstate.Attack_power;
                //     //  playerrid.drag = 5;
                //     HpGage.fillAmount = hp / Playerstate.hp;
                //     GameManager.Instance.playermp.PlayFeedbacks();
                //     hittime = 0;
                //     playerrid.AddForce((this.transform.position - damage.gameObject.transform.position).normalized * 3000f);
                //     joystick = false;
                //     Arrowtwo.gameObject.SetActive(false);
                //     Arrow.gameObject.SetActive(false);
                //     rush_Check = rush_check.idle;
                //     DOTween.KillAll(this);
                //     trail.SetActive(false);
                //     black_whiteoff();
                //     Time.timeScale = 1;
                // }

                break;
        }
        if(hp <= 0)
        {
            GameManager.Instance.UE_die.Invoke();

        }
        if (hp <= 0)
        {
        
        
        }



    }
}

