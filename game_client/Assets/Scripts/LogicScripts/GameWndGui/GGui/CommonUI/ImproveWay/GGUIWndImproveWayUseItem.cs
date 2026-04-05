using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 提升途径-使用道具item
    /// </summary>
    public class GGUIWndImproveWayUseItem : _ATALBasicUISubWnd<GGUIMonoImproveWayUseItem>
    {
        //提升目标类型
        private EImproveTargetType _m_eTargetType;
        //可使用道具列表
        private List<BagItemUseRefObj> _m_lBagItemRefList;
        //前往效果字符串
        private string _m_sEffectStr;

        public GGUIWndImproveWayUseItem(GGUIMonoImproveWayUseItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoTo);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoTo);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_targetType"></param>
        /// <param name="_descKey"></param>
        /// <param name="_effectStr"></param>
        public void setInfo(EImproveTargetType _targetType, string _descKey, string _effectStr)
        {
            if (wnd == null)
                return;

            _m_eTargetType = _targetType;
            _m_sEffectStr = _effectStr;
            _m_lBagItemRefList = new List<BagItemUseRefObj>();
            GRefdataCoreMgr.instance.getBagItemUseRefByImproveTargetType(_m_eTargetType, _m_lBagItemRefList);

            //是否有可使用的道具
            bool canUse = false;
            for (int i = 0; i < _m_lBagItemRefList.Count; i++)
            {

                if (_m_lBagItemRefList[i] != null && GCommon.isItemEnough(ENPItemType.BAG_ITEM, _m_lBagItemRefList[i].id, 1, false))
                {
                    canUse = true;
                    break;
                }
            }

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goNoItemShowList, !canUse);
            ALUGUICommon.setGameObjEnable(wnd.goNoItemHideList, canUse);

            //设置描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_descKey));
        }

        /// <summary>
        /// 点击前往
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickGoTo(GameObject _go)
        {
            if (wnd == null || _m_lBagItemRefList == null)
                return;

            //是否有可使用的道具
            bool canUse = false;
            for (int i = 0; i < _m_lBagItemRefList.Count; i++)
            {
                if (_m_lBagItemRefList[i] != null && GCommon.isItemEnough(ENPItemType.BAG_ITEM, _m_lBagItemRefList[i].id, 1, false))
                {
                    canUse = true;
                    break;
                }
            }

            //没有可使用的道具
            if (!canUse)
            {
                //暂无提升道具
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.bag_noImproveItemCanUse_none);
                return;
            }

            if (string.IsNullOrEmpty(_m_sEffectStr))
                return;

            //执行效果
            _NPPlayerEffectSerializeInfo effectInfo = _NPPlayerEffectSerializeInfo.ReadFromString(_m_sEffectStr);
            effectInfo?.dealEffect();
        }
    }
}
