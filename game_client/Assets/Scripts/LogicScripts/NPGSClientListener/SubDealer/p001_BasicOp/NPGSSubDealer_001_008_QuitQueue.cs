using ALBasicProtocolPack;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    //服务端推送公告
    public class NPGSSubDealer_001_008_QuitQueue : NPSubDealer<NPGS2GC.p001_BasicOp.NPGS2GC_001_008_QuitQueue>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPGS2GC.p001_BasicOp.NPGS2GC_001_008_QuitQueue _createProtocolObj()
        {
            return new NPGS2GC.p001_BasicOp.NPGS2GC_001_008_QuitQueue();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPGS2GC.p001_BasicOp.NPGS2GC_001_008_QuitQueue _msg)
        {
            if (_msg.getErrCode() != 0)
            {
                NPGUIAddSceneCenterTip.instance.showErrorInfo(_msg.getErrCode());
                return;
            }

            //重置登录数据
            GameInit_SelectServer.instance.resetCurLoginFunc();
        }
    }
}

