using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoGraphicMatiralBridgeMgr:MonoBehaviour
    {
        public List<GGUIMonoGraphicMatiralBridge> materialBridgeList;

        void Update()
        {
            foreach (GGUIMonoGraphicMatiralBridge bridge in materialBridgeList)
            {
                if(null == bridge)
                    continue;
                bridge.setMaterialValue();
            }
        }
    }
}