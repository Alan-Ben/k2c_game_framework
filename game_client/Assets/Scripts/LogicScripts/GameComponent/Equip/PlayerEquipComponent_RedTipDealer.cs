// using JetBrains.Annotations;
// using System.Collections.Generic;
//
// namespace GOE
// {
//     /// <summary>
//     /// 藏品组件-红点管理器
//     /// </summary>
//     public partial class PlayerEquipComponent
//     {
//         private class RedTipDealer
//         {
//             //藏品组件
//             private PlayerEquipComponent _m_equipComponent;
//             //红点字典
//             [NotNull] private Dictionary<string, CommonForceRedTipNode> _m_dMyNode = new Dictionary<string, CommonForceRedTipNode>(); // 为了根据自定义的索引取到对应节点的字典
//
//             public RedTipDealer(PlayerEquipComponent _comp)
//             {
//                 _m_equipComponent = _comp;
//             }
//
//             #region 初始化红点
//
//             /// <summary>
//             /// 初始化顾问红点管理器
//             /// </summary>
//             public void init()
//             {
//                 if (_m_equipComponent == null)
//                     return;
//
//                 clear();
//
//                 foreach (EquipInfo equipInfo in _m_equipComponent._m_lEquipInfoList)
//                 {
//                     if (equipInfo == null)
//                         continue;
//
//                     _addNode(RedTipConst.RED_EQUIP_UPGRADE, equipInfo.dbId);//可升级红点
//                 }
//
//                 //刷新一下红点
//                 refreshUpgradeRedTip();
//             }
//
//             //添加红点
//             private void _addNode(long _parentId, long _instanceId)
//             {
//                 string nodeKey = _createRedKey(_parentId, _instanceId);
//                 if (_m_dMyNode.ContainsKey(nodeKey))
//                     return;
//
//                 CommonForceRedTipNode node = new CommonForceRedTipNode(nodeKey);
//                 _m_dMyNode[nodeKey] = node;
//                 RedTipMgr.instance.addRedTipNodeWithParent(node, _parentId);
//             }
//
//             #endregion
//
//             /// <summary>
//             /// 刷新全部升级红点
//             /// </summary>
//             public void refreshUpgradeRedTip()
//             {
//                 foreach (EquipInfo equipInfo in _m_equipComponent._m_lEquipInfoList)
//                 {
//                     if (equipInfo == null)
//                         continue;
//
//                     //刷新升级红点
//                     refreshUpgradeRedTip(equipInfo);
//                 }
//             }
//
//             /// <summary>
//             /// 刷新升级红点
//             /// </summary>
//             public void refreshUpgradeRedTip(EquipInfo _equipInfo)
//             {
//                 if (_equipInfo == null)
//                     return;
//
//                 //是否有顾问穿戴该藏品
//                 bool haveWearHero = _equipInfo.wearHeroId > 0;
//                 //今天是否已读过该红点
//                 bool todayIsRead = AccountSettingMgr.instance.accountSetting.isEquipUpgradeRedTipReadToday(_equipInfo.dbId);
//                 //是否可以升级
//                 bool canUpgrade = _equipInfo.level != _equipInfo.levelLimit && _equipInfo.equipRef != null && GCommon.isItemEnough(_equipInfo.equipRef.cost_item, false);
//
//                 string nodeKey = _createRedKey(RedTipConst.RED_EQUIP_UPGRADE, _equipInfo.dbId);
//                 if (_m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node))
//                     _node?.setCount(haveWearHero && !todayIsRead && canUpgrade ? 1 : 0);
//             }
//
//             /// <summary>
//             /// 是否需要展示红点
//             /// </summary>
//             /// <param name="_redTipId"></param>
//             /// <param name="_instanceId"></param>
//             /// <returns></returns>
//             public bool needShowRedTip(long _redTipId, long _instanceId)
//             {
//                 //判断是否是已解锁红点
//                 RedMonitorRefObj redMonitorRef = GRefdataCoreMgr.instance.redMonitorRefCore.getRef(_redTipId);
//                 if (redMonitorRef != null && !GCommon.isSimpleUnlock(redMonitorRef.simple_unlock_id))
//                     return false;
//
//                 string nodeKey = _createRedKey(_redTipId, _instanceId);
//                 _m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node);
//                 return _node != null && _node.needShow();
//             }
//
//             /// <summary>
//             /// 设置红点已读
//             /// </summary>
//             /// <param name="_redTipId"></param>
//             /// <param name="_instanceId"></param>
//             public void setReadRedTip(long _redTipId, long _instanceId)
//             {
//                 string nodeKey = _createRedKey(_redTipId, _instanceId);
//                 _m_dMyNode.TryGetValue(nodeKey, out CommonForceRedTipNode _node);
//                 _node?.setCount(0);
//             }
//
//             /// <summary>
//             /// 清除数据
//             /// </summary>
//             public void clear()
//             {
//                 _m_dMyNode.Clear();
//             }
//
//             //创建唯一key
//             [NotNull]
//             private string _createRedKey(long _key, long _instanceId)
//             {
//                 return $"{_key}_{_instanceId}";
//             }
//         }
//     }
// }
