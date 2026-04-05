using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DungeonObj
{

/// <summary>
/// 午间副本_结算信息
/// </summary>
public class MiddayDungeon_SettleInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否击杀
/// </summary>
private bool isDefeat;
/// <summary>
/// 击败奖励
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> defeatRewardList;
/// <summary>
/// 掉落宝箱ID
/// </summary>
private long dropBoxId;
/// <summary>
/// 掉落宝箱获得道具列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> boxRewardList;


public MiddayDungeon_SettleInfo() {
	isDefeat = false;
	defeatRewardList = new List<NPCommon.NPCommon_ItemInfo>();
	dropBoxId = (long)0;
	boxRewardList = new List<NPCommon.NPCommon_ItemInfo>();
}

public MiddayDungeon_SettleInfo(
	bool _isDefeat
	, List<NPCommon.NPCommon_ItemInfo> _defeatRewardList
	, long _dropBoxId
	, List<NPCommon.NPCommon_ItemInfo> _boxRewardList
) {	isDefeat = _isDefeat;
	defeatRewardList = _defeatRewardList;
	dropBoxId = _dropBoxId;
	boxRewardList = _boxRewardList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 是否击杀
/// </summary>
public bool getIsDefeat() { return isDefeat; }
/// <summary>
/// 是否击杀
/// </summary>
public void setIsDefeat(bool _isDefeat) { isDefeat = _isDefeat; }
/// <summary>
/// 击败奖励
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getDefeatRewardList() { return defeatRewardList; }
/// <summary>
/// 击败奖励
/// </summary>
public void addDefeatRewardList(NPCommon.NPCommon_ItemInfo _defeatRewardList) { defeatRewardList.Add(_defeatRewardList); }
/// <summary>
/// 掉落宝箱ID
/// </summary>
public long getDropBoxId() { return dropBoxId; }
/// <summary>
/// 掉落宝箱ID
/// </summary>
public void setDropBoxId(long _dropBoxId) { dropBoxId = _dropBoxId; }
/// <summary>
/// 掉落宝箱获得道具列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getBoxRewardList() { return boxRewardList; }
/// <summary>
/// 掉落宝箱获得道具列表
/// </summary>
public void addBoxRewardList(NPCommon.NPCommon_ItemInfo _boxRewardList) { boxRewardList.Add(_boxRewardList); }


public int GetBufSize() {
	int _size = 9;
	_size += 2;
for(int _i = 0; _i < defeatRewardList.Count; _i++) {
	_size += 4 + defeatRewardList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < boxRewardList.Count; _i++) {
	_size += 4 + boxRewardList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;
	_size += 2;
for(int _i = 0; _i < defeatRewardList.Count; _i++) {
	_size += 4 + defeatRewardList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < boxRewardList.Count; _i++) {
	_size += 4 + boxRewardList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
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
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dropBoxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _boxRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _boxRewardListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _boxRewardList = new NPCommon.NPCommon_ItemInfo();
		int __boxRewardListCustLen = _buf.getInt();
	int __boxRewardListCurPos = _buf.getCurPos();
	_boxRewardList.ReadUnzipBuf(_buf, __boxRewardListCurPos + __boxRewardListCustLen);
	_buf.setPosition(__boxRewardListCurPos + __boxRewardListCustLen);

		boxRewardList.Add(_boxRewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isDefeat?(byte)1:(byte)0);
	_buf.putShort((short)defeatRewardList.Count);
	for(int _i = 0; _i < defeatRewardList.Count; _i++) { 
		_buf.putInt(defeatRewardList[_i].GetBufSize());
	defeatRewardList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(dropBoxId);
	_buf.putShort((short)boxRewardList.Count);
	for(int _i = 0; _i < boxRewardList.Count; _i++) { 
		_buf.putInt(boxRewardList[_i].GetBufSize());
	boxRewardList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("defeatRewardList").Append(":").Append(defeatRewardList.ToString()).Append(", ");
	builder.Append("dropBoxId").Append(":").Append(dropBoxId.ToString()).Append(", ");
	builder.Append("boxRewardList").Append(":").Append(boxRewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

