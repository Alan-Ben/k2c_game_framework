using ALBasicProtocolPack;
using GS2GC.p012_ActivityTeamOp;

namespace GOE
{
    /// <summary>
    /// 活动队伍成员退出
    /// </summary>
    public class GSSubDealer_012_053_OnActivityTeamMemberRemove : NPSubDealer<GS2GC_012_053_OnActivityTeamMemberRemove>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_012_053_OnActivityTeamMemberRemove _createProtocolObj()
        {
            return new GS2GC_012_053_OnActivityTeamMemberRemove();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_012_053_OnActivityTeamMemberRemove _msg)
        {
            NPPlayer.instance.commonActivityComp.onActivityTeamMemberRemove(_msg);
        }
    }
}