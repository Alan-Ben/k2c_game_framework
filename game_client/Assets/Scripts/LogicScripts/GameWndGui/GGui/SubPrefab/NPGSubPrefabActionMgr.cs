using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /** 子窗口对象管理器 */
    public class NPSubPrefabContainer
    {
        private bool _m_bEnable;
        private List<NPSubPrefabConditionSubItem> _m_lSubPrefabList;

        public NPSubPrefabContainer()
        {
            _m_bEnable = true;

            _m_lSubPrefabList = new List<NPSubPrefabConditionSubItem>();
        }

        //添加一个窗口，重复则不加载
        public void addSubPrefab(NPGGUISubPrefabInfo _info)
        {
            if(null == _info || null == _info.parentObj)
                return;

            //判断是否有对象
            for(int i = 0; i < _m_lSubPrefabList.Count; i++)
            {
                if(_m_lSubPrefabList[i].prefabInfo == _info)
                {
                    return;
                }
            }

            //创建对象
            NPSubPrefabConditionSubItem newItem = new NPSubPrefabConditionSubItem(_info);
            //加入队列
            _m_lSubPrefabList.Add(newItem);
            //处理加载
            newItem.load(() => { _onLoadDone(newItem); });
        }

        //删除一个指定窗口对象
        public void rmvSubPrefab(NPGGUISubPrefabInfo _info)
        {
            if(null == _info || null == _info.parentObj)
                return;

            //遍历队列处理
            for(int i = 0; i < _m_lSubPrefabList.Count; i++)
            {
                if(_m_lSubPrefabList[i].prefabInfo == _info)
                {
                    _m_lSubPrefabList[i].discard();
                    _m_lSubPrefabList.RemoveAt(i);
                    return;
                }
            }
        }

        //清空所有子窗口
        public void clearAll()
        {
            //遍历队列处理
            for(int i = 0; i < _m_lSubPrefabList.Count; i++)
            {
                _m_lSubPrefabList[i].discard();
            }
            _m_lSubPrefabList.Clear();
        }

        /** 释放所有资源 */
        public void discard()
        {
            _m_bEnable = false;
            //遍历队列处理
            for(int i = 0; i < _m_lSubPrefabList.Count; i++)
            {
                _m_lSubPrefabList[i].discard();
            }
            _m_lSubPrefabList.Clear();
        }

        //加载成功后的处理，否则显示窗口
        protected void _onLoadDone(NPSubPrefabConditionSubItem _item)
        {
            //如果对象无效则直接删除
            if(!_m_bEnable || !_m_lSubPrefabList.Contains(_item))
                _item.discard();
            else
                _item.show();
        }
    }

    /** 专用于本对象加载的加载窗口对象 */
    public class NPSubPrefabConditionSubItem : NPGSubPrefab
    {
        private NPGGUISubPrefabInfo _m_piPrefabInfo;

        public NPGGUISubPrefabInfo prefabInfo { get { return _m_piPrefabInfo; } }


        public NPSubPrefabConditionSubItem(NPGGUISubPrefabInfo _info)
            : base(_info.assetPath, _info.wndObjName, _info.parentObj)
        {
            _m_piPrefabInfo = _info;
        }

        protected override void _onDiscard()
        {
            _m_piPrefabInfo = null;
        }
    }

    /** 条件子窗口对象管理器 */
    public class NPSubPrefabConditionSubInfo : NPSubPrefabContainer
    {
        private NPGGUIMonoConditionPrefab _m_prefab;
        public NPSubPrefabConditionSubInfo(NPGGUIMonoConditionPrefab _prefab)
            : base()
        {
            _m_prefab = _prefab;
        }

        public NPGGUIMonoConditionPrefab prefab { get { return _m_prefab; } }

        //重置所有需要重新处理的对象
        public void refreshAll()
        {
            if(null == prefab)
                return;

            //添加数据
            NPGGUIConditionPrefabInfo tmp = null;
            int canLoadCount = prefab.maxLoadCount;
            for(int n = 0; n < prefab.loadInfoList.Count; n++)
            {
                tmp = prefab.loadInfoList[n];
                if(null == tmp || null == tmp.parentObj)
                    continue;

                //判断是否符合条件，是则添加
                if ((prefab.maxLoadCount <= 0 || canLoadCount > 0) && tmp.conditionGroup.IsEnable(null))
                {
                    addSubPrefab(tmp);
                    //减少加载个数
                    canLoadCount--;
                }
                else
                {
                    //移除对应加载数据
                    rmvSubPrefab(tmp);
                }
            }
        }
    }
    /** 远程效果子窗口对象管理器 */
    public class NPSubPrefabRemoteEffectSubInfo : NPSubPrefabContainer
    {
        private NPGGUIMonoRemoteEffectPrefab _m_prefab;
        public NPSubPrefabRemoteEffectSubInfo(NPGGUIMonoRemoteEffectPrefab _prefab)
            : base()
        {
            _m_prefab = _prefab;
        }
        public NPGGUIMonoRemoteEffectPrefab prefab { get { return _m_prefab; } }
    }

    /// <summary>
    /// 按钮事件管理类,这个类的作用是用于注册策划自由编辑的mono按钮事件
    /// </summary>
    public class NPGSubPrefabActionMgr : WCGSingleton<NPGSubPrefabActionMgr>
    {
        private bool _m_bInited = false;

        //是否需要检查
        private bool _m_bNeedCheck = false;

        //对应的子窗口管理对象，无效时需要删除
        private List<NPSubPrefabConditionSubInfo> _m_lConditionPrefabList = new List<NPSubPrefabConditionSubInfo>();
        private List<NPSubPrefabRemoteEffectSubInfo> _m_lRemoteEffectPrefabList = new List<NPSubPrefabRemoteEffectSubInfo>();

        public void Init()
        {
            if(_m_bInited)
                return;

            _m_bInited = true;

            //注册刷新消息
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, refreshAllConditionPrefab);
        }

        /**********
         * 刷新所有的condition prefab对象
         **/
        public void refreshAllConditionPrefab()
        {
            if(_m_bNeedCheck)
                return;

            _m_bNeedCheck = true;

            //开启下一帧处理
            ALCommonActionMonoTask.addNextFrameTask(_checkAndRefresh);
        }
        public void _checkAndRefresh()
        {
            if(!_m_bNeedCheck)
                return;

            _m_bNeedCheck = false;

            //创建资源
            NPSubPrefabConditionSubInfo info;
            for(int i = 0; i < _m_lConditionPrefabList.Count; i++)
            {
                info = _m_lConditionPrefabList[i];
                if(null == info)
                    continue;

                //处理刷新
                info.refreshAll();
            }
        }

        //对象有效的处理
        public void _onConditionPrefabActive(NPGGUIMonoConditionPrefab _prefab)
        {
            if(null == _prefab)
                return;

            //创建资源
            NPSubPrefabConditionSubInfo info = _getConditionPrefabInfo(_prefab);
            if(null != info)
                return;

            //构造数据
            info = new NPSubPrefabConditionSubInfo(_prefab);
            //加入队列
            _m_lConditionPrefabList.Add(info);

            //添加数据
            NPGGUIConditionPrefabInfo tmp = null;
            int canLoadCount = _prefab.maxLoadCount;
            for(int i = 0; i < _prefab.loadInfoList.Count; i++)
            {
                tmp = _prefab.loadInfoList[i];
                if(null == tmp || null == tmp.parentObj)
                    continue;

                //判断是否符合条件，是则添加
                if((_prefab.maxLoadCount <= 0 || canLoadCount > 0) && tmp.conditionGroup.IsEnable(null))
                {
                    info.addSubPrefab(tmp);
                    //减少加载个数
                    canLoadCount--;
                }

                //超出数量不加载
                if(_prefab.maxLoadCount > 0 && canLoadCount <= 0)
                    break;
            }
        }
        public void _onRemoteEffectPrefabActive(NPGGUIMonoRemoteEffectPrefab _prefab)
        {
            if(null == _prefab)
                return;

            //创建资源
            NPSubPrefabRemoteEffectSubInfo info = _getRemoteEffectInfo(_prefab);
            if(null != info)
                return;

            //构造数据
            info = new NPSubPrefabRemoteEffectSubInfo(_prefab);
            //加入队列
            _m_lRemoteEffectPrefabList.Add(info);

            //添加数据
            NPGGUIRemoteEffectPrefabInfo tmp = null;
            int canLoadCount = _prefab.maxLoadCount;
            for(int i = 0; i < _prefab.loadInfoList.Count; i++)
            {
                tmp = _prefab.loadInfoList[i];
                if(null == tmp || null == tmp.parentObj)
                    continue;

                //获取远程效果信息
                NPRemoteEffectRefObj refObj = GRefdataCoreMgr.instance.remoteEffectMap.getRef(tmp.remoteEffectId);
                //判断是否符合条件，是则添加
                if(refObj.condition.IsEnable(null)
                    && GCommon.isItemEnough(refObj.cost_item_list, false))
                {
                    info.addSubPrefab(tmp);
                    //减少加载个数
                    canLoadCount--;
                }

                //超出数量不加载
                if(canLoadCount <= 0)
                    break;
            }
        }

        //对象无效时的处理
        public void _onConditionPrefabDisactive(NPGGUIMonoConditionPrefab _prefab)
        {
            if(null == _prefab)
                return;

            //释放相关资源
            _discardConditionPrefabInfo(_prefab);
        }
        public void _onRemoteEffectPrefabDisactive(NPGGUIMonoRemoteEffectPrefab _prefab)
        {
            if(null == _prefab)
                return;

            //释放相关资源
            _discardRemoteEffectInfo(_prefab);
        }

        //处理效果
        public void _dealAutoPlayerEffect(NPGUIMonoAutoPlayerEffectBtn _btn)
        {
            if(null == _btn)
                return;

            int count = 0;
            NPGGUIAutoPlayerEffectInfo tmpInfo = null;
            for(int i = 0; i < _btn.autoPlayerEffectList.Count; i++)
            {
                tmpInfo = _btn.autoPlayerEffectList[i];
                if(null == tmpInfo)
                    continue;

                //判断条件
                if(tmpInfo.condition.IsEnable(null))
                {
                    count++;
                    NPPlayerEffectSerializeInfo.dealEffect(tmpInfo.playerEffectList, null);
                }

                //判断是否超过数量
                if(_btn.maxCount > 0 && count >= _btn.maxCount)
                    break;
            }
        }

        //查询对象
        protected NPSubPrefabConditionSubInfo _getConditionPrefabInfo(NPGGUIMonoConditionPrefab _prefab)
        {
            for(int i = 0; i < _m_lConditionPrefabList.Count; i++)
            {
                if(_m_lConditionPrefabList[i].prefab == _prefab)
                    return _m_lConditionPrefabList[i];
            }

            return null;
        }
        protected NPSubPrefabRemoteEffectSubInfo _getRemoteEffectInfo(NPGGUIMonoRemoteEffectPrefab _prefab)
        {
            for(int i = 0; i < _m_lRemoteEffectPrefabList.Count; i++)
            {
                if(_m_lRemoteEffectPrefabList[i].prefab == _prefab)
                    return _m_lRemoteEffectPrefabList[i];
            }

            return null;
        }

        //删除对象
        protected void _discardConditionPrefabInfo(NPGGUIMonoConditionPrefab _prefab)
        {
            for(int i = 0; i < _m_lConditionPrefabList.Count; i++)
            {
                if(_m_lConditionPrefabList[i].prefab == _prefab)
                {
                    _m_lConditionPrefabList[i].discard();
                    _m_lConditionPrefabList.RemoveAt(i);

                    return;
                }
            }
        }
        protected void _discardRemoteEffectInfo(NPGGUIMonoRemoteEffectPrefab _prefab)
        {
            for(int i = 0; i < _m_lRemoteEffectPrefabList.Count; i++)
            {
                if(_m_lRemoteEffectPrefabList[i].prefab == _prefab)
                {
                    _m_lRemoteEffectPrefabList[i].discard();
                    _m_lRemoteEffectPrefabList.RemoveAt(i);

                    return;
                }
            }
        }
    }
}
