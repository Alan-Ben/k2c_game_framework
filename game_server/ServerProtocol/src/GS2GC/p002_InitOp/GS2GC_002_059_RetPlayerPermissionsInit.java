package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_059_RetPlayerPermissionsInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 已生效权限ID列表 */
private java.util.ArrayList<Long> effectIdList;


public GS2GC_002_059_RetPlayerPermissionsInit() {
	effectIdList = new java.util.ArrayList<Long>();
}

public GS2GC_002_059_RetPlayerPermissionsInit(
	 java.util.ArrayList<Long> _effectIdList
) {	effectIdList = _effectIdList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)59; }

/** 已生效权限ID列表 */
public java.util.ArrayList<Long> getEffectIdList() { return effectIdList; }
/** 已生效权限ID列表 */
public void addEffectIdList(long _effectIdList) { effectIdList.add(_effectIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (effectIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (effectIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _effectIdListCount = _buf.getShort();
	for(int _i = 0; _i < _effectIdListCount; _i++) { 
		long _effectIdList = (long)0;
		if(_buf.remaining() > 0) _effectIdList = _buf.getLong();
		effectIdList.add(_effectIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)effectIdList.size());
	for(int _i = 0; _i < effectIdList.size(); _i++) { 
		_buf.putLong(effectIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)59);
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

