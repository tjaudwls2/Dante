using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum room_clear_check
    {
        clear,
        battle


    }


    #region ΩÃ±€≈Ê
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


    public GameObject player;
    public room_clear_check RCC;
    private void Start()
    {
        player = GameObject.Find("Player");

    }


   
    



}
