using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子解锁详情页面加护page
    /// </summary>
    public class GGUIWndUnLockConsortDetailBlessPage : _AGGUIWndUnLockConsortDetailTabPage<GGUIMonoUnLockConsortDetailBlessPage>
    {
        private GGUIWndConsortBlessHeroSimpleIconContainer _m_wRelationHeroContainer;//关联大臣列表
        
        private GGUIWndConsortBlessSkillContainer _m_wConsortBlessSkillContainer;//妃子加护技能列表
        public GGUIWndUnLockConsortDetailBlessPage(NPCommonAssetPathInfo _commonAssetPathInfo, Transform _parent) : base(_commonAssetPathInfo, _parent)
        {
            _m_iCommonAssetPathInfo = _commonAssetPathInfo;
        }

        /// <summary>
        /// 本窗口对应的页签类型
        /// </summary>
        public override EUnLockConsortDetailWndTabType tabPageType { get { return EUnLockConsortDetailWndTabType.BLESS; } }

        protected override void _onWndInitDoneSub()
        {
            if(wnd == null)
                return;

            if(wnd.monoRelationHeroContainer != null)
                _m_wRelationHeroContainer = new GGUIWndConsortBlessHeroSimpleIconContainer(wnd.monoRelationHeroContainer);
            
            if (wnd.monoConsortBlessSkillContainer != null)
                _m_wConsortBlessSkillContainer = new GGUIWndConsortBlessSkillContainer(wnd.monoConsortBlessSkillContainer);
        }

        protected override void _onDiscardSub()
        {
            _m_wRelationHeroContainer?.discard();
            _m_wRelationHeroContainer = null;
            
            _m_wConsortBlessSkillContainer?.discard();
            _m_wConsortBlessSkillContainer = null;
        }

        protected override void _onShowWndSub()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_BLESS_SKILL_INFO_CHG, _onConsortBlessSkillInfoChg);
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT_BLESS_SKILL_UPGRADE_BY_INDEX, _onSimulateClickBlessSkillUpgrade);
        }

        protected override void _onHideWndSub()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_BLESS_SKILL_INFO_CHG, _onConsortBlessSkillInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_CONSORT_BLESS_SKILL_UPGRADE_BY_INDEX, _onSimulateClickBlessSkillUpgrade);

            _m_wRelationHeroContainer?.hideWnd();
            
            _m_wConsortBlessSkillContainer?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wRelationHeroContainer?.resetWnd();
            
            _m_wConsortBlessSkillContainer?.resetWnd();
        }

        protected override void _refreshWndSub()
        {
            if (_m_wRelationHeroContainer != null)
            {
                _m_wRelationHeroContainer.showWnd();
                _m_wRelationHeroContainer.setData(_m_iConsortShowInfo?.consortId ?? 0);
            }
            
            if (_m_wConsortBlessSkillContainer != null)
            {
                _m_wConsortBlessSkillContainer.showWnd();
                _m_wConsortBlessSkillContainer.setData(_m_iConsortShowInfo);
            }
        }

        protected override void _setDataSub()
        {
            if(_m_iConsortShowInfo != null)
                NPPlayer.instance.consortComp.setReadConsortBlessSkillRed(_m_iConsortShowInfo.consortId);
        }

        /// <summary>
        /// 当妃子加护技能数据变化时
        /// </summary>
        private void _onConsortBlessSkillInfoChg()
        {
            if (_m_wRelationHeroContainer != null && _m_wRelationHeroContainer.isShow && wnd != null && wnd.onBlessSkillLevelUpShowSfxId > 0)
            {
                _m_wRelationHeroContainer.dealAllItemWnd((_itemWnd) =>
                {
                    _itemWnd?.playSfx(wnd.onBlessSkillLevelUpShowSfxId);
                });
            }
        }

        //模拟点击家人加护技能升级，item下标从0开始
        private void _onSimulateClickBlessSkillUpgrade(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long index = (long)_objects[0];

            if (_m_wConsortBlessSkillContainer != null)
                _m_wConsortBlessSkillContainer.setClickUpgrade((int)index);
        }
    }
}