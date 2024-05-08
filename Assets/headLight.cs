using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class headLight : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Mob"))
        {
            other.GetComponent<monster_one>().slow = true;
            other.GetComponent<monster_one>().slowtime = 0;

        }
    }

    
}
