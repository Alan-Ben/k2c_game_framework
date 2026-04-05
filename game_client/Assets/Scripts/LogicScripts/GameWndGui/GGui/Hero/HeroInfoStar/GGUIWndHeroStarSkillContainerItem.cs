using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴觉醒技能列表item
    /// </summary>
    public class GGUIWndHeroStarSkillContainerItem : _ATALBasicUISubWnd<GGUIMonoHeroStarSkillContainerItem>
    {
        //觉醒技能图标
        private NPGGuiWndTexture _m_wIconWnd;
        //觉醒技能配置
        private HeroStarSkillRefObj _m_starSkillRef;
        //伙伴信息
        private HeroInfo _m_heroInfo;
        //是否需要展示下一等级
        private bool _m_bNeedShowNextLevel;
        //特效列表
        private List<CommonUISfxObj> _m_lSfxObjList;


        public GGUIWndHeroStarSkillContainerItem(GGUIMonoHeroStarSkillContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIconWnd?.hideWnd();

            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
        }

        protected override void _onReset()
        {
            _m_wIconWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIconWnd?.discard();
            _m_wIconWnd = null;

            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnInfo, _onClickInfo);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.texIcon != null)
                _m_wIconWnd = new NPGGuiWndTexture(wnd.texIcon);

            ALUGUICommon.combineBtnClick(wnd.btnInfo, _onClickInfo);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_id"></param>
        public void setInfo(HeroInfo _heroInfo, long _id, bool _needShowNextLevel)
        {
            if (wnd == null)
                return;

            _m_heroInfo = _heroInfo;
            _m_bNeedShowNextLevel = _needShowNextLevel;
            _m_starSkillRef = GRefdataCoreMgr.instance.heroStarSkillRefCore.getRef(_id);

            _refreshWnd();
        }

        /// <summary>
        /// 播放升级特效
        /// </summary>
        public void playUpgradeSfx()
        {
            //播放特效
            if (_m_lSfxObjList == null)
                _m_lSfxObjList = new List<CommonUISfxObj>();

            if (wnd != null && wnd.upgradeSfxId > 0 && wnd.upgradeSfxParent != null)
            {
                CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(wnd.upgradeSfxId, wnd.upgradeSfxParent);
                _m_lSfxObjList.Add(sfxObj);
            }
        }

        /// <summary>
        /// 设置自定义等级
        /// </summary>
        /// <param name="_level"></param>
        public void setCustomLevel(long _level)
        {
            if(wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _level));
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_starSkillRef == null)
                return;

            //设置图标
            if (_m_wIconWnd != null)
            {
                _m_wIconWnd.showWnd();
                _m_wIconWnd.setTexture(_m_starSkillRef.icon);
            }

            //等级名称
            HeroStarSkillInfo starSkillInfo = null;
            if (_m_heroInfo != null)
                starSkillInfo = _m_heroInfo.starSkillInfoMgr.getStarSkillInfo(_m_starSkillRef.skill_id);

            if (starSkillInfo != null)
            {
                //已解锁
                //设置名称
                ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, starSkillInfo.level));
                ALUGUICommon.setLabelTxt(wnd.txtNextLevel, starSkillInfo.level + 1);
                ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_starSkillRef.name));

                bool isMaxLevel = starSkillInfo.nextStarSkillLevelRefObj == null;
                //设置满级显示
                if (isMaxLevel)
                {
                    ALUGUICommon.setGameObjEnable(wnd.goMaxLevelShowList,true);
                    ALUGUICommon.setGameObjEnable(wnd.goMaxLevelHideList,false);
                    //设置描述
                    ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_starSkillRef.desc, starSkillInfo.curStarSkillLevelRefObj?.skill_desc_args));
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(wnd.goMaxLevelShowList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goMaxLevelHideList, true);
                    //设置描述
                    ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.hero_starSkillDescSplitJoint_str_str,
                        TextTranslate.instance.getLanguage(_m_starSkillRef.desc, starSkillInfo.curStarSkillLevelRefObj?.skill_desc_args),
                        TextTranslate.instance.getLanguage(TransKeyConst.hero_nextLevelAdd_num, starSkillInfo.curStarSkillLevelRefObj?.next_level_add_desc_args)));
                }
            }
            else
            {
                //未获得
                //设置名称
                ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, 0));
                ALUGUICommon.setLabelTxt(wnd.txtNextLevel, 1);
                ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_starSkillRef.name));
                //设置描述
                HeroStarSkillLevelRefObj tempSkillLevelRef = GRefdataCoreMgr.instance.getHeroStarSkillLevelRef(_m_starSkillRef.skill_id, 1);
                if (tempSkillLevelRef != null)
                    ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_starSkillRef.desc, tempSkillLevelRef.skill_desc_args));

                ALUGUICommon.setGameObjEnable(wnd.goMaxLevelShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goMaxLevelHideList, true);
            }

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goShowNextLevelShowList, _m_bNeedShowNextLevel);
            ALUGUICommon.setGameObjEnable(wnd.goShowNextLevelHideList, !_m_bNeedShowNextLevel);
        }

        //点击等级详情按钮
        private void _onClickInfo(GameObject _go)
        {
            if (_m_heroInfo == null || _m_starSkillRef == null || _go == null || wnd == null)
                return;

            QueueMgr.instance.AddNode(new GNodeHeroStarSkillLevelDetailToolTip(_m_heroInfo, _m_starSkillRef.skill_id, (RectTransform) _go.transform, wnd.detailTipIntervalX, wnd.detailTipIntervalY));
        }
    }
}