using System;
using UnityEngine;

namespace GOE
{
    public class GTDMonoChildClassroom : MonoBehaviour
    {
        [ALHeader("视频动画 Mono ")]
        public _AVideoAniMono monoVideoAni;
        [ALHeader("上课特效相关")]
        public Transform educateSfxParent;
        public long educateSfxId;
        public int maxSfxCount = 6;
    }
}