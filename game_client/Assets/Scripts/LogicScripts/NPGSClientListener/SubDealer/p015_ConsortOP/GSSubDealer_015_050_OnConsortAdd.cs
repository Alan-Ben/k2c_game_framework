
using ALBasicProtocolPack;

namespace GOE
{
    public class GSSubDealer_015_050_OnConsortAdd : NPSubDealer<GS2GC.p015_ConsortOp.GS2GC_015_050_OnConsortAdd>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p015_ConsortOp.GS2GC_015_050_OnConsortAdd _createProtocolObj()
        {
            return new GS2GC.p015_ConsortOp.GS2GC_015_050_OnConsortAdd();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p015_ConsortOp.GS2GC_015_050_OnConsortAdd _msg)
        {
            if(_msg == null)
                return;

            NPPlayer.instance.consortComp.onConsortAdd(_msg);
        }
    }
}
