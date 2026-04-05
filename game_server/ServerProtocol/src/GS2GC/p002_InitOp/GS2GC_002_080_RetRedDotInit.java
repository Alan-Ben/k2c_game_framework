package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_080_RetRedDotInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 红点列表 */
private java.util.ArrayList<Common.Common_RedDotInfo> redDotList;


public GS2GC_002_080_RetRedDotInit() {
	redDotList = new java.util.ArrayList<Common.Common_RedDotInfo>();
}

public GS2GC_002_080_RetRedDotInit(
	 java.util.ArrayList<Common.Common_RedDotInfo> _redDotList
) {	redDotList = _redDotList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)80; }

/** 红点列表 */
public java.util.ArrayList<Common.Common_RedDotInfo> getRedDotList() { return redDotList; }
/** 红点列表 */
public void addRedDotList(Common.Common_RedDotInfo _redDotList) { redDotList.add(_redDotList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (redDotList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (redDotList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _redDotListCount = _buf.getShort();
	for(int _i = 0; _i < _redDotListCount; _i++) { 
		Common.Common_RedDotInfo _redDotList = new Common.Common_RedDotInfo();
		if(_buf.remaining() <= 0) return;
	int __redDotListCustLen = _buf.getInt();
	int __redDotListCurPos = _buf.position();
	_redDotList.ReadUnzipBuf(_buf, __redDotListCurPos + __redDotListCustLen);
	_buf.position(__redDotListCurPos + __redDotListCustLen);

		redDotList.add(_redDotList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)redDotList.size());
	for(int _i = 0; _i < redDotList.size(); _i++) { 
		_buf.putInt(redDotList.get(_i).GetBufSize());
	redDotList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)80);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)80);
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

