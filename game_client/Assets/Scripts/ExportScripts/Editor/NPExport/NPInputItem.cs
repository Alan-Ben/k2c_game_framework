using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using UnityEditor;


namespace GOE
{
	public class NPInputTabData {
	    private static NPInputTabData _g_instance = new NPInputTabData();
	    public static NPInputTabData instance
	    {
	        get
	        {
	            if (null == _g_instance)
	                _g_instance = new NPInputTabData();
	            return _g_instance;
	        }
	    }

	    private List<ALExportData> textList = new List<ALExportData>();

	    protected NPInputTabData () {
	        //读取已有数据
	        ALSOExportData dataObj = AssetDatabase.LoadAssetAtPath("Assets/Resources/Refdata/__DLInputSetting.asset", typeof(ALSOExportData)) as ALSOExportData;
	        if (null != dataObj) {
	            //创建新的存储对象
	            textList = new List<ALExportData>(dataObj.valueList);
	        }
	        else {
	            UnityEngine.Debug.LogError("Can not find old setting");
	        }
	    }

	    public void setValue(string _key, string _value)
	    {
	        for (int i = 0; i < textList.Count; i++)
	        {
	            if(textList[i].key == _key)
	            {
	                textList[i] = new ALExportData(_key, _value);
	                return;
	            }
	        }
	        textList.Add(new ALExportData(_key, _value));
	    }

	    public string getValue(string _key)
	    {
	        if (null == textList)
	            return "";

	        for (int i = 0; i < textList.Count; i++)
	        {
	            if (textList[i].key == _key)
	                return textList[i].value;
	        }
	        return "";
	    }

	    public void save () {

	        ALSOExportData dataObj = ScriptableObject.CreateInstance<ALSOExportData>();
	        //设置数据
	        dataObj.valueList = new List<ALExportData>();
	        dataObj.valueList.AddRange(textList);

	        //保存数据
	        AssetDatabase.CreateAsset(dataObj, "Assets/Resources/Refdata/__DLInputSetting.asset");
	    }
	}

	public class NPInputItem : _IALExportMenuInterface {

	    private string key = "";
	    private string text = "";
	    private int height = 15;
	    private bool hasInit = false;

	    public NPInputItem (string _key, int _height) {
	        key = _key;
	        height = _height;
	    }

	    public virtual bool needShow { get { return true; } }

	    public void onGUI () {
	        if (!hasInit) {
	            hasInit = true;
	            text = NPInputTabData.instance.getValue(key);
	        }
	        string newValue = GUILayout.TextField(text, GUILayout.Height(height));
	        if (!newValue.Equals(text)) {
	            text = newValue;
	            //保存数据
	            NPInputTabData.instance.setValue(key, text);
	        }
	    }


	}
}