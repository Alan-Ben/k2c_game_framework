using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    public class ALTexture2DResourceMgr : _AALBaseResourceMgr<Texture2D>
    {

        /*****************
         * 加载资源函数
         **/
        protected override Texture2D _loadResource(string _path)
        {
            Texture2D texture = Resources.Load(_path, typeof(Texture2D)) as Texture2D;

            return texture;
        }

        /************
         * 释放资源的处理函数
         **/
        protected override void _dealFreeResource(ALBaseRsourceObj<Texture2D> _obj)
        {
            //for UnloadAsset(assetToUnload : Object):void function
            //
            //   for ex: http://blog.sina.com.cn/s/blog_6f4f904201016zmd.html
            //
            //Unloads assetToUnload from memory.
            //This function can only be called on Assets that are stored on disk.
            //If there are any references from game objects in the scene to the asset and it is being used then Unity will reload the asset from disk as soon as it is accessed.
            Resources.UnloadAsset(_obj.obj);
        }

        /************
         * 判断是否需要整理内存对象（删除无引用对象）
         **/
        protected override bool _judgeNeedManageMem()
        {
            return false;
        }

        /************
         * 当增加资源时调用的事件函数，在子类中必须要继承
         * 
         * 此时需要进行资源相关负载参考系数的计算
         **/
        protected override void _onAddResource(ALBaseRsourceObj<Texture2D> _obj)
        {
        }

        /************
         * 当删除资源时调用的事件函数，在子类中必须要继承
         * 
         * 此时需要进行资源相关负载参考系数的计算
         **/
        protected override void _onFreeResource(ALBaseRsourceObj<Texture2D> _obj)
        {
        }
    }
}
