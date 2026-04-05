using System;
using ALPackage;
using GOE;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 情人皮肤item
    /// </summary>
    public class GGUIWndConsortSkinItem : _ATNPGGUIWndSingleChoiceItem<GGUIMonoConsortSkinItem, GGUIWndConsortSkinItem>
    {
        private NPGGuiWndTexture _m_texSkinIcon; //皮肤图片
        
        //星级
        private GGUIWndHeroCommonStar _m_wStar;
        
        private GConsortSkinRefObj _m_skinRef; //皮肤配表数据


        public GGUIWndConsortSkinItem(GGUIMonoConsortSkinItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        /// <summary>
        /// 妃子皮肤配置
        /// </summary>
        public GConsortSkinRefObj skinRef { get => _m_skinRef; }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
            _m_texSkinIcon?.hideWnd();
            _m_wStar?.hideWnd();
        }

        protected override void _onResetEx()
        {
            _m_texSkinIcon?.discardTexture();
            _m_wStar?.resetWnd();
        }

        protected override void _onDiscardEx()
        {
            _m_texSkinIcon?.discard();
            _m_texSkinIcon = null;
            
            _m_wStar?.discard();
            _m_wStar = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd == null)
                return;

            if (wnd.skinIcon != null)
                _m_texSkinIcon = new NPGGuiWndTexture(wnd.skinIcon);

            if (wnd.monoStar != null)
                _m_wStar = new GGUIWndHeroCommonStar(wnd.monoStar);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_consortSkinRef"></param>
        public void setInfo(GConsortSkinRefObj _consortSkinRef)
        {
            if (_consortSkinRef == null)
                return;

            _m_skinRef = _consortSkinRef;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_skinRef == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.CONSORT_SKIN, _m_skinRef.id));
            if (_m_texSkinIcon != null)
            {
                _m_texSkinIcon.showWnd();
                _m_texSkinIcon.setTexture(_m_skinRef.skin_card_img);
            }
            
            //星级
            NPQualityExtRefObj qualityExtRef = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long) GCommon.getItemQuality(ENPItemType.CONSORT_SKIN, _m_skinRef.id));
            if (_m_wStar != null)
            {
                _m_wStar.showWnd();
                _m_wStar.setInfo(qualityExtRef != null ? qualityExtRef.consort_skin_star : 0);
            }
            
            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_skinRef.consort_id);//获取已解锁的妃子信息
            GConsortRefObj consortRefObj = consortInfo?.consortRefObj ?? GRefdataCoreMgr.instance.consortRefCore.getRef(_m_skinRef.consort_id);//获取妃子配表数据
            GConsortSkinInfo consortSkinInfo = consortInfo?.getSkinInfo(_m_skinRef.id);//获取已解锁的皮肤信息

            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, consortSkinInfo?.skinLvl ?? 0));
            
            // 解锁状态设置
            EConsortSkinStateType stat = consortInfo?.getSkinUnlockType(_m_skinRef.id) ?? EConsortSkinStateType.CANNOT_UNLOCK;//获取皮肤解锁状态
            NPCommonEnumStatInfo<EConsortSkinStateType>.setStat(wnd.statInfos, stat);
         
            bool isDefaultSkin = consortRefObj != null && consortRefObj.default_skin_id == _m_skinRef.id;
            ALUGUICommon.setGameObjEnable(wnd.goDefaultShowList, isDefaultSkin);
            ALUGUICommon.setGameObjEnable(wnd.goDefaultHideList, !isDefaultSkin);
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        public void refreshWnd()
        {
            _refreshWnd();
        }
    }   
}