using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品通用item
    /// </summary>
    public class GGUIWndEquipCommonItem : _ATALBasicUISubWnd<GGUIMonoEquipCommonItem>
    {
        //藏品信息
        private _IEquipCardShow _m_equipShowInfo;
        //藏品图标
        private NPGGuiWndTexture _m_wEquipIcon;
        //藏品底图
        private GGuiWndSprite _m_wEquipBg;
        //藏品卡牌底图
        private GGuiWndSprite _m_wEquipCardBg;
        //额外等级图标
        private NPGGuiWndTexture _m_wAddLevelIcon;
        //伙伴图标
        private NPGGuiWndTexture _m_wHeroIcon;
        //伙伴品质框
        private GGuiWndSprite _m_wHeroBg;
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;
        //资质key
        private string _m_sTalentKey;

        public GGUIWndEquipCommonItem(GGUIMonoEquipCommonItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wEquipIcon?.hideWnd();
            _m_wEquipBg?.hideWnd();
            _m_wEquipCardBg?.hideWnd();
            _m_wAddLevelIcon?.hideWnd();
            _m_wHeroIcon?.hideWnd();
            _m_wHeroBg?.hideWnd();
            _pushBackQualityGo();
        }

        protected override void _onReset()
        {
            _m_wEquipIcon?.discardTexture();
            _m_wEquipBg?.discardTexture();
            _m_wEquipCardBg?.discardTexture();
            _m_wAddLevelIcon?.discardTexture();
            _m_wHeroIcon?.discardTexture();
            _m_wHeroBg?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wEquipIcon?.discard();
            _m_wEquipIcon = null;
            _m_wEquipBg?.discard();
            _m_wEquipBg = null;
            _m_wEquipCardBg?.discard();
            _m_wEquipCardBg = null;
            _m_wAddLevelIcon?.discard();
            _m_wAddLevelIcon = null;
            _m_wHeroIcon?.discard();
            _m_wHeroIcon = null;
            _m_wHeroBg?.discard();
            _m_wHeroBg = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.imgIcon != null)
                _m_wEquipIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.imgBg != null)
                _m_wEquipBg = new GGuiWndSprite(wnd.imgBg);

            if (wnd.imgCardBg != null)
                _m_wEquipCardBg = new GGuiWndSprite(wnd.imgCardBg);

            if (wnd.imgAddLevelIcon != null)
                _m_wAddLevelIcon = new NPGGuiWndTexture(wnd.imgAddLevelIcon);

            if (wnd.imgHero != null)
                _m_wHeroIcon = new NPGGuiWndTexture(wnd.imgHero);

            if (wnd.imgHeroBg != null)
                _m_wHeroBg = new GGuiWndSprite(wnd.imgHeroBg);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(_IEquipCardShow _equipRef, string _talentKey = TransKeyConst.common_add_num)
        {
            _m_equipShowInfo = _equipRef;
            _m_sTalentKey = _talentKey;
            _refreshWnd();
        }

        //刷新显示
        private void _refreshWnd()
        {
            if (wnd == null || _m_equipShowInfo == null || _m_equipShowInfo.equipRef == null)
                return;

            //品质图标GO
            if (wnd.goQualityIconParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo();
            }

            //设置图标
            if (_m_wEquipIcon != null)
            {
                _m_wEquipIcon.showWnd();
                _m_wEquipIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.EQUIP, _m_equipShowInfo.equipRef.id));
            }

            //设置品质底图
            if (_m_wEquipBg != null)
            {
                _m_wEquipBg.showWnd();
                _m_wEquipBg.setTexture(GCommon.getItemQualityIcon(ENPItemType.EQUIP, _m_equipShowInfo.equipRef.id));
            }

            //设置卡牌底图
            if (_m_wEquipCardBg != null)
            {
                NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.EQUIP, _m_equipShowInfo.equipRef.id);
                _m_wEquipCardBg.showWnd();
                _m_wEquipCardBg.setTexture(qualityExtRef?.equip_card_bg);
            }

            //设置品质等级图标
            if (_m_wAddLevelIcon != null)
            {
                EQuality equipQuality = GCommon.getItemQuality(ENPItemType.EQUIP, _m_equipShowInfo.equipRef.id);
                if (equipQuality == EQuality.RED)
                {
                    _m_wAddLevelIcon.showWnd();
                    _m_wAddLevelIcon.setTexture(_m_equipShowInfo.equipRef.quality_lvl_icon);
                }
                else
                {
                    _m_wAddLevelIcon.hideWnd();
                }
            }

            //设置等级名称
            ALUGUICommon.setLabelTxt(wnd.txtLevel,
                TextTranslate.instance.getLanguage(TransKeyConst.common_level_num,
                    _m_equipShowInfo.equipInfo != null ? _m_equipShowInfo.equipInfo.level : 0));
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.EQUIP, _m_equipShowInfo.equipRef.id));

            //设置资质
            ALUGUICommon.setLabelTxt(wnd.txtTalent, TextTranslate.instance.getLanguage(_m_sTalentKey, _m_equipShowInfo.equipInfo != null ? _m_equipShowInfo.equipInfo.talentValue : 0));

            //是否需要展示佩戴的伙伴
            if (wnd.needShowHero)
            {
                //设置显隐
                bool haveHero = _m_equipShowInfo.equipInfo != null && _m_equipShowInfo.equipInfo.wearHeroId > 0;
                ALUGUICommon.setGameObjEnable(wnd.goHaveHeroShowList, haveHero);
                ALUGUICommon.setGameObjEnable(wnd.goHaveHeroHideList, !haveHero);

                //设置伙伴
                if (haveHero)
                {
                    HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_equipShowInfo.equipInfo.wearHeroId);
                    if (heroInfo != null && heroInfo.heroRefObj != null)
                    {
                        //头像
                        if (_m_wHeroIcon != null)
                        {
                            _m_wHeroIcon.showWnd();
                            _m_wHeroIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.HERO_SKIN, heroInfo.curSkinId));
                        }
                        //背景
                        if (_m_wHeroBg != null)
                        {
                            _m_wHeroBg.showWnd();
                            _m_wHeroBg.setTexture(GCommon.getQualityExtRefObj(ENPItemType.HERO, heroInfo.id)?.hero_head_bg);
                        }
                    }
                }
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goHaveHeroShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goHaveHeroHideList, true);
            }
        }

        //加载品质GO
        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityIconParent == null || _m_equipShowInfo == null || _m_equipShowInfo.equipRef == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.EQUIP, _m_equipShowInfo.equipRef.id);
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
