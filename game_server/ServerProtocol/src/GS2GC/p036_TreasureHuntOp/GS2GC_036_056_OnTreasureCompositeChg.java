package GS2GC.p036_TreasureHuntOp;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-矿石组合变更
 **/
public class GS2GC_036_056_OnTreasureCompositeChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 组合信息 */
private Common.TreasureHuntObj.TreasureHunt_CompositeInfo compositeInfo;


public GS2GC_036_056_OnTreasureCompositeChg() {
	compositeInfo = new Common.TreasureHuntObj.TreasureHunt_CompositeInfo();
}

public GS2GC_036_056_OnTreasureCompositeChg(
	 Common.TreasureHuntObj.TreasureHunt_CompositeInfo _compositeInfo
) {	compositeInfo = _compositeInfo;
}

public final byte getMainOrder() { return (byte)36; }

public final byte getSubOrder() { return (byte)56; }

/** 组合信息 */
public Common.TreasureHuntObj.TreasureHunt_CompositeInfo getCompositeInfo() { return compositeInfo; }
/** 组合信息 */
public void setCompositeInfo(Common.TreasureHuntObj.TreasureHunt_CompositeInfo _compositeInfo) { compositeInfo = _compositeInfo; }


public final int GetBufSize() {
	int _size = 22;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 24;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _compositeInfoCustLen = _buf.getInt();
	int _compositeInfoCurPos = _buf.position();
	compositeInfo.ReadUnzipBuf(_buf, _compositeInfoCurPos + _compositeInfoCustLen);
	_buf.position(_compositeInfoCurPos + _compositeInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(compositeInfo.GetBufSize());
	compositeInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)56);
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

