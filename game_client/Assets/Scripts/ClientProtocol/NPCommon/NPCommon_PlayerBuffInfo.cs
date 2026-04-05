using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class NPCommon_PlayerBuffInfo : ALBasicProtocolPack._IALProtocolStructure {
private long buffId;
private int layer;
private long endMs;
private long startMs;


public NPCommon_PlayerBuffInfo() {
	buffId = (long)0;
	layer = 0;
	endMs = (long)0;
	startMs = (long)0;
}

public NPCommon_PlayerBuffInfo(
	long _buffId
	, int _layer
	, long _endMs
	, long _startMs
) {	buffId = _buffId;
	layer = _layer;
	endMs = _endMs;
	startMs = _startMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getBuffId() { return buffId; }
public void setBuffId(long _buffId) { buffId = _buffId; }
public int getLayer() { return layer; }
public void setLayer(int _layer) { layer = _layer; }
public long getEndMs() { return endMs; }
public void setEndMs(long _endMs) { endMs = _endMs; }
public long getStartMs() { return startMs; }
public void setStartMs(long _startMs) { startMs = _startMs; }


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
	layer = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(buffId);
	_buf.putInt(layer);
	_buf.putLong(endMs);
	_buf.putLong(startMs);
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
	builder.Append("layer").Append(":").Append(layer.ToString()).Append(", ");
	builder.Append("endMs").Append(":").Append(endMs.ToString()).Append(", ");
	builder.Append("startMs").Append(":").Append(startMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

