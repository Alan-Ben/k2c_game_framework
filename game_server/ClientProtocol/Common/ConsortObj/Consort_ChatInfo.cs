using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ConsortObj
{

/// <summary>
/// 家人聊天数据
/// </summary>
public class Consort_ChatInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 家人ID
/// </summary>
private long consortId;
/// <summary>
/// 聊天对话列表
/// </summary>
private List<Common.ConsortObj.Consort_ChatDialogue> dialogueList;
/// <summary>
/// 是否已添加好友
/// </summary>
private bool hasAdd;


public Consort_ChatInfo() {
	consortId = (long)0;
	dialogueList = new List<Common.ConsortObj.Consort_ChatDialogue>();
	hasAdd = false;
}

public Consort_ChatInfo(
	long _consortId
	, List<Common.ConsortObj.Consort_ChatDialogue> _dialogueList
	, bool _hasAdd
) {	consortId = _consortId;
	dialogueList = _dialogueList;
	hasAdd = _hasAdd;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 家人ID
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 家人ID
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 聊天对话列表
/// </summary>
public List<Common.ConsortObj.Consort_ChatDialogue> getDialogueList() { return dialogueList; }
/// <summary>
/// 聊天对话列表
/// </summary>
public void addDialogueList(Common.ConsortObj.Consort_ChatDialogue _dialogueList) { dialogueList.Add(_dialogueList); }
/// <summary>
/// 是否已添加好友
/// </summary>
public bool getHasAdd() { return hasAdd; }
/// <summary>
/// 是否已添加好友
/// </summary>
public void setHasAdd(bool _hasAdd) { hasAdd = _hasAdd; }


public int GetBufSize() {
	int _size = 9;
	_size += 2;
for(int _i = 0; _i < dialogueList.Count; _i++) {
	_size += 4 + dialogueList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;
	_size += 2;
for(int _i = 0; _i < dialogueList.Count; _i++) {
	_size += 4 + dialogueList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _dialogueListCount = _buf.getShort();
	for(int _i = 0; _i < _dialogueListCount; _i++) { 
		Common.ConsortObj.Consort_ChatDialogue _dialogueList = new Common.ConsortObj.Consort_ChatDialogue();
		int __dialogueListCustLen = _buf.getInt();
	int __dialogueListCurPos = _buf.getCurPos();
	_dialogueList.ReadUnzipBuf(_buf, __dialogueListCurPos + __dialogueListCustLen);
	_buf.setPosition(__dialogueListCurPos + __dialogueListCustLen);

		dialogueList.Add(_dialogueList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasAdd = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putShort((short)dialogueList.Count);
	for(int _i = 0; _i < dialogueList.Count; _i++) { 
		_buf.putInt(dialogueList[_i].GetBufSize());
	dialogueList[_i].PutUnzipBuf(_buf);
	}
	_buf.put(hasAdd?(byte)1:(byte)0);
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
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("dialogueList").Append(":").Append(dialogueList.ToString()).Append(", ");
	builder.Append("hasAdd").Append(":").Append(hasAdd.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

