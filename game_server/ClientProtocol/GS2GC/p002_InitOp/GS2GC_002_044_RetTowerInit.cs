using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_044_RetTowerInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 位置信息
/// </summary>
private Common.TowerObj.Tower_PosInfo posInfo;
/// <summary>
/// 已领取研究奖励列表
/// </summary>
private List<long> hadDrawResearchRewardList;
/// <summary>
/// 已到达最高位置信息
/// </summary>
private Common.TowerObj.Tower_PosInfo highestPosHadReach;
/// <summary>
/// 已激活研究位置信息
/// </summary>
private Common.TowerObj.Tower_PosInfo hadActiveResearchPos;
/// <summary>
/// 建筑收益提升万分比
/// </summary>
private int buildingProfitAddPer;


public GS2GC_002_044_RetTowerInit() {
	posInfo = new Common.TowerObj.Tower_PosInfo();
	hadDrawResearchRewardList = new List<long>();
	highestPosHadReach = new Common.TowerObj.Tower_PosInfo();
	hadActiveResearchPos = new Common.TowerObj.Tower_PosInfo();
	buildingProfitAddPer = 0;
}

public GS2GC_002_044_RetTowerInit(
	Common.TowerObj.Tower_PosInfo _posInfo
	, List<long> _hadDrawResearchRewardList
	, Common.TowerObj.Tower_PosInfo _highestPosHadReach
	, Common.TowerObj.Tower_PosInfo _hadActiveResearchPos
	, int _buildingProfitAddPer
) {	posInfo = _posInfo;
	hadDrawResearchRewardList = _hadDrawResearchRewardList;
	highestPosHadReach = _highestPosHadReach;
	hadActiveResearchPos = _hadActiveResearchPos;
	buildingProfitAddPer = _buildingProfitAddPer;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)44; }

/// <summary>
/// 位置信息
/// </summary>
public Common.TowerObj.Tower_PosInfo getPosInfo() { return posInfo; }
/// <summary>
/// 位置信息
/// </summary>
public void setPosInfo(Common.TowerObj.Tower_PosInfo _posInfo) { posInfo = _posInfo; }
/// <summary>
/// 已领取研究奖励列表
/// </summary>
public List<long> getHadDrawResearchRewardList() { return hadDrawResearchRewardList; }
/// <summary>
/// 已领取研究奖励列表
/// </summary>
public void addHadDrawResearchRewardList(long _hadDrawResearchRewardList) { hadDrawResearchRewardList.Add(_hadDrawResearchRewardList); }
/// <summary>
/// 已到达最高位置信息
/// </summary>
public Common.TowerObj.Tower_PosInfo getHighestPosHadReach() { return highestPosHadReach; }
/// <summary>
/// 已到达最高位置信息
/// </summary>
public void setHighestPosHadReach(Common.TowerObj.Tower_PosInfo _highestPosHadReach) { highestPosHadReach = _highestPosHadReach; }
/// <summary>
/// 已激活研究位置信息
/// </summary>
public Common.TowerObj.Tower_PosInfo getHadActiveResearchPos() { return hadActiveResearchPos; }
/// <summary>
/// 已激活研究位置信息
/// </summary>
public void setHadActiveResearchPos(Common.TowerObj.Tower_PosInfo _hadActiveResearchPos) { hadActiveResearchPos = _hadActiveResearchPos; }
/// <summary>
/// 建筑收益提升万分比
/// </summary>
public int getBuildingProfitAddPer() { return buildingProfitAddPer; }
/// <summary>
/// 建筑收益提升万分比
/// </summary>
public void setBuildingProfitAddPer(int _buildingProfitAddPer) { buildingProfitAddPer = _buildingProfitAddPer; }


public int GetBufSize() {
	int _size = 52;
	_size += 2 + (hadDrawResearchRewardList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 54;
	_size += 2 + (hadDrawResearchRewardList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _posInfoCustLen = _buf.getInt();
	int _posInfoCurPos = _buf.getCurPos();
	posInfo.ReadUnzipBuf(_buf, _posInfoCurPos + _posInfoCustLen);
	_buf.setPosition(_posInfoCurPos + _posInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hadDrawResearchRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawResearchRewardListCount; _i++) { 
		long _hadDrawResearchRewardList = (long)0;
		_hadDrawResearchRewardList = _buf.getLong();
		hadDrawResearchRewardList.Add(_hadDrawResearchRewardList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _highestPosHadReachCustLen = _buf.getInt();
	int _highestPosHadReachCurPos = _buf.getCurPos();
	highestPosHadReach.ReadUnzipBuf(_buf, _highestPosHadReachCurPos + _highestPosHadReachCustLen);
	_buf.setPosition(_highestPosHadReachCurPos + _highestPosHadReachCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _hadActiveResearchPosCustLen = _buf.getInt();
	int _hadActiveResearchPosCurPos = _buf.getCurPos();
	hadActiveResearchPos.ReadUnzipBuf(_buf, _hadActiveResearchPosCurPos + _hadActiveResearchPosCustLen);
	_buf.setPosition(_hadActiveResearchPosCurPos + _hadActiveResearchPosCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buildingProfitAddPer = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(posInfo.GetBufSize());
	posInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)hadDrawResearchRewardList.Count);
	for(int _i = 0; _i < hadDrawResearchRewardList.Count; _i++) { 
		_buf.putLong(hadDrawResearchRewardList[_i]);
	}
	_buf.putInt(highestPosHadReach.GetBufSize());
	highestPosHadReach.PutUnzipBuf(_buf);
	_buf.putInt(hadActiveResearchPos.GetBufSize());
	hadActiveResearchPos.PutUnzipBuf(_buf);
	_buf.putInt(buildingProfitAddPer);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)44);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)44);
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
	builder.Append("posInfo").Append(":").Append(posInfo == null ? "null" : posInfo.ToString()).Append(", ");
	builder.Append("hadDrawResearchRewardList").Append(":").Append(hadDrawResearchRewardList.ToString()).Append(", ");
	builder.Append("highestPosHadReach").Append(":").Append(highestPosHadReach == null ? "null" : highestPosHadReach.ToString()).Append(", ");
	builder.Append("hadActiveResearchPos").Append(":").Append(hadActiveResearchPos == null ? "null" : hadActiveResearchPos.ToString()).Append(", ");
	builder.Append("buildingProfitAddPer").Append(":").Append(buildingProfitAddPer.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

