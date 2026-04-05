package GS2GC.p036_TreasureHuntOp;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-获得奇物
 **/
public class GS2GC_036_051_OnTreasureHuntTreasureAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 奇物信息 */
private Common.TreasureHuntObj.TreasureHunt_TreasureInfo treasureInfo;


public GS2GC_036_051_OnTreasureHuntTreasureAdd() {
	treasureInfo = new Common.TreasureHuntObj.TreasureHunt_TreasureInfo();
}

public GS2GC_036_051_OnTreasureHuntTreasureAdd(
	 Common.TreasureHuntObj.TreasureHunt_TreasureInfo _treasureInfo
) {	treasureInfo = _treasureInfo;
}

public final byte getMainOrder() { return (byte)36; }

public final byte getSubOrder() { return (byte)51; }

/** 奇物信息 */
public Common.TreasureHuntObj.TreasureHunt_TreasureInfo getTreasureInfo() { return treasureInfo; }
/** 奇物信息 */
public void setTreasureInfo(Common.TreasureHuntObj.TreasureHunt_TreasureInfo _treasureInfo) { treasureInfo = _treasureInfo; }


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
	int _treasureInfoCustLen = _buf.getInt();
	int _treasureInfoCurPos = _buf.position();
	treasureInfo.ReadUnzipBuf(_buf, _treasureInfoCurPos + _treasureInfoCustLen);
	_buf.position(_treasureInfoCurPos + _treasureInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(treasureInfo.GetBufSize());
	treasureInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)51);
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

