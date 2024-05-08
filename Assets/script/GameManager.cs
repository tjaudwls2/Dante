
using DamageNumbersPro;
using DG.Tweening;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
[System.Serializable] public class BossEvent : UnityEvent<GameObject> { }
public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent UE_Itemget, UE_cri, UE_hit, UE_attack, UE_attack_End, UE_room, UE_die, UE_Kill, UE_Stop,UE_First,UE_Goldplus ; // �����۾�����,ġ��Ÿ��,�ǰݽ�,���ݽ�, �濡 �����,�����,��óġ�� ,����� , ó������ �����ҽ�
    public BossEvent UE_Bossspawn, UE_hitEnemy;//������ ��ȯ�ɽ�, ���� ��ġ�ҽ�


    public monster_state MS;
    public float Gold;
    public DamageNumber dn, cridn, evdn;
    public GameObject hiteff;
    public MMF_Player mP,playermp;
    public enum room_clear_check
    {
        clear,
        battle


    }
    [System.Serializable]
    public class mobspawn_count
    {
      public  int min;
      public  int max;


    }
    [System.Serializable]
    public class spawnmob
    {
        public GameObject mob;
        public float spawn_Per;
    }
    [System.Serializable]
    public class Floors
    {
        public spawnmob[] mobs;
        public GameObject Boss;
        //������


    }

    public int spawnper(Floors floors)
    {
    

        
        float all = 0;
        foreach(spawnmob mobs in floors.mobs)
        {
            all += mobs.spawn_Per;
        }
        float weight = 0;
        int selectNum = Mathf.RoundToInt(all*Random.Range(0.0f,1.0f));
        for(int i = 0; i< floors.mobs.Length; i++)
        {
            weight += floors.mobs[i].spawn_Per;
            if (selectNum <= weight)
            {

                return i;
            }
        }
        return 0;

    }
    public void GoldPlus(float plus)
    {
        Gold += plus;
        UE_Goldplus.Invoke();

    }

    #region �̱���
    private static GameManager instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        player = GameObject.Find("Player");
    }
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }


    #endregion

    [HideInInspector]
    public GameObject player;
    public room_clear_check RCC;
    [HideInInspector]
    public float min_roomSize, max_roomSize;
    public float[] up_mid_low = new float[4];

    public  mobspawn_count mobspawn_countlow;//�����濡�� ��� ��ȯ����
    public  mobspawn_count mobspawn_countmid;//�߰��濡�� ��� ��ȯ����
    public  mobspawn_count mobspawn_countup; //ū�濡��   ��� ��ȯ����

    public Floors[] floors;

    public List<GameObject> mobs;//������� 







    private void Start()
    {
   
        minmax_cal();
    }
   
    public void minmax_cal()//��ũ�⸦ ����Ͽ� ū�� �߰��� �������� ������ �Լ��Դϴ� 
    {
        float max_min = (max_roomSize* max_roomSize) - (min_roomSize* min_roomSize); 
        max_min /= 3;
        //  Debug.Log("������"+max_min);
        up_mid_low[0] = (min_roomSize * min_roomSize);
        up_mid_low[1] = (min_roomSize * min_roomSize) + max_min;
        up_mid_low[2] = (max_roomSize * max_roomSize) - max_min;
        up_mid_low[3] = (max_roomSize * max_roomSize);
   

    }

    public Vector3 spawnposcheck(Vector3 spawnpos,float x,float y)
    {
        RaycastHit hit;
        float min = 0;
        
        if (x > y)
             min= y;
        else
            min=x;

        while (true)
        {

            if (Physics.Raycast(spawnpos + new Vector3(Random.Range(-max_roomSize * 3, max_roomSize * 3), 20, Random.Range(-max_roomSize * 3, max_roomSize * 3)), Vector3.down, out hit, 50))
            {


                //
                if (Vector3.Distance(hit.point, spawnpos) < (min*3) && Vector3.Distance(hit.point, player.transform.position) > 10 && hit.transform.gameObject.layer == LayerMask.NameToLayer("Spawnbox_R"))
                {


                    spawnpos = hit.point;
                    return spawnpos;
                }
            }
       


        }
        
    }


    public void BossSpawn(Vector3 pos, float x, float y)
    {
        GameObject mob_test = Instantiate(floors[0].Boss, spawnposcheck(pos, x, y), Quaternion.identity);

        foreach (Monster_Sheet mobsheet in MS.Sheet1)
        {
            if (mob_test.name == mobsheet.name + "(Clone)")
            {

                mob_test.GetComponent<monster_one>().mobstate.hp = mobsheet.hp;
                mob_test.GetComponent<monster_one>().hp = mobsheet.hp;
                mob_test.GetComponent<monster_one>().mobstate.speed = mobsheet.speed;
                mob_test.GetComponent<monster_one>().mobstate.Attack_power = mobsheet.Attack_power;
                mob_test.GetComponent<monster_one>().mobstate.Shield_power = mobsheet.Shield_power;
                mob_test.GetComponent<monster_one>().mobstate.critcal_Damage = mobsheet.critcal_Damage;
                mob_test.GetComponent<monster_one>().mobstate.critcal_Per = mobsheet.critcal_Per;
                mob_test.GetComponent<monster_one>().mobstate.Attack_speed = mobsheet.Attack_speed;
                mob_test.GetComponent<monster_one>().Attack_dis = mobsheet.Attack_dis;

            }
        }
        UE_Bossspawn.Invoke(mob_test);

        mobs.Add(mob_test);
        RCC = room_clear_check.battle;
    }

    public void MobSpawn(Vector3 pos,float x, float y)
    {
        int mobspawn_count = 0; // ��� �������� ��ũ�⿡ ���� ����
     
        float scale = x * y;
        if (up_mid_low[0] <= scale && scale < up_mid_low[1])
        {
            mobspawn_count = Random.Range(mobspawn_countlow.min,mobspawn_countlow.max+1);
      
        }
        else if(up_mid_low[1] <= scale && scale < up_mid_low[2])
        {
            mobspawn_count = Random.Range(mobspawn_countmid.min, mobspawn_countmid.max+1);
         
        }
        else if (up_mid_low[2] <= scale && scale <= up_mid_low[3])
        {
            mobspawn_count = Random.Range(mobspawn_countup.min, mobspawn_countup.max+1);
         
        }

  
        for (int i = 0; i < mobspawn_count; i++)
        {


            GameObject mob_test = Instantiate(floors[0].mobs[spawnper(floors[0])].mob, spawnposcheck(pos,x,y), Quaternion.identity);
        
            foreach (Monster_Sheet mobsheet in MS.Sheet1)
            {
                if( mob_test.name == mobsheet.name+"(Clone)")
                {
 
                    mob_test.GetComponent<monster_one>().mobstate.hp            = mobsheet.hp            ;
                    mob_test.GetComponent<monster_one>().hp = mobsheet.hp;
                    mob_test.GetComponent<monster_one>().mobstate.speed         = mobsheet.speed         ;
                    mob_test.GetComponent<monster_one>().mobstate.Attack_power  = mobsheet.Attack_power  ;
                    mob_test.GetComponent<monster_one>().mobstate.Shield_power  = mobsheet.Shield_power  ;
                    mob_test.GetComponent<monster_one>().mobstate.critcal_Damage= mobsheet.critcal_Damage;
                    mob_test.GetComponent<monster_one>().mobstate.critcal_Per   = mobsheet.critcal_Per   ;
                    mob_test.GetComponent<monster_one>().mobstate.Attack_speed  = mobsheet.Attack_speed  ;
                    mob_test.GetComponent<monster_one>().Attack_dis  = mobsheet.Attack_dis   ;

                }
            }

            mobs.Add(mob_test);
        }



        RCC = room_clear_check.battle;




    }
    
    public void ColorChage(GameObject target , UnityEngine.Color WantColor)
    {
        Transform[] allChildren = target.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.GetComponent<SkinnedMeshRenderer>() != null)
            {
                child.GetComponent<SkinnedMeshRenderer>().material.DOColor(WantColor, 1f);
            }
            if (child.GetComponent<MeshRenderer>() != null) {
                child.GetComponent<MeshRenderer>().material.DOColor(WantColor, 1f);
            }
        }
    }
    



}
