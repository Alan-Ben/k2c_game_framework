using UnityEngine;

namespace GOE
{
    public interface _IPlayerCuteActorShowInfo
    {
        /// <summary>
        /// actor形象GoIndex
        /// </summary>
        public NPGGoIndex actorGoIndex { get; }
        
        /// <summary>
        /// actor皮肤颜色
        /// </summary>
        public Color actorSkinColor { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CommonPlayerCuteActorShowInfo : _IPlayerCuteActorShowInfo
    {
        private NPGGoIndex _m_actorGoIndex;
        private Color _m_actorSkinColor;
        
        public NPGGoIndex actorGoIndex { get { return _m_actorGoIndex; } }
        public Color actorSkinColor { get { return _m_actorSkinColor; } }

        public CommonPlayerCuteActorShowInfo(NPGGoIndex _goIndex, Color _skinColor)
        {
            update(_goIndex, _skinColor);
        }

        public void update(NPGGoIndex _goIndex, Color _skinColor)
        {
            _m_actorGoIndex = _goIndex;
            _m_actorSkinColor = _skinColor;
        }
    }
}