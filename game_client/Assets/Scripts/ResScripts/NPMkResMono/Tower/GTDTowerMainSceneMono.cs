using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class GTDTowerChapterInfo
    {
        public long chapterId;
        public Transform followTrans;
    }
    public class GTDTowerMainSceneMono : MonoBehaviour
    {
        public List<GTDTowerChapterInfo> chapterInfos;
    }
}