using System;

namespace GOE
{
    /// <summary>
    /// 游戏前公告信息
    /// </summary>
    [Serializable]
    public class GameBeforeNoticeInfo : BaseNoticeInfo
    {
        public int is_show_wnd_before;
        public string mono_asset_path_before;//窗口路径
        public string mono_obj_name_before;//窗口名称

        public override string ToString()
        {
            return $"[{nameof(is_show_wnd_before)}: {is_show_wnd_before}], [{nameof(mono_asset_path_before)}: {mono_asset_path_before}], [{nameof(mono_obj_name_before)}: {mono_obj_name_before}]";
        }


        //当前是否有效
        public bool inValid()
        {
            //由于在弹出该游戏前公告时未获取到服务器时间，先按照缓存的服务器时区和夏令时时差来计算服务器时间，可能由于玩家修改设备时间而不准确
            DateTime serverTime = DateTime.UtcNow.AddHours(GameSetting.instance.serverTimeZone).AddMilliseconds(GameSetting.instance.dstOffset);
            if (serverTime < TimeUtil.parseToLocalTime(start_time, 0) || serverTime >= TimeUtil.parseToLocalTime(end_time, 0))
                return false;

            return true;
        }

        /// <summary>
        /// 获取对应语言的公告
        /// </summary>
        /// <param name="_lan"></param>
        /// <returns></returns>
        public GameNoticeContent getLanguage(ENPLanguage _lan)
        {
            string targetLanguage = _lan.toPHPLanguageCode();
            GameNoticeContent tmpContent = null;
            for (int i = 0; i < content_list.Count; i++)
            {
                tmpContent = content_list[i];
                if (null == tmpContent)
                    continue;

                //判断语言
                if (tmpContent.language == targetLanguage)
                    return tmpContent;
            }

            //如果没有符合的则使用第一个
            if (content_list.Count <= 0)
                return null;

            return content_list[0];
        }
    }
}