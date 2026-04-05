using System;

namespace GOE
{
    /// <summary>
    /// 公告抽象类
    /// </summary>
    public abstract class _ANoticeInfoBase
    {
        //公告内容
        private BaseNoticeInfo _m_info;
        public BaseNoticeInfo noticeInfo
        {
            get { return _m_info; }
        }

        //是否有效
        public abstract bool inValid { get; }

        public _ANoticeInfoBase(BaseNoticeInfo _info)
        {
            _m_info = _info;

        }
    }
}