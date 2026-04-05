using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GachaObj
{

/// <summary>
/// 抽卡卡池信息
/// </summary>
public class Gacha_PoolInfo : ALBasicProtocolPack._IALProtocolStructure {
private long poolId;
/// <summary>
/// 保底信息列表
/// </summary>
private List<Common.GachaObj.Gacha_GuaranteeInfo> guaranteeList;
/// <summary>
/// 抽卡累计奖励次数
/// </summary>
private int cumulativeRewardTimes;


public Gacha_PoolInfo() {
	poolId = (long)0;
	guaranteeList = new List<Common.GachaObj.Gacha_GuaranteeInfo>();
	cumulativeRewardTimes = 0;
}

public Gacha_PoolInfo(
	long _poolId
	, List<Common.GachaObj.Gacha_GuaranteeInfo> _guaranteeList
	, int _cumulativeRewardTimes
) {	poolId = _poolId;
	guaranteeList = _guaranteeList;
	cumulativeRewardTimes = _cumulativeRewardTimes;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getPoolId() { return poolId; }
public void setPoolId(long _poolId) { poolId = _poolId; }
/// <summary>
/// 保底信息列表
/// </summary>
public List<Common.GachaObj.Gacha_GuaranteeInfo> getGuaranteeList() { return guaranteeList; }
/// <summary>
/// 保底信息列表
/// </summary>
public void addGuaranteeList(Common.GachaObj.Gacha_GuaranteeInfo _guaranteeList) { guaranteeList.Add(_guaranteeList); }
/// <summary>
/// 抽卡累计奖励次数
/// </summary>
public int getCumulativeRewardTimes() { return cumulativeRewardTimes; }
/// <summary>
/// 抽卡累计奖励次数
/// </summary>
public void setCumulativeRewardTimes(int _cumulativeRewardTimes) { cumulativeRewardTimes = _cumulativeRewardTimes; }


public int GetBufSize() {
	int _size = 12;
	_size += 2 + (guaranteeList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 2 + (guaranteeList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	poolId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _guaranteeListCount = _buf.getShort();
	for(int _i = 0; _i < _guaranteeListCount; _i++) { 
		Common.GachaObj.Gacha_GuaranteeInfo _guaranteeList = new Common.GachaObj.Gacha_GuaranteeInfo();
		int __guaranteeListCustLen = _buf.getInt();
	int __guaranteeListCurPos = _buf.getCurPos();
	_guaranteeList.ReadUnzipBuf(_buf, __guaranteeListCurPos + __guaranteeListCustLen);
	_buf.setPosition(__guaranteeListCurPos + __guaranteeListCustLen);

		guaranteeList.Add(_guaranteeList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cumulativeRewardTimes = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(poolId);
	_buf.putShort((short)guaranteeList.Count);
	for(int _i = 0; _i < guaranteeList.Count; _i++) { 
		_buf.putInt(guaranteeList[_i].GetBufSize());
	guaranteeList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(cumulativeRewardTimes);
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
	builder.Append("poolId").Append(":").Append(poolId.ToString()).Append(", ");
	builder.Append("guaranteeList").Append(":").Append(guaranteeList.ToString()).Append(", ");
	builder.Append("cumulativeRewardTimes").Append(":").Append(cumulativeRewardTimes.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

