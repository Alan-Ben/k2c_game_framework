package GS2GC.p036_TreasureHuntOp;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-新增矿石
 **/
public class GS2GC_036_053_OnTreasureHuntOreAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿石信息 */
private Common.TreasureHuntObj.TreasureHunt_OreInfo oreInfo;


public GS2GC_036_053_OnTreasureHuntOreAdd() {
	oreInfo = new Common.TreasureHuntObj.TreasureHunt_OreInfo();
}

public GS2GC_036_053_OnTreasureHuntOreAdd(
	 Common.TreasureHuntObj.TreasureHunt_OreInfo _oreInfo
) {	oreInfo = _oreInfo;
}

public final byte getMainOrder() { return (byte)36; }

public final byte getSubOrder() { return (byte)53; }

/** 矿石信息 */
public Common.TreasureHuntObj.TreasureHunt_OreInfo getOreInfo() { return oreInfo; }
/** 矿石信息 */
public void setOreInfo(Common.TreasureHuntObj.TreasureHunt_OreInfo _oreInfo) { oreInfo = _oreInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + oreInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + oreInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _oreInfoCustLen = _buf.getInt();
	int _oreInfoCurPos = _buf.position();
	oreInfo.ReadUnzipBuf(_buf, _oreInfoCurPos + _oreInfoCustLen);
	_buf.position(_oreInfoCurPos + _oreInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(oreInfo.GetBufSize());
	oreInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)53);
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

