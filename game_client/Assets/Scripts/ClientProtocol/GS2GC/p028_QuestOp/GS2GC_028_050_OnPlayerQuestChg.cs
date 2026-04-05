using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p028_QuestOp
{

public class GS2GC_028_050_OnPlayerQuestChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.QuestObj.Quest_info quest;


public GS2GC_028_050_OnPlayerQuestChg() {
	quest = new Common.QuestObj.Quest_info();
}

public GS2GC_028_050_OnPlayerQuestChg(
	Common.QuestObj.Quest_info _quest
) {	quest = _quest;
}

public byte getMainOrder() { return (byte)28; }

public byte getSubOrder() { return (byte)50; }

public Common.QuestObj.Quest_info getQuest() { return quest; }
public void setQuest(Common.QuestObj.Quest_info _quest) { quest = _quest; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + quest.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + quest.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _questCustLen = _buf.getInt();
	int _questCurPos = _buf.getCurPos();
	quest.ReadUnzipBuf(_buf, _questCurPos + _questCustLen);
	_buf.setPosition(_questCurPos + _questCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(quest.GetBufSize());
	quest.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)50);
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
	builder.Append("quest").Append(":").Append(quest == null ? "null" : quest.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

