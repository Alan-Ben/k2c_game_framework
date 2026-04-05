using ALPackage;
using UnityEngine;

namespace GOE
{
    
    /// <summary>
    /// 宴会内的席位的玩家显示
    /// </summary>
    public class GTDDinnerMainPlayerView
    {
        private readonly Transform _m_loadParent;
        private GDinnerActorView _m_actorView;
        public GTDDinnerMainPlayerView(Transform _loadParent)
        {
            _m_loadParent = _loadParent;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void discard()
        {
            _m_actorView?.discard();
            _m_actorView = null;
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="_cuteActorShowInfo"></param>
        public void show(NPGGoIndex _goIndex)
        {
            discard();
            _m_actorView = new GDinnerActorView(_goIndex, _m_loadParent);
            _m_actorView.load();
        }
    }
}