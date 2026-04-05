package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 平台获取的跑马灯信息
 **/
public class ServerObj_PHPMarquee implements ALBasicProtocolPack._IALProtocolStructure {
/** 平台跑马灯ID */
private long phpId;
/** 跑马灯配置ID 0-无配置 */
private long refId;
/** 窗口展示队列 */
private int showPosId;
/** 优先级（越大越优先） */
private int priorityId;
/** 循环播放时长秒（优先于次数） */
private int durationSec;
/** 循环播放次数 */
private int durationCount;
/** 预制体ID */
private long uiResId;
/** 是否可删除类型 */
private CommonEnum.EMarqueeCanDelType canDelType;
/** 生存时间秒 */
private int lifeSec;
/** 玩家离线期间是否需要展示 */
private NPEnum.EMarqueeOfflineNeedShowType offlineNeedShowType;
/** 默认语言 */
private String defaultLang;
/** 跑马灯多语言内容数据列表 */
private java.util.ArrayList<Common.ServerObj.ServerObj_PHPMarqueeContent> marqueeContentList;
/** 渠道列表 */
private java.util.ArrayList<String> channelList;
/** 展示条件（客户端使用） */
private String showCondition;


public ServerObj_PHPMarquee() {
	phpId = (long)0;
	refId = (long)0;
	showPosId = 0;
	priorityId = 0;
	durationSec = 0;
	durationCount = 0;
	uiResId = (long)0;
	canDelType = CommonEnum.EMarqueeCanDelType.values()[0];
	lifeSec = 0;
	offlineNeedShowType = NPEnum.EMarqueeOfflineNeedShowType.values()[0];
	defaultLang = "";
	marqueeContentList = new java.util.ArrayList<Common.ServerObj.ServerObj_PHPMarqueeContent>();
	channelList = new java.util.ArrayList<String>();
	showCondition = "";
}

public ServerObj_PHPMarquee(
	 long _phpId
	, long _refId
	, int _showPosId
	, int _priorityId
	, int _durationSec
	, int _durationCount
	, long _uiResId
	, CommonEnum.EMarqueeCanDelType _canDelType
	, int _lifeSec
	, NPEnum.EMarqueeOfflineNeedShowType _offlineNeedShowType
	, String _defaultLang
	, java.util.ArrayList<Common.ServerObj.ServerObj_PHPMarqueeContent> _marqueeContentList
	, java.util.ArrayList<String> _channelList
	, String _showCondition
) {	phpId = _phpId;
	refId = _refId;
	showPosId = _showPosId;
	priorityId = _priorityId;
	durationSec = _durationSec;
	durationCount = _durationCount;
	uiResId = _uiResId;
	canDelType = _canDelType;
	lifeSec = _lifeSec;
	offlineNeedShowType = _offlineNeedShowType;
	defaultLang = _defaultLang;
	marqueeContentList = _marqueeContentList;
	channelList = _channelList;
	showCondition = _showCondition;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 平台跑马灯ID */
public long getPhpId() { return phpId; }
/** 平台跑马灯ID */
public void setPhpId(long _phpId) { phpId = _phpId; }
/** 跑马灯配置ID 0-无配置 */
public long getRefId() { return refId; }
/** 跑马灯配置ID 0-无配置 */
public void setRefId(long _refId) { refId = _refId; }
/** 窗口展示队列 */
public int getShowPosId() { return showPosId; }
/** 窗口展示队列 */
public void setShowPosId(int _showPosId) { showPosId = _showPosId; }
/** 优先级（越大越优先） */
public int getPriorityId() { return priorityId; }
/** 优先级（越大越优先） */
public void setPriorityId(int _priorityId) { priorityId = _priorityId; }
/** 循环播放时长秒（优先于次数） */
public int getDurationSec() { return durationSec; }
/** 循环播放时长秒（优先于次数） */
public void setDurationSec(int _durationSec) { durationSec = _durationSec; }
/** 循环播放次数 */
public int getDurationCount() { return durationCount; }
/** 循环播放次数 */
public void setDurationCount(int _durationCount) { durationCount = _durationCount; }
/** 预制体ID */
public long getUiResId() { return uiResId; }
/** 预制体ID */
public void setUiResId(long _uiResId) { uiResId = _uiResId; }
/** 是否可删除类型 */
public CommonEnum.EMarqueeCanDelType getCanDelType() { return canDelType; }
/** 是否可删除类型 */
public void setCanDelType(CommonEnum.EMarqueeCanDelType _canDelType) { canDelType = _canDelType; }
/** 生存时间秒 */
public int getLifeSec() { return lifeSec; }
/** 生存时间秒 */
public void setLifeSec(int _lifeSec) { lifeSec = _lifeSec; }
/** 玩家离线期间是否需要展示 */
public NPEnum.EMarqueeOfflineNeedShowType getOfflineNeedShowType() { return offlineNeedShowType; }
/** 玩家离线期间是否需要展示 */
public void setOfflineNeedShowType(NPEnum.EMarqueeOfflineNeedShowType _offlineNeedShowType) { offlineNeedShowType = _offlineNeedShowType; }
/** 默认语言 */
public String getDefaultLang() { return defaultLang; }
/** 默认语言 */
public void setDefaultLang(String _defaultLang) { defaultLang = _defaultLang; }
/** 跑马灯多语言内容数据列表 */
public java.util.ArrayList<Common.ServerObj.ServerObj_PHPMarqueeContent> getMarqueeContentList() { return marqueeContentList; }
/** 跑马灯多语言内容数据列表 */
public void addMarqueeContentList(Common.ServerObj.ServerObj_PHPMarqueeContent _marqueeContentList) { marqueeContentList.add(_marqueeContentList); }
/** 渠道列表 */
public java.util.ArrayList<String> getChannelList() { return channelList; }
/** 渠道列表 */
public void addChannelList(String _channelList) { channelList.add(_channelList); }
/** 展示条件（客户端使用） */
public String getShowCondition() { return showCondition; }
/** 展示条件（客户端使用） */
public void setShowCondition(String _showCondition) { showCondition = _showCondition; }


public final int GetBufSize() {
	int _size = 52;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(defaultLang);
	_size += 2;
	for(int _i = 0; _i < marqueeContentList.size(); _i++) {
	_size += 4 + marqueeContentList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < channelList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelList.get(_i));
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(showCondition);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 54;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(defaultLang);
	_size += 2;
	for(int _i = 0; _i < marqueeContentList.size(); _i++) {
	_size += 4 + marqueeContentList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < channelList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelList.get(_i));
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(showCondition);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) phpId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) showPosId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) priorityId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) durationSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) durationCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uiResId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) canDelType = CommonEnum.EMarqueeCanDelType.EMarqueeCanDelType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lifeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) offlineNeedShowType = NPEnum.EMarqueeOfflineNeedShowType.EMarqueeOfflineNeedShowType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) defaultLang = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _marqueeContentListCount = _buf.getShort();
	for(int _i = 0; _i < _marqueeContentListCount; _i++) { 
		Common.ServerObj.ServerObj_PHPMarqueeContent _marqueeContentList = new Common.ServerObj.ServerObj_PHPMarqueeContent();
		if(_buf.remaining() <= 0) return;
	int __marqueeContentListCustLen = _buf.getInt();
	int __marqueeContentListCurPos = _buf.position();
	_marqueeContentList.ReadUnzipBuf(_buf, __marqueeContentListCurPos + __marqueeContentListCustLen);
	_buf.position(__marqueeContentListCurPos + __marqueeContentListCustLen);

		marqueeContentList.add(_marqueeContentList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _channelListCount = _buf.getShort();
	for(int _i = 0; _i < _channelListCount; _i++) { 
		String _channelList = "";
		if(_buf.remaining() > 0) _channelList = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
		channelList.add(_channelList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) showCondition = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(phpId);
	_buf.putLong(refId);
	_buf.putInt(showPosId);
	_buf.putInt(priorityId);
	_buf.putInt(durationSec);
	_buf.putInt(durationCount);
	_buf.putLong(uiResId);
	_buf.putInt(canDelType.ordinal());

	_buf.putInt(lifeSec);
	_buf.putInt(offlineNeedShowType.ordinal());

	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, defaultLang);
	_buf.putShort((short)marqueeContentList.size());
	for(int _i = 0; _i < marqueeContentList.size(); _i++) { 
		_buf.putInt(marqueeContentList.get(_i).GetBufSize());
	marqueeContentList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)channelList.size());
	for(int _i = 0; _i < channelList.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, channelList.get(_i));
	}
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, showCondition);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

