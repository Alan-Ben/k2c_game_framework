package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 冲榜礼包信息变更
 **/
public class GS2GC_004_064_OnRankGiftPackChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 冲榜礼包信息 */
private Common.RankGiftPackObj.RankGiftPack_Info giftPackInfo;


public GS2GC_004_064_OnRankGiftPackChg() {
	giftPackInfo = new Common.RankGiftPackObj.RankGiftPack_Info();
}

public GS2GC_004_064_OnRankGiftPackChg(
	 Common.RankGiftPackObj.RankGiftPack_Info _giftPackInfo
) {	giftPackInfo = _giftPackInfo;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)64; }

/** 冲榜礼包信息 */
public Common.RankGiftPackObj.RankGiftPack_Info getGiftPackInfo() { return giftPackInfo; }
/** 冲榜礼包信息 */
public void setGiftPackInfo(Common.RankGiftPackObj.RankGiftPack_Info _giftPackInfo) { giftPackInfo = _giftPackInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + giftPackInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + giftPackInfo.GetBufSize();

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
	_buf.put((byte)4);
	_buf.put((byte)64);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)64);
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

