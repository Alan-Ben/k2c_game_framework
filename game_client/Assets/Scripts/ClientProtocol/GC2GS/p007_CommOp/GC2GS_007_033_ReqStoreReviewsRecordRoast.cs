using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p007_CommOp
{

/// <summary>
/// 记录商店评价吐槽内容
/// </summary>
public class GC2GS_007_033_ReqStoreReviewsRecordRoast : ALBasicProtocolPack._IALProtocolStructure {
private string content;


public GC2GS_007_033_ReqStoreReviewsRecordRoast() {
	content = "";
}

public GC2GS_007_033_ReqStoreReviewsRecordRoast(
	string _content
) {	content = _content;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)33; }

public string getContent() { return content; }
public void setContent(string _content) { content = _content; }


public int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	content = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(content);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)33);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)33);
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
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

