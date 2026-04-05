using NPEnum;

namespace GOE
{
    /// <summary>
    /// 家人展示数据 - 不区分已解锁还是未解锁, 但是展示数据会根据是否解锁而不同
    /// 尽量少用, 因为调用字段时会先查找一遍妃子是否解锁
    /// </summary>
    public class ConsortInfo : _IConsortShowInfo , _IShowcaseLeftRightMove
    {
        private long _m_lConsortId;
        private ConsortRefShowInfo _m_consortRefShowInfo;
        private GGottenConsortInfo _m_GGottenConsortInfo;

        public ConsortInfo(long _consortId)
        {
            _m_lConsortId = _consortId;
            _m_consortRefShowInfo = new ConsortRefShowInfo(_m_lConsortId);
        }

        public ConsortInfo(GConsortRefObj _consortRef)
        {
            if (_consortRef == null)
                return;

            _m_lConsortId = _consortRef.id;
            _m_consortRefShowInfo = new ConsortRefShowInfo(_consortRef);
        }

        /// <summary>
        /// 只有已解锁的妃子才会有数据
        /// </summary>
        public GGottenConsortInfo gottenConsortInfo
        {
            get
            {
                if(_m_GGottenConsortInfo == null || _m_GGottenConsortInfo.consortId != _m_lConsortId)
                    _m_GGottenConsortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_lConsortId);

                return _m_GGottenConsortInfo;
            }
        }
        
        public ConsortRefShowInfo consortRefShowInfo { get { return _m_consortRefShowInfo; } }
        
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

        public _IConsortSkinShowInfo consortSkinShowInfo
        {
            get
            {
                if (gottenConsortInfo != null)
                    return gottenConsortInfo.consortSkinShowInfo;

                return _m_consortRefShowInfo?.consortSkinShowInfo;
            }
        }

        public long intimacy
        {
            get
            { 
                if (gottenConsortInfo != null)
                    return gottenConsortInfo.intimacy;

                return _m_consortRefShowInfo?.intimacy ?? 0;
            }
        }

        public long charm
        {
            get
            {
                if (gottenConsortInfo != null)
                    return gottenConsortInfo.charm;
                
                return _m_consortRefShowInfo?.charm ?? 0;
            }
        }

        public EGameCommonUnlockType unlockType
        {
            get
            {
                return NPPlayer.instance.consortComp.getConsortUnlockType(consortId);
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
            if (gottenConsortInfo != null && null != gottenConsortInfo.curSkinInfo)
            {
                return GCommon.getItemName(ENPItemType.CONSORT_SKIN, gottenConsortInfo.curSkinInfo.skinId);
            } 
            return GCommon.getItemName(ENPItemType.CONSORT_SKIN, consortRefObj?.default_skin_id ?? 0);
        }
    }
}