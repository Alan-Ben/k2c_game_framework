using System.Collections.Generic;

namespace GOE
{
    public class NPGGUICustomMonoTDPlayerEffectBtn : _AMonoOutCombatClick
    {
        public string playerEffectStr;//效果字符串
        //效果数据
        private bool _m_bIsInited = false;
        private List<NPPlayerEffectSerializeInfo> _m_lPlayerEffectList;
        public List<NPPlayerEffectSerializeInfo> playerEffectList
        {
            get
            {
                if (_m_bIsInited)
                    return _m_lPlayerEffectList;

                _init();

                return _m_lPlayerEffectList;
            }
        }
        
        //初始操作
        protected void _init()
        {
            _m_lPlayerEffectList = NPPlayerEffectSerializeInfo.readEffectList(playerEffectStr);
            _m_bIsInited = true;
        }
        
        protected override void _onClick()
        {
            NPPlayerEffectSerializeInfo.dealEffect(playerEffectList, null);
        }

        protected override void _onHolding()
        {
        }

        protected override void _onPress()
        {
        }

        protected override void _onUnPress()
        {
        }
    }
}