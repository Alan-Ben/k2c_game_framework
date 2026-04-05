using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_QuestionnaireInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 问卷实例id
/// </summary>
private long questionnaireId;
/// <summary>
/// 链接
/// </summary>
private string urlLink;
/// <summary>
/// 问卷编码
/// </summary>
private string questionnaireCode;
/// <summary>
/// 开始时间
/// </summary>
private long startTimeMs;
/// <summary>
/// 结束时间
/// </summary>
private long endTimeMs;
/// <summary>
/// 奖励列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> rewardList;


public Common_QuestionnaireInfo() {
	questionnaireId = (long)0;
	urlLink = "";
	questionnaireCode = "";
	startTimeMs = (long)0;
	endTimeMs = (long)0;
	rewardList = new List<NPCommon.NPCommon_ItemInfo>();
}

public Common_QuestionnaireInfo(
	long _questionnaireId
	, string _urlLink
	, string _questionnaireCode
	, long _startTimeMs
	, long _endTimeMs
	, List<NPCommon.NPCommon_ItemInfo> _rewardList
) {	questionnaireId = _questionnaireId;
	urlLink = _urlLink;
	questionnaireCode = _questionnaireCode;
	startTimeMs = _startTimeMs;
	endTimeMs = _endTimeMs;
	rewardList = _rewardList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 问卷实例id
/// </summary>
public long getQuestionnaireId() { return questionnaireId; }
/// <summary>
/// 问卷实例id
/// </summary>
public void setQuestionnaireId(long _questionnaireId) { questionnaireId = _questionnaireId; }
/// <summary>
/// 链接
/// </summary>
public string getUrlLink() { return urlLink; }
/// <summary>
/// 链接
/// </summary>
public void setUrlLink(string _urlLink) { urlLink = _urlLink; }
/// <summary>
/// 问卷编码
/// </summary>
public string getQuestionnaireCode() { return questionnaireCode; }
/// <summary>
/// 问卷编码
/// </summary>
public void setQuestionnaireCode(string _questionnaireCode) { questionnaireCode = _questionnaireCode; }
/// <summary>
/// 开始时间
/// </summary>
public long getStartTimeMs() { return startTimeMs; }
/// <summary>
/// 开始时间
/// </summary>
public void setStartTimeMs(long _startTimeMs) { startTimeMs = _startTimeMs; }
/// <summary>
/// 结束时间
/// </summary>
public long getEndTimeMs() { return endTimeMs; }
/// <summary>
/// 结束时间
/// </summary>
public void setEndTimeMs(long _endTimeMs) { endTimeMs = _endTimeMs; }
/// <summary>
/// 奖励列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getRewardList() { return rewardList; }
/// <summary>
/// 奖励列表
/// </summary>
public void addRewardList(NPCommon.NPCommon_ItemInfo _rewardList) { rewardList.Add(_rewardList); }


public int GetBufSize() {
	int _size = 24;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(urlLink);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(questionnaireCode);
	_size += 2;
for(int _i = 0; _i < rewardList.Count; _i++) {
	_size += 4 + rewardList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(urlLink);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(questionnaireCode);
	_size += 2;
for(int _i = 0; _i < rewardList.Count; _i++) {
	_size += 4 + rewardList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questionnaireId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	urlLink = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questionnaireCode = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endTimeMs = _buf.getLong();
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
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(questionnaireId);
	_buf.putString(urlLink);
	_buf.putString(questionnaireCode);
	_buf.putLong(startTimeMs);
	_buf.putLong(endTimeMs);
	_buf.putShort((short)rewardList.Count);
	for(int _i = 0; _i < rewardList.Count; _i++) { 
		_buf.putInt(rewardList[_i].GetBufSize());
	rewardList[_i].PutUnzipBuf(_buf);
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
	builder.Append("questionnaireId").Append(":").Append(questionnaireId.ToString()).Append(", ");
	builder.Append("urlLink").Append(":").Append(urlLink.ToString()).Append(", ");
	builder.Append("questionnaireCode").Append(":").Append(questionnaireCode.ToString()).Append(", ");
	builder.Append("startTimeMs").Append(":").Append(startTimeMs.ToString()).Append(", ");
	builder.Append("endTimeMs").Append(":").Append(endTimeMs.ToString()).Append(", ");
	builder.Append("rewardList").Append(":").Append(rewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

