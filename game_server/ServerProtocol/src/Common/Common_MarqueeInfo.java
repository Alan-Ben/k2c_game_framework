package Common;

import java.nio.ByteBuffer;
public class Common_MarqueeInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long dbId;
private long refId;
/** 过期时间 */
private long expiredTimeMs;
private java.util.ArrayList<String> paramList;
/** 优先级 */
private int priorityId;
/** 循环播放时长秒 */
private int durationSec;
/** 循环播放次数 */
private int durationCount;
/** 预制体ID */
private long uiResId;
/** 内容 */
private String content;
/** 是否可删除类型 */
private CommonEnum.EMarqueeCanDelType canDelType;
/** 渠道列表 */
private java.util.ArrayList<String> channelList;
/** 展示条件（客户端使用） */
private String showCondition;


public Common_MarqueeInfo() {
	dbId = (long)0;
	refId = (long)0;
	expiredTimeMs = (long)0;
	paramList = new java.util.ArrayList<String>();
	priorityId = 0;
	durationSec = 0;
	durationCount = 0;
	uiResId = (long)0;
	content = "";
	canDelType = CommonEnum.EMarqueeCanDelType.values()[0];
	channelList = new java.util.ArrayList<String>();
	showCondition = "";
}

public Common_MarqueeInfo(
	 long _dbId
	, long _refId
	, long _expiredTimeMs
	, java.util.ArrayList<String> _paramList
	, int _priorityId
	, int _durationSec
	, int _durationCount
	, long _uiResId
	, String _content
	, CommonEnum.EMarqueeCanDelType _canDelType
	, java.util.ArrayList<String> _channelList
	, String _showCondition
) {	dbId = _dbId;
	refId = _refId;
	expiredTimeMs = _expiredTimeMs;
	paramList = _paramList;
	priorityId = _priorityId;
	durationSec = _durationSec;
	durationCount = _durationCount;
	uiResId = _uiResId;
	content = _content;
	canDelType = _canDelType;
	channelList = _channelList;
	showCondition = _showCondition;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getDbId() { return dbId; }
public void setDbId(long _dbId) { dbId = _dbId; }
public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
/** 过期时间 */
public long getExpiredTimeMs() { return expiredTimeMs; }
/** 过期时间 */
public void setExpiredTimeMs(long _expiredTimeMs) { expiredTimeMs = _expiredTimeMs; }
public java.util.ArrayList<String> getParamList() { return paramList; }
public void addParamList(String _paramList) { paramList.add(_paramList); }
/** 优先级 */
public int getPriorityId() { return priorityId; }
/** 优先级 */
public void setPriorityId(int _priorityId) { priorityId = _priorityId; }
/** 循环播放时长秒 */
public int getDurationSec() { return durationSec; }
/** 循环播放时长秒 */
public void setDurationSec(int _durationSec) { durationSec = _durationSec; }
/** 循环播放次数 */
public int getDurationCount() { return durationCount; }
/** 循环播放次数 */
public void setDurationCount(int _durationCount) { durationCount = _durationCount; }
/** 预制体ID */
public long getUiResId() { return uiResId; }
/** 预制体ID */
public void setUiResId(long _uiResId) { uiResId = _uiResId; }
/** 内容 */
public String getContent() { return content; }
/** 内容 */
public void setContent(String _content) { content = _content; }
/** 是否可删除类型 */
public CommonEnum.EMarqueeCanDelType getCanDelType() { return canDelType; }
/** 是否可删除类型 */
public void setCanDelType(CommonEnum.EMarqueeCanDelType _canDelType) { canDelType = _canDelType; }
/** 渠道列表 */
public java.util.ArrayList<String> getChannelList() { return channelList; }
/** 渠道列表 */
public void addChannelList(String _channelList) { channelList.add(_channelList); }
/** 展示条件（客户端使用） */
public String getShowCondition() { return showCondition; }
/** 展示条件（客户端使用） */
public void setShowCondition(String _showCondition) { showCondition = _showCondition; }


public final int GetBufSize() {
	int _size = 48;
	_size += 2;
	for(int _i = 0; _i < paramList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(paramList.get(_i));
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
	for(int _i = 0; _i < channelList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelList.get(_i));
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(showCondition);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 50;
	_size += 2;
	for(int _i = 0; _i < paramList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(paramList.get(_i));
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
	for(int _i = 0; _i < channelList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelList.get(_i));
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(showCondition);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expiredTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _paramListCount = _buf.getShort();
	for(int _i = 0; _i < _paramListCount; _i++) { 
		String _paramList = "";
		if(_buf.remaining() > 0) _paramList = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
		paramList.add(_paramList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) priorityId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) durationSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) durationCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uiResId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) canDelType = CommonEnum.EMarqueeCanDelType.EMarqueeCanDelType_FromInt(_buf.getInt());
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
	_buf.putLong(dbId);
	_buf.putLong(refId);
	_buf.putLong(expiredTimeMs);
	_buf.putShort((short)paramList.size());
	for(int _i = 0; _i < paramList.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, paramList.get(_i));
	}
	_buf.putInt(priorityId);
	_buf.putInt(durationSec);
	_buf.putInt(durationCount);
	_buf.putLong(uiResId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
	_buf.putInt(canDelType.ordinal());

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

