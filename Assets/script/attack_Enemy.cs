using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_Enemy : MonoBehaviour
{
    public float damage;
    public int Count_Max;
    public bool notdes;
    int count;

    private void Start()
    {
        if (!notdes)
            Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {

            GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity.normalized *30);
        }
        if (collision.gameObject.CompareTag("Mob"))
        {
            collision.gameObject.GetComponent<monster_one>().Damage_calculate(damage,false);
           GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity.normalized *30);
        
            count++;
            if (count >= Count_Max)
            {
                if(!notdes)
                Destroy(gameObject);
            }
        }
    }
}
