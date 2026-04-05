using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.TileMatchObj
{

/// <summary>
/// 三消-信息
/// </summary>
public class TileMatch_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 阶段奖励信息
/// </summary>
private Hotfix.Common.TileMatchObj.TileMatch_StepRewardInfo stepRewardInfo;
/// <summary>
/// 可领取阶段奖励列表
/// </summary>
private List<Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward> canDrawStepRewardList;
/// <summary>
/// 总分数
/// </summary>
private long totalScore;


public TileMatch_Info() {
	stepRewardInfo = new Hotfix.Common.TileMatchObj.TileMatch_StepRewardInfo();
	canDrawStepRewardList = new List<Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward>();
	totalScore = (long)0;
}

public TileMatch_Info(
	Hotfix.Common.TileMatchObj.TileMatch_StepRewardInfo _stepRewardInfo
	, List<Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward> _canDrawStepRewardList
	, long _totalScore
) {	stepRewardInfo = _stepRewardInfo;
	canDrawStepRewardList = _canDrawStepRewardList;
	totalScore = _totalScore;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 阶段奖励信息
/// </summary>
public Hotfix.Common.TileMatchObj.TileMatch_StepRewardInfo getStepRewardInfo() { return stepRewardInfo; }
/// <summary>
/// 阶段奖励信息
/// </summary>
public void setStepRewardInfo(Hotfix.Common.TileMatchObj.TileMatch_StepRewardInfo _stepRewardInfo) { stepRewardInfo = _stepRewardInfo; }
/// <summary>
/// 可领取阶段奖励列表
/// </summary>
public List<Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward> getCanDrawStepRewardList() { return canDrawStepRewardList; }
/// <summary>
/// 可领取阶段奖励列表
/// </summary>
public void addCanDrawStepRewardList(Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward _canDrawStepRewardList) { canDrawStepRewardList.Add(_canDrawStepRewardList); }
/// <summary>
/// 总分数
/// </summary>
public long getTotalScore() { return totalScore; }
/// <summary>
/// 总分数
/// </summary>
public void setTotalScore(long _totalScore) { totalScore = _totalScore; }


public int GetBufSize() {
	int _size = 28;
	_size += 2 + (canDrawStepRewardList.Count * 12);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;
	_size += 2 + (canDrawStepRewardList.Count * 12);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _stepRewardInfoCustLen = _buf.getInt();
	int _stepRewardInfoCurPos = _buf.getCurPos();
	stepRewardInfo.ReadUnzipBuf(_buf, _stepRewardInfoCurPos + _stepRewardInfoCustLen);
	_buf.setPosition(_stepRewardInfoCurPos + _stepRewardInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _canDrawStepRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _canDrawStepRewardListCount; _i++) { 
		Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward _canDrawStepRewardList = new Hotfix.Common.TileMatchObj.TileMatch_CanDrawStepReward();
		int __canDrawStepRewardListCustLen = _buf.getInt();
	int __canDrawStepRewardListCurPos = _buf.getCurPos();
	_canDrawStepRewardList.ReadUnzipBuf(_buf, __canDrawStepRewardListCurPos + __canDrawStepRewardListCustLen);
	_buf.setPosition(__canDrawStepRewardListCurPos + __canDrawStepRewardListCustLen);

		canDrawStepRewardList.Add(_canDrawStepRewardList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalScore = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(stepRewardInfo.GetBufSize());
	stepRewardInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)canDrawStepRewardList.Count);
	for(int _i = 0; _i < canDrawStepRewardList.Count; _i++) { 
		_buf.putInt(canDrawStepRewardList[_i].GetBufSize());
	canDrawStepRewardList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(totalScore);
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
	builder.Append("stepRewardInfo").Append(":").Append(stepRewardInfo == null ? "null" : stepRewardInfo.ToString()).Append(", ");
	builder.Append("canDrawStepRewardList").Append(":").Append(canDrawStepRewardList.ToString()).Append(", ");
	builder.Append("totalScore").Append(":").Append(totalScore.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

