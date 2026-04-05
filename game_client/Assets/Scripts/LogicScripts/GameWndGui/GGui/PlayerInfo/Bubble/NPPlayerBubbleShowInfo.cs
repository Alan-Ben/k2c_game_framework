
namespace GOE
{
    /// <summary>
    /// 气泡框的展示数据，在UI中存储本数据进行展示处理
    /// </summary>
    public class NPPlayerBubbleShowInfo : _APlayerBaseShowInfo
    {
        //静态数据
        private NPPlayerBubbleRefObj _m_rBubbleRef;

        //基础数据
        private UniformItemObj _m_baseData;

        //对应的玩家实际数据集数据，当本数据为空，表示玩家未拥有
        private NPPlayerBubbleItem _m_tiBubbleItem;

        public NPPlayerBubbleShowInfo(NPPlayerBubbleRefObj _ref)
        {
            if (null == _ref)
                return;
            _m_rBubbleRef = _ref;
            _m_baseData = UniformItemSqliteAssistant.getUnifromItem(NPEnum.ENPItemType.BUBBLE, _ref.id);
            _m_tiBubbleItem = null;

        }
        public NPPlayerBubbleShowInfo(NPPlayerBubbleItem _item)
        {
            if (null == _item)
                return;
            _m_rBubbleRef = _item.refObj;
            _m_baseData = _item.baseData;
            _m_tiBubbleItem = _item;
        }

        //配表数据
        public NPPlayerBubbleRefObj refObj { get { return _m_rBubbleRef; } }
        //基础数据
        public UniformItemObj baseData { get { return _m_baseData; } }
        //玩家数据
        public NPPlayerBubbleItem bubbleItem { get { return _m_tiBubbleItem; } }

        public override NPEnum.ENPItemType itemType { get { return NPEnum.ENPItemType.BUBBLE; } }

        public ENPDressTabType tabType { get { return ENPDressTabType.BUBBLE; } }

        public override long id { get { return _m_rBubbleRef == null ? 0 : _m_rBubbleRef.id; } }

        public override long expiredTimeS { get { return _m_tiBubbleItem == null ? 0 : _m_tiBubbleItem.expiredTimeS; } }

        /// <summary>
        /// 判断是否锁定
        /// 根据是否拥有玩家数据进行判断
        /// </summary>
        public override bool isLock
        {
            get
            {
                if (null == _m_tiBubbleItem)
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
                if (null == _m_tiBubbleItem)
                    return true;

                return _m_tiBubbleItem.isExpired;
            }
        }

        /// <summary>
        /// 是否可以显示红点提示
        /// </summary>
        public override bool canShowRedTip
        {
            get
            {
                return _m_tiBubbleItem != null && _m_tiBubbleItem.isNew && !isLock && !isExpired;
            }
        }

        /// <summary>
        /// 设置为查看状态
        /// </summary>
        public override void setIsViewed()
        {
            if (_m_tiBubbleItem == null || _m_tiBubbleItem.refObj == null)
                return;

            if (_m_tiBubbleItem.isNew && !isLock && !isExpired)
            {
                //设置数据查看过
                _m_tiBubbleItem.setIsViewed(true);
                //并告知服务器
                NPPlayer.instance.bubbleComp.reqIsViewed(_m_tiBubbleItem.refObj.id);
            }
        }
    }
}
