using UnityEngine;
using System;
using System.Text;
using ALPackage;
using ALBasicProtocolPack;
using NPLS2GC.p001_BasicOp;

using System.IO;

namespace GOE
{
    public class NPLSSubDealer_001_002_EnterGameRes : NPSubDealer<NPLS2GC_001_002_EnterGameRes>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPLS2GC_001_002_EnterGameRes _createProtocolObj()
        {
            return new NPLS2GC_001_002_EnterGameRes();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPLS2GC_001_002_EnterGameRes _msg)
        {
            if(!_msg.getRes())
            {
                Debug.LogError("Get Gate Server Error!");
                return;
            }

            //调用登录过程处理对象
            GameInit_LoginProcess.instance.onGetLSEnterGameRes(_msg);

#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning("start reqPlayerInfoStepCounter");
#endif
        }
    }
}
