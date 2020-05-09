using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class ClientManager : MonoBehaviour
    {
        public static ClientManager instance = null;

        public Client authClient;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                Debug.Log("Initialisation du ClientManager");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}