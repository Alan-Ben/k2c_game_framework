using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTravelConsortItem: _ANPGGUIBasicSubWnd<GGUIMonoTravelConsortItem>
    {
        private ConsortInfo _m_consortData;
        private NPGGuiWndTexture _m_iconWnd;
        private bool _m_showUp;
        private bool _m_isUnlock = true;


        public GGUIWndTravelConsortItem(GGUIMonoTravelConsortItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_isUnlock = true;
            _m_iconWnd?.discard();
            _m_iconWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClick, _clickBtn);
            if (null != wnd.icon)
            {
                _m_iconWnd = new NPGGuiWndTexture(wnd.icon);
            }
        }

        /// <summary>
        /// 点击跳转妃子详情页
        /// </summary>
        /// <param name="obj"></param>
        private void _clickBtn(GameObject obj)
        {
            GCommon.enterUIMainNodeShow(ESysSceneType.CONSORT_DETAIL, new List<string>(){_m_consortData.id.ToString()});
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_consortData"></param>
        public void setInfo(ConsortInfo _consortData, bool _showUp = false, bool _isUnlock = true)
        {
            _m_consortData = _consortData;
            _m_showUp = _showUp;
            _m_isUnlock = _isUnlock;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            
            if(null == _m_consortData)
                return;

            int like = 0;
            // int like = _m_consortData.curLike;
            
            // 新项目GOM先移除
            // int maxLike = _m_consortData.maxLike;
            int maxLike = 0;
            ALUGUICommon.setLabelTxt(wnd.txtLike, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, like, maxLike));
            ALUGUICommon.setLabelTxt(wnd.txtIntimacy, TextTranslate.instance.getLanguage(TransKeyConst.travel_intimacy_num, _m_consortData.intimacy));
            if (null != _m_iconWnd)
            {
                _m_iconWnd.showWnd();
                _m_iconWnd.setTexture(_m_consortData?.consortSkinShowInfo?.consortHeadIcon);
            }

            float scale = (maxLike == 0) ? 0 : (float) like / maxLike;
            ALUGUICommon.setSliderScale(wnd.sliderLike, scale);
            
            ALUGUICommon.setGameObjEnable(wnd.upShowList,_m_showUp);
            GGottenConsortInfo gottenConsortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_consortData.id);
            ALUGUICommon.setGameObjEnable(wnd.gotShowList, null != gottenConsortInfo);
            ALUGUICommon.setGameObjEnable(wnd.ungetShowList, null == gottenConsortInfo);
            _refreshUnlock();
        }

        private void _refreshUnlock()
        {
            if (!_m_isUnlock)
            {
                GGameCommonInfo.grayImage(wnd.lockGrayList);
            }
            else
            {
                GGameCommonInfo.disgrayImage(wnd.lockGrayList);
            }
        }
    }
}