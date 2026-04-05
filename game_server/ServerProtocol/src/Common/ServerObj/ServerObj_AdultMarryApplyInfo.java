package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 子嗣联姻请求数据
 **/
public class ServerObj_AdultMarryApplyInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 申请玩家CID */
private long applyCid;
/** 请求联姻子嗣数据 */
private Common.ChildObj.Adult_Info applyAdult;
/** 请求超时时间 */
private int applyExpiredTs;
/** 请求目标玩家CID */
private long targetCid;
/** 联姻奖励 */
private NPCommon.NPCommon_ItemInfo marriedItem;
/** 申请玩家昵称 */
private String applyCname;


public ServerObj_AdultMarryApplyInfo() {
	applyCid = (long)0;
	applyAdult = new Common.ChildObj.Adult_Info();
	applyExpiredTs = 0;
	targetCid = (long)0;
	marriedItem = new NPCommon.NPCommon_ItemInfo();
	applyCname = "";
}

public ServerObj_AdultMarryApplyInfo(
	 long _applyCid
	, Common.ChildObj.Adult_Info _applyAdult
	, int _applyExpiredTs
	, long _targetCid
	, NPCommon.NPCommon_ItemInfo _marriedItem
	, String _applyCname
) {	applyCid = _applyCid;
	applyAdult = _applyAdult;
	applyExpiredTs = _applyExpiredTs;
	targetCid = _targetCid;
	marriedItem = _marriedItem;
	applyCname = _applyCname;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 申请玩家CID */
public long getApplyCid() { return applyCid; }
/** 申请玩家CID */
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/** 请求联姻子嗣数据 */
public Common.ChildObj.Adult_Info getApplyAdult() { return applyAdult; }
/** 请求联姻子嗣数据 */
public void setApplyAdult(Common.ChildObj.Adult_Info _applyAdult) { applyAdult = _applyAdult; }
/** 请求超时时间 */
public int getApplyExpiredTs() { return applyExpiredTs; }
/** 请求超时时间 */
public void setApplyExpiredTs(int _applyExpiredTs) { applyExpiredTs = _applyExpiredTs; }
/** 请求目标玩家CID */
public long getTargetCid() { return targetCid; }
/** 请求目标玩家CID */
public void setTargetCid(long _targetCid) { targetCid = _targetCid; }
/** 联姻奖励 */
public NPCommon.NPCommon_ItemInfo getMarriedItem() { return marriedItem; }
/** 联姻奖励 */
public void setMarriedItem(NPCommon.NPCommon_ItemInfo _marriedItem) { marriedItem = _marriedItem; }
/** 申请玩家昵称 */
public String getApplyCname() { return applyCname; }
/** 申请玩家昵称 */
public void setApplyCname(String _applyCname) { applyCname = _applyCname; }


public final int GetBufSize() {
	int _size = 20;
	_size += 4 + applyAdult.GetBufSize();
	_size += 4 + marriedItem.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(applyCname);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += 4 + applyAdult.GetBufSize();
	_size += 4 + marriedItem.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(applyCname);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _applyAdultCustLen = _buf.getInt();
	int _applyAdultCurPos = _buf.position();
	applyAdult.ReadUnzipBuf(_buf, _applyAdultCurPos + _applyAdultCustLen);
	_buf.position(_applyAdultCurPos + _applyAdultCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyExpiredTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) targetCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _marriedItemCustLen = _buf.getInt();
	int _marriedItemCurPos = _buf.position();
	marriedItem.ReadUnzipBuf(_buf, _marriedItemCurPos + _marriedItemCustLen);
	_buf.position(_marriedItemCurPos + _marriedItemCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyCname = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(applyCid);
	_buf.putInt(applyAdult.GetBufSize());
	applyAdult.PutUnzipBuf(_buf);
	_buf.putInt(applyExpiredTs);
	_buf.putLong(targetCid);
	_buf.putInt(marriedItem.GetBufSize());
	marriedItem.PutUnzipBuf(_buf);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, applyCname);
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

