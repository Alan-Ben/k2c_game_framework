using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 本地推送管理类
    /// </summary>
    public class LocalPushMgr
    {
        private static LocalPushMgr _g_instance;
        [NotNull]
        public static LocalPushMgr instance
        {
            get
            {
                if (_g_instance == null)
                {
                    _g_instance = new LocalPushMgr();
                }
                return _g_instance;
            }
        }

        //本地推送列表
        [NotNull]private List<_ILocalPushDealer> _m_lLocalPushDealerList = new List<_ILocalPushDealer>();

        public LocalPushMgr() { }

        /// <summary>
        /// 注册本地推送
        /// </summary>
        /// <param name="_dealer"></param>
        public void regDealer(_ILocalPushDealer _dealer)
        {
            if (_dealer == null)
                return;

            _m_lLocalPushDealerList.Add(_dealer);
        }

        /// <summary>
        /// 注销本地推送
        /// </summary>
        /// <param name="_dealer"></param>
        public void unRegDealer(_ILocalPushDealer _dealer)
        {
            if (_dealer == null)
                return;

            _m_lLocalPushDealerList.Remove(_dealer);
        }

        /// <summary>
        /// 设置本地推送
        /// </summary>
        public void setLocalPush()
        {
            if(!SDKMgr.instance.isUseSDK)
                return;

            for (int i = 0; i < _m_lLocalPushDealerList.Count; i++)
            {
                _ILocalPushDealer dealer = _m_lLocalPushDealerList[i];
                if(dealer == null || !dealer.isValid)
                    continue;

                IReadOnlyList<LocalPushItem> itemList = dealer.getPushItemList();
                if (itemList == null)
                    continue;

                for (int j = 0; j < itemList.Count; j++)
                {
                    LocalPushItem item = itemList[j];
                    if (item.leftTimeSec <= 0)
                        continue;

                    //设置本地推送
                    SDKMgr.instance.noti_local(
                        item.title,
                        null,
                        item.content,
                        null,
                        item.leftTimeSec);
                }
            }
        }

        /// <summary>
        /// 清空本地推送
        /// </summary>
        public void clearLocalPush()
        {
            if (!SDKMgr.instance.isUseSDK)
                return;

            SDKMgr.instance.noti_removeLocal();
        }
    }
}
