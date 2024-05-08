using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Playables;
using static UnityEditor.Progress;


public class ItemManager : MonoBehaviour
{
    [HideInInspector]
    public List<thispet> thispets = new List<thispet>();
    public List<item> Haveitem;
    public Items items;
    public List<item> itemList;

    public Itemex itemExcel;
    [HideInInspector]
    public Player player;


    #region 싱글톤
    private static ItemManager iteminstance = null;
    void Awake()
    {
        if (null == iteminstance)
        {
            iteminstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        for (int i = 0; i < items.item.Count; i++)
        {
            items.item[i].name = itemExcel.Sheet1[i].name;
            items.item[i].itemtype = (itemtype)itemExcel.Sheet1[i].itemtype;
            itemList.Add(items.item[i].DeepCopy());
        }
    }
    public static ItemManager Iteminstance
    {
        get
        {
            if (null == iteminstance)
            {
                return null;
            }
            return iteminstance;
        }
    }
    #endregion


    public float dishWashMachineCount, earphoneCount;



    private void Start()
    {
        player = GameManager.Instance.player.GetComponent<Player>();
    }

    public item Radomitem()
    {
        item item_S = null;
        List<item> itemlist = new List<item>();
        float all = 0;
        float[] per = new float[5] { 70, 30, 15, 5, 1 };
        int rare = 0;
        itemtype itemtype = 0;
        foreach (float thisper in per)
        {
            all += thisper;
        }
        float weight = 0;
        int selectNum = Mathf.RoundToInt(all * UnityEngine.Random.Range(0.0f, 1.0f)); 
        for (int i = 0; i < per.Length; i++)
        {
          
            weight += per[i];
           
            if (selectNum <= weight)
            {

                rare = i;
                break;
            }
        }
        
        itemtype = (itemtype)rare;

        foreach (item item in itemList)
        {
            if (item.itemtype == itemtype)
                itemlist.Add(item);
        }

        item_S = itemlist[UnityEngine.Random.Range(0, itemlist.Count)];



        return item_S;
    }



    public void Get_Item(item item)
    {
        Haveitem.Add(item);
        statereset();
        item_Check_one(item);

        foreach (item item_ in itemList)
        {
            if (item_.name == item.name)
            {
                itemList.Remove(item_);
                break;
            }
        }
        GameManager.Instance.UE_Itemget.Invoke();
    }

    public void statereset()//스탯 초기화
    {
        foreach (item item_ in Haveitem)
            item_Check_First(item_);

        player.Playerstate.hp = player.startPS.hp;
        player.Playerstate.speed = player.startPS.speed;
        player.Playerstate.Attack_power = player.startPS.Attack_power;
        player.Playerstate.Shield_power = player.startPS.Shield_power;
        player.Playerstate.critcal_Damage = player.startPS.critcal_Damage;
        player.Playerstate.critcal_Per = player.startPS.critcal_Per;
        player.Playerstate.Attack_speed = player.startPS.Attack_speed;
        player.Playerstate.rush_power = player.startPS.rush_power;
        player.Playerstate.rush_time  = player.startPS.rush_time ;
        player.Playerstate.evasion = player.startPS.evasion;

        player.timeslow = 0.6f;
        GameManager.Instance.UE_Itemget.RemoveAllListeners();
        GameManager.Instance.UE_hit.RemoveAllListeners();
        GameManager.Instance.UE_cri.RemoveAllListeners();
        GameManager.Instance.UE_attack.RemoveAllListeners();
        GameManager.Instance.UE_attack_End.RemoveAllListeners();
        GameManager.Instance.UE_room.RemoveAllListeners();
        GameManager.Instance.UE_die.RemoveAllListeners();
        GameManager.Instance.UE_Kill.RemoveAllListeners();
        GameManager.Instance.UE_Stop.RemoveAllListeners();
        GameManager.Instance.UE_First.RemoveAllListeners();
        GameManager.Instance.UE_Bossspawn.RemoveAllListeners();
        GameManager.Instance.UE_hitEnemy.RemoveAllListeners();
        GameManager.Instance.UE_Goldplus.RemoveAllListeners();

        player.Playerstate.speed += dishWashMachineCount * 0.5f;
        foreach (item item_ in Haveitem)
        {
            player.Playerstate.hp += item_.State.hp;
            player.Playerstate.speed += item_.State.speed;
            player.Playerstate.Attack_power += item_.State.Attack_power;
            player.Playerstate.Shield_power += item_.State.Shield_power;
            player.Playerstate.critcal_Damage += item_.State.critcal_Damage;
            player.Playerstate.critcal_Per += item_.State.critcal_Per;
            player.Playerstate.Attack_speed += item_.State.Attack_speed;
            player.Playerstate.rush_power += item_.State.rush_power;
            player.Playerstate.rush_time += item_.State.rush_time;
            player.Playerstate.evasion += item_.State.evasion;



        }

        foreach (item item_ in Haveitem)
        {


            player.Playerstate.hp *= item_.xstate.hp;
            player.Playerstate.speed *= item_.xstate.speed;
            player.Playerstate.Attack_power *= item_.xstate.Attack_power;
            player.Playerstate.Shield_power *= item_.xstate.Shield_power;
            player.Playerstate.critcal_Damage *= item_.xstate.critcal_Damage;
            player.Playerstate.critcal_Per *= item_.xstate.critcal_Per;
            player.Playerstate.Attack_speed *= item_.xstate.Attack_speed;
            player.Playerstate.rush_power *= item_.xstate.rush_power;
            player.Playerstate.rush_time *= item_.xstate.rush_time;
            player.Playerstate.evasion *= item_.xstate.evasion;


        }
        foreach (item item_ in Haveitem)
            item_Check_Last(item_);


    }
    public void item_Check_First(item _item)//이름체크후 함수 발동
    {






        switch (_item.name)
        {

            case "보자기": if (Haveitem.Count >= 5) { _item.State.Attack_power = 2; _item.State.speed = 3; } break;

            case "들짐승눈알": if(player.Playerstate.Attack_power >= 10)  { _item.State.critcal_Per = 20; } break;

            case "짐승도감": if(player.Playerstate.critcal_Damage >= 3) { _item.State.Attack_speed = -0.3f; }break;

            case "인두": _item.xstate.Attack_power = 1+(indoCount * 0.001f); break;

            case "동물 서적 세트": if (player.Playerstate.critcal_Damage >= 2) _item.State.Attack_speed = -0.7f; break;

            case "짐승 해부도": if (player.Playerstate.critcal_Damage > 3) _item.State.Attack_speed = -0.5f; break;

            case "클러치 백": _item.State.Attack_power = GameManager.Instance.Gold * 0.05f; break;
        }
    }

    public void item_Check_one(item _item)
    {
        switch (_item.name)
        {
            case "약과": player.hp += (player.Playerstate.hp * _item.xstate.hp) - player.Playerstate.hp; break;

            case "송편": player.hp += (player.Playerstate.hp * _item.xstate.hp) - player.Playerstate.hp; break;

            case "가래떡": player.hp += (player.Playerstate.hp * _item.xstate.hp) - player.Playerstate.hp; break;

            case "애완용 천사": petitem(smallangle);  break;

            case "애완용 악마": petitem(smalldevil);   break;

            case "애완동물 간식": lurch(); break;

            case "부적 (쥐)": petitem(Ret);         break;

            case "부적 (토끼)": petitem(Rabbit);   break;
           
            case "부적 (원숭이)" : petitem(Monkey); break;
                                                   
            case "부적 (돼지)"   : petitem(Pig);    break;
                                                   
            case "부적 (양)"     : petitem(Goat);  break;
                                                   
            case "부적 (말)"     : petitem(Horse); break;
                                                   
            case "부적 (닭)": petitem(Rooster);    break;

            case "신성한 라이트": GameObject light_ = Instantiate(headLight, player.transform.position, player.transform.rotation); light_.transform.parent = player.transform; break;

            case "얼어붙은 인형":petitem(icedoll); break;



        }

    }

    public void petitem(GameObject pet)
    {
        GameObject pet_ = Instantiate(pet, player.transform.position, Quaternion.identity); thispets.Add(pet_.GetComponent<thispet>()); lurch();
    }

    public void lurch()
    {
        
            foreach (item item in Haveitem)
            {
            if (item.name == "애완동물 간식")
            {
                foreach(thispet tp in thispets)
                { tp.damage_per = 1.2f; }
            }
        }



           
        
    }


    public void item_Check_Last(item _item)//이름체크후 함수 발동
    {
    



        switch (_item.name)
        {
        
            case "작은 주머니": GameManager.Instance.UE_Itemget.AddListener(supply_box); break;

            case "식기세척기": GameManager.Instance.UE_hit.AddListener(dishWashMachine); break;

            case "도둑의 장갑" : GameManager.Instance.UE_cri.AddListener(fiveLuck);  break;

            case "간식 보따리": GameManager.Instance.UE_hit.AddListener(FatigueReliever); break;

            case "해시계": player.timeslow -= 0.2f; break;

            case "시계바늘": GameManager.Instance.UE_attack.AddListener(earphone); GameManager.Instance.UE_attack_End.AddListener(earphone_End);  break;

            case "밤양갱": GameManager.Instance.UE_hit.AddListener(biscuit); break;

            case "사탕":GameManager.Instance.UE_Stop.AddListener(Candy);break;

            case "목화": player.Playerstate.rush_power += (player.Playerstate.speed * 10f);    break;

            case "강아지 회화도":
               
                foreach (item item in Haveitem)
                {
                    if (item.name == "고양이 회화도")
                    {
                        player.Playerstate.Attack_power *= 2;
                        break;
                    }

                }
                break;

            case "고양이 회화도":

                foreach (item item in Haveitem)
                {
                    if (item.name == "강아지 회화도")
                    {
                        player.Playerstate.critcal_Damage *= 2;
                        break;
                    }

                }
                break;
           
            case "짐승 다리": GameManager.Instance.UE_hit.AddListener(beastLeg); break;

            case "새 휠": GameManager.Instance.UE_attack.AddListener(NewWheel); break;

            case "인두": GameManager.Instance.UE_hit.AddListener(indo); GameManager.Instance.UE_Kill.AddListener(indo);  break;

            case "예리한 비녀": GameManager.Instance.UE_Bossspawn.AddListener(benu);  break;

            case "천문시계": player.timeslow -= 0.4f; break;

            case "수배명단":  GameManager.Instance.UE_First.AddListener(wantedList); break;

            case "주리틀기": GameManager.Instance.UE_hit.AddListener(jure); break;

            case "회생의 부적": GameManager.Instance.UE_die.AddListener(resurrection);break;

            case "탐관의 부채": GameManager.Instance.UE_Kill.AddListener(fan);break;

            case "탄성 타이어": GameManager.Instance.UE_attack.AddListener(Tire);break;

            case "얼룩진 사이드미러": GameManager.Instance.UE_hitEnemy.AddListener(SideMirror);break;

            case "깨진 헬멧": GameManager.Instance.UE_hitEnemy.AddListener(brokenHelmet);break;

            case "작은 볼트": GameManager.Instance.UE_hitEnemy.AddListener(Volt);break;

            case "권투 글러브": GameManager.Instance.UE_attack_End.AddListener(Glove); break;

            case "천국제 스프링": GameManager.Instance.UE_hitEnemy.AddListener(hevenspring);   break;

            case "호신용 단도": GameManager.Instance.UE_hitEnemy.AddListener(blood); break;

            case "영혼 기름":GameManager.Instance.UE_attack_End.AddListener(Oil);break;

            case "의식용 칼":  GameManager.Instance.UE_attack.AddListener(Knife);break;

            case "클러치 백": GameManager.Instance.UE_Goldplus.AddListener(bag);  break;

            case "벽돌": GameManager.Instance.UE_attack.AddListener(brick); break;

            case "의식용 방울":GameManager.Instance.UE_attack.AddListener(Bell);break;
        }
    }

    public GameObject foundmob()
    {
        GameObject dis_Mob = null;

        if (GameManager.Instance.mobs.Count > 0)
        {
            dis_Mob = GameManager.Instance.mobs[0];
        }


        foreach (GameObject mobs in GameManager.Instance.mobs)
        {
            if (Vector3.Distance(player.transform.position, mobs.transform.position) < Vector3.Distance(player.transform.position, dis_Mob.transform.position))
            {
                dis_Mob = mobs;
            }

        }
        return dis_Mob;
    }
    public GameObject foundmob_long()
    {
        GameObject dis_Mob = null;

        if (GameManager.Instance.mobs.Count > 0)
        {
            dis_Mob = GameManager.Instance.mobs[0];
        }


        foreach (GameObject mobs in GameManager.Instance.mobs)
        {
            if (Vector3.Distance(player.transform.position, mobs.transform.position) > Vector3.Distance(player.transform.position, dis_Mob.transform.position))
            {
                dis_Mob = mobs;
            }

        }
        return dis_Mob;
    }
    public void supply_box()
    {
        GameManager.Instance.GoldPlus(100);
    }
    public void dishWashMachine()
    {
        dishWashMachineCount++;
        if (dishWashMachineCount >= 10)
            dishWashMachineCount = 10;
    }
    public void fiveLuck()
    {
        GameManager.Instance.GoldPlus(20);
    }
    public void FatigueReliever()
    {
        player.hp += player.Playerstate.hp * 0.01f;
    }
    float earphonePlus;
    public void earphone()
    {
        earphoneCount++;
        if(earphoneCount >= 5)
        {
            player.Playerstate.Attack_power += 5;
            earphonePlus = player.Playerstate.rush_power * 0.5f;
            player.Playerstate.rush_power  += earphonePlus;


         
        }
    }
    public void earphone_End()
    {
        if (earphoneCount >= 5)
        {
            player.Playerstate.Attack_power -= 5;
            player.Playerstate.rush_power -= earphonePlus;
            earphoneCount = 0;
        }
    }
    public void biscuit()
    {
        if(player.hp< (player.Playerstate.hp * 0.2f))
        {
            player.hp = player.Playerstate.hp;
            item items = null;
            foreach(item item in Haveitem)
            {
                if(item.name == "밤양갱")
                items = item;

            }



            Haveitem.Remove(items);
        }
    }
    int CandyCount;
    public void Candy()
    {
        player.hp += 0.2f * player.Playerstate.hp;
        CandyCount++;
        item items = null;
        if (CandyCount >= 5)
        {
            foreach (item item in Haveitem)
            {
                if (item.name == "사탕")
                    items = item;

            }



            Haveitem.Remove(items);
        }

    }
    public void beastLeg()
    {

        foreach (item item in Haveitem)
        {
            if (item.name == "짐승 다리")
            {
                item.xstate.rush_power -= 0.01f;
                if (item.xstate.rush_power < 1)
                    item.xstate.rush_power = 1f;
            }
        }
        statereset();
    }
    public float newwheelcount=0;
    public void NewWheel()
    {
        newwheelcount++;
        if (newwheelcount >= 10) {

            foreach (item item in Haveitem)
            {
                if (item.name == "새 휠")
                {
                    
                    item.xstate.rush_power += 0.05f;
                    if (item.xstate.rush_power >= 1.5f)
                        item.xstate.rush_power = 1.5f;
            
                }
            }

            newwheelcount = 0;
        }
             statereset();
    }
    float indoCount = 0;
    public void indo()
    {
        indoCount++;
        if(indoCount >= 100) indoCount=100;
        statereset();
    }
    public void benu(GameObject Boss)
    {
        Boss.GetComponent<monster_one>().hp *= 0.5f;
        foreach (item item in Haveitem)
        {
            if (item.name == "예리한 비녀")
            {
                 Haveitem.Remove(item);
                break;
            }
        }
    }
    public void wantedList()
    {
        if(GameManager.Instance.mobs.Count == 0) {

            GameManager.Instance.GoldPlus(150);


        }
    }
    public void jure()
    {
        foreach (item item in Haveitem)
        {
            if (item.name == "주리틀기")
            {

                item.xstate.Attack_power+= 0.1f;
                if (item.xstate.Attack_power >= 3f)
                    item.xstate.Attack_power = 3f;

            }
        }
        statereset();
    }
    public void resurrection()
    {
        player.hp = player.Playerstate.hp;
        foreach (item item in Haveitem)
        {
            if (item.name == "회생의 부적")
            {

                Haveitem.Remove(item);

            }
        }
    }
    public void fan()
    {
        GameManager.Instance.GoldPlus(10);
    }
    public GameObject Tire_pre;
    public void Tire()
    {
        GameObject tire =Instantiate(Tire_pre, player.transform.position+new Vector3( 0,2,0),player.transform.rotation);
        tire.GetComponent<Rigidbody>().AddForce(tire.transform.forward * player.Playerstate.rush_power*2f);
        tire.GetComponent<attack_Enemy>().damage = player.Playerstate.Attack_power*0.3f;

    }
    public void SideMirror(GameObject hitenemy)
    {
        float rans = UnityEngine.Random.value;
        if (rans< 0.1f)
        {
            hitenemy.GetComponent<monster_one>().confusion = true;
            hitenemy.GetComponent<monster_one>().contime = 0;
            GameManager.Instance.ColorChage(hitenemy, new Color(142f/255f,0,147f/255f,1));
        }

    }
    public void brokenHelmet(GameObject hitenemy)
    {
        hitenemy.GetComponent<Rigidbody>().AddForce((hitenemy.transform.position - player.transform.position).normalized *8000);

    }
    public GameObject Volt_pre;
    public void Volt(GameObject hitenemy)
    {
        GameObject volt = Instantiate(Volt_pre, player.transform.position + new Vector3(0, 2, 0), Quaternion.Euler(new Vector3(UnityEngine.Random.Range(-180, 180), 0, UnityEngine.Random.Range(-180, 180))));
        volt.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(-180,180),0, UnityEngine.Random.Range(-180, 180)).normalized * player.Playerstate.rush_power);
        volt.GetComponent<attack_Enemy>().damage = player.Playerstate.Attack_power * 0.2f;
    }
    public GameObject Glove_pre;
    public void Glove()
    {
        GameObject glove = Instantiate(Glove_pre, player.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        glove.transform.GetChild(0).GetComponent<attack_Enemy_tri>().damage = player.Playerstate.Attack_power * 0.3f;
        GameObject dis_Mob = foundmob();
        glove.transform.LookAt(new Vector3(dis_Mob.transform.position.x,0, dis_Mob.transform.position.z));
    }
    public GameObject spring_pre;
    public void hevenspring(GameObject hitenemy)
    {
      GameObject boom =  Instantiate(spring_pre, player.transform.position + new Vector3(0,2,0), Quaternion.identity);
        boom.GetComponent<Boom>().damage = player.Playerstate.Attack_power * 0.3f;
    }
    public void blood(GameObject hitenemy)
    {
        float rans = UnityEngine.Random.value;
        if (rans < 0.1f)
        {
            hitenemy.GetComponent<monster_one>().blood = true;
            hitenemy.GetComponent<monster_one>().bloodtime = 0;
            GameManager.Instance.ColorChage(hitenemy, Color.red);
        }
    }
    public GameObject smallangle, smalldevil;
    public GameObject Oil_pre;
    public void Oil()
    {
        GameObject oil = Instantiate(Oil_pre, player.transform.position + new Vector3(0, 2, 0),Quaternion.identity);
        oil.GetComponent<attack_Enemy_tri>().damage = player.Playerstate.Attack_power * 0.1f;
    }
    public GameObject Ret, Rabbit, Tiger, Snake, Rooster, Pig, Ox, Monkey, Horse, Goat, Dragon, Dog;
    public GameObject Knife_pre,headLight;
    public void Knife()
    {
        GameObject knife = Instantiate(Knife_pre, player.transform.position + new Vector3(0, 1, 0), player.transform.rotation);
        knife.transform.parent = player.transform;
    }
    public void bag()
    {
        foreach (item item in Haveitem)
        {
            if (item.name == "클러치 백")
            {

                item.State.Attack_power = GameManager.Instance.Gold * 0.05f;
  

            }
        }


        statereset();
    }
    public GameObject brick_pre,Bell_pre;
    public void brick()
    {
        GameObject brick_ = Instantiate(brick_pre, player.transform.position + new Vector3(0, 2, 0), Quaternion.Euler(new Vector3(UnityEngine.Random.Range(-180, 180), 0, UnityEngine.Random.Range(-180, 180))));
        brick_.GetComponent<Rigidbody>().AddForce((foundmob().transform.position - player.transform.position).normalized * player.Playerstate.rush_power);
        brick_.GetComponent<attack_Enemy>().damage = player.Playerstate.Attack_power * 2f;
       
    }
    public void Bell()
    {
        GameObject bell = Instantiate(Bell_pre, player.transform.position + new Vector3(0, 1, 0), player.transform.rotation);
        bell.transform.parent = player.transform;
    }
    public GameObject icedoll;
}
