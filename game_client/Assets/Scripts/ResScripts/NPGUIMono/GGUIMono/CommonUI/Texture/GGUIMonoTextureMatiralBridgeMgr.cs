using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoTextureMatiralBridgeMgr:MonoBehaviour
    {
        public List<GGUIMonoTextureMatiralBridge> materialBridgeList;

        void Update()
        {
            foreach (GGUIMonoTextureMatiralBridge bridge in materialBridgeList)
            {
                if(null == bridge)
                    continue;
                bridge.setMaterialValue();
            }
        }
    }
}