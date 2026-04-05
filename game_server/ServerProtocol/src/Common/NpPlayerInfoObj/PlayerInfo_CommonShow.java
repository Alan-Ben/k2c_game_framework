package Common.NpPlayerInfoObj;

import java.nio.ByteBuffer;
/*********
 * 玩家通用展示信息
 **/
public class PlayerInfo_CommonShow implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家id */
private long cid;
/** 小头像组合信息 */
private NPCommon.PlayerInfo_IconShow iconShow;
/** 属性列表 */
private Common.Common_LongList attrList;
/** Q版形象id */
private long cuteActorId;
/** 被点赞次数 */
private long beLikeCount;
/** 经验值 */
private long exp;
/** vip经验 */
private long vipExp;
/** 历史最大战力 */
private long maxPower;


public PlayerInfo_CommonShow() {
	cid = (long)0;
	iconShow = new NPCommon.PlayerInfo_IconShow();
	attrList = new Common.Common_LongList();
	cuteActorId = (long)0;
	beLikeCount = (long)0;
	exp = (long)0;
	vipExp = (long)0;
	maxPower = (long)0;
}

public PlayerInfo_CommonShow(
	 long _cid
	, NPCommon.PlayerInfo_IconShow _iconShow
	, Common.Common_LongList _attrList
	, long _cuteActorId
	, long _beLikeCount
	, long _exp
	, long _vipExp
	, long _maxPower
) {	cid = _cid;
	iconShow = _iconShow;
	attrList = _attrList;
	cuteActorId = _cuteActorId;
	beLikeCount = _beLikeCount;
	exp = _exp;
	vipExp = _vipExp;
	maxPower = _maxPower;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家id */
public long getCid() { return cid; }
/** 玩家id */
public void setCid(long _cid) { cid = _cid; }
/** 小头像组合信息 */
public NPCommon.PlayerInfo_IconShow getIconShow() { return iconShow; }
/** 小头像组合信息 */
public void setIconShow(NPCommon.PlayerInfo_IconShow _iconShow) { iconShow = _iconShow; }
/** 属性列表 */
public Common.Common_LongList getAttrList() { return attrList; }
/** 属性列表 */
public void setAttrList(Common.Common_LongList _attrList) { attrList = _attrList; }
/** Q版形象id */
public long getCuteActorId() { return cuteActorId; }
/** Q版形象id */
public void setCuteActorId(long _cuteActorId) { cuteActorId = _cuteActorId; }
/** 被点赞次数 */
public long getBeLikeCount() { return beLikeCount; }
/** 被点赞次数 */
public void setBeLikeCount(long _beLikeCount) { beLikeCount = _beLikeCount; }
/** 经验值 */
public long getExp() { return exp; }
/** 经验值 */
public void setExp(long _exp) { exp = _exp; }
/** vip经验 */
public long getVipExp() { return vipExp; }
/** vip经验 */
public void setVipExp(long _vipExp) { vipExp = _vipExp; }
/** 历史最大战力 */
public long getMaxPower() { return maxPower; }
/** 历史最大战力 */
public void setMaxPower(long _maxPower) { maxPower = _maxPower; }


public final int GetBufSize() {
	int _size = 48;
	_size += 4 + iconShow.GetBufSize();
	_size += 4 + attrList.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 50;
	_size += 4 + iconShow.GetBufSize();
	_size += 4 + attrList.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _iconShowCustLen = _buf.getInt();
	int _iconShowCurPos = _buf.position();
	iconShow.ReadUnzipBuf(_buf, _iconShowCurPos + _iconShowCustLen);
	_buf.position(_iconShowCurPos + _iconShowCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _attrListCustLen = _buf.getInt();
	int _attrListCurPos = _buf.position();
	attrList.ReadUnzipBuf(_buf, _attrListCurPos + _attrListCustLen);
	_buf.position(_attrListCurPos + _attrListCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cuteActorId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) beLikeCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) exp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) vipExp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxPower = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putInt(iconShow.GetBufSize());
	iconShow.PutUnzipBuf(_buf);
	_buf.putInt(attrList.GetBufSize());
	attrList.PutUnzipBuf(_buf);
	_buf.putLong(cuteActorId);
	_buf.putLong(beLikeCount);
	_buf.putLong(exp);
	_buf.putLong(vipExp);
	_buf.putLong(maxPower);
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

