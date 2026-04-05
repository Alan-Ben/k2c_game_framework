
namespace GOE
{
    /// <summary>
    /// 头像的展示数据，在UI中存储本数据进行展示处理
    /// </summary>
    public class PlayerIconBgkShowInfo : _APlayerBaseShowInfo
    {
        //静态数据
        private PlayerIconBgkRefObj _m_rIconBgkRef;

        //基础数据
        private UniformItemObj _m_baseData;

        //对应的玩家实际数据集数据，当本数据为空，表示玩家未拥有本头像框
        private PlayerIconBgkItem _m_tiIconBgkItem;

        public PlayerIconBgkShowInfo(PlayerIconBgkRefObj _ref)
        {
            if (null == _ref)
                return;
            _m_rIconBgkRef = _ref;
            _m_baseData = UniformItemSqliteAssistant.getUnifromItem(NPEnum.ENPItemType.ICON_BGK, _ref.id);
            _m_tiIconBgkItem = null;

        }
        public PlayerIconBgkShowInfo(PlayerIconBgkItem _item)
        {
            if (null == _item)
                return;
            _m_rIconBgkRef = _item.iconBgkRef;
            _m_baseData = _item.baseData;
            _m_tiIconBgkItem = _item;
        }

        //配表数据
        public PlayerIconBgkRefObj refObj { get { return _m_rIconBgkRef; } }
        //基础数据
        public UniformItemObj baseData { get { return _m_baseData; } }
        //玩家数据
        public PlayerIconBgkItem iconBgkItem { get { return _m_tiIconBgkItem; } }

        public override NPEnum.ENPItemType itemType { get { return NPEnum.ENPItemType.ICON_BGK; } }
        public ENPDressTabType tabType { get { return ENPDressTabType.ICON_BGK; } }

        public override long id { get { return null == _m_rIconBgkRef ? 0 : _m_rIconBgkRef.id; } }
        public override long expiredTimeS { get { return _m_tiIconBgkItem == null ? 0 : _m_tiIconBgkItem.expiredTimeS; } }

        /// <summary>
        /// 判断本头像框是否锁定
        /// 根据是否拥有玩家数据进行判断
        /// </summary>
        public override bool isLock
        {
            get
            {
                if (null == _m_tiIconBgkItem)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// 是否过期
        /// </summary>
        public override bool isExpired
        {
            get
            {
                if (null == _m_tiIconBgkItem)
                    return true;

                return _m_tiIconBgkItem.isExpired;
            }
        }

        /// <summary>
        /// 是否可以显示红点提示
        /// </summary>
        public override bool canShowRedTip
        {
            get
            {
                return _m_tiIconBgkItem != null && _m_tiIconBgkItem.isNew && !isLock && !isExpired;
            }
        }

        /// <summary>
        /// 设置为查看状态
        /// </summary>
        public override void setIsViewed()
        {
            if (_m_tiIconBgkItem == null || _m_tiIconBgkItem.iconBgkRef == null)
                return;

            if (_m_tiIconBgkItem.isNew && !isLock && !isExpired)
            {
                //设置数据查看过
                _m_tiIconBgkItem.setIsViewed(true);
                //并告知服务器
                NPPlayer.instance.iconBgkComp.reqIsViewed(_m_tiIconBgkItem.iconBgkRef.id);
            }
        }
    }
}
