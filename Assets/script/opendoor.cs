using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opendoor : MonoBehaviour
{
    Collider Collider;
    Animator anim;
    Player player;
    private void Start()
    {
        Collider = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        player = GameManager.Instance.player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.RCC)
        {
            case GameManager.room_clear_check.clear:
                if (player.rush_Check != rush_check.rush)
                {
                    Collider.isTrigger = true;
                    anim.SetBool("Open", true);
                }
                break;
            case  GameManager.room_clear_check.battle:
                Collider.isTrigger = false;
                anim.SetBool("Open", false);

                break;
        }    
    }
}
