using ALBasicProtocolPack;
using GS2GC.p012_ActivityTeamOp;

namespace GOE
{
    /// <summary>
    /// 退出活动队伍
    /// </summary>
    public class GSSubDealer_012_052_OnQuitActivityTeam : NPSubDealer<GS2GC_012_052_OnQuitActivityTeam>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_012_052_OnQuitActivityTeam _createProtocolObj()
        {
            return new GS2GC_012_052_OnQuitActivityTeam();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_012_052_OnQuitActivityTeam _msg)
        {
            NPPlayer.instance.commonActivityComp.onQuitActivityTeam(_msg);
        }
    }
}