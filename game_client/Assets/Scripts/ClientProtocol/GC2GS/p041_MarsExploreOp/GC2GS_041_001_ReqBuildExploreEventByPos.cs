using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p041_MarsExploreOp
{

/// <summary>
/// 火星探险-创建事件
/// </summary>
public class GC2GS_041_001_ReqBuildExploreEventByPos : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 事件生成位置ID，0-随机
/// </summary>
private long pos;


public GC2GS_041_001_ReqBuildExploreEventByPos() {
	pos = (long)0;
}

public GC2GS_041_001_ReqBuildExploreEventByPos(
	long _pos
) {	pos = _pos;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 事件生成位置ID，0-随机
/// </summary>
public long getPos() { return pos; }
/// <summary>
/// 事件生成位置ID，0-随机
/// </summary>
public void setPos(long _pos) { pos = _pos; }


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
	pos = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(pos);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)1);
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
	builder.Append("pos").Append(":").Append(pos.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

