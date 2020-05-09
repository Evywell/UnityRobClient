using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Network
{
    public class Packet
    {
        
        public int Opcode { get; set; }
        public object Payload { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder($"[{Opcode},");
            builder.Append(JsonUtility.ToJson(Payload));
            builder.Append("]\n");
            return builder.ToString();
        }

    }
}
