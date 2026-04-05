using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ConsortObj
{

/// <summary>
/// 家人聊天句子选择结果
/// </summary>
public class Consort_DialogueOption : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 句子id
/// </summary>
private long sentenceId;
/// <summary>
/// 选项id
/// </summary>
private long optionId;


public Consort_DialogueOption() {
	sentenceId = (long)0;
	optionId = (long)0;
}

public Consort_DialogueOption(
	long _sentenceId
	, long _optionId
) {	sentenceId = _sentenceId;
	optionId = _optionId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 句子id
/// </summary>
public long getSentenceId() { return sentenceId; }
/// <summary>
/// 句子id
/// </summary>
public void setSentenceId(long _sentenceId) { sentenceId = _sentenceId; }
/// <summary>
/// 选项id
/// </summary>
public long getOptionId() { return optionId; }
/// <summary>
/// 选项id
/// </summary>
public void setOptionId(long _optionId) { optionId = _optionId; }


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
	sentenceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	optionId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(sentenceId);
	_buf.putLong(optionId);
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
	builder.Append("sentenceId").Append(":").Append(sentenceId.ToString()).Append(", ");
	builder.Append("optionId").Append(":").Append(optionId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

