using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    //showcase场景根节点mono
    public class NPShowcaseSceneMono : MonoBehaviour
    {
        [ALHeader("Showcase根节点列表，长度代表可以同时展示的上限")]
        public List<Transform> showcaseRootList;
    }
}