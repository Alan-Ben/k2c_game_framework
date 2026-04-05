package GS2GC.p024_DungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_024_011_RetEveningDungeonAttack implements ALBasicProtocolPack._IALProtocolStructure {
/** 伤害血量 */
private long harmHp;
/** 攻击奖励列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> attackRewardList;
/** 是否击杀 */
private boolean isDefeat;
/** 击杀奖励列表 */
private java.util.ArrayList<NPCommon.NPCommon_ItemInfo> defeatRewardList;


public GS2GC_024_011_RetEveningDungeonAttack() {
	harmHp = (long)0;
	attackRewardList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
	isDefeat = false;
	defeatRewardList = new java.util.ArrayList<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_024_011_RetEveningDungeonAttack(
	 long _harmHp
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _attackRewardList
	, boolean _isDefeat
	, java.util.ArrayList<NPCommon.NPCommon_ItemInfo> _defeatRewardList
) {	harmHp = _harmHp;
	attackRewardList = _attackRewardList;
	isDefeat = _isDefeat;
	defeatRewardList = _defeatRewardList;
}

public final byte getMainOrder() { return (byte)24; }

public final byte getSubOrder() { return (byte)11; }

/** 伤害血量 */
public long getHarmHp() { return harmHp; }
/** 伤害血量 */
public void setHarmHp(long _harmHp) { harmHp = _harmHp; }
/** 攻击奖励列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getAttackRewardList() { return attackRewardList; }
/** 攻击奖励列表 */
public void addAttackRewardList(NPCommon.NPCommon_ItemInfo _attackRewardList) { attackRewardList.add(_attackRewardList); }
/** 是否击杀 */
public boolean getIsDefeat() { return isDefeat; }
/** 是否击杀 */
public void setIsDefeat(boolean _isDefeat) { isDefeat = _isDefeat; }
/** 击杀奖励列表 */
public java.util.ArrayList<NPCommon.NPCommon_ItemInfo> getDefeatRewardList() { return defeatRewardList; }
/** 击杀奖励列表 */
public void addDefeatRewardList(NPCommon.NPCommon_ItemInfo _defeatRewardList) { defeatRewardList.add(_defeatRewardList); }


public final int GetBufSize() {
	int _size = 9;
	_size += 2;
	for(int _i = 0; _i < attackRewardList.size(); _i++) {
	_size += 4 + attackRewardList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < defeatRewardList.size(); _i++) {
	_size += 4 + defeatRewardList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;
	_size += 2;
	for(int _i = 0; _i < attackRewardList.size(); _i++) {
	_size += 4 + attackRewardList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < defeatRewardList.size(); _i++) {
	_size += 4 + defeatRewardList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) harmHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _attackRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _attackRewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _attackRewardList = new NPCommon.NPCommon_ItemInfo();
		if(_buf.remaining() <= 0) return;
	int __attackRewardListCustLen = _buf.getInt();
	int __attackRewardListCurPos = _buf.position();
	_attackRewardList.ReadUnzipBuf(_buf, __attackRewardListCurPos + __attackRewardListCustLen);
	_buf.position(__attackRewardListCurPos + __attackRewardListCustLen);

		attackRewardList.add(_attackRewardList);
	}
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
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(harmHp);
	_buf.putShort((short)attackRewardList.size());
	for(int _i = 0; _i < attackRewardList.size(); _i++) { 
		_buf.putInt(attackRewardList.get(_i).GetBufSize());
	attackRewardList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.put(isDefeat?(byte)1:(byte)0);
	_buf.putShort((short)defeatRewardList.size());
	for(int _i = 0; _i < defeatRewardList.size(); _i++) { 
		_buf.putInt(defeatRewardList.get(_i).GetBufSize());
	defeatRewardList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)11);
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

