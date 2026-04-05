using ALPackage;
using Common.HeroObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴藏品入口按钮item
    /// </summary>
    public class GGUIWndHeroEquipBtnItem : _ATALBasicUISubWnd<GGUIMonoHeroEquipBtnItem>
    {
        //藏品信息
        private EquipInfo _m_equipInfo;
        //伙伴信息
        private HeroInfo _m_heroInfo;
        //图标
        private NPGGuiWndTexture _m_wIcon;
        //品质框
        private GGuiWndSprite _m_wQualityBg;
        //额外等级图标
        private NPGGuiWndTexture _m_wAdditionLevelIcon;
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;

        public GGUIWndHeroEquipBtnItem(GGUIMonoHeroEquipBtnItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
            _m_wQualityBg?.hideWnd();
            _m_wAdditionLevelIcon?.hideWnd();
            _pushBackQualityGo();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            _m_wQualityBg?.discardTexture();
            _m_wAdditionLevelIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
            _m_wQualityBg?.discard();
            _m_wQualityBg = null;
            _m_wAdditionLevelIcon?.discard();
            _m_wAdditionLevelIcon = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.imgQualityBg != null)
                _m_wQualityBg = new GGuiWndSprite(wnd.imgQualityBg);

            if (wnd.imgEquipAdditionLevelIcon != null)
                _m_wAdditionLevelIcon = new NPGGuiWndTexture(wnd.imgEquipAdditionLevelIcon);
        }

        /// <summary>
        /// 设置藏品信息
        /// </summary>
        /// <param name="_equipInfo"></param>
        public void setInfo(EquipInfo _equipInfo, HeroInfo _heroInfo)
        {
            if (wnd == null)
                return;

            _m_equipInfo = _equipInfo;
            _m_heroInfo = _heroInfo;

            //刷新红点
            _refreshRedTip();

            if (_equipInfo == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goWearEquipHideList, true);
                ALUGUICommon.setGameObjEnable(wnd.goWearEquipShowList, false);
                return;
            }


            ALUGUICommon.setGameObjEnable(wnd.goWearEquipHideList, false);
            ALUGUICommon.setGameObjEnable(wnd.goWearEquipShowList, true);

            //品质图标GO
            if (wnd.goQualityIconParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo();
            }

            //设置图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.EQUIP, _m_equipInfo.equipRef.id));
            }

            //设置品质底图
            if (_m_wQualityBg != null)
            {
                _m_wQualityBg.showWnd();
                _m_wQualityBg.setTexture(GCommon.getItemQualitySpIcon(ENPItemType.EQUIP, _m_equipInfo.equipRef.id));
            }

            //设置品质等级图标
            if (_m_wAdditionLevelIcon != null)
            {
                EQuality equipQuality = GCommon.getItemQuality(ENPItemType.EQUIP, _m_equipInfo.equipRef.id);
                if (equipQuality == EQuality.RED)
                {
                    _m_wAdditionLevelIcon.showWnd();
                    _m_wAdditionLevelIcon.setTexture(_m_equipInfo.equipRef.quality_lvl_icon);
                }
                else
                {
                    _m_wAdditionLevelIcon.hideWnd();
                }
            }

            //设置等级名称
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_equipInfo.level));
        }

        //刷新红点
        private void _refreshRedTip()
        {
            if (wnd == null || _m_heroInfo == null)
                return;

            EquipInfo equipInfo = NPPlayer.instance.equipComp.getEquipInfoByHeroId(_m_heroInfo.id);
            bool haveNoWearEquip = NPPlayer.instance.equipComp.haveNoWearEquip();
            //是否可穿戴藏品
            bool canWearEquip = equipInfo == null && haveNoWearEquip;
            //是否有更高品质藏品可替换
            bool haveBetterEquip = equipInfo != null && NPPlayer.instance.equipComp.haveHigherQualityNoWear(GCommon.getItemQuality(ENPItemType.EQUIP, equipInfo.equipId));

            ALUGUICommon.setGameObjEnable(wnd.goRedTip, canWearEquip || haveBetterEquip);
        }

        //加载品质GO
        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityIconParent == null || _m_equipInfo == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.EQUIP, _m_equipInfo.equipRef.id);
            if (qualityExtRef != null && qualityExtRef.quality_go_index != null)
            {
                _m_qualityGoIndex = qualityExtRef.quality_go_index;
                GGoIndexCacheMgr.instance.popItem(_m_qualityGoIndex, _go =>
                {
                    if (_go == null || wnd == null || wnd.goQualityIconParent == null)
                        return;

                    _go.transform.SetParent(wnd.goQualityIconParent);
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
    }
}
