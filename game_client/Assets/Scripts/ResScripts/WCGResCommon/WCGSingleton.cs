using UnityEngine;
using System.Collections;

public class WCGSingleton<T> where T : new () {

    private static T _g_instance;

    public static T instance
    {
        get
        {
            if(_g_instance == null) {
                _g_instance = new T();
            }
            return _g_instance;
        }
    }

}