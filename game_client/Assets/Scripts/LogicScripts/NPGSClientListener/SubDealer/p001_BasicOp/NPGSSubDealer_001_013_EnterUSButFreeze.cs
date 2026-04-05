using ALBasicProtocolPack;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    //服务器冻结推送
    public class NPGSSubDealer_001_013_EnterUSButFreeze : NPSubDealer<NPGS2GC.p001_BasicOp.NPGS2GC_001_013_EnterUSButFreeze>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPGS2GC.p001_BasicOp.NPGS2GC_001_013_EnterUSButFreeze _createProtocolObj()
        {
            return new NPGS2GC.p001_BasicOp.NPGS2GC_001_013_EnterUSButFreeze();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPGS2GC.p001_BasicOp.NPGS2GC_001_013_EnterUSButFreeze _msg)
        {
            //调用流程设置请求进入US结果
            GameInit_SelectServer.instance.enterUSButFreeze(_msg);        }
    }
}

