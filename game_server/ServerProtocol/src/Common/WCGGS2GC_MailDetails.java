package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_MailDetails implements ALBasicProtocolPack._IALProtocolStructure {
private long sId;
private String content;
private java.util.ArrayList<String> contentParams;
private Common.WCGGS2GC_UniformPack uniformPack;


public WCGGS2GC_MailDetails() {
	sId = (long)0;
	content = "";
	contentParams = new java.util.ArrayList<String>();
	uniformPack = new Common.WCGGS2GC_UniformPack();
}

public WCGGS2GC_MailDetails(
	 long _sId
	, String _content
	, java.util.ArrayList<String> _contentParams
	, Common.WCGGS2GC_UniformPack _uniformPack
) {	sId = _sId;
	content = _content;
	contentParams = _contentParams;
	uniformPack = _uniformPack;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getSId() { return sId; }
public void setSId(long _sId) { sId = _sId; }
public String getContent() { return content; }
public void setContent(String _content) { content = _content; }
public java.util.ArrayList<String> getContentParams() { return contentParams; }
public void addContentParams(String _contentParams) { contentParams.add(_contentParams); }
public Common.WCGGS2GC_UniformPack getUniformPack() { return uniformPack; }
public void setUniformPack(Common.WCGGS2GC_UniformPack _uniformPack) { uniformPack = _uniformPack; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
	for(int _i = 0; _i < contentParams.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentParams.get(_i));
	}

	_size += 4 + uniformPack.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
	for(int _i = 0; _i < contentParams.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentParams.get(_i));
	}

	_size += 4 + uniformPack.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _contentParamsCount = _buf.getShort();
	for(int _i = 0; _i < _contentParamsCount; _i++) { 
		String _contentParams = "";
		if(_buf.remaining() > 0) _contentParams = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
		contentParams.add(_contentParams);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _uniformPackCustLen = _buf.getInt();
	int _uniformPackCurPos = _buf.position();
	uniformPack.ReadUnzipBuf(_buf, _uniformPackCurPos + _uniformPackCustLen);
	_buf.position(_uniformPackCurPos + _uniformPackCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(sId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
	_buf.putShort((short)contentParams.size());
	for(int _i = 0; _i < contentParams.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, contentParams.get(_i));
	}
	_buf.putInt(uniformPack.GetBufSize());
	uniformPack.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

