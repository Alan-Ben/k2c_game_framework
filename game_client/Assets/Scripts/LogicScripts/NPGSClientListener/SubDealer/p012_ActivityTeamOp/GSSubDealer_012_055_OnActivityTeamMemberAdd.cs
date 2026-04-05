using ALBasicProtocolPack;
using GS2GC.p012_ActivityTeamOp;

namespace GOE
{
    /// <summary>
    /// 活动队伍成员增加
    /// </summary>
    public class GSSubDealer_012_055_OnActivityTeamMemberAdd : NPSubDealer<GS2GC_012_055_OnActivityTeamMemberAdd>
    {
		/// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_012_055_OnActivityTeamMemberAdd _createProtocolObj()
        {
            return new GS2GC_012_055_OnActivityTeamMemberAdd();
        }
		
        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_012_055_OnActivityTeamMemberAdd _msg)
        {
			NPPlayer.instance.commonActivityComp.onActivityTeamMemberAdd(_msg);
        }
    }
}