using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 基础属性自定义上浮提示展示管理器
    /// </summary>
    public class NPGGUIAttrTipDealerMgr
    {
        //tip管理器
        private GGUICommonTipWithParentTransDealerMgr _m_strTipMgr;
        private GGUICommonTipWithParentTransDealerMgr _m_intTipMgr;
        private GGUICommonTipWithParentTransDealerMgr _m_polTipMgr;
        private GGUICommonTipWithParentTransDealerMgr _m_leadTipMgr;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="_coexistMaxCount">同时存在最大数量</param>
        /// <param name="_tipAccMaxRate">最大播放速度</param>
        /// <param name="_tipAccMinNum">数量达到这个数则开始加速</param>
        /// <param name="_tipAccMaxNum">数量达到这个数则达到最高速</param>
        public NPGGUIAttrTipDealerMgr(long _coexistMaxCount = 3, float _tipAccMaxRate = 3f, int _tipAccMinNum = 2, int _tipAccMaxNum = 12)
        {
            _m_strTipMgr = new GGUICommonTipWithParentTransDealerMgr(_coexistMaxCount, _tipAccMaxRate, _tipAccMinNum, _tipAccMaxNum);
            _m_intTipMgr = new GGUICommonTipWithParentTransDealerMgr(_coexistMaxCount, _tipAccMaxRate, _tipAccMinNum, _tipAccMaxNum);
            _m_polTipMgr = new GGUICommonTipWithParentTransDealerMgr(_coexistMaxCount, _tipAccMaxRate, _tipAccMinNum, _tipAccMaxNum);
            _m_leadTipMgr = new GGUICommonTipWithParentTransDealerMgr(_coexistMaxCount, _tipAccMaxRate, _tipAccMinNum, _tipAccMaxNum);
        }

        /// <summary>
        /// 添加提示
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_tipParent"></param>
        /// <param name="_value"></param>
        /// <param name="_tipId"></param>
        public void addTip(EBasicAttrType _type, Transform _tipParent, long _value, long _tipId)
        {
            if (_tipParent == null)
                return;

            BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)_type);
            if (_value > 0 && basicAttrRef != null)
            {
                NPCenterTipsRefObj tipsRef = GRefdataCoreMgr.instance.tipMap.getRef(_tipId);
                // NPIconTextTipDealer dealer = new NPIconTextTipDealer(basicAttrRef.icon, TextTranslate.instance.getLanguage(TransKeyConst.hero_attrAddValue_name_num, TextTranslate.instance.getLanguage(basicAttrRef.name), _value), tipsRef, null);
                // switch (_type)
                // {
                //     case EBasicAttrType.STR:
                //         _m_strTipMgr?.addTip(dealer, _tipParent);
                //         break;
                //     case EBasicAttrType.INT:
                //         _m_intTipMgr?.addTip(dealer, _tipParent);
                //         break;
                //     case EBasicAttrType.POL:
                //         _m_polTipMgr?.addTip(dealer, _tipParent);
                //         break;
                //     case EBasicAttrType.LEAD:
                //         _m_leadTipMgr?.addTip(dealer, _tipParent);
                //         break;
                // }
            }
        }

        /// <summary>
        /// 清空tip
        /// </summary>
        public void clear()
        {
            _m_strTipMgr?.clear();
            _m_intTipMgr?.clear();
            _m_polTipMgr?.clear();
            _m_leadTipMgr?.clear();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void discard()
        {
            clear();
            _m_strTipMgr = null;
            _m_intTipMgr = null;
            _m_polTipMgr = null;
            _m_leadTipMgr = null;
        }
    }
}
