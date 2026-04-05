package Common.ConsortObj;

import java.nio.ByteBuffer;
/*********
 * 家人CG数据
 **/
public class Consort_CGInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 家人CG */
private long cgId;
/** 是否领取奖励 */
private boolean rewarded;


public Consort_CGInfo() {
	cgId = (long)0;
	rewarded = false;
}

public Consort_CGInfo(
	 long _cgId
	, boolean _rewarded
) {	cgId = _cgId;
	rewarded = _rewarded;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 家人CG */
public long getCgId() { return cgId; }
/** 家人CG */
public void setCgId(long _cgId) { cgId = _cgId; }
/** 是否领取奖励 */
public boolean getRewarded() { return rewarded; }
/** 是否领取奖励 */
public void setRewarded(boolean _rewarded) { rewarded = _rewarded; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cgId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rewarded = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cgId);
	_buf.put(rewarded?(byte)1:(byte)0);
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

