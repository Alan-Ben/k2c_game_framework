
using ALBasicProtocolPack;
using GS2GC.p002_InitOp;


namespace GOE
{
    public class GSSubDealer_002_050_RetAchieveInit : NPSubDealer<GS2GC_002_050_RetAchieveInit>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_050_RetAchieveInit _createProtocolObj()
        {
            return new GS2GC_002_050_RetAchieveInit();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_050_RetAchieveInit _msg)
        {
            if (null == _msg)
                return;

            NPPlayer.instance.achieveComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.achieveComp.retAchieveInit(_msg);
            });
        }
    }
}
