using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAnecdoteEarningProgressFollower : GGUIMonoAnecdoteFollower
    {
        [ALHeader("进度文本")]
        public Text txtProgress;
        [ALHeader("触发事件的按钮")]
        public GameObject btnTrigger;
    }
}