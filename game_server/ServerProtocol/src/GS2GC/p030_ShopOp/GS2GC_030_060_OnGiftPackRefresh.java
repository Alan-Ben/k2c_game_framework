package GS2GC.p030_ShopOp;

import java.nio.ByteBuffer;
/*********
 * 礼包刷新推送
 **/
public class GS2GC_030_060_OnGiftPackRefresh implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包信息列表 */
private java.util.ArrayList<Common.CommonFuncObj.GiftPack_Info> giftPack;


public GS2GC_030_060_OnGiftPackRefresh() {
	giftPack = new java.util.ArrayList<Common.CommonFuncObj.GiftPack_Info>();
}

public GS2GC_030_060_OnGiftPackRefresh(
	 java.util.ArrayList<Common.CommonFuncObj.GiftPack_Info> _giftPack
) {	giftPack = _giftPack;
}

public final byte getMainOrder() { return (byte)30; }

public final byte getSubOrder() { return (byte)60; }

/** 礼包信息列表 */
public java.util.ArrayList<Common.CommonFuncObj.GiftPack_Info> getGiftPack() { return giftPack; }
/** 礼包信息列表 */
public void addGiftPack(Common.CommonFuncObj.GiftPack_Info _giftPack) { giftPack.add(_giftPack); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (giftPack.size() * 24);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (giftPack.size() * 24);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _giftPackCount = _buf.getShort();
	for(int _i = 0; _i < _giftPackCount; _i++) { 
		Common.CommonFuncObj.GiftPack_Info _giftPack = new Common.CommonFuncObj.GiftPack_Info();
		if(_buf.remaining() <= 0) return;
	int __giftPackCustLen = _buf.getInt();
	int __giftPackCurPos = _buf.position();
	_giftPack.ReadUnzipBuf(_buf, __giftPackCurPos + __giftPackCustLen);
	_buf.position(__giftPackCurPos + __giftPackCustLen);

		giftPack.add(_giftPack);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)giftPack.size());
	for(int _i = 0; _i < giftPack.size(); _i++) { 
		_buf.putInt(giftPack.get(_i).GetBufSize());
	giftPack.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
	_recBuf.put((byte)60);
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

