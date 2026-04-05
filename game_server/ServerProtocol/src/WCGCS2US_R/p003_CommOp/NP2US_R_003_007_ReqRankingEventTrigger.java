package WCGCS2US_R.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 排行榜事件触发
 **/
public class NP2US_R_003_007_ReqRankingEventTrigger implements ALBasicProtocolPack._IALProtocolStructure {
private long dealSerial;
private long cid;
private long scoreSourceId;
private long chgValue;


public NP2US_R_003_007_ReqRankingEventTrigger() {
	dealSerial = (long)0;
	cid = (long)0;
	scoreSourceId = (long)0;
	chgValue = (long)0;
}

public NP2US_R_003_007_ReqRankingEventTrigger(
	 long _dealSerial
	, long _cid
	, long _scoreSourceId
	, long _chgValue
) {	dealSerial = _dealSerial;
	cid = _cid;
	scoreSourceId = _scoreSourceId;
	chgValue = _chgValue;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)7; }

public long getDealSerial() { return dealSerial; }
public void setDealSerial(long _dealSerial) { dealSerial = _dealSerial; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getScoreSourceId() { return scoreSourceId; }
public void setScoreSourceId(long _scoreSourceId) { scoreSourceId = _scoreSourceId; }
public long getChgValue() { return chgValue; }
public void setChgValue(long _chgValue) { chgValue = _chgValue; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) scoreSourceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chgValue = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dealSerial);
	_buf.putLong(cid);
	_buf.putLong(scoreSourceId);
	_buf.putLong(chgValue);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)7);
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

