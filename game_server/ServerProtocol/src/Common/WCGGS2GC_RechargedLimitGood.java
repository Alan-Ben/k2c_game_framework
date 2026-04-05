package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_RechargedLimitGood implements ALBasicProtocolPack._IALProtocolStructure {
private long goodId;
private int rechargedNum;
private int limitNum;
private int restRemainSec;


public WCGGS2GC_RechargedLimitGood() {
	goodId = (long)0;
	rechargedNum = 0;
	limitNum = 0;
	restRemainSec = 0;
}

public WCGGS2GC_RechargedLimitGood(
	 long _goodId
	, int _rechargedNum
	, int _limitNum
	, int _restRemainSec
) {	goodId = _goodId;
	rechargedNum = _rechargedNum;
	limitNum = _limitNum;
	restRemainSec = _restRemainSec;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getGoodId() { return goodId; }
public void setGoodId(long _goodId) { goodId = _goodId; }
public int getRechargedNum() { return rechargedNum; }
public void setRechargedNum(int _rechargedNum) { rechargedNum = _rechargedNum; }
public int getLimitNum() { return limitNum; }
public void setLimitNum(int _limitNum) { limitNum = _limitNum; }
public int getRestRemainSec() { return restRemainSec; }
public void setRestRemainSec(int _restRemainSec) { restRemainSec = _restRemainSec; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) goodId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rechargedNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) limitNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) restRemainSec = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(goodId);
	_buf.putInt(rechargedNum);
	_buf.putInt(limitNum);
	_buf.putInt(restRemainSec);
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

