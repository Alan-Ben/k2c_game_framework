using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using Common.DinnerEnum;
using NPEnum;
using GOE.FollowItem;

namespace GOE
{
    /// <summary>
    /// 大臣item
    /// </summary>
    public class GGUIWndChapterBossHeroInspire : _ATALBasicUISubWnd<GGUIMonoChapterBossHeroInspire>
    {
        private long _m_power;
        private GGUIWndHeroCommonCardItem _m_wHeroInfo;//骑士信息
        private Action<long> _m_onReduceHp;//执行扣血表现回调
        
        public long power
        {
            get { return _m_power; }
        }

        public GGUIWndChapterBossHeroInspire(GGUIMonoChapterBossHeroInspire _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            if (_m_wHeroInfo != null) 
                _m_wHeroInfo.showWnd();
        }

        protected override void _onReset()
        {
            _m_wHeroInfo?.resetWnd();
            _m_onReduceHp = null;
        }

        protected override void _onHideWnd()
        {
            _m_wHeroInfo?.hideWnd();
        }

        protected override void _onDiscard()
        {
            _m_wHeroInfo?.discard();
            _m_wHeroInfo = null;

            _m_onReduceHp = null;
            
            if (wnd != null) 
                wnd.onEventReduceHp -= _onAniEventReduceHp;
            
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHeroInfo != null)
                _m_wHeroInfo = new GGUIWndHeroCommonCardItem(wnd.monoHeroInfo);

            wnd.onEventReduceHp += _onAniEventReduceHp;
        }

        //执行扣血表现回调
        private void _onAniEventReduceHp()
        {
            if (_m_onReduceHp != null) 
                _m_onReduceHp(_m_power);
        }

        /// <summary>
        /// 设置tip数据
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_str"></param>
        public void setInfo(HeroInfo _heroInfo, long _power, string _bossTalk, Transform _parent)
        {
            if (wnd == null || null == _heroInfo || null == _heroInfo.heroRefObj)
                return;

            _m_power = _power;
            
            wnd.transform.parent = _parent;
            wnd.transform.localPosition = Vector3.zero;
            wnd.transform.localScale = Vector3.one;
      
            if (_m_wHeroInfo != null)
            {
                _m_wHeroInfo.setInfo(_heroInfo);
                _m_wHeroInfo.setPower(_m_power);
            }
            
            //大臣气泡
            List<long> voiceIdList = _heroInfo.heroRefObj.getCanPlayVoiceIdList(EHeroVoiceType.FIGHT);
            VoiceKeyRefObj voiceKeyRef = GRefdataCoreMgr.instance.voiceKeyRefCore.getRef(voiceIdList.GetRandomItem());
            if(null != voiceKeyRef)
                ALUGUICommon.setLabelTxt(wnd.txtHeroTalk, TextTranslate.instance.getLanguage(voiceKeyRef.voice_key));
            
            //boss气泡
            ALUGUICommon.setLabelTxt(wnd.txtBossTalk, TextTranslate.instance.getLanguage(_bossTalk));
        }
        
        public void regReduceHpCallback(Action<long> _callback)
        {
            _m_onReduceHp += _callback;
        }
        
        public void playShowAni(string _aniName)
        {
            if (wnd == null || wnd.showAni == null)
                return;
            wnd.showAni.Play(_aniName);
        }
    }
}