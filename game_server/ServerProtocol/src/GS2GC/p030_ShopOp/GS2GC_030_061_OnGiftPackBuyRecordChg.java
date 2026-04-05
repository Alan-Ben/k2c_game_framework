package GS2GC.p030_ShopOp;

import java.nio.ByteBuffer;
/*********
 * 礼包变更推送
 **/
public class GS2GC_030_061_OnGiftPackBuyRecordChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 礼包信息 */
private Common.CommonFuncObj.GiftPack_Info giftPackInfo;


public GS2GC_030_061_OnGiftPackBuyRecordChg() {
	giftPackInfo = new Common.CommonFuncObj.GiftPack_Info();
}

public GS2GC_030_061_OnGiftPackBuyRecordChg(
	 Common.CommonFuncObj.GiftPack_Info _giftPackInfo
) {	giftPackInfo = _giftPackInfo;
}

public final byte getMainOrder() { return (byte)30; }

public final byte getSubOrder() { return (byte)61; }

/** 礼包信息 */
public Common.CommonFuncObj.GiftPack_Info getGiftPackInfo() { return giftPackInfo; }
/** 礼包信息 */
public void setGiftPackInfo(Common.CommonFuncObj.GiftPack_Info _giftPackInfo) { giftPackInfo = _giftPackInfo; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _giftPackInfoCustLen = _buf.getInt();
	int _giftPackInfoCurPos = _buf.position();
	giftPackInfo.ReadUnzipBuf(_buf, _giftPackInfoCurPos + _giftPackInfoCustLen);
	_buf.position(_giftPackInfoCurPos + _giftPackInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(giftPackInfo.GetBufSize());
	giftPackInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)30);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)30);
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

