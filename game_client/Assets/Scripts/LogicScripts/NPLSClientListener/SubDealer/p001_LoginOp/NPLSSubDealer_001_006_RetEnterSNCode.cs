using UnityEngine;
using ALPackage;
using ALBasicProtocolPack;
using NPLS2GC.p001_BasicOp;

namespace GOE
{
    public class NPLSSubDealer_001_006_RetEnterSNCode : NPSubDealer<NPLS2GC_001_006_RetEnterSNCode>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPLS2GC_001_006_RetEnterSNCode _createProtocolObj()
        {
            return new NPLS2GC_001_006_RetEnterSNCode();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPLS2GC_001_006_RetEnterSNCode _msg)
        {
            if(_msg.getErrCode() != 0)
            {
                //激活码错误表现
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.active_code_error, _msg.getErrCode()), TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);

                NPPGUIWndActivationCodeVerification.instance.showCodeError();
                return;
            }

            //退出验证码节点
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Login_SN);
        }
    }
}
