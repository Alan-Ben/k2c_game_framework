using System;
using UnityEngine;

namespace GOE.MiniGame
{
    public abstract class _AMainAdditionMiniGameTDScene : _ABasicAdditionMainTDScene
    {
        protected MiniGameMainRefObj _m_rMiniGameMainRefObj;
        public sealed override bool needDiscardOnSwitch { get { return true; } }

        public void setMiniGameMainRefObj(MiniGameMainRefObj _miniGameMainRefObj)
        {
            _m_rMiniGameMainRefObj = _miniGameMainRefObj;
        }
        
        public override SceneInfoRefObj sceneRefObj
        {
            get
            {
                if (_m_rMiniGameMainRefObj == null || _m_rMiniGameMainRefObj.scene_id <= 0)
                    return null;
                
                return GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(_m_rMiniGameMainRefObj.scene_id);
            }
        }
    }
}