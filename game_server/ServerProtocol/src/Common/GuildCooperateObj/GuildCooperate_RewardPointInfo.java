package Common.GuildCooperateObj;

import java.nio.ByteBuffer;
/*********
 * 联盟协作奖励据点信息
 **/
public class GuildCooperate_RewardPointInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 位置 */
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos pos;
/** 奖励据点ID */
private long posId;
/** 是否已解锁 */
private boolean isUnlock;
/** 盟主cid */
private long leaderCid;
/** 属性据点信息列表 */
private java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo> propertyPointList;


public GuildCooperate_RewardPointInfo() {
	pos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
	posId = (long)0;
	isUnlock = false;
	leaderCid = (long)0;
	propertyPointList = new java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo>();
}

public GuildCooperate_RewardPointInfo(
	 Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos
	, long _posId
	, boolean _isUnlock
	, long _leaderCid
	, java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo> _propertyPointList
) {	pos = _pos;
	posId = _posId;
	isUnlock = _isUnlock;
	leaderCid = _leaderCid;
	propertyPointList = _propertyPointList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 位置 */
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getPos() { return pos; }
/** 位置 */
public void setPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos) { pos = _pos; }
/** 奖励据点ID */
public long getPosId() { return posId; }
/** 奖励据点ID */
public void setPosId(long _posId) { posId = _posId; }
/** 是否已解锁 */
public boolean getIsUnlock() { return isUnlock; }
/** 是否已解锁 */
public void setIsUnlock(boolean _isUnlock) { isUnlock = _isUnlock; }
/** 盟主cid */
public long getLeaderCid() { return leaderCid; }
/** 盟主cid */
public void setLeaderCid(long _leaderCid) { leaderCid = _leaderCid; }
/** 属性据点信息列表 */
public java.util.ArrayList<Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo> getPropertyPointList() { return propertyPointList; }
/** 属性据点信息列表 */
public void addPropertyPointList(Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo _propertyPointList) { propertyPointList.add(_propertyPointList); }


public final int GetBufSize() {
	int _size = 33;
	_size += 2 + (propertyPointList.size() * 28);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 35;
	_size += 2 + (propertyPointList.size() * 28);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.position();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.position(_posCurPos + _posCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) posId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isUnlock = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) leaderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _propertyPointListCount = _buf.getShort();
	for(int _i = 0; _i < _propertyPointListCount; _i++) { 
		Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo _propertyPointList = new Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo();
		if(_buf.remaining() <= 0) return;
	int __propertyPointListCustLen = _buf.getInt();
	int __propertyPointListCurPos = _buf.position();
	_propertyPointList.ReadUnzipBuf(_buf, __propertyPointListCurPos + __propertyPointListCustLen);
	_buf.position(__propertyPointListCurPos + __propertyPointListCustLen);

		propertyPointList.add(_propertyPointList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
	_buf.putLong(posId);
	_buf.put(isUnlock?(byte)1:(byte)0);
	_buf.putLong(leaderCid);
	_buf.putShort((short)propertyPointList.size());
	for(int _i = 0; _i < propertyPointList.size(); _i++) { 
		_buf.putInt(propertyPointList.get(_i).GetBufSize());
	propertyPointList.get(_i).PutUnzipBuf(_buf);
	}
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

