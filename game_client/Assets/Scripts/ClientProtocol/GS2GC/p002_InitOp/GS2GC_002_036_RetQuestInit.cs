using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_036_RetQuestInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务列表
/// </summary>
private List<Common.QuestObj.Quest_info> questList;


public GS2GC_002_036_RetQuestInit() {
	questList = new List<Common.QuestObj.Quest_info>();
}

public GS2GC_002_036_RetQuestInit(
	List<Common.QuestObj.Quest_info> _questList
) {	questList = _questList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)36; }

/// <summary>
/// 任务列表
/// </summary>
public List<Common.QuestObj.Quest_info> getQuestList() { return questList; }
/// <summary>
/// 任务列表
/// </summary>
public void addQuestList(Common.QuestObj.Quest_info _questList) { questList.Add(_questList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < questList.Count; _i++) {
	_size += 4 + questList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < questList.Count; _i++) {
	_size += 4 + questList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _questListCount = _buf.getShort();
	for(int _i = 0; _i < _questListCount; _i++) { 
		Common.QuestObj.Quest_info _questList = new Common.QuestObj.Quest_info();
		int __questListCustLen = _buf.getInt();
	int __questListCurPos = _buf.getCurPos();
	_questList.ReadUnzipBuf(_buf, __questListCurPos + __questListCustLen);
	_buf.setPosition(__questListCurPos + __questListCustLen);

		questList.Add(_questList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)questList.Count);
	for(int _i = 0; _i < questList.Count; _i++) { 
		_buf.putInt(questList[_i].GetBufSize());
	questList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)36);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)36);
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
	builder.Append("questList").Append(":").Append(questList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

