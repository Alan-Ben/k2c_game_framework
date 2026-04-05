using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p041_MarsExploreOp
{

/// <summary>
/// 火星探险-通知火星矿最新数据
/// </summary>
public class GC2GS_041_013_ReqNoticeMarsMine : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 火星矿实例ID
/// </summary>
private long id;
/// <summary>
/// 检查状态序列号，0表示不检查都要返回
/// </summary>
private long checkSerialize;


public GC2GS_041_013_ReqNoticeMarsMine() {
	id = (long)0;
	checkSerialize = (long)0;
}

public GC2GS_041_013_ReqNoticeMarsMine(
	long _id
	, long _checkSerialize
) {	id = _id;
	checkSerialize = _checkSerialize;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)13; }

/// <summary>
/// 火星矿实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 火星矿实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 检查状态序列号，0表示不检查都要返回
/// </summary>
public long getCheckSerialize() { return checkSerialize; }
/// <summary>
/// 检查状态序列号，0表示不检查都要返回
/// </summary>
public void setCheckSerialize(long _checkSerialize) { checkSerialize = _checkSerialize; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	checkSerialize = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(checkSerialize);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)13);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("checkSerialize").Append(":").Append(checkSerialize.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

