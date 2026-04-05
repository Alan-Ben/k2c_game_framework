using UnityEngine;
using ALPackage;
using ALBasicProtocolPack;
using NPLS2GC.p001_BasicOp;

namespace GOE
{
    public class NPLSSubDealer_001_004_EnterQueue : NPSubDealer<NPLS2GC_001_004_EnterQueue>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPLS2GC_001_004_EnterQueue _createProtocolObj()
        {
            return new NPLS2GC_001_004_EnterQueue();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPLS2GC_001_004_EnterQueue _msg)
        {

        }
    }
}
