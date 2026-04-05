package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 开宴玩家结算数据
 **/
public class ServerObj_DinnerResult implements ALBasicProtocolPack._IALProtocolStructure {
/** 开宴玩家CID */
private long ownerCid;
/** 开始时间戳（秒） */
private int startTs;
/** 宴会结算数据 */
private Common.DinnerObj.Dinner_ResultInfo result;


public ServerObj_DinnerResult() {
	ownerCid = (long)0;
	startTs = 0;
	result = new Common.DinnerObj.Dinner_ResultInfo();
}

public ServerObj_DinnerResult(
	 long _ownerCid
	, int _startTs
	, Common.DinnerObj.Dinner_ResultInfo _result
) {	ownerCid = _ownerCid;
	startTs = _startTs;
	result = _result;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 开宴玩家CID */
public long getOwnerCid() { return ownerCid; }
/** 开宴玩家CID */
public void setOwnerCid(long _ownerCid) { ownerCid = _ownerCid; }
/** 开始时间戳（秒） */
public int getStartTs() { return startTs; }
/** 开始时间戳（秒） */
public void setStartTs(int _startTs) { startTs = _startTs; }
/** 宴会结算数据 */
public Common.DinnerObj.Dinner_ResultInfo getResult() { return result; }
/** 宴会结算数据 */
public void setResult(Common.DinnerObj.Dinner_ResultInfo _result) { result = _result; }


public final int GetBufSize() {
	int _size = 12;
	_size += 4 + result.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + result.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) ownerCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _resultCustLen = _buf.getInt();
	int _resultCurPos = _buf.position();
	result.ReadUnzipBuf(_buf, _resultCurPos + _resultCustLen);
	_buf.position(_resultCurPos + _resultCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(ownerCid);
	_buf.putInt(startTs);
	_buf.putInt(result.GetBufSize());
	result.PutUnzipBuf(_buf);
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

