using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p042_GuildRelatedOp
{

/// <summary>
/// 联盟宝箱奖励物品列表
/// </summary>
public class GS2GC_042_056_OnGuildBoxRewardShow : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宝箱类型
/// </summary>
private Common.GuildEnum.EGuildBoxType boxType;
/// <summary>
/// 宝箱奖励列表数据
/// </summary>
private List<Common.GuildObj.Guild_BoxReward> rewardList;


public GS2GC_042_056_OnGuildBoxRewardShow() {
	boxType = 0;
	rewardList = new List<Common.GuildObj.Guild_BoxReward>();
}

public GS2GC_042_056_OnGuildBoxRewardShow(
	Common.GuildEnum.EGuildBoxType _boxType
	, List<Common.GuildObj.Guild_BoxReward> _rewardList
) {	boxType = _boxType;
	rewardList = _rewardList;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)56; }

/// <summary>
/// 宝箱类型
/// </summary>
public Common.GuildEnum.EGuildBoxType getBoxType() { return boxType; }
/// <summary>
/// 宝箱类型
/// </summary>
public void setBoxType(Common.GuildEnum.EGuildBoxType _boxType) { boxType = _boxType; }
/// <summary>
/// 宝箱奖励列表数据
/// </summary>
public List<Common.GuildObj.Guild_BoxReward> getRewardList() { return rewardList; }
/// <summary>
/// 宝箱奖励列表数据
/// </summary>
public void addRewardList(Common.GuildObj.Guild_BoxReward _rewardList) { rewardList.Add(_rewardList); }


public int GetBufSize() {
	int _size = 4;
	_size += 2;
for(int _i = 0; _i < rewardList.Count; _i++) {
	_size += 4 + rewardList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2;
for(int _i = 0; _i < rewardList.Count; _i++) {
	_size += 4 + rewardList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	boxType = (Common.GuildEnum.EGuildBoxType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		Common.GuildObj.Guild_BoxReward _rewardList = new Common.GuildObj.Guild_BoxReward();
		int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.getCurPos();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.setPosition(__rewardListCurPos + __rewardListCustLen);

		rewardList.Add(_rewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)boxType);

	_buf.putShort((short)rewardList.Count);
	for(int _i = 0; _i < rewardList.Count; _i++) { 
		_buf.putInt(rewardList[_i].GetBufSize());
	rewardList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)56);
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
	builder.Append("boxType").Append(":").Append(boxType.ToString()).Append(", ");
	builder.Append("rewardList").Append(":").Append(rewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

