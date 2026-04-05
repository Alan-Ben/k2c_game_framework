using System;

namespace GOE
{
    public interface _IMarsIntelligentControlEffectPlayer
    {
        /// <summary>
        /// 进行智能控制效果播放
        /// </summary>
        /// <param name="_intelligentControlId"></param>
        /// <param name="_complete"></param>
        void playIntelligentControlEffect(long _intelligentControlId, Action _complete = null);
    }
}