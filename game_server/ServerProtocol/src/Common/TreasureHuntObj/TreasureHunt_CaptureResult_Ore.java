package Common.TreasureHuntObj;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-捕捉结果_矿石
 **/
public class TreasureHunt_CaptureResult_Ore implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿石ID */
private long oreId;
/** 重量 */
private int weight;
/** 是否首次捕捉 */
private boolean isFirstCapture;
/** 最大矿石归属者cid */
private long serverMaxCid;
/** 最大矿石重量 */
private int serverMaxWeight;
/** 是否首次捕捉高级 */
private boolean isFirstDrawAdvanced;


public TreasureHunt_CaptureResult_Ore() {
	oreId = (long)0;
	weight = 0;
	isFirstCapture = false;
	serverMaxCid = (long)0;
	serverMaxWeight = 0;
	isFirstDrawAdvanced = false;
}

public TreasureHunt_CaptureResult_Ore(
	 long _oreId
	, int _weight
	, boolean _isFirstCapture
	, long _serverMaxCid
	, int _serverMaxWeight
	, boolean _isFirstDrawAdvanced
) {	oreId = _oreId;
	weight = _weight;
	isFirstCapture = _isFirstCapture;
	serverMaxCid = _serverMaxCid;
	serverMaxWeight = _serverMaxWeight;
	isFirstDrawAdvanced = _isFirstDrawAdvanced;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 矿石ID */
public long getOreId() { return oreId; }
/** 矿石ID */
public void setOreId(long _oreId) { oreId = _oreId; }
/** 重量 */
public int getWeight() { return weight; }
/** 重量 */
public void setWeight(int _weight) { weight = _weight; }
/** 是否首次捕捉 */
public boolean getIsFirstCapture() { return isFirstCapture; }
/** 是否首次捕捉 */
public void setIsFirstCapture(boolean _isFirstCapture) { isFirstCapture = _isFirstCapture; }
/** 最大矿石归属者cid */
public long getServerMaxCid() { return serverMaxCid; }
/** 最大矿石归属者cid */
public void setServerMaxCid(long _serverMaxCid) { serverMaxCid = _serverMaxCid; }
/** 最大矿石重量 */
public int getServerMaxWeight() { return serverMaxWeight; }
/** 最大矿石重量 */
public void setServerMaxWeight(int _serverMaxWeight) { serverMaxWeight = _serverMaxWeight; }
/** 是否首次捕捉高级 */
public boolean getIsFirstDrawAdvanced() { return isFirstDrawAdvanced; }
/** 是否首次捕捉高级 */
public void setIsFirstDrawAdvanced(boolean _isFirstDrawAdvanced) { isFirstDrawAdvanced = _isFirstDrawAdvanced; }


public final int GetBufSize() {
	int _size = 26;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 28;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) weight = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isFirstCapture = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverMaxCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverMaxWeight = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isFirstDrawAdvanced = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(oreId);
	_buf.putInt(weight);
	_buf.put(isFirstCapture?(byte)1:(byte)0);
	_buf.putLong(serverMaxCid);
	_buf.putInt(serverMaxWeight);
	_buf.put(isFirstDrawAdvanced?(byte)1:(byte)0);
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

