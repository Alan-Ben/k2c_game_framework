
namespace GOE
{
    public static partial class GCommon
    {
        /// <summary>
        /// 初始化每日红点提示
        /// </summary>
        public static void initDailyRedTip()
        {
            //网页充值入口每日红点
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_DAILY_WEB_RECHARGE, 
                AccountSettingMgr.instance.dailyTagSaver.isNewDay(DailyTagConst.WEB_RECHARGE_ENTRANCE) ? 1 : 0);
        }
    }
}