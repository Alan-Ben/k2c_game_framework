package NPGateServer.NPGeneralListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NP2GS_B.p001_BasicOp.NP2GS_B_001_005_USVersionChg;
import NP2GS_B.p001_BasicOp.NP2GS_B_001_006_USIndexListInit;
import NP2GS_B.p001_BasicOp.NP2GS_B_001_007_USIndexListChg;
import NPCommon.Dispather.NPCustomMsgDispatcher;
import NPGateServer.USRefVersionMgr.USVersionInfo;
import NPGateServer.USServerInfoListMgr.USServerIndexInfoListMgr;

/**************
 * 客户端协议处理对象
 *
 * @author alzq.z
 * @email zhuangfan@vip.163.com
 * @time 2020年12月9日 下午10:52:56
 */
public class GSBroadMsgDispather extends NPCustomMsgDispatcher
{
    private static GSBroadMsgDispather _g_instance = new GSBroadMsgDispather();

    public static GSBroadMsgDispather getInstance()
    {
        return _g_instance;
    }

    protected GSBroadMsgDispather()
    {
        this.regHandler(new NPCustomMsgDealer<NP2GS_B_001_005_USVersionChg>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2GS_B_001_005_USVersionChg _msg)
            {
                //转化数据对象
                USVersionInfo info = (USVersionInfo) _receiver;
                if (null == info)
                {
                    return;
                }

                //设置数据
                info.updateServerInfo(_msg.getServerVersion(), _msg.getResVersion());
            }
        });
        this.regHandler(new NPCustomMsgDealer<NP2GS_B_001_006_USIndexListInit>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2GS_B_001_006_USIndexListInit _msg)
            {
                //根据CS发来的US服务器信息列表初始化本地US信息列表记录
                USServerIndexInfoListMgr.getInstance().initLoadServerList(_msg.getServerIndexList());
            }
        });
        this.regHandler(new NPCustomMsgDealer<NP2GS_B_001_007_USIndexListChg>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2GS_B_001_007_USIndexListChg _msg)
            {
                //根据CS发来的US服务器信息列表更新本地US信息列表记录
                USServerIndexInfoListMgr.getInstance().updateServerItem(_msg.getServerIndexList());
            }
        });
    }
}
