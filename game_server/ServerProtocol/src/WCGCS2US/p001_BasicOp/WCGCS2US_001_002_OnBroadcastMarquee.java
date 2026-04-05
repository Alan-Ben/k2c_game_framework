package WCGCS2US.p001_BasicOp;

import java.nio.ByteBuffer;
public class WCGCS2US_001_002_OnBroadcastMarquee implements ALBasicProtocolPack._IALProtocolStructure {
private int clentType;
private String content;
private java.util.ArrayList<String> params;


public WCGCS2US_001_002_OnBroadcastMarquee() {
	clentType = 0;
	content = "";
	params = new java.util.ArrayList<String>();
}

public WCGCS2US_001_002_OnBroadcastMarquee(
	 int _clentType
	, String _content
	, java.util.ArrayList<String> _params
) {	clentType = _clentType;
	content = _content;
	params = _params;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

public int getClentType() { return clentType; }
public void setClentType(int _clentType) { clentType = _clentType; }
public String getContent() { return content; }
public void setContent(String _content) { content = _content; }
public java.util.ArrayList<String> getParams() { return params; }
public void addParams(String _params) { params.add(_params); }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
	for(int _i = 0; _i < params.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(params.get(_i));
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
	for(int _i = 0; _i < params.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(params.get(_i));
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clentType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _paramsCount = _buf.getShort();
	for(int _i = 0; _i < _paramsCount; _i++) { 
		String _params = "";
		if(_buf.remaining() > 0) _params = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
		params.add(_params);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(clentType);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
	_buf.putShort((short)params.size());
	for(int _i = 0; _i < params.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, params.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)2);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

