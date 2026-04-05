using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_UniformItemTranform : ALBasicProtocolPack._IALProtocolStructure {
private long fromId;
private long fromNum;
private long toId;
private long toNum;


public WCGGS2GC_UniformItemTranform() {
	fromId = (long)0;
	fromNum = (long)0;
	toId = (long)0;
	toNum = (long)0;
}

public WCGGS2GC_UniformItemTranform(
	long _fromId
	, long _fromNum
	, long _toId
	, long _toNum
) {	fromId = _fromId;
	fromNum = _fromNum;
	toId = _toId;
	toNum = _toNum;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getFromId() { return fromId; }
public void setFromId(long _fromId) { fromId = _fromId; }
public long getFromNum() { return fromNum; }
public void setFromNum(long _fromNum) { fromNum = _fromNum; }
public long getToId() { return toId; }
public void setToId(long _toId) { toId = _toId; }
public long getToNum() { return toNum; }
public void setToNum(long _toNum) { toNum = _toNum; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	fromId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	fromNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	toId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	toNum = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(fromId);
	_buf.putLong(fromNum);
	_buf.putLong(toId);
	_buf.putLong(toNum);
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
	builder.Append("fromId").Append(":").Append(fromId.ToString()).Append(", ");
	builder.Append("fromNum").Append(":").Append(fromNum.ToString()).Append(", ");
	builder.Append("toId").Append(":").Append(toId.ToString()).Append(", ");
	builder.Append("toNum").Append(":").Append(toNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

