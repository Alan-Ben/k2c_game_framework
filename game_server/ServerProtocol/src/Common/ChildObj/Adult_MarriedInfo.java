package Common.ChildObj;

import java.nio.ByteBuffer;
/*********
 * 子嗣（成年已婚）数据
 **/
public class Adult_MarriedInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 子嗣数据 */
private Common.ChildObj.Adult_Info adult;
/** 结婚对象玩家CID */
private long marriedCid;
/** 结婚对象子嗣数据 */
private Common.ChildObj.Adult_Info marriedAdult;
/** 结婚时间（秒） */
private int marriedTs;


public Adult_MarriedInfo() {
	adult = new Common.ChildObj.Adult_Info();
	marriedCid = (long)0;
	marriedAdult = new Common.ChildObj.Adult_Info();
	marriedTs = 0;
}

public Adult_MarriedInfo(
	 Common.ChildObj.Adult_Info _adult
	, long _marriedCid
	, Common.ChildObj.Adult_Info _marriedAdult
	, int _marriedTs
) {	adult = _adult;
	marriedCid = _marriedCid;
	marriedAdult = _marriedAdult;
	marriedTs = _marriedTs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 子嗣数据 */
public Common.ChildObj.Adult_Info getAdult() { return adult; }
/** 子嗣数据 */
public void setAdult(Common.ChildObj.Adult_Info _adult) { adult = _adult; }
/** 结婚对象玩家CID */
public long getMarriedCid() { return marriedCid; }
/** 结婚对象玩家CID */
public void setMarriedCid(long _marriedCid) { marriedCid = _marriedCid; }
/** 结婚对象子嗣数据 */
public Common.ChildObj.Adult_Info getMarriedAdult() { return marriedAdult; }
/** 结婚对象子嗣数据 */
public void setMarriedAdult(Common.ChildObj.Adult_Info _marriedAdult) { marriedAdult = _marriedAdult; }
/** 结婚时间（秒） */
public int getMarriedTs() { return marriedTs; }
/** 结婚时间（秒） */
public void setMarriedTs(int _marriedTs) { marriedTs = _marriedTs; }


public final int GetBufSize() {
	int _size = 12;
	_size += 4 + adult.GetBufSize();
	_size += 4 + marriedAdult.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + adult.GetBufSize();
	_size += 4 + marriedAdult.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _adultCustLen = _buf.getInt();
	int _adultCurPos = _buf.position();
	adult.ReadUnzipBuf(_buf, _adultCurPos + _adultCustLen);
	_buf.position(_adultCurPos + _adultCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) marriedCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _marriedAdultCustLen = _buf.getInt();
	int _marriedAdultCurPos = _buf.position();
	marriedAdult.ReadUnzipBuf(_buf, _marriedAdultCurPos + _marriedAdultCustLen);
	_buf.position(_marriedAdultCurPos + _marriedAdultCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) marriedTs = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(adult.GetBufSize());
	adult.PutUnzipBuf(_buf);
	_buf.putLong(marriedCid);
	_buf.putInt(marriedAdult.GetBufSize());
	marriedAdult.PutUnzipBuf(_buf);
	_buf.putInt(marriedTs);
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

