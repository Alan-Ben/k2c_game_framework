using System;
using System.Collections.Generic;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 默认普通配表的基类，带唯一id那种
    /// </summary>
    public abstract class _AHotfixBaseRefObj : _IBaseHotfixRefObj
    {
        public abstract long _refId { get; }
        
        //解析用的Dealer
        private HotfixRefCommonParseDealer _m_parseDealer;
        
        public void parseFromString(string _line, string[] _titleArray, string _contextString)
        {
            if(_titleArray == null)
            {
                Debug.LogError($"{GetType()}表没有标题行错误");
                return;
            }
            
            //pop获取一个解析对象
            _m_parseDealer = HotfixRefCommonParseDealer.pop();
            _m_parseDealer.setTag(_contextString);
            
            string[] lineSplit = _line.Split('\t');
            //TODO 这边有bug != 解析\t会多出null
            if(lineSplit.Length  > _titleArray.Length)
            {
                Debug.LogError($"{GetType()}表格式错误");
                return;
            }
            
            for (int i = 0; i < lineSplit.Length; ++i)
            {
                if(string.IsNullOrEmpty(_titleArray[i]) || _titleArray[i].StartsWith("#"))
                    continue;

                _m_parseDealer.addKeyValue(_titleArray[i], lineSplit[i]);
            }
            
            try
            {
                _parseFromString(_line);
            }
            catch (Exception e)
            {
                Debug.LogError($"{this}解析异常:{e.ToString_ILRuntime()}\n");
                return;
            }
            finally
            {
                //不管怎样都要回收解析对象
                HotfixRefCommonParseDealer.pushBack(_m_parseDealer);
                _m_parseDealer = null;
            }
        }
        
        //具体解析函数
        protected abstract void _parseFromString(string _line);
        
        #region getXXX

        protected string getString(string _key)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return String.Empty;
            }
            
            return _m_parseDealer.getString(_key);
        }

        protected int getInt(string _name)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return 0;
            }
            
            return _m_parseDealer.getInt(_name);
        }

        protected long getLong(string _name, bool _canNull = true)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return 0;
            }
            
            return _m_parseDealer.getLong(_name, _canNull);
        }

        protected bool getBool(string _name, bool _canNull = true)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return false;
            }
            
            return _m_parseDealer.getBool(_name, _canNull);
        }

        protected float getFloat(string _name, bool _canNull = true)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return 0;
            }
            
            return _m_parseDealer.getFloat(_name, _canNull);
        }
        
        protected TEnum getEnum<TEnum> (string _name, bool _canNull = true)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return (TEnum) Activator.CreateInstance(typeof(TEnum));
            }
            
            return _m_parseDealer.getEnum<TEnum>(_name, _canNull);
        }

        protected Vector2 getVector2(string _name, bool _canNull = true)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return Vector2.zero;
            }
            
            return _m_parseDealer.getVector2(_name, _canNull);
        }
        
        protected Vector2Int getVector2Int(string _name, bool _canNull = true)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return Vector2Int.zero;
            }
            
            return _m_parseDealer.getVector2Int(_name, _canNull);
        }

        protected Vector3 getVector3(string _name, bool _canNull = true)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return Vector3.zero;
            }
            
            return _m_parseDealer.getVector3(_name, _canNull);
        }
        
        protected Vector3Int getVector3Int(string _name, bool _canNull = true)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return Vector3Int.zero;
            }
            
            return _m_parseDealer.getVector3Int(_name, _canNull);
        }

        protected Color getColor(string _name, bool _canNull = true)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return Color.white;
            }
            
            return _m_parseDealer.getColor(_name, _canNull);
        }
        
        protected Rect getRect(string _name, bool _canNull = true)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return Rect.zero;
            }
            
            return _m_parseDealer.getRect(_name, _canNull);
        }

        protected List<T> getList<T>(string _name, bool _canNull = true)
        {
            if(null == _m_parseDealer)
            {
                Debug.Log_EditorOnly($"配表解析还没popDealer就开始，流程有问题，请检查");
                return null;
            }
            
            return _m_parseDealer.getList<T>(_name, _canNull);
        }
        
        #endregion
    }
}