package Common.TreasureHuntObj;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-矿石数量信息
 **/
public class TreasureHunt_OreNumInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 已获得数量 */
private int totalGainNum;
/** 普通矿石待处理数量 */
private int normalPendingNum;
/** 高级矿石待处理数量 */
private int advancedPendingNum;


public TreasureHunt_OreNumInfo() {
	totalGainNum = 0;
	normalPendingNum = 0;
	advancedPendingNum = 0;
}

public TreasureHunt_OreNumInfo(
	 int _totalGainNum
	, int _normalPendingNum
	, int _advancedPendingNum
) {	totalGainNum = _totalGainNum;
	normalPendingNum = _normalPendingNum;
	advancedPendingNum = _advancedPendingNum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 已获得数量 */
public int getTotalGainNum() { return totalGainNum; }
/** 已获得数量 */
public void setTotalGainNum(int _totalGainNum) { totalGainNum = _totalGainNum; }
/** 普通矿石待处理数量 */
public int getNormalPendingNum() { return normalPendingNum; }
/** 普通矿石待处理数量 */
public void setNormalPendingNum(int _normalPendingNum) { normalPendingNum = _normalPendingNum; }
/** 高级矿石待处理数量 */
public int getAdvancedPendingNum() { return advancedPendingNum; }
/** 高级矿石待处理数量 */
public void setAdvancedPendingNum(int _advancedPendingNum) { advancedPendingNum = _advancedPendingNum; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalGainNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) normalPendingNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) advancedPendingNum = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(totalGainNum);
	_buf.putInt(normalPendingNum);
	_buf.putInt(advancedPendingNum);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

