using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iceboom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mob"))
        {
            other.GetComponent<monster_one>().Ice = true;
            other.GetComponent<monster_one>().icetime = 0;


        }
    }


}
