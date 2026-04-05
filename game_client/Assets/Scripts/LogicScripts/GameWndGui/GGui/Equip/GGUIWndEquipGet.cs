using System;
using ALPackage;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获得藏品弹窗
    /// </summary>
    public class GGUIWndEquipGet : _ATALBasicUIWnd<GGUIMonoEquipGet>
    {
        private static GGUIWndEquipGet _g_instance;
        public static GGUIWndEquipGet instance
        {
            get
            {
                if (null == _g_instance)
                {
                    _g_instance = new GGUIWndEquipGet();
                }
                return _g_instance;
            }
        }

        //藏品信息
        private NPCommon_ItemInfo _m_itemInfo;
        //图标
        private NPGGuiWndTexture _m_wIcon;
        //等级附加图标
        private NPGGuiWndTexture _m_wAdditionQualityIcon;
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;
        //关闭回调
        private Action _m_onClose;
        
        public GGUIWndEquipGet() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoEquipGet.assetPath; }
        protected override string _monoObjName { get => GGUIMonoEquipGet.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
            _m_wAdditionQualityIcon?.hideWnd();

            _pushBackQualityGo();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            _m_wAdditionQualityIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
            _m_wAdditionQualityIcon?.discard();
            _m_wAdditionQualityIcon = null;

            if (_m_onClose != null)
            {
                Action onClose = _m_onClose;
                _m_onClose = null;
                onClose?.Invoke();
            }
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.imgAdditionQualityIcon != null)
                _m_wAdditionQualityIcon = new NPGGuiWndTexture(wnd.imgAdditionQualityIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_itemInfo"></param>
        public void setInfo(NPCommon_ItemInfo _itemInfo, Action _onClose)
        {
            _m_itemInfo = _itemInfo;
            _m_onClose = _onClose;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_itemInfo == null)
                return;

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.EQUIP, _m_itemInfo.getSubId()));

            //设置图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.EQUIP, _m_itemInfo.getSubId()));
            }

            //品质图标GO
            if (wnd.goQualityParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo();
            }

            //设置品质颜色
            EQuality equipQuality = GCommon.getItemQuality(ENPItemType.EQUIP, _m_itemInfo.getSubId());
            NPQualityRefObj qualityRef = GRefdataCoreMgr.instance.getQuality(ENPQualityClass.EQUIP, equipQuality);
            if (qualityRef != null)
                ALUGUICommon.setUIObjColor(wnd.setColorByQualityImage, qualityRef.color);

            //设置等级附加图标
            EquipRefObj equipRef = GRefdataCoreMgr.instance.equipRefCore.getRef(_m_itemInfo.getSubId());
            EQuality curQuality = GCommon.getItemQuality(ENPItemType.EQUIP, _m_itemInfo.getSubId());
            if (_m_wAdditionQualityIcon != null && equipRef != null)
            {
                if (curQuality == EQuality.RED)
                {
                    _m_wAdditionQualityIcon.showWnd();
                    _m_wAdditionQualityIcon.setTexture(equipRef.quality_lvl_icon);
                }
                else
                    _m_wAdditionQualityIcon.hideWnd();
            }
        }

        //加载品质GO
        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityParent == null || _m_itemInfo == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.EQUIP, _m_itemInfo.getSubId());
            if (qualityExtRef != null && qualityExtRef.quality_go_index != null)
            {
                _m_qualityGoIndex = qualityExtRef.quality_go_index;
                GGoIndexCacheMgr.instance.popItem(_m_qualityGoIndex, _go =>
                {
                    if (_go == null || wnd == null || wnd.goQualityParent == null)
                        return;

                    _go.transform.SetParent(wnd.goQualityParent);
                    _go.transform.localPosition = Vector3.zero;
                    _go.transform.localScale = Vector3.one;
                    _m_qualityGo = _go;
                });
            }
        }

        //回收品质GO
        private void _pushBackQualityGo()
        {
            if (_m_qualityGoIndex != null && _m_qualityGo != null)
                GGoIndexCacheMgr.instance.pushbackItem(_m_qualityGoIndex, _m_qualityGo);
            _m_qualityGoIndex = null;
            _m_qualityGo = null;
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EQUIP_GET);
        }
    }
}