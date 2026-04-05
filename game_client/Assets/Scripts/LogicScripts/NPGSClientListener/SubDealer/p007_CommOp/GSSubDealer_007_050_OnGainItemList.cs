
using System.Collections.Generic;
using ALBasicProtocolPack;
using NPCommon;
using NPEnum;

namespace GOE
{
    public class GSSubDealer_007_050_OnGainItemList : NPSubDealer<GS2GC.p007_CommOp.GS2GC_007_050_OnGainItemList>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p007_CommOp.GS2GC_007_050_OnGainItemList _createProtocolObj()
        {
            return new GS2GC.p007_CommOp.GS2GC_007_050_OnGainItemList();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p007_CommOp.GS2GC_007_050_OnGainItemList _msg)
        {
            if(_msg == null)
                return;

            //处理宠物展示
            // GCommon.dealGainPet(_msg.getGainPetList());

            //走统一函数处理
            switch (_msg.getRewardShowType())
            {
                case ENpRewardShowType.TIP:
                    GCommon.showGainRewardTip(_msg.getItemList(), true);
                    break;
                case ENpRewardShowType.DEFAULT:
                    GCommon.dealGainItem(_msg.getItemList());
                    break;
                case ENpRewardShowType.NOT_DISPLAY:
                    //不表现
                    break;
                default:
                    GCommon.dealGainItem(_msg.getItemList());
                    break;
            }
        }
    }
}
