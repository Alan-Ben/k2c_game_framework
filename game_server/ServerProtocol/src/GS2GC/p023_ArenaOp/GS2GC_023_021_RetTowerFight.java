package GS2GC.p023_ArenaOp;

import java.nio.ByteBuffer;
public class GS2GC_023_021_RetTowerFight implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否打败 */
private boolean isDefeat;
/** 奖励列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> rewardList;
/** 玩家ID 如果为0则为NPC */
private long cid;


public GS2GC_023_021_RetTowerFight() {
	isDefeat = false;
	rewardList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	cid = (long)0;
}

public GS2GC_023_021_RetTowerFight(
	 boolean _isDefeat
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _rewardList
	, long _cid
) {	isDefeat = _isDefeat;
	rewardList = _rewardList;
	cid = _cid;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)21; }

/** 是否打败 */
public boolean getIsDefeat() { return isDefeat; }
/** 是否打败 */
public void setIsDefeat(boolean _isDefeat) { isDefeat = _isDefeat; }
/** 奖励列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getRewardList() { return rewardList; }
/** 奖励列表 */
public void addRewardList(NPCommon.NPCommon_ItemInfo _rewardList) { rewardList.add(_rewardList); }
/** 玩家ID 如果为0则为NPC */
public long getCid() { return cid; }
/** 玩家ID 如果为0则为NPC */
public void setCid(long _cid) { cid = _cid; }


public final int GetBufSize() {
	int _size = 9;
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;
	_size += 2;
	for(int _i = 0; _i < rewardList.size(); _i++) {
	_size += 4 + rewardList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isDefeat = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _rewardList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.position();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.position(__rewardListCurPos + __rewardListCustLen);

		rewardList.add(_rewardList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isDefeat?(byte)1:(byte)0);
	_buf.putShort((short)rewardList.size());
	for(int _i = 0; _i < rewardList.size(); _i++) { 
		_buf.putInt(rewardList.get(_i).GetBufSize());
	rewardList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(cid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)21);
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

