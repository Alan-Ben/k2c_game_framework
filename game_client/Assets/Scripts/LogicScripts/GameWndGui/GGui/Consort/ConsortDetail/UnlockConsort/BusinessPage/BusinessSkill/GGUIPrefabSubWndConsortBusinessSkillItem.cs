using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 经营技能item
    /// </summary>
    public class GGUIPrefabSubWndConsortBusinessSkillItem : _ANPGGUIBasicLoadPrefabSubWnd<GGUIConsortMonoBusinessSkillItem>
    {
        private ConsortBusinessSkillRefObj _m_rBusinessSkillRefObj;//经营技能数据
        private Transform _m_tParent;//父节点
        
        private long _m_lNowIntimacy;//当前亲密度
        private ConsortBusinessSkillInfo _m_businessSkillInfo;//经营技能服务端信息
        private bool _m_bSelected;//是否被选中
        
        private Func<GGUIPrefabSubWndConsortBusinessSkillItem, bool> _m_fNeedShowRedTipFunc;//是否需要显示红点的回调函数

        private List<GGuiWndSprite> _m_wIconList;//图标
        private List<CommonUISfxObj> _m_lSfxList;//特效列表
        
        private NPGGUIWndCommonRedTip _m_wRedTip;//红点
        
        public GGUIPrefabSubWndConsortBusinessSkillItem(ConsortBusinessSkillRefObj _businessSkillRefObj, Func<GGUIPrefabSubWndConsortBusinessSkillItem, bool> _needShowRedTipFunc, Transform _parent) : base(_parent)
        {
            _m_rBusinessSkillRefObj = _businessSkillRefObj;
            _m_fNeedShowRedTipFunc = _needShowRedTipFunc;
            _m_tParent = _parent;
        }

        protected override string _monoAssetPath { get { return _m_rBusinessSkillRefObj?.item_prefab_asset_path?.asset_path; } }
        protected override string _monoObjName { get { return _m_rBusinessSkillRefObj?.item_prefab_asset_path?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        public ConsortBusinessSkillRefObj businessSkillRefObj { get { return _m_rBusinessSkillRefObj; } }
        public ConsortBusinessSkillInfo businessSkillInfo { get { return _m_businessSkillInfo; } }
        public long nowIntimacy { get { return _m_lNowIntimacy; } }

        public event Action<GGUIPrefabSubWndConsortBusinessSkillItem> onItemClick;
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (_m_wIconList == null)
                _m_wIconList = new List<GGuiWndSprite>();
            if (wnd.iconList != null)
            {
                foreach (var img in wnd.iconList)
                {
                    if(img != null)
                        _m_wIconList.Add(new GGuiWndSprite(img));
                }
            }

            if (wnd.addProSlider != null)
            {
                wnd.addProSlider.wholeNumbers = false;
                wnd.addProSlider.minValue = 0;
                wnd.addProSlider.maxValue = 1;
            }
            
            if(wnd.monoRedTip != null)
                _m_wRedTip = new NPGGUIWndCommonRedTip(wnd.monoRedTip);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onItemClick);
        }
        
        protected override void _onDiscard()
        {
            onItemClick = null;

            if (_m_wIconList != null)
            {
                foreach (var wnd in _m_wIconList)
                {
                    wnd?.discard();
                }
                _m_wIconList.Clear();
            }
            _m_wIconList = null;

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onItemClick);
            }
            
            _discardAllSfx();
            _m_lSfxList = null;
            
            _m_wRedTip?.discard();
            _m_wRedTip = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            if (_m_wIconList != null)
            {
                foreach (var wnd in _m_wIconList)
                {
                    wnd?.hideWnd();
                }
            }
            
            _discardAllSfx();
            
            _m_wRedTip?.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wIconList != null)
            {
                foreach (var wnd in _m_wIconList)
                {
                    wnd?.discardTexture();
                }
            }
            
            _discardAllSfx();
            
            _m_wRedTip?.resetWnd();
        }
        
        /// <summary>
        /// 设置当前亲密度
        /// </summary>
        /// <param name="_lNowIntimacy"></param>
        public void setNowIntimacy(long _lNowIntimacy)
        {
            _m_lNowIntimacy = _lNowIntimacy;

            _refreshWnd();
        }
        
        /// <summary>
        /// 设置当前服务端经营技能信息
        /// </summary>
        /// <param name="_businessSkillInfo"></param>
        public void setBusinessSkillInfo(ConsortBusinessSkillInfo _businessSkillInfo)
        {
            if (_m_rBusinessSkillRefObj == null)
                return;
            
            if (_businessSkillInfo == null || _businessSkillInfo.skillId != _m_rBusinessSkillRefObj.id)
                _m_businessSkillInfo = null;
            else
                _m_businessSkillInfo = _businessSkillInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        { 
            if(wnd == null || _m_rBusinessSkillRefObj == null)
                return;

            BasicAttrRefObj addAttrRefObj = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((int) _m_rBusinessSkillRefObj.property);
            
            if (_m_wIconList != null && addAttrRefObj != null)
            {
                foreach (var imgWnd in _m_wIconList)
                {
                    if (imgWnd != null)
                    {
                        imgWnd.showWnd();
                        imgWnd.setTexture(addAttrRefObj.spt_icon);
                    }
                }
            }

            ALUGUICommon.setLabelTxt(wnd.txtUnlockIntimacy, _m_rBusinessSkillRefObj.unlock_need_intimacy);

            ALUGUICommon.setLabelTxt(wnd.txtAddPro,
                TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, _m_businessSkillInfo == null ? 0 : _m_businessSkillInfo.proAdd / 100f));

            if (wnd.addProSlider != null)
            {
                if (_m_businessSkillInfo == null || _m_businessSkillInfo.maxAddValue <= 0)
                {
                    wnd.addProSlider.value = 1;
                }
                else
                {
                    wnd.addProSlider.value = _m_businessSkillInfo.proAdd / (float) _m_businessSkillInfo.maxAddValue;
                }
            }
            
            EConsortBusinessSkillItemState skillItemState = ConsortUtil.getBusinessSkillState(_m_rBusinessSkillRefObj, _m_businessSkillInfo, _m_lNowIntimacy);
            wnd.setState(skillItemState);

            setSelect(_m_bSelected);
            refreshRedTipShow();
        }

        /// <summary>
        /// 设置是否被选中
        /// </summary>
        /// <param name="_select"></param>
        public void setSelect(bool _select)
        {
            _m_bSelected = _select;

            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.selectShow, _m_bSelected);
                ALUGUICommon.setGameObjEnable(wnd.selectHide, !_m_bSelected);
            }
        }

        /// <summary>
        /// 刷新是否显示红点
        /// </summary>
        public void refreshRedTipShow()
        {
            if (_m_wRedTip != null && _m_fNeedShowRedTipFunc != null)
            {
                _m_wRedTip.showWnd();
                _m_wRedTip.showRedTipNum(_m_fNeedShowRedTipFunc(this) ? 1 : 0);
            }
        }
        
        #region 特效

        /// <summary>
        /// 播放加成提升成功特效
        /// </summary>
        public void playProAddSuccessSfx()
        {
            if(wnd == null)
                return;
            
            _playSfx(wnd.proAddSuccessSfxId);
        }
        
        /// <summary>
        /// 播放加成提升失败特效
        /// </summary>
        public void playProAddFailSfx()
        {
            if(wnd == null)
                return;
            
            _playSfx(wnd.proAddFailSfxId);
        }
        
        /// <summary>
        /// 播放特效
        /// </summary>
        /// <param name="_sfxId"></param>
        private void _playSfx(long _sfxId)
        {
            if(wnd == null || !isShow || _sfxId <= 0 || wnd.sfxParent == null)
                return;
            
            if (_m_lSfxList == null)
                _m_lSfxList = new List<CommonUISfxObj>();

            CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(_sfxId, wnd.sfxParent);
            if(sfxObj == null)
                return;
            
            _m_lSfxList.Add(sfxObj);

            // 判断特效数量是否超过上限, 若是的话从头开始移除特效
            if (_m_lSfxList.Count > 0 && _m_lSfxList.Count > wnd.sfxLimitCount)
            {
                int needRemoveCount = _m_lSfxList.Count - wnd.sfxLimitCount;
                for (int i = 0; i < needRemoveCount; i++)
                {
                    _m_lSfxList[i]?.forceDiscard();
                }
                _m_lSfxList.RemoveRange(0, needRemoveCount);
            }
        }

        /// <summary>
        /// 销毁所有特效
        /// </summary>
        private void _discardAllSfx()
        {
            if (_m_lSfxList != null)
            {
                foreach (CommonUISfxObj sfxObj in _m_lSfxList)
                {
                    sfxObj?.forceDiscard();
                }
                _m_lSfxList.Clear();
            }
        }

        #endregion
        
        /// <summary>
        /// 当item被点击时
        /// </summary>
        private void _onItemClick(GameObject _go)
        {
            onItemClick?.Invoke(this);
        }

        protected override Transform _getParentTransForm()
        {
            return _m_tParent;
        }
        
        public void setParentTransform(Transform _parent)
        {
            _m_tParent = _parent;
            if(wnd != null && wnd.transform != null)
                wnd.transform.SetParent(_parent);
        }
    }
}