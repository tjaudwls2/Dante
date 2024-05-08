using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;


public class spawnbox : MonoBehaviour
{
    public bool BossSpawn;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().firstRoomin = true;
           gameObject.layer = LayerMask.NameToLayer("Spawnbox_R");
         
            if(!BossSpawn)
            GameManager.Instance.MobSpawn(this.transform.position,GetComponent<BoxCollider>().size.x+1,GetComponent<BoxCollider>().size.z+1);
            else
            {
                GameManager.Instance.BossSpawn(this.transform.position, GetComponent<BoxCollider>().size.x + 1, GetComponent<BoxCollider>().size.z + 1);
            }
            
            Destroy(gameObject);

        }
    }





}
