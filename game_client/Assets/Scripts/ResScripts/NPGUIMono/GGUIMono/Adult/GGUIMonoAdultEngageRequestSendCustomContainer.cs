using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoAdultEngageRequestSendCustomContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoAdultEngageRequestSendCustomContainerItem>
    {
        [ALHeader("列表为空时显示的GO列表")]
        public List<GameObject> goEmptyShowList;
    }
}