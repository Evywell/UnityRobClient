namespace Opcode
{
    public class Opcode
    {

        public enum SMSG_AUTH: int
        {
            LOGON_CHALLENGE_SUCCEED = 0x01
        }

        public enum CMSG_AUTH: short
        {
            LOGON_CHALLENGE = 0x00
        }

    }
}

