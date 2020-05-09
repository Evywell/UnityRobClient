using System;

namespace Opcode
{
    public interface Handler
    {
        void Run(int opcode, object payload);
        Type getPayloadTemplate();
    }
}
