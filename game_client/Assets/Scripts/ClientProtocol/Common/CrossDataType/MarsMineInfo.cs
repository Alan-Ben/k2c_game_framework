using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CrossDataType
{

public class MarsMineInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 矿实例Id，注意要区分服务器
/// </summary>
private long mineInstanceId;
/// <summary>
/// 静态数据Id
/// </summary>
private long mineRefId;
/// <summary>
/// 矿超时时间戳
/// </summary>
private long mineExpireTimeMS;


public MarsMineInfo() {
	mineInstanceId = (long)0;
	mineRefId = (long)0;
	mineExpireTimeMS = (long)0;
}

public MarsMineInfo(
	long _mineInstanceId
	, long _mineRefId
	, long _mineExpireTimeMS
) {	mineInstanceId = _mineInstanceId;
	mineRefId = _mineRefId;
	mineExpireTimeMS = _mineExpireTimeMS;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 矿实例Id，注意要区分服务器
/// </summary>
public long getMineInstanceId() { return mineInstanceId; }
/// <summary>
/// 矿实例Id，注意要区分服务器
/// </summary>
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }
/// <summary>
/// 静态数据Id
/// </summary>
public long getMineRefId() { return mineRefId; }
/// <summary>
/// 静态数据Id
/// </summary>
public void setMineRefId(long _mineRefId) { mineRefId = _mineRefId; }
/// <summary>
/// 矿超时时间戳
/// </summary>
public long getMineExpireTimeMS() { return mineExpireTimeMS; }
/// <summary>
/// 矿超时时间戳
/// </summary>
public void setMineExpireTimeMS(long _mineExpireTimeMS) { mineExpireTimeMS = _mineExpireTimeMS; }


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
	mineInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mineRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mineExpireTimeMS = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(mineInstanceId);
	_buf.putLong(mineRefId);
	_buf.putLong(mineExpireTimeMS);
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
	builder.Append("mineInstanceId").Append(":").Append(mineInstanceId.ToString()).Append(", ");
	builder.Append("mineRefId").Append(":").Append(mineRefId.ToString()).Append(", ");
	builder.Append("mineExpireTimeMS").Append(":").Append(mineExpireTimeMS.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

