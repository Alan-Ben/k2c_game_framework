using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p032_GuildOp
{

/// <summary>
/// 请求联盟协作攻击日志列表
/// </summary>
public class GC2GS_032_041_ReqGuildCooperateAttackLogList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 上一次读取的数据id
/// </summary>
private long lastDbId;
/// <summary>
/// 所需日志数量
/// </summary>
private int num;


public GC2GS_032_041_ReqGuildCooperateAttackLogList() {
	lastDbId = (long)0;
	num = 0;
}

public GC2GS_032_041_ReqGuildCooperateAttackLogList(
	long _lastDbId
	, int _num
) {	lastDbId = _lastDbId;
	num = _num;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)41; }

/// <summary>
/// 上一次读取的数据id
/// </summary>
public long getLastDbId() { return lastDbId; }
/// <summary>
/// 上一次读取的数据id
/// </summary>
public void setLastDbId(long _lastDbId) { lastDbId = _lastDbId; }
/// <summary>
/// 所需日志数量
/// </summary>
public int getNum() { return num; }
/// <summary>
/// 所需日志数量
/// </summary>
public void setNum(int _num) { num = _num; }


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
	lastDbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	num = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(lastDbId);
	_buf.putInt(num);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)41);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)41);
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
	builder.Append("lastDbId").Append(":").Append(lastDbId.ToString()).Append(", ");
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

