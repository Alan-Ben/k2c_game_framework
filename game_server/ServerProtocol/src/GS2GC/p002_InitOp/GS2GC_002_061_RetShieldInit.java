package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 屏蔽数据组件初始化
 **/
public class GS2GC_002_061_RetShieldInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 已屏蔽玩家CID列表 */
private java.util.ArrayList<Long> shieldCidList;


public GS2GC_002_061_RetShieldInit() {
	shieldCidList = new java.util.ArrayList<Long>();
}

public GS2GC_002_061_RetShieldInit(
	 java.util.ArrayList<Long> _shieldCidList
) {	shieldCidList = _shieldCidList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)61; }

/** 已屏蔽玩家CID列表 */
public java.util.ArrayList<Long> getShieldCidList() { return shieldCidList; }
/** 已屏蔽玩家CID列表 */
public void addShieldCidList(long _shieldCidList) { shieldCidList.add(_shieldCidList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (shieldCidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (shieldCidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _shieldCidListCount = _buf.getShort();
	for(int _i = 0; _i < _shieldCidListCount; _i++) { 
		long _shieldCidList = (long)0;
		if(_buf.remaining() > 0) _shieldCidList = _buf.getLong();
		shieldCidList.add(_shieldCidList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)shieldCidList.size());
	for(int _i = 0; _i < shieldCidList.size(); _i++) { 
		_buf.putLong(shieldCidList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)61);
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

