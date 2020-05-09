using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using Network;

public class UIManager : MonoBehaviour
{
    
    public static UIManager instance;

    public GameObject startMenu;
    public InputField username;
    public InputField password;

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        LoginPayload payload = new LoginPayload
        {
            login = username.text,
            password = password.text
        };
        Packet pck = new Packet
        {
            Opcode = (int)Opcode.Opcode.CMSG_AUTH.LOGON_CHALLENGE,
            Payload = payload
        };
        Client authClient = ClientManager.instance.authClient;
        authClient.SetAuthenticationPacket(pck);
        authClient.ConnectToServer();

        /*
        Network.Packet packet = new Network.Packet
        {
            Opcode = 0,
            Payload = payload
        };
        Debug.Log(packet.ToString());
        */
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
