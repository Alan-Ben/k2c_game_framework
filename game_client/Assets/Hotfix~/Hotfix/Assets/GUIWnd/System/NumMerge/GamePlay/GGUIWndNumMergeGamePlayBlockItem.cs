using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 2048游戏棋子Item
    /// </summary>
    public class GGUIWndNumMergeGamePlayBlockItem : _AHotfixBaseSubWnd<GGUIMonoNumMergeGamePlayBlockItem>
    {
        private readonly Action<Vector2Int> _m_clickDelegate;
        private NPGGuiWndTexture _m_iconWnd;
        
        private NumMergeBlockRefObj _m_blockRefObj;
        private int _m_buffStep;
        private Vector2Int _m_gridPos;
        private Vector3 _m_localPosition;
        [ItemNotNull, NotNull] private readonly List<CommonUISfxObj> _m_sfxObjList;
        private const int MAX_SFX_COUNT = 4;

        private int _m_showSerialize;


        public GGUIWndNumMergeGamePlayBlockItem(GGUIHotfixCommonMono _wnd, Action<Vector2Int> _clickDelegate) : base(_wnd)
        {
            _m_sfxObjList = new List<CommonUISfxObj>();
            _m_clickDelegate = _clickDelegate;
            
            initWnd();
        }
        
        
        public NumMergeBlockRefObj blockRefObj { get { return _m_blockRefObj; } }
        public Vector3 localPosition
        {
            get { return _m_localPosition; }
            set
            {
                _m_localPosition = value;
                if (wnd != null)
                    wnd.transform.localPosition = _m_localPosition;
            }
        }
        public Vector2Int gridPos { get { return _m_gridPos; } set { _m_gridPos = value; } }
        public bool hasBuff { get { return _m_buffStep > 0; } }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
            _m_showSerialize = ALSerializeOpMgr.next();

            _clearAllSfx();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;
            
            if (hotfixWnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnClick, _onClick);
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (wnd != null)
                wnd.transform.localPosition = _m_localPosition;
            
            if (hotfixWnd == null)
                return;
            
            if (hotfixWnd.imgBlockIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(hotfixWnd.imgBlockIcon);
            
            ALUGUICommon.combineBtnClick(hotfixWnd.btnClick, _onClick);
        }


        public void refreshRef(NumMergeBlockRefObj _blockRefObj)
        {
            _m_blockRefObj = _blockRefObj;
            if (!_m_bIsShow)
                return;
            
            _m_iconWnd?.setTexture(_m_blockRefObj?.icon);
        }
        public void refreshBuff(int _buffStep)
        {
            if (_buffStep == _m_buffStep)
                return;
            
            _m_buffStep = _buffStep;
            if (!_m_bIsShow)
                return;

            if (hotfixWnd != null)
            {
                hotfixWnd.setHasBuff(_m_buffStep > 0);
                ALUGUICommon.setLabelTxt(hotfixWnd.txtBuffRemainStep, _m_buffStep);
            }
        }
        public void refreshWnd()
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            // 显示图标
            _m_iconWnd?.setTexture(_m_blockRefObj?.icon);
            hotfixWnd.setHasBuff(_m_buffStep > 0);
            ALUGUICommon.setLabelTxt(hotfixWnd.txtBuffRemainStep, _m_buffStep);
        }
        public void playResetAllAnim()
        {
            _playAnimation(hotfixWnd?.resetAllAnimName, null, 0);
            _clearAllSfx();
        }
        public void playSpawnAnim(Action _complete, float _delay)
        {
            if (_delay > 0)
                _sampleAnimation(hotfixWnd?.spawnAnimName, 0);
            _playAnimation(hotfixWnd?.spawnAnimName, _complete, _delay);
        }
        public void playOrganizeSpawnAnim(Action _complete, float _delay)
        {
            if (_delay > 0)
                _sampleAnimation(hotfixWnd?.organizeSpawnAnimName, 0);
            _playAnimation(hotfixWnd?.organizeSpawnAnimName, _complete, _delay);
        }
        public void playMergeSpawnAnim(Action _complete)
        {
            if (_m_blockRefObj == null)
            {
                _complete?.Invoke();
                return;
            }
            
            _playAnimation(_m_blockRefObj.merge_anim_name, _complete, 0);
            _playSfx(_m_blockRefObj.merge_sfx_id);
            
            if (string.IsNullOrEmpty(_m_blockRefObj.merge_tip_key))
                return;
            string tip;
            if (_m_blockRefObj.merge_tip_params is { Count: > 0 })
                tip = TextTranslate.instance.getLanguage(_m_blockRefObj.merge_tip_key, _m_blockRefObj.merge_tip_params);
            else
                tip = TextTranslate.instance.getLanguage(_m_blockRefObj.merge_tip_key);
            NPGUIAddSceneCenterTip.instance.showTextTip(tip, _m_blockRefObj.merge_tip_id);
        }
        public void playBuffMergeSpawnAnim(Action _complete)
        {
            if (_m_blockRefObj == null)
            {
                _complete?.Invoke();
                return;
            }
            
            _playAnimation(_m_blockRefObj.buff_merge_anim_name, _complete, 0);
            _playSfx(_m_blockRefObj.buff_merge_sfx_id);
            
            if (string.IsNullOrEmpty(_m_blockRefObj.merge_tip_key))
                return;
            string tip;
            if (_m_blockRefObj.merge_tip_params is { Count: > 0 })
                tip = TextTranslate.instance.getLanguage(_m_blockRefObj.merge_tip_key, _m_blockRefObj.merge_tip_params);
            else
                tip = TextTranslate.instance.getLanguage(_m_blockRefObj.merge_tip_key);
            NPGUIAddSceneCenterTip.instance.showTextTip(tip, _m_blockRefObj.merge_tip_id);
        }
        public void playDestroyAnim(Action _complete)
        {
            _playAnimation(hotfixWnd?.destroyAnimName, _complete, 0);
        }
        public void playOrganizeDestroyAnim(Action _complete, float _delay)
        {
            if (_delay > 0)
                _sampleAnimation(hotfixWnd?.organizeDestroyAnimName, 0);
            _playAnimation(hotfixWnd?.organizeDestroyAnimName, _complete, _delay);
        }
        public void playGameOverDestroyAnim(Action _complete, float _delay)
        {
            if (_delay > 0)
                _sampleAnimation(hotfixWnd?.gameOverDestroyAnimName, 0);
            _playAnimation(hotfixWnd?.gameOverDestroyAnimName, _complete, _delay);
        }
        
        
        private void _playAnimation(string _animName, Action _onComplete, float _delay)
        {
            if (hotfixWnd?.behaviorAnim == null || string.IsNullOrEmpty(_animName))
            {
                _onComplete?.Invoke();
                return;
            }

            if (_delay <= 0f)
                hotfixWnd.behaviorAnim.ForcePlay(_animName, 0, _onComplete);
            else
            {
                int serialize = _m_showSerialize;
                CommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (serialize != _m_showSerialize)
                    {
                        _onComplete?.Invoke();
                        return;
                    }
                
                    hotfixWnd.behaviorAnim.ForcePlay(_animName, 0, _onComplete);
                }, _delay);
            }
        }
        private void _playSfx(long _sfxId)
        {
            Transform sfxParent = hotfixWnd?.sfxParent;
            if (sfxParent == null)
                return;

            CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(_sfxId, sfxParent);
            if (sfxObj == null)
                return;
            
            _m_sfxObjList.Add(sfxObj);
            if (_m_sfxObjList.Count > MAX_SFX_COUNT)
            {
                _m_sfxObjList[0].forceDiscard();
                _m_sfxObjList.RemoveAt(0);
            }
        }
        private void _sampleAnimation(string _animName, float _normalizedTime)
        {
            if (hotfixWnd?.behaviorAnim == null || string.IsNullOrEmpty(_animName))
                return;

            hotfixWnd.behaviorAnim.Sample(_animName, _normalizedTime);
        }
        private void _onClick(GameObject _obj)
        {
            _m_clickDelegate?.Invoke(_m_gridPos);    
        }
        private void _clearAllSfx()
        {
            foreach (CommonUISfxObj sfx in _m_sfxObjList)
                sfx.forceDiscard();
            _m_sfxObjList.Clear();
        }
    }
}
