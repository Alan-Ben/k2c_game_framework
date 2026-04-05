using UnityEngine;
using System.Collections;
using ALBasicProtocolPack;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 被踢下线协议推送，服务端延迟10秒掉线。 需要客户端主动触发掉线
    /// </summary>
    public class NPGSSubDealer_001_030_BeDeviceKicked : _AALBasicProtocolSubBasicOrderDealer<NPGS2GC.p001_BasicOp.NPGS2GC_001_030_BeDeviceKicked>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPGS2GC.p001_BasicOp.NPGS2GC_001_030_BeDeviceKicked _createProtocolObj()
        {
            return new NPGS2GC.p001_BasicOp.NPGS2GC_001_030_BeDeviceKicked();
        }

        protected override void _dealProtocol(_IALProtocolDealer _dealer, NPGS2GC.p001_BasicOp.NPGS2GC_001_030_BeDeviceKicked _msg)
        {
            //需要退出相关连接对象
            LSMgr.instance.resetLoginState();
            if(NPGSClientListener.instance != null)
                NPGSClientListener.instance.logout();
            
            NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.be_device_kick), TextTranslate.instance.getLanguage(TransKeyConst.ok)
                , ()=> {
                    //重登处理
                    Game.instance.relogin();
                });
        }
    }
}
