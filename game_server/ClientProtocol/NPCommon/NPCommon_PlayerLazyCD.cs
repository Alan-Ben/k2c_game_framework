using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 玩家CD数据
/// </summary>
public class NPCommon_PlayerLazyCD : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// CD唯一ID
/// </summary>
private int cdId;
/// <summary>
/// 上次count变动时间（毫秒）
/// </summary>
private long lastCalTimeMS;
/// <summary>
/// 当前数量
/// </summary>
private int count;
/// <summary>
/// 最大数量
/// </summary>
private int maxCount;
/// <summary>
/// 每次恢复点数
/// </summary>
private int addCountPerTime;
/// <summary>
/// CD时长
/// </summary>
private int cdDurationMs;
/// <summary>
/// 上次有效时长和实际计算时长之间的差额时长
/// </summary>
private long fullGetNextCdRemainTimeMs;
/// <summary>
/// 关联的活动实例ID
/// </summary>
private long relativeActivityInstanceId;


public NPCommon_PlayerLazyCD() {
	cdId = 0;
	lastCalTimeMS = (long)0;
	count = 0;
	maxCount = 0;
	addCountPerTime = 0;
	cdDurationMs = 0;
	fullGetNextCdRemainTimeMs = (long)0;
	relativeActivityInstanceId = (long)0;
}

public NPCommon_PlayerLazyCD(
	int _cdId
	, long _lastCalTimeMS
	, int _count
	, int _maxCount
	, int _addCountPerTime
	, int _cdDurationMs
	, long _fullGetNextCdRemainTimeMs
	, long _relativeActivityInstanceId
) {	cdId = _cdId;
	lastCalTimeMS = _lastCalTimeMS;
	count = _count;
	maxCount = _maxCount;
	addCountPerTime = _addCountPerTime;
	cdDurationMs = _cdDurationMs;
	fullGetNextCdRemainTimeMs = _fullGetNextCdRemainTimeMs;
	relativeActivityInstanceId = _relativeActivityInstanceId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// CD唯一ID
/// </summary>
public int getCdId() { return cdId; }
/// <summary>
/// CD唯一ID
/// </summary>
public void setCdId(int _cdId) { cdId = _cdId; }
/// <summary>
/// 上次count变动时间（毫秒）
/// </summary>
public long getLastCalTimeMS() { return lastCalTimeMS; }
/// <summary>
/// 上次count变动时间（毫秒）
/// </summary>
public void setLastCalTimeMS(long _lastCalTimeMS) { lastCalTimeMS = _lastCalTimeMS; }
/// <summary>
/// 当前数量
/// </summary>
public int getCount() { return count; }
/// <summary>
/// 当前数量
/// </summary>
public void setCount(int _count) { count = _count; }
/// <summary>
/// 最大数量
/// </summary>
public int getMaxCount() { return maxCount; }
/// <summary>
/// 最大数量
/// </summary>
public void setMaxCount(int _maxCount) { maxCount = _maxCount; }
/// <summary>
/// 每次恢复点数
/// </summary>
public int getAddCountPerTime() { return addCountPerTime; }
/// <summary>
/// 每次恢复点数
/// </summary>
public void setAddCountPerTime(int _addCountPerTime) { addCountPerTime = _addCountPerTime; }
/// <summary>
/// CD时长
/// </summary>
public int getCdDurationMs() { return cdDurationMs; }
/// <summary>
/// CD时长
/// </summary>
public void setCdDurationMs(int _cdDurationMs) { cdDurationMs = _cdDurationMs; }
/// <summary>
/// 上次有效时长和实际计算时长之间的差额时长
/// </summary>
public long getFullGetNextCdRemainTimeMs() { return fullGetNextCdRemainTimeMs; }
/// <summary>
/// 上次有效时长和实际计算时长之间的差额时长
/// </summary>
public void setFullGetNextCdRemainTimeMs(long _fullGetNextCdRemainTimeMs) { fullGetNextCdRemainTimeMs = _fullGetNextCdRemainTimeMs; }
/// <summary>
/// 关联的活动实例ID
/// </summary>
public long getRelativeActivityInstanceId() { return relativeActivityInstanceId; }
/// <summary>
/// 关联的活动实例ID
/// </summary>
public void setRelativeActivityInstanceId(long _relativeActivityInstanceId) { relativeActivityInstanceId = _relativeActivityInstanceId; }


public int GetBufSize() {
	int _size = 44;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 46;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cdId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastCalTimeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addCountPerTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cdDurationMs = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	fullGetNextCdRemainTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	relativeActivityInstanceId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(cdId);
	_buf.putLong(lastCalTimeMS);
	_buf.putInt(count);
	_buf.putInt(maxCount);
	_buf.putInt(addCountPerTime);
	_buf.putInt(cdDurationMs);
	_buf.putLong(fullGetNextCdRemainTimeMs);
	_buf.putLong(relativeActivityInstanceId);
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
	builder.Append("cdId").Append(":").Append(cdId.ToString()).Append(", ");
	builder.Append("lastCalTimeMS").Append(":").Append(lastCalTimeMS.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("maxCount").Append(":").Append(maxCount.ToString()).Append(", ");
	builder.Append("addCountPerTime").Append(":").Append(addCountPerTime.ToString()).Append(", ");
	builder.Append("cdDurationMs").Append(":").Append(cdDurationMs.ToString()).Append(", ");
	builder.Append("fullGetNextCdRemainTimeMs").Append(":").Append(fullGetNextCdRemainTimeMs.ToString()).Append(", ");
	builder.Append("relativeActivityInstanceId").Append(":").Append(relativeActivityInstanceId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

