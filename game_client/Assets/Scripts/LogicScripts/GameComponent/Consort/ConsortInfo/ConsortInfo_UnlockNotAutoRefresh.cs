using NPEnum;

namespace GOE
{
    /// <summary>
    /// 家人展示数据 - 不区分已解锁还是未解锁, 但是展示数据会根据是否解锁而不同
    /// 不会自动更新是否以获取的家人数据
    /// </summary>
    public class ConsortInfo_UnlockNotAutoRefresh : _IConsortShowInfo , _IShowcaseLeftRightMove
    {
        private long _m_lConsortId;
        private ConsortRefShowInfo _m_consortRefShowInfo;
        private GGottenConsortInfo _m_GGottenConsortInfo;

        public ConsortInfo_UnlockNotAutoRefresh(long _consortId)
        {
            _m_lConsortId = _consortId;
            _m_consortRefShowInfo = new ConsortRefShowInfo(_m_lConsortId);
            updateGGottenConsortInfo();
        }

        public ConsortInfo_UnlockNotAutoRefresh(GConsortRefObj _consortRef)
        {
            if (_consortRef == null)
                return;

            _m_lConsortId = _consortRef.id;
            _m_consortRefShowInfo = new ConsortRefShowInfo(_consortRef);
            updateGGottenConsortInfo();
        }

        public GGottenConsortInfo gottenConsortInfo { get { return _m_GGottenConsortInfo; } }
        public ConsortRefShowInfo consortRefShowInfo { get { return _m_consortRefShowInfo; } }

        /// <summary>
        /// 更新以获取妃子数据
        /// </summary>
        public void updateGGottenConsortInfo()
        {
            _m_GGottenConsortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_lConsortId);
        }

        #region _IConsortShowInfo接口

        public long consortId { get { return _m_lConsortId; } }
        public string consortTransName { get { return _m_consortRefShowInfo?.consortTransName; } }

        public NPGTextureIndex consortNameBg
        {
            get
            {
                NPQualityExtRefObj qualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long)GCommon.getItemQuality(ENPItemType.CONSORT, _m_lConsortId));
                return qualityExtRefObj?.consort_name_bg;
            }
        }

        public string consortTransTitle { get { return _m_consortRefShowInfo.consortTransTitle; } }

        public EQuality consortQuality { get { return GCommon.getItemQuality(ENPItemType.CONSORT, _m_lConsortId); } }
        
        public _IConsortSkinShowInfo consortSkinShowInfo { get { return _m_GGottenConsortInfo == null ? _m_consortRefShowInfo?.consortSkinShowInfo : _m_GGottenConsortInfo.consortSkinShowInfo; } }

        public long intimacy
        {
            get
            { 
                if (_m_GGottenConsortInfo != null)
                    return _m_GGottenConsortInfo.intimacy;

                return _m_consortRefShowInfo?.intimacy ?? 0;
            }
        }

        public long charm
        {
            get
            {
                if (_m_GGottenConsortInfo != null)
                    return _m_GGottenConsortInfo.charm;
                
                return _m_consortRefShowInfo?.charm ?? 0;
            }
        }

        public EGameCommonUnlockType unlockType
        {
            get
            {
                return _m_GGottenConsortInfo == null ? EGameCommonUnlockType.LOCK : EGameCommonUnlockType.UNLOCK;
            }
        }

        #endregion

        /// <summary>
        /// 妃子配表数据
        /// </summary>
        public GConsortRefObj consortRefObj { get { return _m_consortRefShowInfo?.consortRefObj; } }

        public long id { get => _m_lConsortId; }

        public NPGGoIndex unitIndex { get => consortSkinShowInfo?.tdShow; }

        public NPGGoIndex bgIndex
        {
            get
            {
                // return GRefdataCoreMgr.instance.npGeneral.consort_default_detail_bg;
                return consortSkinShowInfo?.tdBgIndex;
            }
        }

        /// <summary>
        /// 当前皮肤名
        /// </summary>
        /// <returns></returns>
        public string getSkinName()
        {
            if (_m_GGottenConsortInfo != null && null != _m_GGottenConsortInfo.curSkinInfo)
            {
                return GCommon.getItemName(ENPItemType.CONSORT_SKIN, _m_GGottenConsortInfo.curSkinInfo.skinId);
            } 
            return GCommon.getItemName(ENPItemType.CONSORT_SKIN, consortRefObj.default_skin_id);
        }
    }
}