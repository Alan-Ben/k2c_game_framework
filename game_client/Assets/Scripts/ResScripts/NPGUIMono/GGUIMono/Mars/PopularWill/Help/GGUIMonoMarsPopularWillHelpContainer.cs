using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsPopularWillHelpContainer : _TALUGUIMonoContainerWnd<GGUIMonoMarsPopularWillHelpItem>
    {
        [ALHeader("没有求助时显示")]
        public List<GameObject> noItemShow;
    }
}