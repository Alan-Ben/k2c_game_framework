using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 晚间副本初始化
    /// </summary>
    public class NPGSSubDealer_002_052_RetEveningDungeonInit : NPSubDealer<GS2GC.p002_InitOp.GS2GC_002_052_RetEveningDungeonInit>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p002_InitOp.GS2GC_002_052_RetEveningDungeonInit _createProtocolObj()
        {
            return new GS2GC.p002_InitOp.GS2GC_002_052_RetEveningDungeonInit();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p002_InitOp.GS2GC_002_052_RetEveningDungeonInit _msg)
        {
            if (null == _msg)
                return;

            NPPlayer.instance.eveningDungeonComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.eveningDungeonComp.retEveningDungeonInit(_msg);
            }); 
        }
    }
}
