using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 玩家固定时间刷新CD数据
/// </summary>
public class NPCommon_PlayerFixedCD : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// CD唯一ID
/// </summary>
private int cdId;
/// <summary>
/// 当前数量
/// </summary>
private int count;
/// <summary>
/// 上次恢复时间
/// </summary>
private long lastCalTimeMS;
/// <summary>
/// 最大数量
/// </summary>
private int maxCount;
/// <summary>
/// 每次恢复点数
/// </summary>
private int addCountPerTime;


public NPCommon_PlayerFixedCD() {
	cdId = 0;
	count = 0;
	lastCalTimeMS = (long)0;
	maxCount = 0;
	addCountPerTime = 0;
}

public NPCommon_PlayerFixedCD(
	int _cdId
	, int _count
	, long _lastCalTimeMS
	, int _maxCount
	, int _addCountPerTime
) {	cdId = _cdId;
	count = _count;
	lastCalTimeMS = _lastCalTimeMS;
	maxCount = _maxCount;
	addCountPerTime = _addCountPerTime;
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
/// 当前数量
/// </summary>
public int getCount() { return count; }
/// <summary>
/// 当前数量
/// </summary>
public void setCount(int _count) { count = _count; }
/// <summary>
/// 上次恢复时间
/// </summary>
public long getLastCalTimeMS() { return lastCalTimeMS; }
/// <summary>
/// 上次恢复时间
/// </summary>
public void setLastCalTimeMS(long _lastCalTimeMS) { lastCalTimeMS = _lastCalTimeMS; }
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


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cdId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastCalTimeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addCountPerTime = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(cdId);
	_buf.putInt(count);
	_buf.putLong(lastCalTimeMS);
	_buf.putInt(maxCount);
	_buf.putInt(addCountPerTime);
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
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("lastCalTimeMS").Append(":").Append(lastCalTimeMS.ToString()).Append(", ");
	builder.Append("maxCount").Append(":").Append(maxCount.ToString()).Append(", ");
	builder.Append("addCountPerTime").Append(":").Append(addCountPerTime.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

