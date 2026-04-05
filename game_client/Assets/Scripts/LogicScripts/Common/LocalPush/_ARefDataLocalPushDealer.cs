using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 配表相关本地推送处理类
    /// </summary>
    public abstract class _ARefDataLocalPushDealer : _ILocalPushDealer
    {
        //本地推送配置数据
        private LocalPushRefObj _m_localPushRef;

        public _ARefDataLocalPushDealer(ELocalPushType _type)
        {
            _m_localPushRef = GRefdataCoreMgr.instance.localPushCore.getRef((long) _type);
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool isValid { get { return _m_localPushRef != null && 
                                           GCommon.isSimpleUnlock(_m_localPushRef.simple_unlock_id) && 
                                           GCommon.isFuncUnlock(_m_localPushRef.func_type) && 
                                           GameSetting.instance.getLocalPushSwitchIsOpen(_m_localPushRef.type); } }

        /// <summary>
        /// 获取推送数据项列表（标题/内容支持文本参数，自动扣除提前通知时间）
        /// </summary>
        public IReadOnlyList<LocalPushItem> getPushItemList()
        {
            if (_m_localPushRef == null)
                return null;

            IReadOnlyList<PushItemData> itemDataList = getItemDataList();
            if (itemDataList == null || itemDataList.Count == 0)
                return null;

            long advanceTime = _m_localPushRef.advance_time;
            List<LocalPushItem> itemList = new List<LocalPushItem>(itemDataList.Count);
            for (int i = 0; i < itemDataList.Count; i++)
            {
                PushItemData data = itemDataList[i];
                string title = data.titleArgs != null && data.titleArgs.Length > 0
                    ? TextTranslate.instance.getLanguage(_m_localPushRef.title, data.titleArgs)
                    : TextTranslate.instance.getLanguage(_m_localPushRef.title);
                string content = data.contentArgs != null && data.contentArgs.Length > 0
                    ? TextTranslate.instance.getLanguage(_m_localPushRef.content, data.contentArgs)
                    : TextTranslate.instance.getLanguage(_m_localPushRef.content);
                itemList.Add(new LocalPushItem(title, content, data.leftTimeSec - advanceTime));
            }
            return itemList;
        }

        /// <summary>
        /// 获取推送原始数据项列表（子类实现，支持返回多条）
        /// </summary>
        protected abstract IReadOnlyList<PushItemData> getItemDataList();

        public override string ToString()
        {
            string type = _m_localPushRef != null ? _m_localPushRef.type.ToString() : null;
            IReadOnlyList<LocalPushItem> items = getPushItemList();
            return $"【_ARefDataLocalPushDealer】type:{type}\nisValid:{isValid}\nitemsCount:{items.Count}";
        }

        /// <summary>
        /// 推送原始数据项（由子类填充）
        /// </summary>
        public struct PushItemData
        {
            /// <summary>
            /// 剩余时间（秒）
            /// </summary>
            public long leftTimeSec;
            /// <summary>
            /// 标题文本参数
            /// </summary>
            public object[] titleArgs;
            /// <summary>
            /// 内容文本参数
            /// </summary>
            public object[] contentArgs;

            public PushItemData(long _leftTimeSec, object[] _titleArgs = null, object[] _contentArgs = null)
            {
                this.leftTimeSec = _leftTimeSec;
                this.titleArgs = _titleArgs;
                this.contentArgs = _contentArgs;
            }
        }
    }
}

