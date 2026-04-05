using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

public interface _ILanguageSqliteInterface
{
    string GetLanguage(string _key);

    //NOTICE: 带参数的获取语言表字符串,对于可接受可变数量参数的方法的调用会对性能造成一定的损失,不带参数的请调用GetLanguage接口
    string getLanguage(string _langKey, params object[] _args);
}

public class TextTranslate : WCGSingleton<TextTranslate>
{
    private _ILanguageSqliteInterface _m_toTranslateObj;

    public void regTranslateObj(_ILanguageSqliteInterface _obj)
    {
        _m_toTranslateObj = _obj;
    }

    /*****************
     * 获取语言
     **/
    public string getLanguage(string _str)
    {
        if (null == _m_toTranslateObj)
            return _str;

        return _m_toTranslateObj.GetLanguage(_str);
    }
    //NOTICE: 带参数的获取语言表字符串,对于可接受可变数量参数的方法的调用会对性能造成一定的损失,不带参数的请调用GetLanguage接口
    public string getLanguage(string _langKey, params object[] _args)
    {
        if (null == _m_toTranslateObj)
            return _langKey;

        return _m_toTranslateObj.getLanguage(_langKey, _args);
    }

    public string getLanguage(string _langKey, List<string> _args)
    {
        return getLanguage(_langKey, _args.ToArray());
    }

    public string getLanguage(string _langKey, List<int> _args)
    {
        return getLanguage(_langKey, _args.ConvertAll<string>(x => x.ToString()).ToArray());
    }

    public string getLanguage(string _langKey, List<float> _args)
    {
        return getLanguage(_langKey, _args.ConvertAll<string>(x => x.ToString()).ToArray());
    }
    
    public string getLanguage(string _langKey, List<long> _args)
    {
        return getLanguage(_langKey, _args.ConvertAll<string>(x => x.ToString()).ToArray());
    }
}
