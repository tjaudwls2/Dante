using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public GameObject explo;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {

        Invoke("boomex", 2);

    }
    public void boomex()
    {

       GameObject boom = Instantiate(explo,this.transform.position,Quaternion.identity);
        boom.GetComponent<attack_Enemy_tri>().damage = damage;
        Destroy(gameObject);

    }


  
}
