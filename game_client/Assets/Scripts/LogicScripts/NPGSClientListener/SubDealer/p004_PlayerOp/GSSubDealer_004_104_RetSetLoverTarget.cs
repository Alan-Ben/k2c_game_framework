using ALBasicProtocolPack;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 情人收集-选择目标情人返回
    /// </summary>
    public class GSSubDealer_004_104_RetSetLoverTarget : NPSubDealer<GS2GC_004_104_RetSetLoverTarget>
    {
        protected override GS2GC_004_104_RetSetLoverTarget _createProtocolObj()
        {
            return new GS2GC_004_104_RetSetLoverTarget();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_004_104_RetSetLoverTarget _msg)
        {
        }
    }
}
