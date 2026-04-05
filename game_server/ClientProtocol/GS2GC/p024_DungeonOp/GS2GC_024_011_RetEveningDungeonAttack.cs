using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p024_DungeonOp
{

public class GS2GC_024_011_RetEveningDungeonAttack : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 伤害血量
/// </summary>
private long harmHp;
/// <summary>
/// 攻击奖励列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> attackRewardList;
/// <summary>
/// 是否击杀
/// </summary>
private bool isDefeat;
/// <summary>
/// 击杀奖励列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> defeatRewardList;


public GS2GC_024_011_RetEveningDungeonAttack() {
	harmHp = (long)0;
	attackRewardList = new List<NPCommon.NPCommon_ItemInfo>();
	isDefeat = false;
	defeatRewardList = new List<NPCommon.NPCommon_ItemInfo>();
}

public GS2GC_024_011_RetEveningDungeonAttack(
	long _harmHp
	, List<NPCommon.NPCommon_ItemInfo> _attackRewardList
	, bool _isDefeat
	, List<NPCommon.NPCommon_ItemInfo> _defeatRewardList
) {	harmHp = _harmHp;
	attackRewardList = _attackRewardList;
	isDefeat = _isDefeat;
	defeatRewardList = _defeatRewardList;
}

public byte getMainOrder() { return (byte)24; }

public byte getSubOrder() { return (byte)11; }

/// <summary>
/// 伤害血量
/// </summary>
public long getHarmHp() { return harmHp; }
/// <summary>
/// 伤害血量
/// </summary>
public void setHarmHp(long _harmHp) { harmHp = _harmHp; }
/// <summary>
/// 攻击奖励列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getAttackRewardList() { return attackRewardList; }
/// <summary>
/// 攻击奖励列表
/// </summary>
public void addAttackRewardList(NPCommon.NPCommon_ItemInfo _attackRewardList) { attackRewardList.Add(_attackRewardList); }
/// <summary>
/// 是否击杀
/// </summary>
public bool getIsDefeat() { return isDefeat; }
/// <summary>
/// 是否击杀
/// </summary>
public void setIsDefeat(bool _isDefeat) { isDefeat = _isDefeat; }
/// <summary>
/// 击杀奖励列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getDefeatRewardList() { return defeatRewardList; }
/// <summary>
/// 击杀奖励列表
/// </summary>
public void addDefeatRewardList(NPCommon.NPCommon_ItemInfo _defeatRewardList) { defeatRewardList.Add(_defeatRewardList); }


public int GetBufSize() {
	int _size = 9;
	_size += 2;
for(int _i = 0; _i < attackRewardList.Count; _i++) {
	_size += 4 + attackRewardList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < defeatRewardList.Count; _i++) {
	_size += 4 + defeatRewardList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;
	_size += 2;
for(int _i = 0; _i < attackRewardList.Count; _i++) {
	_size += 4 + attackRewardList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < defeatRewardList.Count; _i++) {
	_size += 4 + defeatRewardList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	harmHp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _attackRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _attackRewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _attackRewardList = new NPCommon.NPCommon_ItemInfo();
		int __attackRewardListCustLen = _buf.getInt();
	int __attackRewardListCurPos = _buf.getCurPos();
	_attackRewardList.ReadUnzipBuf(_buf, __attackRewardListCurPos + __attackRewardListCustLen);
	_buf.setPosition(__attackRewardListCurPos + __attackRewardListCustLen);

		attackRewardList.Add(_attackRewardList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isDefeat = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _defeatRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _defeatRewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _defeatRewardList = new NPCommon.NPCommon_ItemInfo();
		int __defeatRewardListCustLen = _buf.getInt();
	int __defeatRewardListCurPos = _buf.getCurPos();
	_defeatRewardList.ReadUnzipBuf(_buf, __defeatRewardListCurPos + __defeatRewardListCustLen);
	_buf.setPosition(__defeatRewardListCurPos + __defeatRewardListCustLen);

		defeatRewardList.Add(_defeatRewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(harmHp);
	_buf.putShort((short)attackRewardList.Count);
	for(int _i = 0; _i < attackRewardList.Count; _i++) { 
		_buf.putInt(attackRewardList[_i].GetBufSize());
	attackRewardList[_i].PutUnzipBuf(_buf);
	}
	_buf.put(isDefeat?(byte)1:(byte)0);
	_buf.putShort((short)defeatRewardList.Count);
	for(int _i = 0; _i < defeatRewardList.Count; _i++) { 
		_buf.putInt(defeatRewardList[_i].GetBufSize());
	defeatRewardList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)11);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("harmHp").Append(":").Append(harmHp.ToString()).Append(", ");
	builder.Append("attackRewardList").Append(":").Append(attackRewardList.ToString()).Append(", ");
	builder.Append("isDefeat").Append(":").Append(isDefeat.ToString()).Append(", ");
	builder.Append("defeatRewardList").Append(":").Append(defeatRewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

