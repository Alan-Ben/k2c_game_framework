using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_QuestionnaireRewardInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 问卷实例id
/// </summary>
private long questionnaireId;
/// <summary>
/// 是否已领取
/// </summary>
private bool hasDraw;


public Common_QuestionnaireRewardInfo() {
	questionnaireId = (long)0;
	hasDraw = false;
}

public Common_QuestionnaireRewardInfo(
	long _questionnaireId
	, bool _hasDraw
) {	questionnaireId = _questionnaireId;
	hasDraw = _hasDraw;
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
/// 是否已领取
/// </summary>
public bool getHasDraw() { return hasDraw; }
/// <summary>
/// 是否已领取
/// </summary>
public void setHasDraw(bool _hasDraw) { hasDraw = _hasDraw; }


public int GetBufSize() {
	int _size = 9;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	questionnaireId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasDraw = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(questionnaireId);
	_buf.put(hasDraw?(byte)1:(byte)0);
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
	builder.Append("hasDraw").Append(":").Append(hasDraw.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

