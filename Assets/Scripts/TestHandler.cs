using System;
using UnityEngine;

public class TestHandler : Opcode.Handler
{
    public Type getPayloadTemplate()
    {
        return typeof(LoginPayload);
    }

    public void Run(int opcode, object payload)
    {
        Debug.Log(((LoginPayload) payload).login);
    }
}
