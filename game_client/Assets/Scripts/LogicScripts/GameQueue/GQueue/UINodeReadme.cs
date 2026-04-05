
/// <summary>
/// 为了方便日常开发，一些抽象好的通用Node基类的使用说明
/// 
/// _ANPMainUISceneBasicNode - 需要在某个主视图下加载子视图时的通用基类对象，由此对象派生下面两个子类
/// -- _ANPMainUIAddSceneNode - 在某个主视图下以子视图方式加载子视图时的Node基类，此类的IsMainViewNode默认为false
///                             此视图对象非主视图，一般用于：
///                             - 单个附加视图的展示（如：作弊弹窗）
/// -- _ANPMainUISceneNode - 在某个主视图下以主视图切换的方式加载另外的主视图时的Node基类，此类的IsMainViewNode默认为true
///                          子类 _ANPMainUISceneNode_InGame 提供是否展示回退按钮的带入参数_needBackBtn
///                          子类 NPMainUISceneNode_InGame 提供调整窗口是否纯UI视图属性的_isOnlyUINode
///                          ** 纯UI视图的设定会关闭主摄像机的渲染，只进行UI渲染。此属性用于性能优化处理。
/// 
/// _ANPGAddNode_SingleWnd - 单个窗口显示加载的通用基类节点。本类有以下特点
///                          本类会自动加载一个透明背景。背景默认点击关闭。（如果不需要点击关闭，那么需要对此类进行调整）
///                          本节点默认进入下一个节点会自动删除 - NeedAutoRemove 为true
///                          继承本类，需要带入窗口对象，同时实现窗口加载完成后的回调处理。
///                          本节点会自动调用窗口的加载，并在加载完成之后调用实现的回调。
///                          子类 NPGAddNode_SingleWnd 将回调作为参数带入并存储，简化日常特殊流程的调用，不需要每个流程单独创建一个node，可以通过动态代码直接编写逻辑。
///                          本节点一般用于单个窗口的弹窗展示和处理。如：物品出售、礼包多选一、卡牌升星等
/// 
/// </summary>
/// 

