using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thisitem : MonoBehaviour
{
    public item _item;
    #region Å×½ºÆ®
    public string thisname;
    public void Start()
    {
        foreach (item item in ItemManager.Iteminstance.itemList)
        {
            if(item.name == thisname)
                _item = item;
        }

    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ItemManager.Iteminstance.Get_Item(_item);
            Destroy(gameObject);

        }

    }

}
