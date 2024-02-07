
using UnityEngine;



public class EncaserList:MonoBehaviour
{
   
    public GameObject[] objects = new GameObject[] {};

    //public getter method
    public GameObject[] GetList()
    {
        return objects;
    }

    public void Clear()
    {
        objects= new GameObject[] {};
    }
}
