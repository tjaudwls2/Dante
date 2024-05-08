using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class Itemex : ScriptableObject
{
    public List<item_sheet> Sheet1;
}
[System.Serializable]
public class item_sheet
{
    public string name, ex;
    public int itemtype;

}
