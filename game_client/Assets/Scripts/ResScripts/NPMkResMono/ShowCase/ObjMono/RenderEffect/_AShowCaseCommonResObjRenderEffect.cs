using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase resObj通用渲染表现接口
    /// </summary>
    public abstract class _AShowCaseCommonResObjRenderEffect : MonoBehaviour
    {
        //spine的ZSpacing
        public virtual void setZSpacing(float _value) {}
        //设置透明度
        public abstract void setAlpha(float _alpha);
    }
}