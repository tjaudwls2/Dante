using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_Manager : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    { player = GameObject.Find("Player");
        this.transform.LookAt(player.transform.position);       
    }
    private void Update()
    {
        this.transform.position = player.transform.position + new Vector3(0,18,-8.5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {

            if (other.transform.GetComponent<MeshRenderer>() != null)
            {
            //    other.GetComponent<MeshRenderer>().material.SetFloat("_Surface", 1f);
                DOTween.Kill(other.GetComponent<MeshRenderer>().material);
                other.GetComponent<MeshRenderer>().material.DOColor(new Color(1, 1, 1, 0f), 0.4f); //SetColor("_Color", new Color(1, 1, 1, 30f / 255f));
        
            }
            for (int i = 0;i<other.transform.childCount;i++)
            {
               if(other.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                {
                  //  other.transform.GetChild(i).GetComponent<MeshRenderer>().material.SetFloat("_Surface", 1f);
                    DOTween.Kill(other.transform.GetChild(i).GetComponent<MeshRenderer>().material);
                    other.transform.GetChild(i).GetComponent<MeshRenderer>().material.DOColor(new Color(1, 1, 1, 0f), 0.4f);//SetColor("_Color", new Color(1, 1, 1, 30f / 255f));
             
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            if (other.transform.GetComponent<MeshRenderer>() != null)
            {
               // other.GetComponent<MeshRenderer>().material.SetFloat("_Surface", 0f);
                DOTween.Kill(other.GetComponent<MeshRenderer>().material);
                other.GetComponent<MeshRenderer>().material.DOColor(new Color(1, 1, 1, 1), 1f);//SetColor("_Color", Color.white);
          
            }
            for (int i = 0; i < other.transform.childCount; i++)
            {
                if (other.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                {
                   // other.transform.GetChild(i).GetComponent<MeshRenderer>().material.SetFloat("_Surface", 0f);
                    DOTween.Kill(other.transform.GetChild(i).GetComponent<MeshRenderer>().material);
                    other.transform.GetChild(i).GetComponent<MeshRenderer>().material.DOColor(new Color(1, 1, 1, 1), 1f);//SetColor("_Color", Color.white);
                 
                }
            }
        }
    }
}
