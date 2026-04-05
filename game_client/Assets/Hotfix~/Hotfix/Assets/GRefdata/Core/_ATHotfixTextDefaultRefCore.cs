using System.Collections.Generic;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 默认配表的解析refcore 带唯一id那种
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _ATHotfixTextDefaultRefCore<T> : _ATHotfixTextRefCore where T : _IBaseHotfixRefObj, new()
    {
        //根据对应文本解析数据到List<T>
        protected override void _parseStringToRefList(string _string)
        {
            if(string.IsNullOrEmpty(_string))
            {
                Debug.LogError("csv为空");
                return;
            }
            
            string[] lineArray = _string.Split('\n');
            List<T> result = new List<T>(lineArray.Length);
            string[] titleArray = null;
            for (int i = 0; i < lineArray.Length; i++)
            {
                if(string.IsNullOrEmpty(lineArray[i]))
                    continue;

                //第一行为标题行,获取每个数据对象的字段名字
                if (i == 0)
                {
                    //获取到标题列表
                    titleArray = lineArray[i].Split('\t');
                }
                else
                {
                    //解析数据
                    T obj = new T();
                    obj.parseFromString(lineArray[i], titleArray, $"{_objName}表的第{i}行");
                    result.Add(obj);
                }
            }

            _initData(result);
        }
        
        //初始化数据
        protected abstract void _initData(List<T> _refList);
    }
}