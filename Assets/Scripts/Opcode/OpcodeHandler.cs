using System;
using System.Collections.Generic;
using UnityEngine;

namespace Opcode
{
    public abstract class OpcodeHandler
    {

        protected Dictionary<int, Handler> handlers;

        public OpcodeHandler()
        {
            this.handlers = new Dictionary<int, Handler>();
        }

        public abstract void RegisterHandler();

        public void Handle(int opcode, string payload)
        {
            if (!this.handlers.ContainsKey(opcode))
            {
                return;
            }
            Handler handler;
            this.handlers.TryGetValue(opcode, out handler);
            Type type = handler.getPayloadTemplate();
            handler.Run(opcode, typeof(JsonUtility).GetMethod("FromJson", new[] { typeof(string) }).MakeGenericMethod(type).Invoke(null, new object[] { payload }));
        }

    }
}

