namespace Opcode
{
    public class AuthOpcodeHandler : OpcodeHandler
    {
        public override void RegisterHandler()
        {
            this.handlers.Add((int)Opcode.SMSG_AUTH.LOGON_CHALLENGE_SUCCEED, new Authentication.LogonChallengeHandler());
        }
    }
}
