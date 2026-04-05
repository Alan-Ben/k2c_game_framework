using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p041_MarsExploreOp
{

/// <summary>
/// 火星探索-战报数据增加
/// </summary>
public class GS2GC_041_061_OnMarsExplorePVPLogAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 战报创建时间（毫秒），服务端以此排序
/// </summary>
private long createdAt;


public GS2GC_041_061_OnMarsExplorePVPLogAdd() {
	createdAt = (long)0;
}

public GS2GC_041_061_OnMarsExplorePVPLogAdd(
	long _createdAt
) {	createdAt = _createdAt;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)61; }

/// <summary>
/// 战报创建时间（毫秒），服务端以此排序
/// </summary>
public long getCreatedAt() { return createdAt; }
/// <summary>
/// 战报创建时间（毫秒），服务端以此排序
/// </summary>
public void setCreatedAt(long _createdAt) { createdAt = _createdAt; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createdAt = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(createdAt);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)61);
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
	builder.Append("createdAt").Append(":").Append(createdAt.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

