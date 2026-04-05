using Common.MarsObj;

namespace GOE
{
    /// <summary>
    /// 用于在探索界面展示矿相关对象的接口类型
    /// </summary>
    public interface _IMarsExploreMineItem
    {
        long instanceId { get; }
        MarsExploreMineRefObj refObj { get; }
        long occupiedCid { get; }
        bool isMe { get; }
        long occupiedTeamId { get; }
        long posId { get; }
        long startTime { get; }
        long remainNum { get; }
        /// <summary>
        /// 尝试检查信息，通过41-13发送矿消息，附带状态序列号可以检查
        /// </summary>
        void checkInfo();
    }
}