package Common.DungeonObj;

import java.nio.ByteBuffer;
/*********
 * 午间副本_结算信息
 **/
public class MiddayDungeon_SettleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否击杀 */
private boolean isDefeat;
/** 击败奖励 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> defeatRewardList;
/** 掉落宝箱ID */
private long dropBoxId;
/** 掉落宝箱获得道具列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> boxRewardList;


public MiddayDungeon_SettleInfo() {
	isDefeat = false;
	defeatRewardList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	dropBoxId = (long)0;
	boxRewardList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public MiddayDungeon_SettleInfo(
	 boolean _isDefeat
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _defeatRewardList
	, long _dropBoxId
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _boxRewardList
) {	isDefeat = _isDefeat;
	defeatRewardList = _defeatRewardList;
	dropBoxId = _dropBoxId;
	boxRewardList = _boxRewardList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 是否击杀 */
public boolean getIsDefeat() { return isDefeat; }
/** 是否击杀 */
public void setIsDefeat(boolean _isDefeat) { isDefeat = _isDefeat; }
/** 击败奖励 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getDefeatRewardList() { return defeatRewardList; }
/** 击败奖励 */
public void addDefeatRewardList(NPCommon.NPCommon_ItemInfo _defeatRewardList) { defeatRewardList.add(_defeatRewardList); }
/** 掉落宝箱ID */
public long getDropBoxId() { return dropBoxId; }
/** 掉落宝箱ID */
public void setDropBoxId(long _dropBoxId) { dropBoxId = _dropBoxId; }
/** 掉落宝箱获得道具列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getBoxRewardList() { return boxRewardList; }
/** 掉落宝箱获得道具列表 */
public void addBoxRewardList(NPCommon.NPCommon_ItemInfo _boxRewardList) { boxRewardList.add(_boxRewardList); }


public final int GetBufSize() {
	int _size = 9;
	_size += 2;
	for(int _i = 0; _i < defeatRewardList.size(); _i++) {
	_size += 4 + defeatRewardList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < boxRewardList.size(); _i++) {
	_size += 4 + boxRewardList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;
	_size += 2;
	for(int _i = 0; _i < defeatRewardList.size(); _i++) {
	_size += 4 + defeatRewardList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < boxRewardList.size(); _i++) {
	_size += 4 + boxRewardList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isDefeat = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _defeatRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _defeatRewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _defeatRewardList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __defeatRewardListCustLen = _buf.getInt();
	int __defeatRewardListCurPos = _buf.position();
	_defeatRewardList.ReadUnzipBuf(_buf, __defeatRewardListCurPos + __defeatRewardListCustLen);
	_buf.position(__defeatRewardListCurPos + __defeatRewardListCustLen);

		defeatRewardList.add(_defeatRewardList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dropBoxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _boxRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _boxRewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _boxRewardList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __boxRewardListCustLen = _buf.getInt();
	int __boxRewardListCurPos = _buf.position();
	_boxRewardList.ReadUnzipBuf(_buf, __boxRewardListCurPos + __boxRewardListCustLen);
	_buf.position(__boxRewardListCurPos + __boxRewardListCustLen);

		boxRewardList.add(_boxRewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isDefeat?(byte)1:(byte)0);
	_buf.putShort((short)defeatRewardList.size());
	for(int _i = 0; _i < defeatRewardList.size(); _i++) { 
		_buf.putInt(defeatRewardList.get(_i).GetBufSize());
	defeatRewardList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(dropBoxId);
	_buf.putShort((short)boxRewardList.size());
	for(int _i = 0; _i < boxRewardList.size(); _i++) { 
		_buf.putInt(boxRewardList.get(_i).GetBufSize());
	boxRewardList.get(_i).PutUnzipBuf(_buf);
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

