package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 联姻子嗣结婚数据
 **/
public class ServerObj_AdultMarriedInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家CID */
private long cid;
/** 玩家子嗣实例ID */
private long adultId;
/** 联姻玩家CID */
private long marriedCid;
/** 联姻子嗣数据 */
private Common.ChildObj.Adult_Info marriedAdult;
/** 联姻奖励 */
private NPCommon.NPCommon_ItemInfo marriedItem;
/** 联姻玩家昵称 */
private String marriedCname;


public ServerObj_AdultMarriedInfo() {
	cid = (long)0;
	adultId = (long)0;
	marriedCid = (long)0;
	marriedAdult = new Common.ChildObj.Adult_Info();
	marriedItem = new NPCommon.NPCommon_ItemInfo();
	marriedCname = "";
}

public ServerObj_AdultMarriedInfo(
	 long _cid
	, long _adultId
	, long _marriedCid
	, Common.ChildObj.Adult_Info _marriedAdult
	, NPCommon.NPCommon_ItemInfo _marriedItem
	, String _marriedCname
) {	cid = _cid;
	adultId = _adultId;
	marriedCid = _marriedCid;
	marriedAdult = _marriedAdult;
	marriedItem = _marriedItem;
	marriedCname = _marriedCname;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家CID */
public long getCid() { return cid; }
/** 玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 玩家子嗣实例ID */
public long getAdultId() { return adultId; }
/** 玩家子嗣实例ID */
public void setAdultId(long _adultId) { adultId = _adultId; }
/** 联姻玩家CID */
public long getMarriedCid() { return marriedCid; }
/** 联姻玩家CID */
public void setMarriedCid(long _marriedCid) { marriedCid = _marriedCid; }
/** 联姻子嗣数据 */
public Common.ChildObj.Adult_Info getMarriedAdult() { return marriedAdult; }
/** 联姻子嗣数据 */
public void setMarriedAdult(Common.ChildObj.Adult_Info _marriedAdult) { marriedAdult = _marriedAdult; }
/** 联姻奖励 */
public NPCommon.NPCommon_ItemInfo getMarriedItem() { return marriedItem; }
/** 联姻奖励 */
public void setMarriedItem(NPCommon.NPCommon_ItemInfo _marriedItem) { marriedItem = _marriedItem; }
/** 联姻玩家昵称 */
public String getMarriedCname() { return marriedCname; }
/** 联姻玩家昵称 */
public void setMarriedCname(String _marriedCname) { marriedCname = _marriedCname; }


public final int GetBufSize() {
	int _size = 24;
	_size += 4 + marriedAdult.GetBufSize();
	_size += 4 + marriedItem.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(marriedCname);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;
	_size += 4 + marriedAdult.GetBufSize();
	_size += 4 + marriedItem.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(marriedCname);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) adultId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) marriedCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _marriedAdultCustLen = _buf.getInt();
	int _marriedAdultCurPos = _buf.position();
	marriedAdult.ReadUnzipBuf(_buf, _marriedAdultCurPos + _marriedAdultCustLen);
	_buf.position(_marriedAdultCurPos + _marriedAdultCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _marriedItemCustLen = _buf.getInt();
	int _marriedItemCurPos = _buf.position();
	marriedItem.ReadUnzipBuf(_buf, _marriedItemCurPos + _marriedItemCustLen);
	_buf.position(_marriedItemCurPos + _marriedItemCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) marriedCname = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(adultId);
	_buf.putLong(marriedCid);
	_buf.putInt(marriedAdult.GetBufSize());
	marriedAdult.PutUnzipBuf(_buf);
	_buf.putInt(marriedItem.GetBufSize());
	marriedItem.PutUnzipBuf(_buf);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, marriedCname);
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

