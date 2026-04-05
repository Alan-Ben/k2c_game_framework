using UnityEngine;

namespace GOE
{
    public class GTDMonoInnGuest : MonoBehaviour
    {
        [ALHeader("动画组件"),
         ALInfo("参数 state\n" +
                "=>Moving: 0 \n" +
                "=>Idle: 1 \n" +
                "=>Serving: 2 \n" +
                "\n" +
                "参数 moveDir\n" +
                "=>Vertical: 0 \n" +
                "=>Horizontal: 1 \n")]
        public Animator anim;
    }
}