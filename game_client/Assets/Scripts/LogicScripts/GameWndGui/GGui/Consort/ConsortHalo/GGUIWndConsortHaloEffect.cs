using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 星辉效果弹窗
    /// </summary>
    public class GGUIWndConsortHaloEffect : _ATALBasicUIWnd<GGUIMonoConsortHaloEffect>
    {
        private static GGUIWndConsortHaloEffect _g_instance;
        public static GGUIWndConsortHaloEffect instance { get { return _g_instance ??= new GGUIWndConsortHaloEffect(); } }

        private ConsortHaloInfo _m_iConsortHaloInfo;//星辉信息
        [NotNull] private List<ConsortHaloSkillInfo> _m_lNeedShowHaloSkillInfoList = new List<ConsortHaloSkillInfo>();//需要显示的星辉技能信息列表
        
        private GGUIWndConsortHaloSkillLvlItemContainer _m_wSkillLvlItemContainer;//技能等级itemContainer
        
        public GGUIWndConsortHaloEffect() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortHaloEffect.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortHaloEffect.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.haloSkillLvlItemContainer != null)
                _m_wSkillLvlItemContainer = new GGUIWndConsortHaloSkillLvlItemContainer(wnd.haloSkillLvlItemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            }
            
            _m_wSkillLvlItemContainer?.discard();
            _m_wSkillLvlItemContainer = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wSkillLvlItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSkillLvlItemContainer?.resetWnd();
        }
        
        public void setData(ConsortHaloInfo _haloInfo)
        {
            _m_iConsortHaloInfo = _haloInfo;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            if (_m_iConsortHaloInfo == null || !_m_iConsortHaloInfo.isUnlock || _m_iConsortHaloInfo.nowHaloLvlRefObj == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noEffectShow, true);
                ALUGUICommon.setGameObjEnable(wnd.hasHaloSkillShow, false);
                ALUGUICommon.setGameObjEnable(wnd.hasConsortPropertyAddShow, false);
                
                _m_wSkillLvlItemContainer?.hideWnd();
                
                return;
            }

            ALUGUICommon.setGameObjEnable(wnd.noEffectShow, false);
            
            bool hasIntimacyAdd = _m_iConsortHaloInfo.nowHaloLvlRefObj.add_intimacy > 0;//是否有亲密度加成
            bool hasCharmAdd = _m_iConsortHaloInfo.nowHaloLvlRefObj.add_charm > 0;//是否有加护力加成
            bool hasNeedShowEffect = hasIntimacyAdd || hasCharmAdd;//是否有需要显示的效果
            if (hasIntimacyAdd || hasCharmAdd)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasConsortPropertyAddShow, true);

                string intimacyAddKey = string.IsNullOrEmpty(wnd.txtIntimacyAddKey) ? TransKeyConst.common_add_num : wnd.txtIntimacyAddKey;
                ALUGUICommon.setLabelTxt(wnd.txtIntimacyAdd, TextTranslate.instance.getLanguage(intimacyAddKey, _m_iConsortHaloInfo.nowHaloLvlRefObj.add_intimacy));
                
                string charmAddKey = string.IsNullOrEmpty(wnd.txtCharmAddKey) ? TransKeyConst.common_add_num : wnd.txtCharmAddKey;
                ALUGUICommon.setLabelTxt(wnd.txtCharmAdd, TextTranslate.instance.getLanguage(charmAddKey, _m_iConsortHaloInfo.nowHaloLvlRefObj.add_charm));

            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasConsortPropertyAddShow, false);
            }

            _m_lNeedShowHaloSkillInfoList.Clear();
            if (_m_iConsortHaloInfo.nowHaloLvlRefObj.halo_skill_list != null)
            {
                foreach (var haloSkillInfo in _m_iConsortHaloInfo.nowHaloLvlRefObj.halo_skill_list)
                {
                    if(haloSkillInfo == null || haloSkillInfo.haloSkillLvlRefObj == null || 
                       haloSkillInfo.haloSkillLvlRefObj.add_basic_attr == null || haloSkillInfo.haloSkillLvlRefObj.add_bonus_attr == null)
                        continue;
                    
                    if(!haloSkillInfo.haloSkillLvlRefObj.add_basic_attr.isEmpty() || !haloSkillInfo.haloSkillLvlRefObj.add_bonus_attr.isEmpty())
                    {
                        _m_lNeedShowHaloSkillInfoList.Add(haloSkillInfo);
                    }
                }
            }
            if (_m_lNeedShowHaloSkillInfoList.Count > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasHaloSkillShow, true);

                if (_m_wSkillLvlItemContainer != null)
                {
                    _m_wSkillLvlItemContainer.showWnd();
                    _m_wSkillLvlItemContainer.setData(_m_lNeedShowHaloSkillInfoList);
                }
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasHaloSkillShow, false);
                
                _m_wSkillLvlItemContainer?.hideWnd();
            }
            
            hasNeedShowEffect = hasNeedShowEffect || _m_lNeedShowHaloSkillInfoList.Count > 0;
            ALUGUICommon.setGameObjEnable(wnd.noEffectShow, !hasNeedShowEffect);
        }
        
        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onBtnCloseClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_HALO_EFFECT_DETAIL);
        }
    }
}