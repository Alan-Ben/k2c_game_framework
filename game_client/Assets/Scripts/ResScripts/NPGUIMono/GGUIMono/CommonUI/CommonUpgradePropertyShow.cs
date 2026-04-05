
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class CommonUpgradePropertyShow<T>
    {
        [ALHeader("当前的值")]
        public T current;
        [ALHeader("变化后的值")]
        public T next;
        [ALHeader("两个值如果相同或是不同时会展示的内容")]
        public List<GameObject> listSameShow;
        public List<GameObject> listDiffShow;
    }
}