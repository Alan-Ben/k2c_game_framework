using System;
using NPEnum;

namespace GOE
{
    public struct RoomJoinInfo : IEquatable<RoomJoinInfo>
    {
        public ENPChatRoomType roomType;
        public long extId;

        public RoomJoinInfo(ENPChatRoomType _roomType, long _extId)
        {
            this.roomType = _roomType;
            this.extId = _extId;
        }

        public bool Equals(RoomJoinInfo other)
        {
            return roomType == other.roomType && extId == other.extId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) roomType, extId);
        }
    }
}