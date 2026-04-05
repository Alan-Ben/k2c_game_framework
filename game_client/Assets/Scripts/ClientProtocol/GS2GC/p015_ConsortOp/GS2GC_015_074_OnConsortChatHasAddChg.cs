using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人对话是否添加好友标志位变更
/// </summary>
public class GS2GC_015_074_OnConsortChatHasAddChg : ALBasicProtocolPack._IALProtocolStructure {
private long consortId;
/// <summary>
/// 是否添加好友
/// </summary>
private bool hasAdd;


public GS2GC_015_074_OnConsortChatHasAddChg() {
	consortId = (long)0;
	hasAdd = false;
}

public GS2GC_015_074_OnConsortChatHasAddChg(
	long _consortId
	, bool _hasAdd
) {	consortId = _consortId;
	hasAdd = _hasAdd;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)74; }

public long getConsortId() { return consortId; }
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 是否添加好友
/// </summary>
public bool getHasAdd() { return hasAdd; }
/// <summary>
/// 是否添加好友
/// </summary>
public void setHasAdd(bool _hasAdd) { hasAdd = _hasAdd; }


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
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasAdd = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.put(hasAdd?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)74);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)74);
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
	builder.Append("hasAdd").Append(":").Append(hasAdd.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

