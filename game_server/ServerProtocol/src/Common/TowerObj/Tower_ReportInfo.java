package Common.TowerObj;

import java.nio.ByteBuffer;
/*********
 * 爬塔_战报信息
 **/
public class Tower_ReportInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 所在位置 */
private Common.TowerObj.Tower_PosInfo posInfo;
/** 攻击者Cid */
private long attackerCid;
/** 攻击是否成功 */
private boolean isSucc;
/** 下降层数 */
private int downLevel;
/** 时间戳 毫秒 */
private long timestamp;


public Tower_ReportInfo() {
	posInfo = new Common.TowerObj.Tower_PosInfo();
	attackerCid = (long)0;
	isSucc = false;
	downLevel = 0;
	timestamp = (long)0;
}

public Tower_ReportInfo(
	 Common.TowerObj.Tower_PosInfo _posInfo
	, long _attackerCid
	, boolean _isSucc
	, int _downLevel
	, long _timestamp
) {	posInfo = _posInfo;
	attackerCid = _attackerCid;
	isSucc = _isSucc;
	downLevel = _downLevel;
	timestamp = _timestamp;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 所在位置 */
public Common.TowerObj.Tower_PosInfo getPosInfo() { return posInfo; }
/** 所在位置 */
public void setPosInfo(Common.TowerObj.Tower_PosInfo _posInfo) { posInfo = _posInfo; }
/** 攻击者Cid */
public long getAttackerCid() { return attackerCid; }
/** 攻击者Cid */
public void setAttackerCid(long _attackerCid) { attackerCid = _attackerCid; }
/** 攻击是否成功 */
public boolean getIsSucc() { return isSucc; }
/** 攻击是否成功 */
public void setIsSucc(boolean _isSucc) { isSucc = _isSucc; }
/** 下降层数 */
public int getDownLevel() { return downLevel; }
/** 下降层数 */
public void setDownLevel(int _downLevel) { downLevel = _downLevel; }
/** 时间戳 毫秒 */
public long getTimestamp() { return timestamp; }
/** 时间戳 毫秒 */
public void setTimestamp(long _timestamp) { timestamp = _timestamp; }


public final int GetBufSize() {
	int _size = 37;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 39;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _posInfoCustLen = _buf.getInt();
	int _posInfoCurPos = _buf.position();
	posInfo.ReadUnzipBuf(_buf, _posInfoCurPos + _posInfoCustLen);
	_buf.position(_posInfoCurPos + _posInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attackerCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isSucc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) downLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timestamp = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(posInfo.GetBufSize());
	posInfo.PutUnzipBuf(_buf);
	_buf.putLong(attackerCid);
	_buf.put(isSucc?(byte)1:(byte)0);
	_buf.putInt(downLevel);
	_buf.putLong(timestamp);
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

