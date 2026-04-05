using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_042_RetQuestionnaireInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 问卷列表
/// </summary>
private List<Common.Common_QuestionnaireInfo> infoList;
/// <summary>
/// 问卷奖励列表
/// </summary>
private List<Common.Common_QuestionnaireRewardInfo> rewardList;


public GS2GC_002_042_RetQuestionnaireInit() {
	infoList = new List<Common.Common_QuestionnaireInfo>();
	rewardList = new List<Common.Common_QuestionnaireRewardInfo>();
}

public GS2GC_002_042_RetQuestionnaireInit(
	List<Common.Common_QuestionnaireInfo> _infoList
	, List<Common.Common_QuestionnaireRewardInfo> _rewardList
) {	infoList = _infoList;
	rewardList = _rewardList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)42; }

/// <summary>
/// 问卷列表
/// </summary>
public List<Common.Common_QuestionnaireInfo> getInfoList() { return infoList; }
/// <summary>
/// 问卷列表
/// </summary>
public void addInfoList(Common.Common_QuestionnaireInfo _infoList) { infoList.Add(_infoList); }
/// <summary>
/// 问卷奖励列表
/// </summary>
public List<Common.Common_QuestionnaireRewardInfo> getRewardList() { return rewardList; }
/// <summary>
/// 问卷奖励列表
/// </summary>
public void addRewardList(Common.Common_QuestionnaireRewardInfo _rewardList) { rewardList.Add(_rewardList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < infoList.Count; _i++) {
	_size += 4 + infoList[_i].GetBufSize();
	}

	_size += 2 + (rewardList.Count * 13);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < infoList.Count; _i++) {
	_size += 4 + infoList[_i].GetBufSize();
	}

	_size += 2 + (rewardList.Count * 13);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _infoListCount = _buf.getShort();
	for(int _i = 0; _i < _infoListCount; _i++) { 
		Common.Common_QuestionnaireInfo _infoList = new Common.Common_QuestionnaireInfo();
		int __infoListCustLen = _buf.getInt();
	int __infoListCurPos = _buf.getCurPos();
	_infoList.ReadUnzipBuf(_buf, __infoListCurPos + __infoListCustLen);
	_buf.setPosition(__infoListCurPos + __infoListCustLen);

		infoList.Add(_infoList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _rewardListCount = _buf.getShort();
	for(int _i = 0; _i < _rewardListCount; _i++) { 
		Common.Common_QuestionnaireRewardInfo _rewardList = new Common.Common_QuestionnaireRewardInfo();
		int __rewardListCustLen = _buf.getInt();
	int __rewardListCurPos = _buf.getCurPos();
	_rewardList.ReadUnzipBuf(_buf, __rewardListCurPos + __rewardListCustLen);
	_buf.setPosition(__rewardListCurPos + __rewardListCustLen);

		rewardList.Add(_rewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)infoList.Count);
	for(int _i = 0; _i < infoList.Count; _i++) { 
		_buf.putInt(infoList[_i].GetBufSize());
	infoList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)rewardList.Count);
	for(int _i = 0; _i < rewardList.Count; _i++) { 
		_buf.putInt(rewardList[_i].GetBufSize());
	rewardList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)42);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)42);
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
	builder.Append("infoList").Append(":").Append(infoList.ToString()).Append(", ");
	builder.Append("rewardList").Append(":").Append(rewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

