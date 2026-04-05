using UnityEngine;
using ALPackage;
using ALBasicProtocolPack;
using NPLS2GC.p001_BasicOp;

namespace GOE
{
    public class NPLSSubDealer_001_050_OnNotifyNeedCheckSN : NPSubDealer<NPLS2GC_001_050_OnNotifyNeedCheckSN>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPLS2GC_001_050_OnNotifyNeedCheckSN _createProtocolObj()
        {
            return new NPLS2GC_001_050_OnNotifyNeedCheckSN();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPLS2GC_001_050_OnNotifyNeedCheckSN _msg)
        {
            //进入验证码输入节点
            QueueMgr.instance.addNode_Login_MainUIAddWnd(NPPGUIWndActivationCodeVerification.instance, UINodeTagConst.C_Login_SN);
        }
    }
}
