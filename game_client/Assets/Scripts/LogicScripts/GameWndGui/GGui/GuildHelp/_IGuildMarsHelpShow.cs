namespace GOE
{
    public interface _IGuildMarsHelpShow
    {
        public bool isMyHelp { get; }
        public long senderCid{ get; }// 发起玩家CID
        public int dealLimit{ get; }// 求助允许处理的次数上限
        public int dealedCount{ get; }// 被帮助的次数
        public string getDetailStr();
        public string getReduceTimeStr();
    }
}