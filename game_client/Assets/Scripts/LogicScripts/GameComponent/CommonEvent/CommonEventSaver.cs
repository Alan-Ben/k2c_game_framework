using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用事件存储器
    /// </summary>
    public class CommonEventSaver : _AALBasicSettingInfo
    {
        private const char _m_fieldSplit = '|';//不同字段间的分隔符
        private const char _m_enumeratorItemSplit = ';';//可遍历元素的item间的分隔符

        private List<long> _m_lHasShowBeforeEventDialogDBIdList;//已经显示了事件前置剧情的事件dbId列表
        
        public CommonEventSaver(long _accountCid) : base(string.Format("{0}_common_event_saver", _accountCid))
        {
        }

        protected override string _makeSettingStr()
        {
            StringBuilder sb = new StringBuilder();

            // 序列化_m_lHasShowBeforeEventDialogDBIdList
            if (_m_lHasShowBeforeEventDialogDBIdList != null)
            {
                for (int i = 0, count = _m_lHasShowBeforeEventDialogDBIdList.Count; i < count; i++)
                {
                    if (i != 0)
                        sb.Append(_m_enumeratorItemSplit);

                    sb.Append(_m_lHasShowBeforeEventDialogDBIdList[i]);
                }
            }
            sb.Append(_m_fieldSplit);

            
            
            // 后续序列化内容
            
            return sb.ToString();
        }

        protected override void _initSettingStr(string _infoStr)
        {
            if (string.IsNullOrEmpty(_infoStr))
                return;

            string[] fieldStrArr = _infoStr.Split(_m_fieldSplit);
            if(fieldStrArr == null)
                return;

            // 解析出字段数量大于1时, 对_m_lHasShowBeforeEventDialogDBIdList进行反序列化
            if (fieldStrArr.Length >= 1 && !string.IsNullOrEmpty(fieldStrArr[0]))
            {
                if (_m_lHasShowBeforeEventDialogDBIdList == null)
                    _m_lHasShowBeforeEventDialogDBIdList = new List<long>();
                _m_lHasShowBeforeEventDialogDBIdList.Clear();
                
                string[] hasShowBeforeEventDialogDBIdStrArr = fieldStrArr[0].Split(_m_enumeratorItemSplit, StringSplitOptions.RemoveEmptyEntries);
                if (hasShowBeforeEventDialogDBIdStrArr != null)
                {
                    foreach (string idStr in hasShowBeforeEventDialogDBIdStrArr)
                    {
                        if(string.IsNullOrEmpty(idStr))
                            continue;

                        if (long.TryParse(idStr, out long _dbId))
                        {
                            _m_lHasShowBeforeEventDialogDBIdList.Add(_dbId);   
                        }
                    }
                }
            }
            
            
            // 进行后续反序列化
        }

        /// <summary>
        /// 添加已经显示事件前置剧情的事件dbId
        /// </summary>
        /// <param name="_dbId"></param>
        public void addHasShowBeforeEventDialogDBId(long _dbId)
        {
            if (_m_lHasShowBeforeEventDialogDBIdList == null)
                _m_lHasShowBeforeEventDialogDBIdList = new List<long>();
            
            _m_lHasShowBeforeEventDialogDBIdList.Add(_dbId);
        }

        /// <summary>
        /// 移除已经显示事件后置剧情的事件dbId
        /// </summary>
        /// <param name="_dbId"></param>
        public void removeHasShowBeforeEventDialogDBId(long _dbId)
        {
            _m_lHasShowBeforeEventDialogDBIdList?.Remove(_dbId);
        }

        /// <summary>
        /// 是否已经显示过事件前置剧情
        /// </summary>
        /// <param name="_dbId"></param>
        /// <returns></returns>
        public bool hasShowBeforeEventDialogDBId(long _dbId)
        {
            return _m_lHasShowBeforeEventDialogDBIdList?.Contains(_dbId) ?? false;
        }
    }
}