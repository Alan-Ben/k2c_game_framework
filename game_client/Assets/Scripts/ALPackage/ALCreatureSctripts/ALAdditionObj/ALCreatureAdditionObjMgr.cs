using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALCreatureAdditionObjMgr
    {
        protected _AALBasicCreatureControl creatureControl;
        /** 存放添加的子物体队列 */
        protected List<ALCreatureChildAdditionObj> additionObjList;

        /** 需要进行网格合并的对象列表 */
        protected List<ALCreatureChildAdditionMeshObj> _m_lMeshCombineObjList;
        /** 需要关联的骨骼对象列表 */
        protected List<ALCreatureChildAdditionLinkBoneObj> _m_lNeedLinkBonesObjList;

        /** 基本的蒙皮信息对象 */
        protected List<SkinnedMeshRenderer> _m_lBasicSkinnedRenderList;

        public ALCreatureAdditionObjMgr(_AALBasicCreatureControl _creature)
        {
            creatureControl = _creature;
            additionObjList = new List<ALCreatureChildAdditionObj>();

            _m_lMeshCombineObjList = new List<ALCreatureChildAdditionMeshObj>();
            _m_lNeedLinkBonesObjList = new List<ALCreatureChildAdditionLinkBoneObj>();
            _m_lBasicSkinnedRenderList = new List<SkinnedMeshRenderer>();

            //从默认对象中获取基本蒙皮对象列表
            _m_lBasicSkinnedRenderList.AddRange(_creature.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true));
        }

        public _AALBasicCreatureControl getCreatureControl() { return creatureControl; }

        /***************
         * 添加本对象，根据本对象属性进行添加
         **/
        public ALCreatureAdditionObj addAdditionObj(_AALSOBasicAdditionObjInfo _additionObj)
        {
            if (null == _additionObj)
                return null;

            //查询父物体是否存在
            Transform parentObj = _additionObj.parentObjInfo.getCreatureChildObj(getCreatureControl());

            if (null == parentObj)
                return null;

            //创建物件
            GameObject newObj = _additionObj.createObj(getCreatureControl(), parentObj);

            if (null == newObj)
                return null;

            //根据不同的坐标参考放置对象
            if (AdditionPositionType.WORLD == _additionObj.additionPositionType)
            {
                newObj.transform.SetParent(null);
            }
            else if (AdditionPositionType.PARENT == _additionObj.additionPositionType)
            {
                newObj.transform.SetParent(parentObj);
            }

            //根据物体归属模式创建对应的控制对象
            if (AdditionLifeType.INSTANCE == _additionObj.additionLifeType)
            {
                //物体归属模式为独立个体时不允许永久存在的对象
                //此时当作普通添加物件处理
                ALCreatureAdditionObj additionObj = ALStaticAdditionObjMgr.instance._regChildAdditionObj(_additionObj, newObj);

                if (_additionObj.playTime > 0)
                {
                    //添加定时脚本
                    ALAdditionObjTimer timerObj = newObj.AddComponent<ALAdditionObjTimer>();

                    //设定脚本参数
                    timerObj.lifeTime = _additionObj.playTime;
                    timerObj.additionObj = additionObj;
                }

                return additionObj;
            }
            else if (AdditionLifeType.CHILD == _additionObj.additionLifeType)
            {
                //物体归属模式为子物体时，作为动作附加物件处理，注册物件并获取返回的注册对象
                ALCreatureChildAdditionObj additionObj = _regChildAdditionObj(_additionObj, newObj);

                //根据不同的存留模式进行处理
                if (AdditionTimeType.TIMER == _additionObj.additionTimeType)
                {
                    //定时才需要处理，增加对应定时控制脚本
                    ALAdditionObjTimer timerObj = newObj.AddComponent<ALAdditionObjTimer>();

                    if (null != timerObj)
                    {
                        timerObj.additionObj = additionObj;
                        timerObj.lifeTime = _additionObj.playTime;
                    }
                }

                return additionObj;
            }
            else
            {
                return null;
            }
        }

        /********************
         * 清除所有添加的物件
         **/
        public virtual void removeAllAdditionObj()
        {
            //删除所有添加物件
            for (int i = 0; i < additionObjList.Count; i++)
            {
                //触发移除对象时的事件函数
                _onRemoveChildAdditionObj(additionObjList[i]);

                additionObjList[i]._removeGameObject();
            }
            additionObjList.Clear();

            //删除本对象的网格合并列表
            _m_lMeshCombineObjList.Clear();

            //添加本对象到需要合并网格的处理对象中
            ALCreatureMeshCombineMgr.instance.addNeedCombineMgr(this);
        }

        /********************
         * 重新处理绑定操作
         **/
        public void combine()
        {
            //无效对象不进行绑定操作
            if (null == creatureControl)
                return;

            if (null == _m_lMeshCombineObjList || _m_lMeshCombineObjList.Count <= 0)
            {
                //只是设置SMR脚本无效
                creatureControl.transform.GetComponent<SkinnedMeshRenderer>().enabled = false;
                return;
            }

            //筛选出需要绑定的子对象列表
            List<SkinnedMeshRenderer> combineList = new List<SkinnedMeshRenderer>();
            for (int i = 0; i < _m_lMeshCombineObjList.Count; i++)
            {
                ALCreatureChildAdditionMeshObj meshObj = _m_lMeshCombineObjList[i];
                if (null == meshObj)
                    continue;

                //判断对象根节点对象是否有效决定是否进行绑定操作
                if (meshObj.active)
                    combineList.AddRange(meshObj.renderList);
            }

            //调用统一绑定函数
            ALUnityCommon.combineSkinnedMesh(creatureControl.transform, _m_lBasicSkinnedRenderList, combineList);
        }

        /********************
        * 处理链接骨骼操作
        **/
        public void linkBones()
        {
            //无效对象不进行绑定操作
            if (null == creatureControl)
                return;

            if (null == _m_lNeedLinkBonesObjList || _m_lNeedLinkBonesObjList.Count <= 0)
                return;

            //筛选出需要绑定的子对象列表
            List<SkinnedMeshRenderer> linkList = new List<SkinnedMeshRenderer>();
            for (int i = 0; i < _m_lNeedLinkBonesObjList.Count; i++)
            {
                ALCreatureChildAdditionLinkBoneObj linkObj = _m_lNeedLinkBonesObjList[i];
                if (null == linkObj || null == linkObj.go)
                    continue;

                //查询对象下的蒙皮对象
                SkinnedMeshRenderer[] childRender = linkObj.go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                if (null == childRender || childRender.Length <= 0)
                    continue;

                linkList.AddRange(childRender);
            }

            //清空需要连接的骨骼列表
            _m_lNeedLinkBonesObjList.Clear();

            //调用统一绑定函数
            ALUnityCommon.linkSkinnedMeshBones(_m_lBasicSkinnedRenderList, linkList);
        }

        /********************
         * 将指定的添加物件删除
         **/
        public void removeChildAdditionObj(_AALSOBasicAdditionObjInfo _objInfo)
        {
            if (null == _objInfo)
                return;

            for (int i = 0; i < additionObjList.Count; )
            {
                if (null != additionObjList[i].tag && additionObjList[i].tag.CompareTo(_objInfo.objName) == 0)
                {
                    //名称一致
                    //触发移除对象时的事件函数
                    _onRemoveChildAdditionObj(additionObjList[i]);

                    //删除物件
                    additionObjList[i]._removeGameObject();

                    //从队列删除
                    additionObjList.RemoveAt(i);
                }
                else
                {
                    //不删除时才增加下标
                    i++;
                }
            }
        }
        public void removeChildAdditionObj(ALCreatureAdditionObj _objInfo)
        {
            if (null == _objInfo)
                return;

            for (int i = 0; i < additionObjList.Count; i++)
            {
                if (_objInfo == additionObjList[i])
                {
                    //触发移除对象时的事件函数
                    _onRemoveChildAdditionObj(additionObjList[i]);

                    //删除物件
                    additionObjList[i]._removeGameObject();

                    //从队列删除
                    additionObjList.RemoveAt(i);

                    return;
                }
            }
        }

        /*********************
         * 释放所有资源对象
         **/
        public void discard()
        {
            //删除所有添加物件
            for (int i = 0; i < additionObjList.Count; i++)
            {
                //触发移除对象时的事件函数，如在删除蒙皮对象时需要
                _onRemoveChildAdditionObj(additionObjList[i]);

                additionObjList[i]._removeGameObject();
            }
            //清空附加物队列
            additionObjList.Clear();

            //删除本对象的网格合并列表
            _m_lMeshCombineObjList.Clear();

            //删除默认网格对象列表
            _m_lBasicSkinnedRenderList.Clear();
        }

        /********************
         * 向本物体中注册添加物件，并返回注册的节点对象
         **/
        protected internal virtual ALCreatureChildAdditionObj _regChildAdditionObj(_AALSOBasicAdditionObjInfo _objInfo, GameObject _gameObj)
        {
            if (_objInfo is _AALSOBasicAdditionMeshObjInfo)
            {
                //当对象是Mesh附加物时，需要使用不同的处理方式
                _AALSOBasicAdditionMeshObjInfo meshObjInfo = (_AALSOBasicAdditionMeshObjInfo)_objInfo;
                ALCreatureChildAdditionMeshObj additionObj = new ALCreatureChildAdditionMeshObj(this, meshObjInfo, _gameObj);

                //添加到基类数据集合中
                additionObjList.Add(additionObj);

                //添加到本对象的合并网格对象队列中
                _m_lMeshCombineObjList.Add(additionObj);

                //判断添加物的网格合并是否有效，有效则将本对象加入需要合并的数据集中
                if (additionObj.active)
                    ALCreatureMeshCombineMgr.instance.addNeedCombineMgr(this);

                return additionObj;
            }
            else if (_objInfo is _AALSOBasicAdditionLinkBonesObjInfo)
            {
                //当对象是需要关联骨骼的对象时
                _AALSOBasicAdditionLinkBonesObjInfo objInfo = (_AALSOBasicAdditionLinkBonesObjInfo)_objInfo;
                ALCreatureChildAdditionLinkBoneObj additionObj = new ALCreatureChildAdditionLinkBoneObj(this, objInfo, _gameObj);

                additionObjList.Add(additionObj);

                //添加到需要附加合并网格的对象队列中
                _m_lNeedLinkBonesObjList.Add(additionObj);

                //添加需要连接骨骼对象
                ALCreatureMeshCombineMgr.instance.addNeedLinkBoneMgr(this);

                return additionObj;
            }
            else
            {
                ALCreatureChildAdditionObj additionObj = new ALCreatureChildAdditionObj(this, _objInfo, _gameObj);

                additionObjList.Add(additionObj);

                return additionObj;
            }
        }

        /********************
         * 向本物体中删除物件关联
         **/
        protected internal virtual void _unregChildAdditionObj(ALCreatureChildAdditionObj _obj)
        {
            additionObjList.Remove(_obj);

            //触发移除对象时的事件函数
            _onRemoveChildAdditionObj(_obj);
        }

        /****************
         * 在移除附加对象时触发的函数
         **/
        protected void _onRemoveChildAdditionObj(ALCreatureChildAdditionObj _obj)
        {
            if (_obj is ALCreatureChildAdditionMeshObj)
            {
                ALCreatureChildAdditionMeshObj meshObj = (ALCreatureChildAdditionMeshObj)_obj;

                //从数据集移除对象，如原先存在对象，则根据合并标记判断是否需要加入待处理集合
                if (_m_lMeshCombineObjList.Remove(meshObj))
                {
                    //清除其中render队列
                    meshObj.clearRenderList();

                    //判断对象原先是否有效，有效则需要进行下一次绑定
                    if (meshObj.active)
                        ALCreatureMeshCombineMgr.instance.addNeedCombineMgr(this);
                }
            }
        }
    }
}
#endif
