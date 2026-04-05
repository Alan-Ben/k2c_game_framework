using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GSfxMovePathMonoAutoMgr:MonoBehaviour
    {
        public List<GSfxMovePathMono> monoList;
        
        #if NP_GAME

        private void OnEnable()
        {
            foreach (GSfxMovePathMono pathMono in monoList)
            {
                if(null == pathMono)
                    continue;
                pathMono.startMove();
            }
        }

#endif
    }
}