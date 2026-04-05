using System;
using System.Collections.Generic;
using ALPackage;
using GOE;

namespace Hotfix
{
    public partial class HotfixRefdataCoreMgr : _AALBasicRefdataCoreMgr
    {
        private static HotfixRefdataCoreMgr _g_instance;
        public static HotfixRefdataCoreMgr instance
        {
            get
            {
                if(_g_instance == null)
                {
                    _g_instance = new HotfixRefdataCoreMgr();
                }
                return _g_instance;
            }
        }

        protected override void _onRefInitFail(Type _class, _IALInitRefObj _obj)
        {
#if UNITY_EDITOR
            if(null == _class)
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_gameres_fail_str, "unkown"), TextTranslate.instance.getLanguage(TransKeyConst.confirm), _obj.finalDelegate);
            else
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_gameres_fail_str, _class.ToString()), TextTranslate.instance.getLanguage(TransKeyConst.confirm), _obj.finalDelegate);
#else
        if (null == _class)
            NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_gameres_fail_str, "unkown"), TextTranslate.instance.getLanguage(TransKeyConst.confirm), _obj.finalDelegate);
        else
            NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_gameres_fail_str, _class.ToString()), TextTranslate.instance.getLanguage(TransKeyConst.confirm), _obj.finalDelegate);
#endif
        }


        //测试配表，当作范例用
        // public HotfixGeneralRefObj generalRefObj = new HotfixGeneralRefObj(RefdataResCore.instance);
        // public HotfixRefListCore<TestIdRefObj> testIdRefCore = new HotfixRefListCore<TestIdRefObj>(RefdataResCore.instance, TestIdRefObj.assetPath, TestIdRefObj.objName);

        //三消活动
        public TileMatchOtherRefObj tileMatchOtherRefObj = new TileMatchOtherRefObj(RefdataResCore.instance);
        public HotfixRefListCore<TileMatchBlockRefObj> tileMatchBlockRefCore = new HotfixRefListCore<TileMatchBlockRefObj>(RefdataResCore.instance, TileMatchBlockRefObj.assetPath, TileMatchBlockRefObj.objName);
        public HotfixRefListCore<TileMatchBlockShowRefObj> tileMatchBlockShowRefCore = new HotfixRefListCore<TileMatchBlockShowRefObj>(RefdataResCore.instance, TileMatchBlockShowRefObj.assetPath, TileMatchBlockShowRefObj.objName);
        public HotfixRefListCore<TileMatchModeRefObj> tileMatchModeRefCore = new HotfixRefListCore<TileMatchModeRefObj>(RefdataResCore.instance, TileMatchModeRefObj.assetPath, TileMatchModeRefObj.objName);
        public HotfixRefListCore<TileMatchTaskRefObj> tileMatchTaskRefCore = new HotfixRefListCore<TileMatchTaskRefObj>(RefdataResCore.instance, TileMatchTaskRefObj.assetPath, TileMatchTaskRefObj.objName);
        public HotfixRefListCore<TileMatchStepRewardRefObj> tileMatchStepRewardRefCore = new HotfixRefListCore<TileMatchStepRewardRefObj>(RefdataResCore.instance, TileMatchStepRewardRefObj.assetPath, TileMatchStepRewardRefObj.objName);
        public HotfixRefListCore<TileMatchJackpotGroupRefObj> tileMatchJackpotGroupRefCore = new HotfixRefListCore<TileMatchJackpotGroupRefObj>(RefdataResCore.instance, TileMatchJackpotGroupRefObj.assetPath, TileMatchJackpotGroupRefObj.objName);

        //2048活动
        public NumMergeOtherRefObj numMergeOtherRefObj = new NumMergeOtherRefObj(RefdataResCore.instance);
        public HotfixRefListCore<NumMergeModeRefObj> numMergeModeRefCore = new HotfixRefListCore<NumMergeModeRefObj>(RefdataResCore.instance, NumMergeModeRefObj.assetPath, NumMergeModeRefObj.objName);
        public HotfixRefListCore<NumMergeBlockRefObj> numMergeBlockRefCore = new HotfixRefListCore<NumMergeBlockRefObj>(RefdataResCore.instance, NumMergeBlockRefObj.assetPath, NumMergeBlockRefObj.objName);
        public HotfixRefListCore<NumMergeBoxRefObj> numMergeBoxRefCore = new HotfixRefListCore<NumMergeBoxRefObj>(RefdataResCore.instance, NumMergeBoxRefObj.assetPath, NumMergeBoxRefObj.objName);


        //>>>>>>>>>> AUTO GENERATE START <<<<<<<<<<
        //========万能活动========
        //万能活动商店表
        public HotfixRefMapCore<RegularEventShopRefObj> regularEventShopRefCore = new HotfixRefMapCore<RegularEventShopRefObj>(RefdataResCore.instance, RegularEventShopRefObj.assetPath, RegularEventShopRefObj.objName);
        //万能活动商店商品表
        public HotfixRefMapListCore<RegularEventShopItemRefObj> regularEventShopItemRefCore = new HotfixRefMapListCore<RegularEventShopItemRefObj>(RefdataResCore.instance, RegularEventShopItemRefObj.assetPath, RegularEventShopItemRefObj.objName);
		//>>>>>>>>>> AUTO GENERATE END   <<<<<<<<<<




        /// <summary>
        /// 开始处理需要初始化的数据
        /// </summary>
        /// <param name="_list"></param>
        protected override void _initLoadList(List<_IALInitRefObj> _list)
        {
            if(null == _list)
                return;

            // _list.Add(generalRefObj);
            // _list.Add(testIdRefCore);

            // 三消活动
            _list.Add(tileMatchOtherRefObj);
            _list.Add(tileMatchBlockRefCore);
            _list.Add(tileMatchBlockShowRefCore);
            _list.Add(tileMatchModeRefCore);
            _list.Add(tileMatchTaskRefCore);
            _list.Add(tileMatchStepRewardRefCore);
            _list.Add(tileMatchJackpotGroupRefCore);
            
            // 2048活动
            _list.Add(numMergeOtherRefObj);
            _list.Add(numMergeModeRefCore);
            _list.Add(numMergeBlockRefCore);
            _list.Add(numMergeBoxRefCore);

            //>>>>>>>>>> AUTO GENERATE ADD LIST START <<<<<<<<<<
            _list.Add(regularEventShopRefCore);//万能活动商店表
            _list.Add(regularEventShopItemRefCore);//万能活动商店商品表
			//>>>>>>>>>> AUTO GENERATE ADD LIST END   <<<<<<<<<<
        }

        /// <summary>
        /// 当所有配表初始化完成
        /// </summary>
        protected override void _onInitAllRefCore()
        {
            try
            {
                _initNumMerge();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}