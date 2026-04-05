using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_255_OnQuestCountInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 任务计数列表
/// </summary>
private List<Common.QuestObj.Quest_Count> questCountList;


public GS2GC_002_255_OnQuestCountInit() {
	questCountList = new List<Common.QuestObj.Quest_Count>();
}

public GS2GC_002_255_OnQuestCountInit(
	List<Common.QuestObj.Quest_Count> _questCountList
) {	questCountList = _questCountList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)255; }

/// <summary>
/// 任务计数列表
/// </summary>
public List<Common.QuestObj.Quest_Count> getQuestCountList() { return questCountList; }
/// <summary>
/// 任务计数列表
/// </summary>
public void addQuestCountList(Common.QuestObj.Quest_Count _questCountList) { questCountList.Add(_questCountList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (questCountList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (questCountList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _questCountListCount = _buf.getShort();
	for(int _i = 0; _i < _questCountListCount; _i++) { 
		Common.QuestObj.Quest_Count _questCountList = new Common.QuestObj.Quest_Count();
		int __questCountListCustLen = _buf.getInt();
	int __questCountListCurPos = _buf.getCurPos();
	_questCountList.ReadUnzipBuf(_buf, __questCountListCurPos + __questCountListCustLen);
	_buf.setPosition(__questCountListCurPos + __questCountListCustLen);

		questCountList.Add(_questCountList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)questCountList.Count);
	for(int _i = 0; _i < questCountList.Count; _i++) { 
		_buf.putInt(questCountList[_i].GetBufSize());
	questCountList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)255);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)255);
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
	builder.Append("questCountList").Append(":").Append(questCountList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

