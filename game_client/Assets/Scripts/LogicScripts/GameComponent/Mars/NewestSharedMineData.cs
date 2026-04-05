using System;

namespace GOE
{
    /// <summary>
    /// 最新的共享矿数据
    /// </summary>
    [Serializable]
    public struct NewestSharedMineData
    {
        // 最新的矿 id
        public long newestId;
        // 矿所属的联盟 id
        public long guildId;
    }
}