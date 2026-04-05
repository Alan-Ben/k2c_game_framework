using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 公告信息基类
    /// </summary>
    [Serializable]
    public class BaseNoticeInfo
    {
        public string start_time;//开始时间
        public string end_time;//结束时间
        public int is_auto_open;//自动弹出  0 不弹出，
        public List<GameNoticeContent> content_list;//内容列表
    }
    
    /// <summary>
    /// 游戏公告内容
    /// </summary>
    [Serializable]
    public class GameNoticeContent
    {
        public string language;//语言
        public string title;//标题
        public string content;//内容

        public override string ToString()
        {
            return $"[{nameof(language)}: {language}], [{nameof(title)}: {title}], [{nameof(content)}: {content}]";
        }
    }
}