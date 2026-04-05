package GC2GS.p030_ShopOp;

import java.nio.ByteBuffer;
/*********
 * 刷新礼包
 **/
public class GC2GS_030_021_ReqRefreshGiftPack implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包id列表 */
private java.util.ArrayList<Long> giftPackList;


public GC2GS_030_021_ReqRefreshGiftPack() {
	giftPackList = new java.util.ArrayList<Long>();
}

public GC2GS_030_021_ReqRefreshGiftPack(
	 java.util.ArrayList<Long> _giftPackList
) {	giftPackList = _giftPackList;
}

public final byte getMainOrder() { return (byte)30; }

public final byte getSubOrder() { return (byte)21; }

/** 礼包id列表 */
public java.util.ArrayList<Long> getGiftPackList() { return giftPackList; }
/** 礼包id列表 */
public void addGiftPackList(long _giftPackList) { giftPackList.add(_giftPackList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (giftPackList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (giftPackList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _giftPackListCount = _buf.getShort();
	for(int _i = 0; _i < _giftPackListCount; _i++) { 
		long _giftPackList = (long)0;
		if(_buf.remaining() > 0) _giftPackList = _buf.getLong();
		giftPackList.add(_giftPackList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)giftPackList.size());
	for(int _i = 0; _i < giftPackList.size(); _i++) { 
		_buf.putLong(giftPackList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)21);
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

