using UnityEngine;
using ALPackage;
using ALBasicProtocolPack;
using NPLS2GC.p001_BasicOp;

namespace GOE
{
    public class NPLSSubDealer_001_003_CancelQueueRes : NPSubDealer<NPLS2GC_001_003_CancelQueueRes>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPLS2GC_001_003_CancelQueueRes _createProtocolObj()
        {
            return new NPLS2GC_001_003_CancelQueueRes();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPLS2GC_001_003_CancelQueueRes _msg)
        {
        }
    }
}
