
using ALPackage;
using UnityEngine;

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 适用于根据一个枚举显示不同的 GameObject 的展示
    /// </summary>
    [Serializable]
    public class MultiStateShow<T> 
        where T : Enum 
    {
        private readonly int _m_animatorStateTypeHash = Animator.StringToHash("stateType");
        
        /// <summary>
        /// 内部的显示类
        /// </summary>
        [Serializable]
        public class StateShowData
        {
            [NotNull] public T type;
            [NotNull] public List<GameObject> showList;
        }

        [ALInfo("这个 Animator 会根据当前的显示状态，设置 int 型 parameter ”stateType“ 为对应显示状态的枚举值")]
        [ALHeader("不同状态下切换的状态机")]
        public Animator animator;
        // 不同状态下的显示列表
        [NotNull][ItemNotNull] public List<StateShowData> stateShowDataList;

        // 记录 setShowData 方法中筛选出的 index 的列表
        [NotNull] private List<int> _m_checkIndexList = new List<int>(1);
        
        /// <summary>
        /// 根据传入的枚举，显示对应的内容
        /// </summary>
        public void setShowData(T _type)
        {
            // todo: 优化一下 T 到 object 的拆装箱问题
            if (animator != null)
                animator.SetInteger(_m_animatorStateTypeHash, Convert.ToInt32(_type));
            
            _m_checkIndexList.Clear();
            for (int i = 0; i < stateShowDataList.Count; i++)
            {
                StateShowData stateShowData = stateShowDataList[i];
                if (stateShowData.type.Equals(_type))
                    _m_checkIndexList.Add(i);
                else
                    ALUGUICommon.setGameObjEnable(stateShowData.showList, false);
            }

            foreach (int showIndex in _m_checkIndexList)
            {
                ALUGUICommon.setGameObjEnable(stateShowDataList[showIndex].showList, true);
            }
        }
    }
    /// <summary>
    /// 适用于根据一个枚举显示不同的 GameObject 的展示
    /// </summary>
    [Serializable]
    public class MultiStateShow<T, Y> 
        where T : Enum 
        where Y : Enum
    {
        private readonly int _m_animatorStateTypeAHash = Animator.StringToHash("stateTypeA");
        private readonly int _m_animatorStateTypeBHash = Animator.StringToHash("stateTypeB");
        
        /// <summary>
        /// 内部的显示类
        /// </summary>
        [Serializable]
        public class StateShowData
        {
            [NotNull] public T typeA;
            [NotNull] public Y typeB;
            [NotNull] public List<GameObject> showList;
        }
        
        [ALInfo("这个 Animator 会根据当前的显示状态，设置 int 型 parameter ”stateType“ 为对应显示状态的枚举值")]
        [ALHeader("不同状态下切换的状态机")]
        public Animator animator;
        // 不同状态下的显示列表
        [NotNull][ItemNotNull] public List<StateShowData> stateShowDataList;

        // 记录 setShowData 方法中筛选出的 index 的列表
        [NotNull] private List<int> _m_checkIndexList = new List<int>(1);
        
        /// <summary>
        /// 根据传入的枚举，显示对应的内容
        /// </summary>
        public void setShowData(T _typeA, Y _typeB)
        {
            // todo: 优化一下 T Y 到 object 的拆装箱问题
            if (animator != null)
            {
                animator.SetInteger(_m_animatorStateTypeAHash, Convert.ToInt32(_typeA));
                animator.SetInteger(_m_animatorStateTypeBHash, Convert.ToInt32(_typeB));
            }
            
            _m_checkIndexList.Clear();
            for (int i = 0; i < stateShowDataList.Count; i++)
            {
                StateShowData stateShowData = stateShowDataList[i];
                if (stateShowData.typeA.Equals(_typeA) && stateShowData.typeB.Equals(_typeB))
                    _m_checkIndexList.Add(i);
                else
                    ALUGUICommon.setGameObjEnable(stateShowData.showList, false);
            }

            foreach (int showIndex in _m_checkIndexList)
            {
                ALUGUICommon.setGameObjEnable(stateShowDataList[showIndex].showList, true);
            }
        }
    }
}