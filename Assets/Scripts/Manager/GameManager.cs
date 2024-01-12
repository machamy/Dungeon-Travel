using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Manager;
using Scripts.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class GameManager
{
    public const string NAME = "@Game";
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
                instance.init();
            }

            return instance;
        }
    }
    public void init()
    {
        GameObject root = GameObject.Find(NAME);
        if (root == null)
        {
            root = new GameObject { name = NAME };
            Object.DontDestroyOnLoad(root);

            PartyManager = new PartyManager();
        }
    }

    public PartyManager PartyManager;
    
    
    
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    
}
