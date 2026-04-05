using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_MailDetails : ALBasicProtocolPack._IALProtocolStructure {
private long sId;
private string content;
private List<string> contentParams;
private Common.WCGGS2GC_UniformPack uniformPack;


public WCGGS2GC_MailDetails() {
	sId = (long)0;
	content = "";
	contentParams = new List<string>();
	uniformPack = new Common.WCGGS2GC_UniformPack();
}

public WCGGS2GC_MailDetails(
	long _sId
	, string _content
	, List<string> _contentParams
	, Common.WCGGS2GC_UniformPack _uniformPack
) {	sId = _sId;
	content = _content;
	contentParams = _contentParams;
	uniformPack = _uniformPack;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getSId() { return sId; }
public void setSId(long _sId) { sId = _sId; }
public string getContent() { return content; }
public void setContent(string _content) { content = _content; }
public List<string> getContentParams() { return contentParams; }
public void addContentParams(string _contentParams) { contentParams.Add(_contentParams); }
public Common.WCGGS2GC_UniformPack getUniformPack() { return uniformPack; }
public void setUniformPack(Common.WCGGS2GC_UniformPack _uniformPack) { uniformPack = _uniformPack; }


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
for(int _i = 0; _i < contentParams.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentParams[_i]);
	}

	_size += 4 + uniformPack.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
for(int _i = 0; _i < contentParams.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentParams[_i]);
	}

	_size += 4 + uniformPack.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	content = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _contentParamsCount = _buf.getShort();
	for(int _i = 0; _i < _contentParamsCount; _i++) { 
		string _contentParams = "";
		_contentParams = _buf.getString();
		contentParams.Add(_contentParams);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _uniformPackCustLen = _buf.getInt();
	int _uniformPackCurPos = _buf.getCurPos();
	uniformPack.ReadUnzipBuf(_buf, _uniformPackCurPos + _uniformPackCustLen);
	_buf.setPosition(_uniformPackCurPos + _uniformPackCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(sId);
	_buf.putString(content);
	_buf.putShort((short)contentParams.Count);
	for(int _i = 0; _i < contentParams.Count; _i++) { 
		_buf.putString(contentParams[_i]);
	}
	_buf.putInt(uniformPack.GetBufSize());
	uniformPack.PutUnzipBuf(_buf);
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
	builder.Append("sId").Append(":").Append(sId.ToString()).Append(", ");
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("contentParams").Append(":").Append(contentParams.ToString()).Append(", ");
	builder.Append("uniformPack").Append(":").Append(uniformPack == null ? "null" : uniformPack.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

