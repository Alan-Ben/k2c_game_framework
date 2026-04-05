package GS2GC.p036_TreasureHuntOp;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-矿石数量变更
 **/
public class GS2GC_036_054_OnTreasureHuntOreNumChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿石ID */
private long oreId;
/** 矿石数量信息 */
private Common.TreasureHuntObj.TreasureHunt_OreNumInfo numInfo;


public GS2GC_036_054_OnTreasureHuntOreNumChg() {
	oreId = (long)0;
	numInfo = new Common.TreasureHuntObj.TreasureHunt_OreNumInfo();
}

public GS2GC_036_054_OnTreasureHuntOreNumChg(
	 long _oreId
	, Common.TreasureHuntObj.TreasureHunt_OreNumInfo _numInfo
) {	oreId = _oreId;
	numInfo = _numInfo;
}

public final byte getMainOrder() { return (byte)36; }

public final byte getSubOrder() { return (byte)54; }

/** 矿石ID */
public long getOreId() { return oreId; }
/** 矿石ID */
public void setOreId(long _oreId) { oreId = _oreId; }
/** 矿石数量信息 */
public Common.TreasureHuntObj.TreasureHunt_OreNumInfo getNumInfo() { return numInfo; }
/** 矿石数量信息 */
public void setNumInfo(Common.TreasureHuntObj.TreasureHunt_OreNumInfo _numInfo) { numInfo = _numInfo; }


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
	if(_buf.remaining() > 0) oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _numInfoCustLen = _buf.getInt();
	int _numInfoCurPos = _buf.position();
	numInfo.ReadUnzipBuf(_buf, _numInfoCurPos + _numInfoCustLen);
	_buf.position(_numInfoCurPos + _numInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(oreId);
	_buf.putInt(numInfo.GetBufSize());
	numInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)54);
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

