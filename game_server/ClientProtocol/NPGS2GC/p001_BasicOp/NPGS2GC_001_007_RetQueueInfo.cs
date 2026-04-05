using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGS2GC.p001_BasicOp
{

public class NPGS2GC_001_007_RetQueueInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 客户端用于识别的序列号
/// </summary>
private long clientSerialize;
/// <summary>
/// 当前客户端进入的索引信息
/// </summary>
private long curEnterIndex;


public NPGS2GC_001_007_RetQueueInfo() {
	clientSerialize = (long)0;
	curEnterIndex = (long)0;
}

public NPGS2GC_001_007_RetQueueInfo(
	long _clientSerialize
	, long _curEnterIndex
) {	clientSerialize = _clientSerialize;
	curEnterIndex = _curEnterIndex;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)7; }

/// <summary>
/// 客户端用于识别的序列号
/// </summary>
public long getClientSerialize() { return clientSerialize; }
/// <summary>
/// 客户端用于识别的序列号
/// </summary>
public void setClientSerialize(long _clientSerialize) { clientSerialize = _clientSerialize; }
/// <summary>
/// 当前客户端进入的索引信息
/// </summary>
public long getCurEnterIndex() { return curEnterIndex; }
/// <summary>
/// 当前客户端进入的索引信息
/// </summary>
public void setCurEnterIndex(long _curEnterIndex) { curEnterIndex = _curEnterIndex; }


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
	clientSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curEnterIndex = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(clientSerialize);
	_buf.putLong(curEnterIndex);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)7);
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
	builder.Append("clientSerialize").Append(":").Append(clientSerialize.ToString()).Append(", ");
	builder.Append("curEnterIndex").Append(":").Append(curEnterIndex.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

