
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取唯一存在的 Object 的获取器
    /// </summary>
    /// <typeparam name="T_KEY">用于获取时索引的 key </typeparam>
    /// <typeparam name="T_OBJECT"> Object 的类型</typeparam>
    public abstract class _AUniqueObjectSharedGetter<T_KEY, T_OBJECT>
    {
        private static uint _g_handle = 0;
        private static uint _getNextHandle()
        {
            _g_handle += 1;
            if (_g_handle == 0)
                _g_handle += 1;
            
            return _g_handle++;
        }
        
        [NotNull] private readonly Dictionary<T_KEY, ObjectData> _m_keyToObjectData;
        [NotNull] private readonly Dictionary<uint, ObjectData> _m_handleToObjectData;

        protected _AUniqueObjectSharedGetter()
        {
            _m_keyToObjectData = new Dictionary<T_KEY, ObjectData>();
            _m_handleToObjectData = new Dictionary<uint, ObjectData>();
        }
        
        /// <summary>
        /// 获取对应的 Object
        /// </summary>
        /// <remarks>
        /// 注意这个 _complete 在获取失败（ handle 为 0 ）时不会调用，在你不再需要这个 Object 时（还没 complete 你就调用了 release）也不会被调用
        /// </remarks>
        /// <param name="_key">获取这个 Object 的 key </param>
        /// <param name="_complete">获取成功的回调</param>
        /// <returns>这一次获取的 handle </returns>
        public uint get(T_KEY _key, Action<T_OBJECT> _complete)
        {
            // 没传入 complete 不知道你要干嘛，就不给你
            if (_complete == null)
                return 0;
            
            // 如果 key 不存在，则取不到东西
            if (_key == null)
                return 0;

            // 生成一个新的 handle
            uint handle = _getNextHandle();
            // 尝试从已有的数据中获取 Object
            if (_m_keyToObjectData.TryGetValue(_key, out ObjectData resultObject))
            {
                // 增加引用计数
                resultObject.referenceCount++;
                // 把 handle 和这个数据对应上
                _m_handleToObjectData.Add(handle, resultObject);
                
                // 如果这个对象没有加载完，就把 complete 存下来
                if (!resultObject.isLoadDone)
                    resultObject.loadDoneFunction.Add(handle, _complete);
                // 如果加载完了，就直接调用完成回调
                else
                    _complete.Invoke(resultObject.obj);

                return handle;
            }
            
            // 已有的数据中没有对应的 Object ，就生成新的数据，并加载
            resultObject = new ObjectData()
            {
                obj = default,
                referenceCount = 1,
                loadDoneFunction = new Dictionary<uint, Action<T_OBJECT>>(),
                isLoadDone = false,
                lastBeActiveTime = float.PositiveInfinity,
                isObsolete = false
            };
            
            resultObject.loadDoneFunction.Add(handle, _complete);
            // 添加到数据中，稍后开始加载
            _m_keyToObjectData.Add(_key, resultObject);
            // 把 handle 和这个数据对应上
            _m_handleToObjectData.Add(handle, resultObject);
            // 开始对应 key 的 Object
            _loadObject(_key, _obj =>
            {
                // // 最少也延迟一帧再执行，防止外部需要在 complete 回调中调用 release（这时 handle 还没赋值）
                // ALCommonTaskController.CommonActionAddNextFrameTask(() =>
                // {
                    // 如果数据已经被废弃了，就销毁加载出来的东西
                    if (resultObject.isObsolete)
                    {
                        _discardObject(_key, _obj);
                        return;
                    }

                    resultObject.isLoadDone = true;
                    resultObject.obj = _obj;
                    // 如果还有引用计数，就调用完成回调，并情况
                    if (resultObject.referenceCount > 0)
                    {
                        foreach (Action<T_OBJECT> loadDoneFunction in resultObject.loadDoneFunction.Values)
                        {
                            loadDoneFunction?.Invoke(_obj);
                        }
                        resultObject.loadDoneFunction.Clear();
                    }
                    
                    // 调用预载完成并清空数据
                    resultObject.preloadDoneFunction?.Invoke(_obj != null);
                    resultObject.preloadDoneFunction = null;
                //});
            });
            
            return handle;
        }
        /// <summary>
        /// 根据 handle 来释放某个 Object 的占用
        /// </summary>
        /// <remarks>
        /// 同一个 Object 必须要先调用 release ，才能 get
        /// </remarks>
        /// <param name="_handle">调用 get 方法时得到的返回值</param>
        public bool release(uint _handle)
        {
            if (_handle == 0)
                return false;
            
            // 尝试根据 handle 删除对应数据
            if (_m_handleToObjectData.Remove(_handle, out ObjectData objectData))
            {
                // 把引用计数删去
                objectData.referenceCount--;
                // 如果还有加载方法在其中（说明还没加载完），就删去
                objectData.loadDoneFunction.Remove(_handle);
                // 如果没有东西在引用了，就标记一下这时的时间
                if (objectData.referenceCount <= 0)
                    objectData.lastBeActiveTime = Time.realtimeSinceStartup;
            }

            return true;
        }

        [NotNull] private readonly List<T_KEY> _waitToDelete_tempList_discardCache = new List<T_KEY>();
        [NotNull] private readonly List<Action<bool>> _waitToDelete_tempList_preloadDone = new List<Action<bool>>();
        /// <summary>
        /// 清空多少时间内没有被使用的缓存
        /// </summary>
        public void discardCache(float _time)
        {
            float now = Time.realtimeSinceStartup;
            
            _waitToDelete_tempList_discardCache.Clear();
            _waitToDelete_tempList_preloadDone.Clear();
            foreach (KeyValuePair<T_KEY, ObjectData> keyValuePair in _m_keyToObjectData)
            {
                // 如果还有对象引用着，就跳过
                if (keyValuePair.Value.referenceCount > 0)
                    continue;
                
                // 如果经过的时间比传入的时间大，就删除缓存
                if (now - keyValuePair.Value.lastBeActiveTime > _time)
                {
                    // 标记数据为已废弃
                    keyValuePair.Value.isObsolete = true;
                    // 如果有预载完成的回调，就调用并清空
                    if (keyValuePair.Value.preloadDoneFunction != null)
                    {
                        _waitToDelete_tempList_preloadDone.Add(keyValuePair.Value.preloadDoneFunction);
                        keyValuePair.Value.preloadDoneFunction = null;
                    }
                    // 删除 object
                    _discardObject(keyValuePair.Key, keyValuePair.Value.obj);
                    // 加入列表中，一会删除字典中的数据
                    _waitToDelete_tempList_discardCache.Add(keyValuePair.Key);
                }
            }

            foreach (T_KEY key in _waitToDelete_tempList_discardCache)
            {
                _m_keyToObjectData.Remove(key);
            }
            
            foreach (Action<bool> action in _waitToDelete_tempList_preloadDone)
            {
                action.Invoke(false);
            }
        }
        /// <summary>
        /// 预载某个资源
        /// </summary>
        public void preload(T_KEY _key, Action<bool> _complete)
        {
            if (_key == null)
            {
                _complete?.Invoke(false);
                return;
            }
            
            // 尝试从已有的数据中获取 Object
            if (_m_keyToObjectData.TryGetValue(_key, out ObjectData objectData))
            {
                // 更新活跃时间为现在
                objectData.lastBeActiveTime = Time.realtimeSinceStartup;
                // 如果 Object 已经加载完，就直接调用 complete
                if (objectData.isLoadDone)
                    _complete?.Invoke(objectData.obj != null);
                // 如果 Object 还在加载，就等待加载完成后调用回调
                else
                    objectData.preloadDoneFunction += _complete;
                return;
            }
            
            // 已有的数据中没有对应的 Object ，就生成新的数据，并加载
            ObjectData preloadObj = new ObjectData()
            {
                obj = default,
                referenceCount = 0,
                loadDoneFunction = new Dictionary<uint, Action<T_OBJECT>>(),
                preloadDoneFunction = _complete,
                isLoadDone = false,
                lastBeActiveTime = Time.realtimeSinceStartup,
                isObsolete = false
            };
            _m_keyToObjectData.Add(_key, preloadObj);
            // 开始对应 key 的 Object
            _loadObject(_key, _obj =>
            {
                // // 最少也延迟一帧再执行，防止外部需要在 complete 回调中调用 release（这时 handle 还没赋值）
                // ALCommonTaskController.CommonActionAddNextFrameTask(() =>
                // {
                    // 如果数据已经被废弃了，就销毁加载出来的东西
                    if (preloadObj.isObsolete)
                    {
                        _discardObject(_key, _obj);
                        return;
                    }

                    preloadObj.isLoadDone = true;
                    preloadObj.obj = _obj;
                    // 如果这个 object 被别的东西占用，就调用
                    if (preloadObj.referenceCount > 0)
                    {
                        foreach (Action<T_OBJECT> loadDoneFunction in preloadObj.loadDoneFunction.Values)
                        {
                            loadDoneFunction?.Invoke(_obj);
                        }
                    }
                    // 调用预载完成并清空数据
                    preloadObj.preloadDoneFunction?.Invoke(_obj != null);
                    preloadObj.preloadDoneFunction = null;
                //});
            });
        }


        protected abstract void _loadObject(T_KEY _key, [NotNull] Action<T_OBJECT> _complete);
        protected abstract void _discardObject(T_KEY _key, T_OBJECT _object);

        private class ObjectData
        {
            public T_OBJECT obj;
            // 被引用的计数
            public uint referenceCount;
            // 加载完的时候触发的方法
            public Dictionary<uint, Action<T_OBJECT>> loadDoneFunction;
            // 预载完成的回调
            public Action<bool> preloadDoneFunction;
            // 是否加载完
            public bool isLoadDone;
            // 上次被占用对象释放时的时间点
            public float lastBeActiveTime;
            // 这个数据是否已被废弃
            public bool isObsolete;
        }
    }
}