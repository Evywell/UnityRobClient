using System;
using Opcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Authentication
{
    class LogonChallengeHandler : Handler
    {
        public Type getPayloadTemplate()
        {
            return typeof(LogonChallengeSucceedPayload);
        }

        public void Run(int opcode, object payload)
        {
            Debug.Log($"J'ai reçu le token {((LogonChallengeSucceedPayload)payload).token}");
        }
    }
}
