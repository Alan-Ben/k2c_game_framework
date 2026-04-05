using UnityEngine;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 伙伴皮肤衣柜页签
    /// </summary>
    public class GGUIWndHeroSkinClosetPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoHeroSkinClosetPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //皮肤配置数据
        private HeroSkinRefObj _m_skinRef;
        //解锁道具
        private NPGGUIWndCommonItem _m_wUnlockItem;

        public GGUIWndHeroSkinClosetPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
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
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_SKIN_CHG, _onSkinChg);
            WinMsg.RegisterMsg(WinMsgType.ON_HERO_CUR_SKIN_ID_CHG, _onSkinChg);
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_SKIN_CHG, _onSkinChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_HERO_CUR_SKIN_ID_CHG, _onSkinChg);
            _m_wUnlockItem?.hideWnd();
        }
        
        protected override void _onReset()
        {
            _m_wUnlockItem?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            _m_wUnlockItem?.discard();
            _m_wUnlockItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnWear, _onClickWear);
            ALUGUICommon.uncombineBtnClick(wnd.btnUnlock, _onClickUnlock);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoUnlockItem != null)
                _m_wUnlockItem = new NPGGUIWndCommonItem(wnd.monoUnlockItem);

            ALUGUICommon.combineBtnClick(wnd.btnWear, _onClickWear);
            ALUGUICommon.combineBtnClick(wnd.btnUnlock, _onClickUnlock);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_heroId"></param>
        public void setInfo(HeroSkinRefObj _skinRef)
        {
            _m_skinRef = _skinRef;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_skinRef == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.HERO_SKIN, _m_skinRef.id));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, GCommon.getItemDesc(ENPItemType.HERO_SKIN, _m_skinRef.id));

            if (_m_wUnlockItem != null)
            {
                _m_wUnlockItem.showWnd();
                _m_wUnlockItem.setItem(_m_skinRef.unlock_item);
            }

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_skinRef.hero_id);
            bool isCurWear = heroInfo != null && heroInfo.curSkinId == _m_skinRef.id;
            bool isUnlock = heroInfo != null && heroInfo.heroRefObj != null &&
                            (heroInfo.heroRefObj.default_skin_id == _m_skinRef.id ||
                             heroInfo.heroSkinInfoMgr.getSkinInfo(_m_skinRef.id) != null);

            if(isCurWear)
                GGameCommonInfo.grayImage(wnd.goWearGrayList);
            else
                GGameCommonInfo.disgrayImage(wnd.goWearGrayList);

            ALUGUICommon.setGameObjEnable(wnd.goUnlockShowList, isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goUnlockHideList, !isUnlock);
        }

        //点击穿戴按钮
        private void _onClickWear(GameObject _go)
        {
            if (_m_skinRef == null)
                return;

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_skinRef.hero_id);
            if (heroInfo == null || 
                heroInfo.curSkinId == _m_skinRef.id || //已经是当前穿戴不处理
                heroInfo.heroRefObj == null ||
                (heroInfo.heroRefObj.default_skin_id != _m_skinRef.id && heroInfo.heroSkinInfoMgr.getSkinInfo(_m_skinRef.id) == null))//不是默认的未解锁不处理
                return;

            NPPlayer.instance.heroComponent.reqHeroSetSkin(_m_skinRef.id);
        }

        //点击解锁按钮
        private void _onClickUnlock(GameObject _go)
        {
            if (_m_skinRef == null)
                return;

            if(!GCommon.isItemEnough(_m_skinRef.unlock_item,true))
                return;

            NPPlayer.instance.heroComponent.reqHeroSkinUnlock(_m_skinRef.id);
        }

        //皮肤数据更新
        private void _onSkinChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            HeroInfo heroInfo = _objects[0] as HeroInfo;

            if (heroInfo == null || _m_skinRef == null || _m_skinRef.hero_id != heroInfo.id)
                return;

            _refreshWnd();
        }
    }
}