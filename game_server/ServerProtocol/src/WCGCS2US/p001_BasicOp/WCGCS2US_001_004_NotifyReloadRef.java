package WCGCS2US.p001_BasicOp;

import java.nio.ByteBuffer;
public class WCGCS2US_001_004_NotifyReloadRef implements ALBasicProtocolPack._IALProtocolStructure {
private String refList;


public WCGCS2US_001_004_NotifyReloadRef() {
	refList = "";
}

public WCGCS2US_001_004_NotifyReloadRef(
	 String _refList
) {	refList = _refList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)4; }

public String getRefList() { return refList; }
public void setRefList(String _refList) { refList = _refList; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(refList);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(refList);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refList = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, refList);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)4);
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

