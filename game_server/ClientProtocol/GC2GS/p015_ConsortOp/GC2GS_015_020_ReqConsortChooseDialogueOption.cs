using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p015_ConsortOp
{

/// <summary>
/// 家人-选择对话选项
/// </summary>
public class GC2GS_015_020_ReqConsortChooseDialogueOption : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 家人ID
/// </summary>
private long consortId;
/// <summary>
/// 对话ID
/// </summary>
private long dialogueId;
/// <summary>
/// 句子ID
/// </summary>
private long sentenceId;
/// <summary>
/// 选项ID
/// </summary>
private long optionId;


public GC2GS_015_020_ReqConsortChooseDialogueOption() {
	consortId = (long)0;
	dialogueId = (long)0;
	sentenceId = (long)0;
	optionId = (long)0;
}

public GC2GS_015_020_ReqConsortChooseDialogueOption(
	long _consortId
	, long _dialogueId
	, long _sentenceId
	, long _optionId
) {	consortId = _consortId;
	dialogueId = _dialogueId;
	sentenceId = _sentenceId;
	optionId = _optionId;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)20; }

/// <summary>
/// 家人ID
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 家人ID
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 对话ID
/// </summary>
public long getDialogueId() { return dialogueId; }
/// <summary>
/// 对话ID
/// </summary>
public void setDialogueId(long _dialogueId) { dialogueId = _dialogueId; }
/// <summary>
/// 句子ID
/// </summary>
public long getSentenceId() { return sentenceId; }
/// <summary>
/// 句子ID
/// </summary>
public void setSentenceId(long _sentenceId) { sentenceId = _sentenceId; }
/// <summary>
/// 选项ID
/// </summary>
public long getOptionId() { return optionId; }
/// <summary>
/// 选项ID
/// </summary>
public void setOptionId(long _optionId) { optionId = _optionId; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dialogueId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sentenceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	optionId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putLong(dialogueId);
	_buf.putLong(sentenceId);
	_buf.putLong(optionId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)20);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)20);
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
	builder.Append("dialogueId").Append(":").Append(dialogueId.ToString()).Append(", ");
	builder.Append("sentenceId").Append(":").Append(sentenceId.ToString()).Append(", ");
	builder.Append("optionId").Append(":").Append(optionId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

