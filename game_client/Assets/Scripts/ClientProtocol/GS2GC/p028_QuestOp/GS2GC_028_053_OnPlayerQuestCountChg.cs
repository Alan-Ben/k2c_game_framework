using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p028_QuestOp
{

public class GS2GC_028_053_OnPlayerQuestCountChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.QuestObj.Quest_Count questCount;


public GS2GC_028_053_OnPlayerQuestCountChg() {
	questCount = new Common.QuestObj.Quest_Count();
}

public GS2GC_028_053_OnPlayerQuestCountChg(
	Common.QuestObj.Quest_Count _questCount
) {	questCount = _questCount;
}

public byte getMainOrder() { return (byte)28; }

public byte getSubOrder() { return (byte)53; }

public Common.QuestObj.Quest_Count getQuestCount() { return questCount; }
public void setQuestCount(Common.QuestObj.Quest_Count _questCount) { questCount = _questCount; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _questCountCustLen = _buf.getInt();
	int _questCountCurPos = _buf.getCurPos();
	questCount.ReadUnzipBuf(_buf, _questCountCurPos + _questCountCustLen);
	_buf.setPosition(_questCountCurPos + _questCountCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(questCount.GetBufSize());
	questCount.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)28);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)28);
	_recBuf.put((byte)53);
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
	builder.Append("questCount").Append(":").Append(questCount == null ? "null" : questCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

