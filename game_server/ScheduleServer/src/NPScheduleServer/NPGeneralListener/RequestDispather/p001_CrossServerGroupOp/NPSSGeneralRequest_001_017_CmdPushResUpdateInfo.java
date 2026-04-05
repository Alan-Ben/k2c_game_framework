package NPScheduleServer.NPGeneralListener.RequestDispather.p001_CrossServerGroupOp;

import NP2SS_R.p001_ScheduleOp.ToSS_R_001_017_CmdPushResUpdateInfo;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.Result.Result;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleDataMgr;
import NPScheduleServer.NPGeneralListener.RequestDispather.Writer.NP2SS_RB_Writer_001_CrossServerGroupOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 推送资源更新信息请求处理器
 *
 * 功能：
 * 1. 接收HttpServer转发的后台资源更新配置
 * 2. 根据phpScheduleId查找对应的已激活排期
 * 3. 验证版本号和分组列表匹配
 * 4. 更新排期的资源信息（resFile、resMd5、resDir）
 *
 * 协议流向：Backend HTTP → HttpServer(007-004) → ScheduleServer(001-017)
 */
public class NPSSGeneralRequest_001_017_CmdPushResUpdateInfo extends NPRequestDealer<ToSS_R_001_017_CmdPushResUpdateInfo>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, ToSS_R_001_017_CmdPushResUpdateInfo _msg)
    {
        Result result = ActivityScheduleDataMgr.getInstance().updateScheduleResInfo(_msg.getResUpdateInfo());

        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(
            NP2SS_RB_Writer_001_CrossServerGroupOp.make_017_CmdPushResUpdateInfo());
    }
}
