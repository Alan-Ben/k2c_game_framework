using ALBasicProtocolPack;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    //服务端推送公告
    public class NPGSSubDealer_001_005_EnterUSRes : NPSubDealer<NPGS2GC.p001_BasicOp.NPGS2GC_001_005_EnterUSRes>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPGS2GC.p001_BasicOp.NPGS2GC_001_005_EnterUSRes _createProtocolObj()
        {
            return new NPGS2GC.p001_BasicOp.NPGS2GC_001_005_EnterUSRes();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPGS2GC.p001_BasicOp.NPGS2GC_001_005_EnterUSRes _msg)
        {
            if (_msg.getError() != 0)
            {
                NPGUIAddSceneCenterTip.instance.showErrorInfo(_msg.getError());
                return;
            }

            //调用流程设置请求进入US结果
            GameInit_SelectServer.instance.retEnterUSRes(_msg);
        }
    }
}

