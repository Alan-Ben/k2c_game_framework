using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ChildObj
{

/// <summary>
/// 向玩家请求指定联姻基础数据
/// </summary>
public class Adult_ToMeApplyBaseInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 请求子嗣实例ID
/// </summary>
private long applyAdultId;
/// <summary>
/// 请求过期截至时间（秒）
/// </summary>
private int expiredTs;


public Adult_ToMeApplyBaseInfo() {
	applyAdultId = (long)0;
	expiredTs = 0;
}

public Adult_ToMeApplyBaseInfo(
	long _applyAdultId
	, int _expiredTs
) {	applyAdultId = _applyAdultId;
	expiredTs = _expiredTs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 请求子嗣实例ID
/// </summary>
public long getApplyAdultId() { return applyAdultId; }
/// <summary>
/// 请求子嗣实例ID
/// </summary>
public void setApplyAdultId(long _applyAdultId) { applyAdultId = _applyAdultId; }
/// <summary>
/// 请求过期截至时间（秒）
/// </summary>
public int getExpiredTs() { return expiredTs; }
/// <summary>
/// 请求过期截至时间（秒）
/// </summary>
public void setExpiredTs(int _expiredTs) { expiredTs = _expiredTs; }


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
	applyAdultId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	expiredTs = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(applyAdultId);
	_buf.putInt(expiredTs);
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
	builder.Append("applyAdultId").Append(":").Append(applyAdultId.ToString()).Append(", ");
	builder.Append("expiredTs").Append(":").Append(expiredTs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

