using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    public static UIManager instance;

    public GameObject startMenu;

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        Network.Client.instance.ConnectToServer();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Awake UIManager, destroying the object...");
            Destroy(this);
        }
    }

}
