using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ALPackage;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;


namespace GOE
{
	public class NPArtToolWnd : EditorWindow {
	    public static void showExportWnd<T> (Rect _rect, string _title) where T : NPArtToolWnd {
	        T window = (T)EditorWindow.GetWindowWithRect(typeof(T), _rect, true, _title);
	        window.Show();

	    }

	    private static NPArtToolWnd _g_instance = null;
	    public static NPArtToolWnd instance { get { return _g_instance; } }

	    private List<_IALExportMenuInterface> _m_lMenuItemList;
	    //滚动区域位置
	    private Vector2 _m_vScrollViewPos;


	    public NPArtToolWnd () {
	        _g_instance = this;

	        _m_lMenuItemList = new List<_IALExportMenuInterface>();
	        _regMenuItem(new setProComfirm(processSelectObjs));

	    }

	     /*********
	    * gui处理函数
	    **/
	    void OnGUI () {
	        _m_vScrollViewPos = GUILayout.BeginScrollView(_m_vScrollViewPos);

	        //开始纵向布局
	        EditorGUILayout.BeginVertical();

	        //显示文本
	        EditorGUILayout.LabelField("快捷设置美术资源的属性");

	        //开始逐项进行显示
	        for (int i = 0; i < _m_lMenuItemList.Count; i++) {
	            _m_lMenuItemList[i].onGUI();
	        }

	        //结束最外围纵向布局
	        EditorGUILayout.EndVertical();

	        GUILayout.EndScrollView();
	    }

	     /***********************
	    * 注册资源导出菜单项
	    **/
	    protected void _regMenuItem (_IALExportMenuInterface _item) {
	        if (null == _item)
	            return;

	        if (_m_lMenuItemList.Contains(_item)) {
	            UnityEngine.Debug.LogError("reg menu item multi times!");
	            return;
	        }

	        _m_lMenuItemList.Add(_item);
	    }




	    /******************
	     * 具体的导出操作
	     **/


	    private void processSelectObjs () {
	        Object[] selectedObjs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
	        if (selectedObjs.Length <= 0) {
	            Debug.LogError("当前没有选择任何对象,先选择个要操作的对象啊....");
	            return;
	        }
            
	        foreach (Object obj in selectedObjs) {
	            setProShortcuts(obj);
	        }
	    }

	    private void setProShortcuts (Object _obj ) {
	       if(_obj == null)
	           return;
	        GameObject go = _obj as GameObject;
	        if(go == null)
	            return;

	        GameObject childGo = GameObject.Instantiate(_obj) as GameObject;
	        childGo.transform.parent = go.transform;
	        childGo.name = string.Format("{0}_shadow", go.name);
        
	        //父物体的属性配置
	        go.layer = LayerMask.NameToLayer("GAME_UNIT");
	        go.transform.localPosition = Vector3.zero;
	        go.transform.localRotation = Quaternion.Euler(-45f, -45f, 0f);
	        go.transform.localScale = Vector3.one * 2;
	        SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
	        if (spriteRenderer == null)
	            spriteRenderer = go.AddComponent<SpriteRenderer>();
	        spriteRenderer.flipX = true;
	        spriteRenderer.flipY = false;

	        if (go.GetComponent<FaceCamera>() == null)
	            go.AddComponent<FaceCamera>();

	        //子物体设置 
	        childGo.layer = LayerMask.NameToLayer("GAME_UNIT_SHADOW");
	        childGo.transform.localPosition = Vector3.zero;
	        childGo.transform.localRotation = Quaternion.Euler(-45f, 0f, -45f);
	        childGo.transform.localScale = Vector3.one * 1;
	        spriteRenderer = childGo.GetComponent<SpriteRenderer>();
	        if (spriteRenderer == null)
	            spriteRenderer = childGo.AddComponent<SpriteRenderer>();
	        spriteRenderer.color = new Color(0, 0, 0, 255);
	        spriteRenderer.sortingOrder = 1;
	        if (childGo.GetComponent<FaceCamera>() == null)
	            childGo.AddComponent<FaceCamera>();
	    }




	   //包含一个确定按钮
	    public class setProComfirm : _IALExportMenuInterface {
	        private string _m_sBtnText = "确定";
	        private System.Action _m_dDelegate;


	        public setProComfirm (System.Action _onComfirm) {
	            _m_dDelegate = _onComfirm;
	        }

	        public virtual bool needShow { get { return true; } }

	        public void onGUI () {
	            //按钮对象
	            if (GUILayout.Button(_m_sBtnText, GUILayout.Height(30)) && null != _m_dDelegate) {
	                _m_dDelegate();
	            }
	        }
	    }



	}
}