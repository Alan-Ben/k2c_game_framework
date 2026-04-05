using System;
using ALPackage;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 三消任务的item
    /// </summary>
    public class GGUIWndTileMatchTaskBlockItem : _AHotfixBaseSubWnd<GGUIMonoTileMatchTaskBlockItem>
    {
        private GGuiWndSprite _m_wIcon;//图标
        
        private long _m_lBlockId;//方块ID
        private int _m_iDoneNum;//完成数量
        private int _m_iNeedTotalNum;//需要总数量
        private TileMatchBlockShowRefObj _m_rBlockShowRefObj;//方块表现配表数据
        
        public GGUIWndTileMatchTaskBlockItem(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        public long blockId { get { return _m_lBlockId; } }
        public int doneNum { get { return _m_iDoneNum; } }
        public int needTotalNum { get { return _m_iNeedTotalNum; } }

        protected override void _onWndInitDoneHotfix()
        {
            if(hotfixWnd == null)
                return;

            if (hotfixWnd.icon != null)
                _m_wIcon = new GGuiWndSprite(hotfixWnd.icon);
        }
        
        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
        }

        public void setData(long _blockId, int _doneNum, int _needTotalNum)
        {
            _m_lBlockId = _blockId;
            _m_iDoneNum = _doneNum;
            _m_iNeedTotalNum = _needTotalNum;
            if (_m_iDoneNum > _m_iNeedTotalNum)
                _m_iDoneNum = _m_iNeedTotalNum;
            
            _m_rBlockShowRefObj = HotfixRefdataCoreMgr.instance.getTileMatchBlockShowRefObj(_blockId, HotfixAccountSettingMgr.instance.hotfixAccountSetting.getTileMatchModelType());

            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (_m_wIcon != null)
            {
                if(_m_rBlockShowRefObj == null)
                    _m_wIcon.hideWnd();
                else
                {
                    _m_wIcon.showWnd();
                    _m_wIcon.setTexture(_m_rBlockShowRefObj.icon);
                }
            }

            _refreshProgress();
        }

        private void _refreshProgress()
        {
            if (hotfixWnd == null)
                return;

            string txtProgress = TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, _m_iDoneNum, _m_iNeedTotalNum);
            txtProgress = GCommon.addColorForRichText(txtProgress, _m_iDoneNum >= _m_iNeedTotalNum ? hotfixWnd.progressCompletedColor : hotfixWnd.progressUncompletedColor);
            ALUGUICommon.setLabelTxt(hotfixWnd.txtProgress, txtProgress);
        }
        
        /// <summary>
        /// 增加完成数量
        /// </summary>
        /// <param name="_addNum"></param>
        public void addDoneNum(int _addNum)
        {
            if(!isShow)
                return;
            
            if (_addNum <= 0)
                return;

            _m_iDoneNum += _addNum;
            if (_m_iDoneNum > _m_iNeedTotalNum)
                _m_iDoneNum = _m_iNeedTotalNum;

            _refreshProgress();
        }
        
        #region 动画

        /// <summary>
        /// 播放动画
        /// </summary>
        private void _playAnimation(string _aniName, Action _playDone)
        {
            if (hotfixWnd == null || hotfixWnd.ani == null || string.IsNullOrEmpty(_aniName))
            {
                _playDone?.Invoke();
                return;
            }

            hotfixWnd.ani.Play(_aniName, _playDone);
        }

        private void _sample(string _aniName, float _normalizedTime)
        {
            if (hotfixWnd == null || hotfixWnd.ani == null || string.IsNullOrEmpty(_aniName))
            {
                return;
            }
            
            hotfixWnd.ani.Sample(_aniName, _normalizedTime);
        }

        #endregion
    }
}