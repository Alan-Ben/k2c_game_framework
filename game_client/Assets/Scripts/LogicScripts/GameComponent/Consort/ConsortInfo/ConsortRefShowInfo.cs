using NPEnum;

namespace GOE
{
    /// <summary>
    /// 妃子的配表展示数据, 纯配表数据展示数据, 除了解锁状态外其他数据不会根据妃子是否解锁而改变
    /// 也可作为未解锁妃子的展示数据, 因为未解锁妃子没有服务端数据, 能展示的只有配表数据
    /// </summary>
    public class ConsortRefShowInfo : _IConsortShowInfo
    {
        private long _m_lConsortId;
        private GConsortRefObj _m_consortRefObj;
        
        private EQuality _m_eConsortQuality;//妃子品质
        private NPQualityExtRefObj _m_consortQualityExtRefObj;//妃子品质扩展表

        public ConsortRefShowInfo(long consortId)
        {
            _m_lConsortId = consortId;
            _m_consortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(consortId);
            
            _m_eConsortQuality = GCommon.getItemQuality(ENPItemType.CONSORT, _m_lConsortId);
            _m_consortQualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long)_m_eConsortQuality);
        }

        public ConsortRefShowInfo(GConsortRefObj _consortRefObj)
        {
            _m_lConsortId = _consortRefObj?.id ?? 0;
            _m_consortRefObj = _consortRefObj;
            
            _m_eConsortQuality = GCommon.getItemQuality(ENPItemType.CONSORT, _m_lConsortId);
            _m_consortQualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long)_m_eConsortQuality);
        }

        public GConsortRefObj consortRefObj
        {
            get
            {
                if (_m_consortRefObj == null || _m_consortRefObj.id != _m_lConsortId)
                    _m_consortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_lConsortId);
                
                return _m_consortRefObj;
            }
        }

        public _IConsortSkinShowInfo consortSkinShowInfo { get { return consortRefObj?.defaultSkinRefObj; } }

        public long consortId { get { return _m_lConsortId; } }
        public string consortTransName { get { return consortRefObj?.transName; } }
        public string consortTransTitle { get { return TextTranslate.instance.getLanguage(consortRefObj?.consort_title); } }
        public EQuality consortQuality { get { return _m_eConsortQuality; } }
        public NPGTextureIndex consortNameBg { get { return _m_consortQualityExtRefObj?.consort_name_bg; } }
        
        /// <summary>
        /// 直接给初始好感度
        /// </summary>
        public long intimacy { get { return consortRefObj?.init_intimacy ?? 0; } }
        
        /// <summary>
        /// 直接给初始魅力
        /// </summary>
        public long charm { get { return consortRefObj?.init_charm ?? 0; } }
        public EGameCommonUnlockType unlockType { get { return NPPlayer.instance.consortComp.getConsortUnlockType(_m_lConsortId); } }
    }
}