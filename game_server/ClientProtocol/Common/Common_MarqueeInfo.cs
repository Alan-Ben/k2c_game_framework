using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_MarqueeInfo : ALBasicProtocolPack._IALProtocolStructure {
private long dbId;
private long refId;
/// <summary>
/// 过期时间
/// </summary>
private long expiredTimeMs;
private List<string> paramList;
/// <summary>
/// 优先级
/// </summary>
private int priorityId;
/// <summary>
/// 循环播放时长秒
/// </summary>
private int durationSec;
/// <summary>
/// 循环播放次数
/// </summary>
private int durationCount;
/// <summary>
/// 预制体ID
/// </summary>
private long uiResId;
/// <summary>
/// 内容
/// </summary>
private string content;
/// <summary>
/// 是否可删除类型
/// </summary>
private CommonEnum.EMarqueeCanDelType canDelType;
/// <summary>
/// 渠道列表
/// </summary>
private List<string> channelList;
/// <summary>
/// 展示条件（客户端使用）
/// </summary>
private string showCondition;


public Common_MarqueeInfo() {
	dbId = (long)0;
	refId = (long)0;
	expiredTimeMs = (long)0;
	paramList = new List<string>();
	priorityId = 0;
	durationSec = 0;
	durationCount = 0;
	uiResId = (long)0;
	content = "";
	canDelType = 0;
	channelList = new List<string>();
	showCondition = "";
}

public Common_MarqueeInfo(
	long _dbId
	, long _refId
	, long _expiredTimeMs
	, List<string> _paramList
	, int _priorityId
	, int _durationSec
	, int _durationCount
	, long _uiResId
	, string _content
	, CommonEnum.EMarqueeCanDelType _canDelType
	, List<string> _channelList
	, string _showCondition
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getDbId() { return dbId; }
public void setDbId(long _dbId) { dbId = _dbId; }
public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// 过期时间
/// </summary>
public long getExpiredTimeMs() { return expiredTimeMs; }
/// <summary>
/// 过期时间
/// </summary>
public void setExpiredTimeMs(long _expiredTimeMs) { expiredTimeMs = _expiredTimeMs; }
public List<string> getParamList() { return paramList; }
public void addParamList(string _paramList) { paramList.Add(_paramList); }
/// <summary>
/// 优先级
/// </summary>
public int getPriorityId() { return priorityId; }
/// <summary>
/// 优先级
/// </summary>
public void setPriorityId(int _priorityId) { priorityId = _priorityId; }
/// <summary>
/// 循环播放时长秒
/// </summary>
public int getDurationSec() { return durationSec; }
/// <summary>
/// 循环播放时长秒
/// </summary>
public void setDurationSec(int _durationSec) { durationSec = _durationSec; }
/// <summary>
/// 循环播放次数
/// </summary>
public int getDurationCount() { return durationCount; }
/// <summary>
/// 循环播放次数
/// </summary>
public void setDurationCount(int _durationCount) { durationCount = _durationCount; }
/// <summary>
/// 预制体ID
/// </summary>
public long getUiResId() { return uiResId; }
/// <summary>
/// 预制体ID
/// </summary>
public void setUiResId(long _uiResId) { uiResId = _uiResId; }
/// <summary>
/// 内容
/// </summary>
public string getContent() { return content; }
/// <summary>
/// 内容
/// </summary>
public void setContent(string _content) { content = _content; }
/// <summary>
/// 是否可删除类型
/// </summary>
public CommonEnum.EMarqueeCanDelType getCanDelType() { return canDelType; }
/// <summary>
/// 是否可删除类型
/// </summary>
public void setCanDelType(CommonEnum.EMarqueeCanDelType _canDelType) { canDelType = _canDelType; }
/// <summary>
/// 渠道列表
/// </summary>
public List<string> getChannelList() { return channelList; }
/// <summary>
/// 渠道列表
/// </summary>
public void addChannelList(string _channelList) { channelList.Add(_channelList); }
/// <summary>
/// 展示条件（客户端使用）
/// </summary>
public string getShowCondition() { return showCondition; }
/// <summary>
/// 展示条件（客户端使用）
/// </summary>
public void setShowCondition(string _showCondition) { showCondition = _showCondition; }


public int GetBufSize() {
	int _size = 48;
	_size += 2;
for(int _i = 0; _i < paramList.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(paramList[_i]);
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
for(int _i = 0; _i < channelList.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelList[_i]);
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(showCondition);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 50;
	_size += 2;
for(int _i = 0; _i < paramList.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(paramList[_i]);
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
for(int _i = 0; _i < channelList.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channelList[_i]);
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(showCondition);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expiredTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _paramListCount = _buf.getShort();
	for(int _i = 0; _i < _paramListCount; _i++) { 
		string _paramList = "";
		_paramList = _buf.getString();
		paramList.Add(_paramList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	priorityId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	durationSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	durationCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uiResId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	content = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	canDelType = (CommonEnum.EMarqueeCanDelType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _channelListCount = _buf.getShort();
	for(int _i = 0; _i < _channelListCount; _i++) { 
		string _channelList = "";
		_channelList = _buf.getString();
		channelList.Add(_channelList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	showCondition = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putLong(refId);
	_buf.putLong(expiredTimeMs);
	_buf.putShort((short)paramList.Count);
	for(int _i = 0; _i < paramList.Count; _i++) { 
		_buf.putString(paramList[_i]);
	}
	_buf.putInt(priorityId);
	_buf.putInt(durationSec);
	_buf.putInt(durationCount);
	_buf.putLong(uiResId);
	_buf.putString(content);
	_buf.putInt((int)canDelType);

	_buf.putShort((short)channelList.Count);
	for(int _i = 0; _i < channelList.Count; _i++) { 
		_buf.putString(channelList[_i]);
	}
	_buf.putString(showCondition);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("expiredTimeMs").Append(":").Append(expiredTimeMs.ToString()).Append(", ");
	builder.Append("paramList").Append(":").Append(paramList.ToString()).Append(", ");
	builder.Append("priorityId").Append(":").Append(priorityId.ToString()).Append(", ");
	builder.Append("durationSec").Append(":").Append(durationSec.ToString()).Append(", ");
	builder.Append("durationCount").Append(":").Append(durationCount.ToString()).Append(", ");
	builder.Append("uiResId").Append(":").Append(uiResId.ToString()).Append(", ");
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("canDelType").Append(":").Append(canDelType.ToString()).Append(", ");
	builder.Append("channelList").Append(":").Append(channelList.ToString()).Append(", ");
	builder.Append("showCondition").Append(":").Append(showCondition.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

