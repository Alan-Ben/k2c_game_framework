using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

public class NPCommon_MailBriefInfo : ALBasicProtocolPack._IALProtocolStructure {
private long id;
private string title;
private long createTimeMs;
private long readTimeMs;
private long gainTimeMs;


public NPCommon_MailBriefInfo() {
	id = (long)0;
	title = "";
	createTimeMs = (long)0;
	readTimeMs = (long)0;
	gainTimeMs = (long)0;
}

public NPCommon_MailBriefInfo(
	long _id
	, string _title
	, long _createTimeMs
	, long _readTimeMs
	, long _gainTimeMs
) {	id = _id;
	title = _title;
	createTimeMs = _createTimeMs;
	readTimeMs = _readTimeMs;
	gainTimeMs = _gainTimeMs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
public string getTitle() { return title; }
public void setTitle(string _title) { title = _title; }
public long getCreateTimeMs() { return createTimeMs; }
public void setCreateTimeMs(long _createTimeMs) { createTimeMs = _createTimeMs; }
public long getReadTimeMs() { return readTimeMs; }
public void setReadTimeMs(long _readTimeMs) { readTimeMs = _readTimeMs; }
public long getGainTimeMs() { return gainTimeMs; }
public void setGainTimeMs(long _gainTimeMs) { gainTimeMs = _gainTimeMs; }


public int GetBufSize() {
	int _size = 32;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	title = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	createTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	readTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putString(title);
	_buf.putLong(createTimeMs);
	_buf.putLong(readTimeMs);
	_buf.putLong(gainTimeMs);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("title").Append(":").Append(title.ToString()).Append(", ");
	builder.Append("createTimeMs").Append(":").Append(createTimeMs.ToString()).Append(", ");
	builder.Append("readTimeMs").Append(":").Append(readTimeMs.ToString()).Append(", ");
	builder.Append("gainTimeMs").Append(":").Append(gainTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

