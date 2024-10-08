using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>You must approach through `GoogleSheetManager.SO<GoogleSheetSO>()`</summary>
public class GoogleSheetSO : ScriptableObject
{
	public List<Sheet1> Sheet1List;
}

[Serializable]
public class Sheet1
{
	public string name;
	public string description;
	public int id;
	public float dmg;
	public int[] arr_test;
}

