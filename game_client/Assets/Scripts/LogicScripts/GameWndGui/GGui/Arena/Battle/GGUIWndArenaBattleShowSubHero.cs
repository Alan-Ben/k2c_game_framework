using System;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗伙伴展示信息附加窗口
    /// </summary>
    public class GGUIWndArenaBattleShowSubHero : _ATALBasicUISubWnd<GGUIMonoArenaBattleShowSubHero>
    {
        //伙伴配置
        private HeroRefObj _m_heroRef;
        //伙伴皮肤配置
        private HeroSkinRefObj _m_heroSkinRef;
        //伙伴等级
        private long _m_lLevel;
        //总的实力
        private long _m_lTotalPower;
        //当前实力
        private long _m_lCurPower;
        //上个实力
        private long _m_lLastPower;
        //伙伴头像
        private NPGGuiWndTexture _m_wHeroIcon;
        //伙伴头像背景
        private GGuiWndSprite _m_wHeroIconBg;
        //伙伴形象
        private NPGGuiWndTexture _m_wHeroTexture;
        //血量进度条
        private GGUISubWndCommonBlood _m_wHpProgress;
        //显示序列号
        private long _m_lShowSerialize;

        public GGUIWndArenaBattleShowSubHero(GGUIMonoArenaBattleShowSubHero _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wHeroIcon?.hideWnd();
            _m_wHeroIconBg?.hideWnd();
            _m_wHeroTexture?.hideWnd();
            _m_wHpProgress?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wHeroIcon?.discardTexture();
            _m_wHeroIconBg?.discardTexture();
            _m_wHeroTexture?.discardTexture();
            _m_wHpProgress?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wHeroIcon?.discard();
            _m_wHeroIcon = null;
            _m_wHeroIconBg?.discard();
            _m_wHeroIconBg = null;
            _m_wHeroTexture?.discard();
            _m_wHeroTexture = null;
            _m_wHpProgress?.discard();
            _m_wHpProgress = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgHeroIcon != null)
                _m_wHeroIcon = new NPGGuiWndTexture(wnd.imgHeroIcon);
            if (wnd.imgHeroIconBg != null)
                _m_wHeroIconBg = new GGuiWndSprite(wnd.imgHeroIconBg);
            if (wnd.imgHero != null)
                _m_wHeroTexture = new NPGGuiWndTexture(wnd.imgHero);
            if(wnd.monoHP != null)
                _m_wHpProgress = new GGUISubWndCommonBlood(wnd.monoHP);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(HeroRefObj _heroRef, HeroSkinRefObj _heroSkinRef, long _level, long _totalPower, long _curPower, long _lastPower)
        {
            _m_heroRef = _heroRef;
            _m_heroSkinRef = _heroSkinRef;
            _m_lLevel = _level;
            _m_lTotalPower = _totalPower;
            _m_lCurPower = _curPower;
            _m_lLastPower = _lastPower;
            _refreshWnd();
        }

        //刷新显示
        private void _refreshWnd()
        {
            _refreshBaseInfo();
            _refreshBloodInfo();
        }

        //刷新基础信息
        private void _refreshBaseInfo()
        {
            if (wnd == null || _m_heroRef == null || _m_heroSkinRef == null)
                return;

            _m_wHeroTexture?.showWnd();
            _m_wHeroTexture?.setTexture(_m_heroSkinRef.card_image);

            _m_wHeroIcon?.showWnd();
            _m_wHeroIcon?.setTexture(GCommon.getItemTexIcon(ENPItemType.HERO_SKIN, _m_heroSkinRef.id));

            _m_wHeroIconBg?.showWnd();
            _m_wHeroIconBg?.setTexture(GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_heroRef.id)?.hero_head_bg);

            ALUGUICommon.setLabelTxt(wnd.txtHeroName, _m_heroRef.transName);
            ALUGUICommon.setLabelTxt(wnd.txtHeroLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_lLevel));
        }

        //刷新血量信息
        private void _refreshBloodInfo()
        {
            if (wnd == null)
                return;

            _m_wHpProgress?.showWnd();
            _m_wHpProgress?.setBloodData(_m_lLastPower, _m_lTotalPower);

            if(_m_lCurPower < 0)
                _m_lCurPower  = 0;

            //总的伤害量
            long totalDamage = _m_lLastPower - _m_lCurPower;
            if (totalDamage > _m_lTotalPower)
                totalDamage = _m_lTotalPower;

            //如果有配置的血量变化参数，则使用配置的参数
            if (wnd.bloodDelayChangeParmList != null && wnd.bloodDelayChangeParmList.Count > 0)
            {
                long serialize = _m_lShowSerialize;
                long leftDamage = totalDamage;
                for (int i = 0; i < wnd.bloodDelayChangeParmList.Count; i++)
                {
                    GGUIArenaBattleBloodDelayChangeParam bloodDelayChangeParam = wnd.bloodDelayChangeParmList[i];
                    if (bloodDelayChangeParam == null)
                        continue;

                    //延时时间
                    float delayTime = bloodDelayChangeParam.delayTimeSec;
                    //本轮伤害量
                    long thisRoundDamage = (long)Math.Floor(totalDamage * bloodDelayChangeParam.changePercentage / 100f);

                    //如果本轮伤害比剩余血量还大或者是最后一轮，则本轮伤害等于剩余血量
                    if (thisRoundDamage > leftDamage || i == wnd.bloodDelayChangeParmList.Count - 1)
                        thisRoundDamage = leftDamage;
                    leftDamage -= thisRoundDamage;

                    //延时展示
                    ALCommonActionMonoTask.addMonoTask(() =>
                    {
                        if (serialize != _m_lShowSerialize)
                            return;

                        _m_wHpProgress?.damage(thisRoundDamage);
                    }, delayTime);

                    //如果剩余血量小于等于0，则退出
                    if (leftDamage <= 0)
                        break;
                }
            }
            else
                _m_wHpProgress?.damage(totalDamage);
        }
    }
}
