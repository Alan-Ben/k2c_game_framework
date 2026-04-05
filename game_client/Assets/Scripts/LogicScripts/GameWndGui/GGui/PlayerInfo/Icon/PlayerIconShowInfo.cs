
namespace GOE
{
    /// <summary>
    /// 头像的展示数据，在UI中存储本数据进行展示处理
    /// </summary>
    public class PlayerIconShowInfo : _APlayerBaseShowInfo
    {
        //静态数据
        private PlayerIconRefObj _m_rIconRef;

        //基础数据
        private UniformItemObj _m_baseData;

        //对应的玩家实际数据集数据，当本数据为空，表示玩家未拥有本头像
        private NPPlayerIconItem _m_tiIconItem;

        public PlayerIconShowInfo(PlayerIconRefObj _ref)
        {
            if (null == _ref)
                return;
            _m_rIconRef = _ref;
            _m_baseData = UniformItemSqliteAssistant.getUnifromItem(NPEnum.ENPItemType.ICON, _ref.id);
            _m_tiIconItem = null;

        }
        public PlayerIconShowInfo(NPPlayerIconItem _item)
        {
            if (null == _item)
                return;
            _m_rIconRef = _item.playerIconRef;
            _m_baseData = _item.baseData;
            _m_tiIconItem = _item;
        }

        //配表数据
        public PlayerIconRefObj refObj { get { return _m_rIconRef; } }
        //基础数据
        public UniformItemObj baseData { get { return _m_baseData; } }
        //玩家数据
        public NPPlayerIconItem iconItem { get { return _m_tiIconItem; } }

        public override NPEnum.ENPItemType itemType { get { return NPEnum.ENPItemType.ICON; } }
        public  EPlayerInfoIconType iconType { get { return _m_rIconRef.show_type; } }

        //头像id
        public override long id { get { return null == _m_rIconRef ? 0 : _m_rIconRef.id; } }

        public override long expiredTimeS { get { return _m_tiIconItem == null ? 0 : _m_tiIconItem.expiredTimeS; } }

        /// <summary>
        /// 判断本称号是否锁定
        /// 根据是否拥有玩家数据进行判断
        /// </summary>
        public override bool isLock
        {
            get
            {
                if (null == _m_tiIconItem)
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
                if (null == _m_tiIconItem)
                    return true;

                return _m_tiIconItem.isExpired;
            }
        }

        /// <summary>
        /// 是否可以显示红点提示
        /// </summary>
        public override bool canShowRedTip
        {
            get
            {
                return _m_tiIconItem != null && _m_tiIconItem.isNew && !isLock && !isExpired;
            }
        }

        /// <summary>
        /// 设置为查看状态
        /// </summary>
        public override void setIsViewed()
        {
            if (_m_tiIconItem == null || _m_tiIconItem.playerIconRef == null)
                return;

            if (_m_tiIconItem.isNew && !isLock && !isExpired)
            {
                //设置数据查看过
                _m_tiIconItem.setIsViewed(true);
                //并告知服务器
                NPPlayer.instance.iconComp.reqIsViewed(_m_tiIconItem.playerIconRef.id);
            }
        }
    }
}
