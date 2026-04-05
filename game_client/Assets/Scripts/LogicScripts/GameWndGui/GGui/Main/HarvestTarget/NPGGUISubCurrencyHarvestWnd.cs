
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 货币对应的一个资源收集目标对象
    /// </summary>
    public class NPGGUISubCurrencyHarvestWnd : _ANPGGUISubNumHarvestWnd
    {
        //货币对象
        private CommonEnum.ECurrency _m_eCurrency;
        private RectTransform _m_sfxParent;
        
        private long _m_lPlayerResValueBigTextSfxSerialze = 0;
        
        public NPGGUISubCurrencyHarvestWnd(CommonEnum.ECurrency _currency, Text _txtNum, RectTransform _target, RectTransform _sfxParent, string _transKey, EHarvestType _harvestResType)
            : base(_harvestResType, _txtNum, _target, _transKey)
        {
            _m_eCurrency = _currency;
            _m_sfxParent = _sfxParent;
        }
        public NPGGUISubCurrencyHarvestWnd(CommonEnum.ECurrency _currency, Text _txtNum, RectTransform _sfxParent, string _transKey, EHarvestType _harvestResType)
            : base(_harvestResType, _txtNum, _transKey)
        {
            _m_eCurrency = _currency;
            _m_sfxParent = _sfxParent;
        }

        protected override void _onInit()
        {
            base._onInit();

 
            // 注册玩家资源更新事件
            NPPlayer.instance.rescourceComp.onResourceCountChg += _onPlayerResChged;
            
            WinMsg.RegisterMsg(WinMsgType.SHOW_PLAYER_RES_VALUE_BIG_TEXT_SFX, _showPlayerResValueBigTextSfx);
        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            WinMsg.UnregisterMsg(WinMsgType.SHOW_PLAYER_RES_VALUE_BIG_TEXT_SFX, _showPlayerResValueBigTextSfx);
            _m_lPlayerResValueBigTextSfxSerialze = ALSerializeOpMgr.next();
            _m_sfxParent = null;

            
            // 注销事件
            NPPlayer.instance.rescourceComp.onResourceCountChg -= _onPlayerResChged;
        }

        // 响应玩家资源更新事件
        private void _onPlayerResChged(CommonEnum.ECurrency _currencyType, long _oldCount, long _newCount)
        {
            //判断类型，一致的情况下刷新显示
            if (_m_eCurrency == _currencyType)
            {
                updateNum(_newCount);
            }
        }
        
        protected override long _getRealCount()
        {
            return NPPlayer.instance.rescourceComp.getShowValue(_m_eCurrency);
        }

        /// <summary>
        /// 显示玩家资源值大文本特效
        /// </summary>
        /// <param name="_objs"></param>
        private void _showPlayerResValueBigTextSfx(params object[] _objs)
        {
            if(_objs == null || _objs.Length <= 0 || !(_objs[0] is CommonEnum.ECurrency _currency) || _m_eCurrency != _currency)
                return;

            
            long serialze = _m_lPlayerResValueBigTextSfxSerialze = ALSerializeOpMgr.next();
            
        }
    }
}