using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ClientData
{

/// <summary>
/// 情人-客户端数据
/// </summary>
public class AvatarGacha_PoolDataClientData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 卡池Id
/// </summary>
private long poolRefId;
/// <summary>
/// 上次查看卡池的时间
/// </summary>
private int lastSeeTimeSec;


public AvatarGacha_PoolDataClientData() {
	poolRefId = (long)0;
	lastSeeTimeSec = 0;
}

public AvatarGacha_PoolDataClientData(
	long _poolRefId
	, int _lastSeeTimeSec
) {	poolRefId = _poolRefId;
	lastSeeTimeSec = _lastSeeTimeSec;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 卡池Id
/// </summary>
public long getPoolRefId() { return poolRefId; }
/// <summary>
/// 卡池Id
/// </summary>
public void setPoolRefId(long _poolRefId) { poolRefId = _poolRefId; }
/// <summary>
/// 上次查看卡池的时间
/// </summary>
public int getLastSeeTimeSec() { return lastSeeTimeSec; }
/// <summary>
/// 上次查看卡池的时间
/// </summary>
public void setLastSeeTimeSec(int _lastSeeTimeSec) { lastSeeTimeSec = _lastSeeTimeSec; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	poolRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastSeeTimeSec = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(poolRefId);
	_buf.putInt(lastSeeTimeSec);
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
	builder.Append("poolRefId").Append(":").Append(poolRefId.ToString()).Append(", ");
	builder.Append("lastSeeTimeSec").Append(":").Append(lastSeeTimeSec.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

