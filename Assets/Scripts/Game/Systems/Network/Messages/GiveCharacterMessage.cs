using Mirror;

namespace DefaultNamespace.Messages
{
    public struct GiveCharacterMessage : NetworkMessage
    {
        public uint actor;
    }
}