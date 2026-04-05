using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GTDMonoMarsExploreScene : MonoBehaviour
    {
        [ALHeader("单位的父节点")]
        public Transform unitRoot;
        [ALHeader("主基地")]
        public GTDMonoMarsExploreHomeBase monoHomeBase;
        [ALHeader("探索事件点列表")]
        public List<GTDMonoMarsExploreEventPos> eventPosList;
        [ALHeader("事件出现的特效 id ")]
        public long eventShowSfxId;
    }
}