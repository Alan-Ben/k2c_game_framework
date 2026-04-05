
using ALBasicProtocolPack;

namespace GOE
{
    public class GSSubDealer_009_054_OnMailReaded : NPSubDealer<GS2GC.p009_MailOp.GS2GC_009_054_OnMailReaded>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p009_MailOp.GS2GC_009_054_OnMailReaded _createProtocolObj()
        {
            return new GS2GC.p009_MailOp.GS2GC_009_054_OnMailReaded();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p009_MailOp.GS2GC_009_054_OnMailReaded _msg)
        {
            if(_msg == null)
                return;

            NPPlayer.instance.mailComp.onMailReaded(_msg);
        }
    }
}
