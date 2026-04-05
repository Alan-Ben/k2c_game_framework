using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_BuffInfo : ALBasicProtocolPack._IALProtocolStructure {
private long buffId;
private long addTimeMs;
private long leftTimeMs;
private int layer;


public WCGGS2GC_BuffInfo() {
	buffId = (long)0;
	addTimeMs = (long)0;
	leftTimeMs = (long)0;
	layer = 0;
}

public WCGGS2GC_BuffInfo(
	long _buffId
	, long _addTimeMs
	, long _leftTimeMs
	, int _layer
) {	buffId = _buffId;
	addTimeMs = _addTimeMs;
	leftTimeMs = _leftTimeMs;
	layer = _layer;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getBuffId() { return buffId; }
public void setBuffId(long _buffId) { buffId = _buffId; }
public long getAddTimeMs() { return addTimeMs; }
public void setAddTimeMs(long _addTimeMs) { addTimeMs = _addTimeMs; }
public long getLeftTimeMs() { return leftTimeMs; }
public void setLeftTimeMs(long _leftTimeMs) { leftTimeMs = _leftTimeMs; }
public int getLayer() { return layer; }
public void setLayer(int _layer) { layer = _layer; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buffId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	leftTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	layer = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(buffId);
	_buf.putLong(addTimeMs);
	_buf.putLong(leftTimeMs);
	_buf.putInt(layer);
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
	builder.Append("buffId").Append(":").Append(buffId.ToString()).Append(", ");
	builder.Append("addTimeMs").Append(":").Append(addTimeMs.ToString()).Append(", ");
	builder.Append("leftTimeMs").Append(":").Append(leftTimeMs.ToString()).Append(", ");
	builder.Append("layer").Append(":").Append(layer.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

