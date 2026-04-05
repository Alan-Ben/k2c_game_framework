using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 资源建筑的收集进度表现接口
    /// </summary>
    public abstract class _AMonoProcessShow : MonoBehaviour
    {
        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="_value"></param>
        public abstract void setProcess(float _value);
        
        /// <summary>
        /// 重置
        /// </summary>
        public abstract void reset();
    }
}