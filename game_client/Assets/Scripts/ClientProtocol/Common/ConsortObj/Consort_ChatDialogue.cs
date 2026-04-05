using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ConsortObj
{

/// <summary>
/// 家人聊天对话数据
/// </summary>
public class Consort_ChatDialogue : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 对话id
/// </summary>
private long dialogueId;
/// <summary>
/// 触发时间戳
/// </summary>
private long triggerTimeMs;
/// <summary>
/// 选项列表
/// </summary>
private List<Common.ConsortObj.Consort_DialogueOption> optionList;
/// <summary>
/// 是否领奖
/// </summary>
private bool hadDraw;
/// <summary>
/// 领奖时间ms
/// </summary>
private long drawRewardTimeMs;


public Consort_ChatDialogue() {
	dialogueId = (long)0;
	triggerTimeMs = (long)0;
	optionList = new List<Common.ConsortObj.Consort_DialogueOption>();
	hadDraw = false;
	drawRewardTimeMs = (long)0;
}

public Consort_ChatDialogue(
	long _dialogueId
	, long _triggerTimeMs
	, List<Common.ConsortObj.Consort_DialogueOption> _optionList
	, bool _hadDraw
	, long _drawRewardTimeMs
) {	dialogueId = _dialogueId;
	triggerTimeMs = _triggerTimeMs;
	optionList = _optionList;
	hadDraw = _hadDraw;
	drawRewardTimeMs = _drawRewardTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 对话id
/// </summary>
public long getDialogueId() { return dialogueId; }
/// <summary>
/// 对话id
/// </summary>
public void setDialogueId(long _dialogueId) { dialogueId = _dialogueId; }
/// <summary>
/// 触发时间戳
/// </summary>
public long getTriggerTimeMs() { return triggerTimeMs; }
/// <summary>
/// 触发时间戳
/// </summary>
public void setTriggerTimeMs(long _triggerTimeMs) { triggerTimeMs = _triggerTimeMs; }
/// <summary>
/// 选项列表
/// </summary>
public List<Common.ConsortObj.Consort_DialogueOption> getOptionList() { return optionList; }
/// <summary>
/// 选项列表
/// </summary>
public void addOptionList(Common.ConsortObj.Consort_DialogueOption _optionList) { optionList.Add(_optionList); }
/// <summary>
/// 是否领奖
/// </summary>
public bool getHadDraw() { return hadDraw; }
/// <summary>
/// 是否领奖
/// </summary>
public void setHadDraw(bool _hadDraw) { hadDraw = _hadDraw; }
/// <summary>
/// 领奖时间ms
/// </summary>
public long getDrawRewardTimeMs() { return drawRewardTimeMs; }
/// <summary>
/// 领奖时间ms
/// </summary>
public void setDrawRewardTimeMs(long _drawRewardTimeMs) { drawRewardTimeMs = _drawRewardTimeMs; }


public int GetBufSize() {
	int _size = 25;
	_size += 2 + (optionList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 27;
	_size += 2 + (optionList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dialogueId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	triggerTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _optionListCount = _buf.getShort();
	for(int _i = 0; _i < _optionListCount; _i++) { 
		Common.ConsortObj.Consort_DialogueOption _optionList = new Common.ConsortObj.Consort_DialogueOption();
		int __optionListCustLen = _buf.getInt();
	int __optionListCurPos = _buf.getCurPos();
	_optionList.ReadUnzipBuf(_buf, __optionListCurPos + __optionListCustLen);
	_buf.setPosition(__optionListCurPos + __optionListCustLen);

		optionList.Add(_optionList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDraw = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	drawRewardTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dialogueId);
	_buf.putLong(triggerTimeMs);
	_buf.putShort((short)optionList.Count);
	for(int _i = 0; _i < optionList.Count; _i++) { 
		_buf.putInt(optionList[_i].GetBufSize());
	optionList[_i].PutUnzipBuf(_buf);
	}
	_buf.put(hadDraw?(byte)1:(byte)0);
	_buf.putLong(drawRewardTimeMs);
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
	builder.Append("dialogueId").Append(":").Append(dialogueId.ToString()).Append(", ");
	builder.Append("triggerTimeMs").Append(":").Append(triggerTimeMs.ToString()).Append(", ");
	builder.Append("optionList").Append(":").Append(optionList.ToString()).Append(", ");
	builder.Append("hadDraw").Append(":").Append(hadDraw.ToString()).Append(", ");
	builder.Append("drawRewardTimeMs").Append(":").Append(drawRewardTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

