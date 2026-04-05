using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoChapterStoryStageContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoChapterStoryStageItem>
    {
        [ALHeader("没有一项时显示")]
        public List<GameObject> noItemShow;
    }
}