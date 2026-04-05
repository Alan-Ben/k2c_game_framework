using System;

namespace GOE
{
        
    [Serializable]
    public class ServerDataInfo
    {
        public int serverId;
        public int usTypeId;
        public string serverName;
        public int onlineState;
        public int showState;
        public string groupTag;

        public override string ToString()
        {
            return $"[{nameof(serverId)}: {serverId}], [{nameof(usTypeId)}: {usTypeId}], [{nameof(serverName)}: {serverName}], [{nameof(onlineState)}: {onlineState}], [{nameof(showState)}: {showState}], [{nameof(groupTag)}: {groupTag}]";
        }
    }
}