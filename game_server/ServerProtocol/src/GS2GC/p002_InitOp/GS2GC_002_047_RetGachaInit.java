package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_047_RetGachaInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 卡池列表 */
private java.util.ArrayList<Common.GachaObj.Gacha_PoolInfo> poolList;


public GS2GC_002_047_RetGachaInit() {
	poolList = new java.util.ArrayList<Common.GachaObj.Gacha_PoolInfo>();
}

public GS2GC_002_047_RetGachaInit(
	 java.util.ArrayList<Common.GachaObj.Gacha_PoolInfo> _poolList
) {	poolList = _poolList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)47; }

/** 卡池列表 */
public java.util.ArrayList<Common.GachaObj.Gacha_PoolInfo> getPoolList() { return poolList; }
/** 卡池列表 */
public void addPoolList(Common.GachaObj.Gacha_PoolInfo _poolList) { poolList.add(_poolList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < poolList.size(); _i++) {
	_size += 4 + poolList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < poolList.size(); _i++) {
	_size += 4 + poolList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _poolListCount = _buf.getShort();
	for(int _i = 0; _i < _poolListCount; _i++) { 
		Common.GachaObj.Gacha_PoolInfo _poolList = new Common.GachaObj.Gacha_PoolInfo();
		if(_buf.remaining() <= 0) return;
	int __poolListCustLen = _buf.getInt();
	int __poolListCurPos = _buf.position();
	_poolList.ReadUnzipBuf(_buf, __poolListCurPos + __poolListCustLen);
	_buf.position(__poolListCurPos + __poolListCustLen);

		poolList.add(_poolList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)poolList.size());
	for(int _i = 0; _i < poolList.size(); _i++) { 
		_buf.putInt(poolList.get(_i).GetBufSize());
	poolList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)47);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)47);
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

