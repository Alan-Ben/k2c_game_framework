package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_067_RetGuildMarsHelpInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 可以帮助的火星求助实例ID列表 */
private java.util.ArrayList<Long> canDealIdList;


public GS2GC_002_067_RetGuildMarsHelpInit() {
	canDealIdList = new java.util.ArrayList<Long>();
}

public GS2GC_002_067_RetGuildMarsHelpInit(
	 java.util.ArrayList<Long> _canDealIdList
) {	canDealIdList = _canDealIdList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)67; }

/** 可以帮助的火星求助实例ID列表 */
public java.util.ArrayList<Long> getCanDealIdList() { return canDealIdList; }
/** 可以帮助的火星求助实例ID列表 */
public void addCanDealIdList(long _canDealIdList) { canDealIdList.add(_canDealIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (canDealIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (canDealIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _canDealIdListCount = _buf.getShort();
	for(int _i = 0; _i < _canDealIdListCount; _i++) { 
		long _canDealIdList = (long)0;
		if(_buf.remaining() > 0) _canDealIdList = _buf.getLong();
		canDealIdList.add(_canDealIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)canDealIdList.size());
	for(int _i = 0; _i < canDealIdList.size(); _i++) { 
		_buf.putLong(canDealIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)67);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)67);
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

