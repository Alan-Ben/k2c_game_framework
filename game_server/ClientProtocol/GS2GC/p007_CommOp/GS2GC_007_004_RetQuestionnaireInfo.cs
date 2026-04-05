using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_004_RetQuestionnaireInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否有奖励
/// </summary>
private bool hasReward;
/// <summary>
/// 问卷奖励信息
/// </summary>
private Common.Common_QuestionnaireRewardInfo rewardInfo;


public GS2GC_007_004_RetQuestionnaireInfo() {
	hasReward = false;
	rewardInfo = new Common.Common_QuestionnaireRewardInfo();
}

public GS2GC_007_004_RetQuestionnaireInfo(
	bool _hasReward
	, Common.Common_QuestionnaireRewardInfo _rewardInfo
) {	hasReward = _hasReward;
	rewardInfo = _rewardInfo;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 是否有奖励
/// </summary>
public bool getHasReward() { return hasReward; }
/// <summary>
/// 是否有奖励
/// </summary>
public void setHasReward(bool _hasReward) { hasReward = _hasReward; }
/// <summary>
/// 问卷奖励信息
/// </summary>
public Common.Common_QuestionnaireRewardInfo getRewardInfo() { return rewardInfo; }
/// <summary>
/// 问卷奖励信息
/// </summary>
public void setRewardInfo(Common.Common_QuestionnaireRewardInfo _rewardInfo) { rewardInfo = _rewardInfo; }


public int GetBufSize() {
	int _size = 14;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 16;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _rewardInfoCustLen = _buf.getInt();
	int _rewardInfoCurPos = _buf.getCurPos();
	rewardInfo.ReadUnzipBuf(_buf, _rewardInfoCurPos + _rewardInfoCustLen);
	_buf.setPosition(_rewardInfoCurPos + _rewardInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(hasReward?(byte)1:(byte)0);
	_buf.putInt(rewardInfo.GetBufSize());
	rewardInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)4);
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
	builder.Append("hasReward").Append(":").Append(hasReward.ToString()).Append(", ");
	builder.Append("rewardInfo").Append(":").Append(rewardInfo == null ? "null" : rewardInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

