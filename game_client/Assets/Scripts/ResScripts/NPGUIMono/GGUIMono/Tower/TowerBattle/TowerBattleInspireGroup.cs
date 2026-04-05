using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TowerBattleInspire
{
    [ALHeader("父节点")]
    public Transform parent;
    [ALHeader("是否用左预制体")]
    public bool isLeft;
}
public class TowerBattleInspireGroup : MonoBehaviour
{
    public List<TowerBattleInspire> inspireParentList = new List<TowerBattleInspire>();
    public Animation showAnim;
    public string showAnimName;
    public string showAnim10xName;
    public Action onEventReduceHp;

    /// <summary>
    /// 执行扣血表现
    /// </summary>
    public void ReduceHP()
    {
        onEventReduceHp?.Invoke();
    }

}