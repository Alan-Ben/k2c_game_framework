using ALBasicProtocolPack;
using CommonEnum;
using GOE;

namespace Hotfix
{
    public partial class HotfixMain
    {
        //是否初始化过
        private static bool _m_hasInitActivity = false;

        //初始化注册
        private static void _initPatchActivity()
        {
            //注册协议处理
            if (_m_hasInitActivity)
            {
                return;//只注册一次
            }
            _m_hasInitActivity = true;
            
            //这边热更的活动adapter没办法支持有参构造函数
            CommonActivityFactory.instance.regCreator(ECommonActivityType.REGULAR_EVENT, (activityServerInfo) =>
            {
                HotfixDefaultActivityInfo activityInfo = new HotfixDefaultActivityInfo();
                activityInfo.init(activityServerInfo);
                return activityInfo;
            });
            CommonActivityFactory.instance.regCreator(ECommonActivityType.TILE_MATCH, (activityServerInfo) =>
            {
                HotfixDefaultActivityInfo activityInfo = new HotfixDefaultActivityInfo();
                activityInfo.init(activityServerInfo);
                return activityInfo;
            });
            CommonActivityFactory.instance.regCreator(ECommonActivityType.NUM_MERGE, (activityServerInfo) =>
            {
                HotfixDefaultActivityInfo activityInfo = new HotfixDefaultActivityInfo();
                activityInfo.init(activityServerInfo);
                return activityInfo;
            });
        }
    }
}