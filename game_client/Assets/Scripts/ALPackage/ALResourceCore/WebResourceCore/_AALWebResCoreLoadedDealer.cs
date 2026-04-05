using UnityEngine;
using System.Collections;

namespace ALPackage
{
    /// <summary>
    /// _AALWebResCore的加载资源方法的回调类，实现这个类，并作为参数在_AALWebResCore加载时传入
    /// </summary>
    public abstract class _AALWebResCoreLoadedDealer
    {
        public abstract void onLoadSuccess(string _localFilePath);

        public abstract void onLoadFail();
    }
}