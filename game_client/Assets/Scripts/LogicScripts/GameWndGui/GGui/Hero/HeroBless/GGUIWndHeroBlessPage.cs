using System;
using UnityEngine;
using ALPackage;
using NPEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 伙伴信息加护页签
    /// </summary>
    public class GGUIWndHeroBlessPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoHeroBlessPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //伙伴配置
        private HeroRefObj _m_lHeroRef;
        //点击关闭按钮
        private Action _m_aOnClickClose;
        //当前选中的家人id
        private long _m_lCurSelectConsortId;
        //家人列表
        private GGUIWndHeroBlessNullableContainer _m_wBlessContainer;

        public GGUIWndHeroBlessPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }
        
        protected override void _onHideWnd()
        {
            _m_wBlessContainer?.hideWnd();
            _m_lCurSelectConsortId = 0;
        }
        
        protected override void _onReset()
        {
            _m_wBlessContainer?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            _m_wBlessContainer?.discard();
            _m_wBlessContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoTo);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnPreview, _onClickPreview);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoBlessContainer != null)
            {
                _m_wBlessContainer = new GGUIWndHeroBlessNullableContainer(wnd.monoBlessContainer);
                _m_wBlessContainer.onClickItem += _onSelectItem;
            }

            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoTo);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnPreview, _onClickPreview);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroId"></param>
        public void setInfo(HeroRefObj _heroRef, Action _onClickClose)
        {
            _m_lHeroRef = _heroRef;
            _m_aOnClickClose = _onClickClose;

            _refreshBlessContainer();
        }

        //刷新家人列表
        private void _refreshBlessContainer()
        {
            if (_m_lHeroRef == null)
                return;

            if (_m_wBlessContainer != null)
            {
                _m_wBlessContainer.showWnd();
                _m_wBlessContainer.showItemList(_m_lHeroRef.relationConsortIdList);
            }
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_lHeroRef == null)
                return;

            GConsortRefObj consortRef = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_lCurSelectConsortId);
            bool isUnlock = NPPlayer.instance.consortComp.getConsortUnlockType(_m_lCurSelectConsortId) == EGameCommonUnlockType.UNLOCK;
            if (consortRef == null)
                return;

            //设置描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.hero_blessHeroDesc_str_str,
                    GCommon.getItemName(ENPItemType.CONSORT, _m_lCurSelectConsortId),
                    GCommon.getItemName(ENPItemType.HERO, _m_lHeroRef.id)));

            //设置加成
            if (isUnlock)
            {
                GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_lCurSelectConsortId);
                long consortAddValue = consortInfo != null ? consortInfo.getBlessSkillHeroAddPower(_m_lHeroRef.id) : 0;
                ALUGUICommon.setLabelTxt(wnd.txtAddPower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, consortAddValue));
            }
            else
            {
                //{0}（暂未获取）
                ALUGUICommon.setLabelTxt(wnd.txtLockDesc, TextTranslate.instance.getLanguage(TransKeyConst.hero_blessConsortNotGet_str, 
                    GCommon.getItemName(ENPItemType.CONSORT, _m_lCurSelectConsortId)));
            }

            //是否解锁显隐
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, !isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goLockHideList, isUnlock);
        }

        #region 点击事件

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            _m_aOnClickClose?.Invoke();
        }

        //选中item
        private void _onSelectItem(GGUIWndHeroBlessNullableContainerItem _item)
        {
            if (_item == null || _m_lCurSelectConsortId == _item.consortId)
                return;

            _m_lCurSelectConsortId = _item.consortId;
            _refreshWnd();
        }

        //点击前往按钮
        private void _onClickGoTo(GameObject _go)
        {
            List<GGottenConsortInfo> consortList = new List<GGottenConsortInfo>();
            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_lCurSelectConsortId);
            if (consortInfo != null)
                consortList.Add(consortInfo);

            GNodeUnLockConsortDetail.addConsortNode(consortList, _m_lCurSelectConsortId);
        }

        //点击预览
        private void _onClickPreview(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeLockConsortDetail(new ConsortRefShowInfo(_m_lCurSelectConsortId)));
        }

        #endregion
    }
}