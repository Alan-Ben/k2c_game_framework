using ALBasicProtocolPack;
using System.Collections.Generic;
using UnityEngine;

using ALPackage;

namespace GOE
{
    //服务端推送公告
    public class NPGSSubDealer_001_001_RetBasicInfo : NPSubDealer<NPGS2GC.p001_BasicOp.NPGS2GC_001_001_RetBasicInfo>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPGS2GC.p001_BasicOp.NPGS2GC_001_001_RetBasicInfo _createProtocolObj()
        {
            return new NPGS2GC.p001_BasicOp.NPGS2GC_001_001_RetBasicInfo();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPGS2GC.p001_BasicOp.NPGS2GC_001_001_RetBasicInfo _msg)
        {
            //设置服务器版本信息
            GameResCore.instance.setServerVersion(_msg.getVersion());

#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning($"Server Version: {_msg.getResVersion()}====RefVersion:{RefdataResCore.instance.remoteVersionNum}");
#endif

            Game.instance.ServerTimeZone = _msg.getTimezone();
            GameSetting.instance.setServerTimeZone(_msg.getTimezone());
            Game.instance.DstOffset = _msg.getDstOffset();
            GameSetting.instance.setDstOffset(_msg.getDstOffset());
        }
    }
}

