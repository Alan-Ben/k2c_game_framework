namespace GOE
{
    /// <summary>
    /// 通用UI特效的对象
    /// </summary>
    public class CommonUISfxObj : _BaseSfxObj<NPSfxMono>
    {
        protected override void _onInit()
        {
            
        }

        protected override void _onLoadDonePlay()
        {
            if (_m_sfxMono != null) 
                _m_sfxMono.checkUILayerMono();
        }

        protected override void _onDiscard()
        {
            
        }
    }
}