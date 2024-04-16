using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opendoor : MonoBehaviour
{
    Collider Collider;
    private void Start()
    {
        Collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.RCC)
        {
            case GameManager.room_clear_check.clear: Collider.enabled = false;                      break;
            case GameManager.room_clear_check.battle: Collider.enabled = true; break;
        }    
    }
}
