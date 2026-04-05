package GS2GC.p030_ShopOp;

import java.nio.ByteBuffer;
/*********
 * 礼包记录移除推送
 **/
public class GS2GC_030_063_OnGiftPackBuyRecordRemove implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包id列表 */
private java.util.ArrayList<Long> giftPackIdList;


public GS2GC_030_063_OnGiftPackBuyRecordRemove() {
	giftPackIdList = new java.util.ArrayList<Long>();
}

public GS2GC_030_063_OnGiftPackBuyRecordRemove(
	 java.util.ArrayList<Long> _giftPackIdList
) {	giftPackIdList = _giftPackIdList;
}

public final byte getMainOrder() { return (byte)30; }

public final byte getSubOrder() { return (byte)63; }

/** 礼包id列表 */
public java.util.ArrayList<Long> getGiftPackIdList() { return giftPackIdList; }
/** 礼包id列表 */
public void addGiftPackIdList(long _giftPackIdList) { giftPackIdList.add(_giftPackIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (giftPackIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (giftPackIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _giftPackIdListCount = _buf.getShort();
	for(int _i = 0; _i < _giftPackIdListCount; _i++) { 
		long _giftPackIdList = (long)0;
		if(_buf.remaining() > 0) _giftPackIdList = _buf.getLong();
		giftPackIdList.add(_giftPackIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)giftPackIdList.size());
	for(int _i = 0; _i < giftPackIdList.size(); _i++) { 
		_buf.putLong(giftPackIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)63);
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

