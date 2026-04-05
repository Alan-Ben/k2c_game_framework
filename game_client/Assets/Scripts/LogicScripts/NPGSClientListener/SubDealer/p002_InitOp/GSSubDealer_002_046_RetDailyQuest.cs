using ALBasicProtocolPack;
using GS2GC.p002_InitOp;

namespace GOE
{
    //日常周常任务组件初始化
    public class GSSubDealer_002_046_RetDailyQuest : NPSubDealer<GS2GC_002_046_RetDailyQuest>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC_002_046_RetDailyQuest _createProtocolObj()
        {
            return new GS2GC_002_046_RetDailyQuest();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC_002_046_RetDailyQuest _msg)
        {
            if (null == _msg)
                return;

            NPPlayer.instance.dailyQuestComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.dailyQuestComp.retDailyQuestListData(_msg.getDailyquestInfoList());
            });
        }
    }
}
