using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DinnerObj
{

/// <summary>
/// 宴会详情
/// </summary>
public class Dinner_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 简要信息
/// </summary>
private Common.DinnerObj.Dinner_Idx idx;
/// <summary>
/// 开始时间戳（秒）
/// </summary>
private int startTs;
/// <summary>
/// 凭证类型
/// </summary>
private Common.DinnerEnum.EDinnerPermitType permitType;
/// <summary>
/// 凭证类型ID
/// </summary>
private long permitTypeId;
/// <summary>
/// 赴宴玩家列表
/// </summary>
private List<Common.DinnerObj.Dinner_Joiner> joinerList;


public Dinner_Info() {
	idx = new Common.DinnerObj.Dinner_Idx();
	startTs = 0;
	permitType = 0;
	permitTypeId = (long)0;
	joinerList = new List<Common.DinnerObj.Dinner_Joiner>();
}

public Dinner_Info(
	Common.DinnerObj.Dinner_Idx _idx
	, int _startTs
	, Common.DinnerEnum.EDinnerPermitType _permitType
	, long _permitTypeId
	, List<Common.DinnerObj.Dinner_Joiner> _joinerList
) {	idx = _idx;
	startTs = _startTs;
	permitType = _permitType;
	permitTypeId = _permitTypeId;
	joinerList = _joinerList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 简要信息
/// </summary>
public Common.DinnerObj.Dinner_Idx getIdx() { return idx; }
/// <summary>
/// 简要信息
/// </summary>
public void setIdx(Common.DinnerObj.Dinner_Idx _idx) { idx = _idx; }
/// <summary>
/// 开始时间戳（秒）
/// </summary>
public int getStartTs() { return startTs; }
/// <summary>
/// 开始时间戳（秒）
/// </summary>
public void setStartTs(int _startTs) { startTs = _startTs; }
/// <summary>
/// 凭证类型
/// </summary>
public Common.DinnerEnum.EDinnerPermitType getPermitType() { return permitType; }
/// <summary>
/// 凭证类型
/// </summary>
public void setPermitType(Common.DinnerEnum.EDinnerPermitType _permitType) { permitType = _permitType; }
/// <summary>
/// 凭证类型ID
/// </summary>
public long getPermitTypeId() { return permitTypeId; }
/// <summary>
/// 凭证类型ID
/// </summary>
public void setPermitTypeId(long _permitTypeId) { permitTypeId = _permitTypeId; }
/// <summary>
/// 赴宴玩家列表
/// </summary>
public List<Common.DinnerObj.Dinner_Joiner> getJoinerList() { return joinerList; }
/// <summary>
/// 赴宴玩家列表
/// </summary>
public void addJoinerList(Common.DinnerObj.Dinner_Joiner _joinerList) { joinerList.Add(_joinerList); }


public int GetBufSize() {
	int _size = 73;
	_size += 2 + (joinerList.Count * 40);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 75;
	_size += 2 + (joinerList.Count * 40);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _idxCustLen = _buf.getInt();
	int _idxCurPos = _buf.getCurPos();
	idx.ReadUnzipBuf(_buf, _idxCurPos + _idxCustLen);
	_buf.setPosition(_idxCurPos + _idxCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	permitType = (Common.DinnerEnum.EDinnerPermitType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	permitTypeId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _joinerListCount = _buf.getShort();
	for(int _i = 0; _i < _joinerListCount; _i++) { 
		Common.DinnerObj.Dinner_Joiner _joinerList = new Common.DinnerObj.Dinner_Joiner();
		int __joinerListCustLen = _buf.getInt();
	int __joinerListCurPos = _buf.getCurPos();
	_joinerList.ReadUnzipBuf(_buf, __joinerListCurPos + __joinerListCustLen);
	_buf.setPosition(__joinerListCurPos + __joinerListCustLen);

		joinerList.Add(_joinerList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(idx.GetBufSize());
	idx.PutUnzipBuf(_buf);
	_buf.putInt(startTs);
	_buf.putInt((int)permitType);

	_buf.putLong(permitTypeId);
	_buf.putShort((short)joinerList.Count);
	for(int _i = 0; _i < joinerList.Count; _i++) { 
		_buf.putInt(joinerList[_i].GetBufSize());
	joinerList[_i].PutUnzipBuf(_buf);
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
	builder.Append("idx").Append(":").Append(idx == null ? "null" : idx.ToString()).Append(", ");
	builder.Append("startTs").Append(":").Append(startTs.ToString()).Append(", ");
	builder.Append("permitType").Append(":").Append(permitType.ToString()).Append(", ");
	builder.Append("permitTypeId").Append(":").Append(permitTypeId.ToString()).Append(", ");
	builder.Append("joinerList").Append(":").Append(joinerList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

