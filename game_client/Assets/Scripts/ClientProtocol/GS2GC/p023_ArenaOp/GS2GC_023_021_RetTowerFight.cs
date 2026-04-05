using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p023_ArenaOp
{

public class GS2GC_023_021_RetTowerFight : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否打败
/// </summary>
private bool isDefeat;
/// <summary>
/// 奖励列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> rewardList;
/// <summary>
/// 玩家ID 如果为0则为NPC
/// </summary>
private long cid;


public GS2GC_023_021_RetTowerFight() {
	isDefeat = false;
	rewardList = new List<NPCommon.NPCommon_ItemInfo>();
	cid = (long)0;
}

public GS2GC_023_021_RetTowerFight(
	bool _isDefeat
	, List<NPCommon.NPCommon_ItemInfo> _rewardList
	, long _cid
) {	isDefeat = _isDefeat;
	rewardList = _rewardList;
	cid = _cid;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)21; }

/// <summary>
/// 是否打败
/// </summary>
public bool getIsDefeat() { return isDefeat; }
/// <summary>
/// 是否打败
/// </summary>
public void setIsDefeat(bool _isDefeat) { isDefeat = _isDefeat; }
/// <summary>
/// 奖励列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getRewardList() { return rewardList; }
/// <summary>
/// 奖励列表
/// </summary>
public void addRewardList(NPCommon.NPCommon_ItemInfo _rewardList) { rewardList.Add(_rewardList); }
/// <summary>
/// 玩家ID 如果为0则为NPC
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家ID 如果为0则为NPC
/// </summary>
public void setCid(long _cid) { cid = _cid; }


public int GetBufSize() {
	int _size = 9;
	_size += 2;
for(int _i = 0; _i < rewardList.Count; _i++) {
	_size += 4 + rewardList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;
	_size += 2;
for(int _i = 0; _i < rewardList.Count; _i++) {
	_size += 4 + rewardList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isDefeat = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _rewardList = new NPCommon.NPCommon_ItemInfo();
		int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.getCurPos();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.setPosition(__rewardListCurPos + __rewardListCustLen);

		rewardList.Add(_rewardList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isDefeat?(byte)1:(byte)0);
	_buf.putShort((short)rewardList.Count);
	for(int _i = 0; _i < rewardList.Count; _i++) { 
		_buf.putInt(rewardList[_i].GetBufSize());
	rewardList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(cid);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)21);
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
	builder.Append("isDefeat").Append(":").Append(isDefeat.ToString()).Append(", ");
	builder.Append("rewardList").Append(":").Append(rewardList.ToString()).Append(", ");
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

