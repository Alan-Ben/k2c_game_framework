using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    public class ALBaseRsourceObj<T> where T : UnityEngine.Object
    {
        private _AALBaseResourceMgr<T> mgrObj;

        /** 对应路径 */
        public string path;

        public T obj;

        /** 引用的次数 */
        private int useCount;

        public ALBaseRsourceObj(_AALBaseResourceMgr<T> _mgr, T _obj)
        {
            mgrObj = _mgr;
            path = "";
            obj = _obj;
            useCount = 0;
        }

        public ALBaseRsourceObj<T> request()
        {
            if (useCount == 0)
            {
                //促使需要将对象从空列表中删除
                mgrObj._removeObjFromEmptyList(this);
            }
            else if (useCount < 0)
            {
                return null;
            }

            useCount++;

            return this;
        }

        public void release()
        {
            if (useCount == 0)
            {
                Debug.LogError("Resource: " + path + " release operation error!");
                return;
            }
            else if (useCount < 0)
            {
                Debug.LogError("Resource has been released!");
                return;
            }

            //小于0表示已经被释放
            if (useCount < 0)
                return;

            useCount--;

            if (useCount == 0)
            {
                //此时需要添加到管理对象的空列表
                mgrObj._addObjToEmptyList(this);
            }
        }

        public K getObject<K>() where K : UnityEngine.Object
        {
            try
            {
                return obj as K;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                return null;
            }
        }

        public int getRelateCount()
        {
            return useCount;
        }

        protected internal void free()
        {
            mgrObj = null;
            path = "";
            obj = null;
            useCount = -1;
        }
    }
}
