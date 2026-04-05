package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 平台获取的跑马灯内容信息
 **/
public class ServerObj_PHPMarqueeContent implements ALBasicProtocolPack._IALProtocolStructure {
/** 语言 */
private String lang;
/** 参数列表 */
private java.util.ArrayList<String> paramList;
/** 内容 */
private String content;


public ServerObj_PHPMarqueeContent() {
	lang = "";
	paramList = new java.util.ArrayList<String>();
	content = "";
}

public ServerObj_PHPMarqueeContent(
	 String _lang
	, java.util.ArrayList<String> _paramList
	, String _content
) {	lang = _lang;
	paramList = _paramList;
	content = _content;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 语言 */
public String getLang() { return lang; }
/** 语言 */
public void setLang(String _lang) { lang = _lang; }
/** 参数列表 */
public java.util.ArrayList<String> getParamList() { return paramList; }
/** 参数列表 */
public void addParamList(String _paramList) { paramList.add(_paramList); }
/** 内容 */
public String getContent() { return content; }
/** 内容 */
public void setContent(String _content) { content = _content; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(lang);
	_size += 2;
	for(int _i = 0; _i < paramList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(paramList.get(_i));
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(lang);
	_size += 2;
	for(int _i = 0; _i < paramList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(paramList.get(_i));
	}

	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lang = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _paramListCount = _buf.getShort();
	for(int _i = 0; _i < _paramListCount; _i++) { 
		String _paramList = "";
		if(_buf.remaining() > 0) _paramList = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
		paramList.add(_paramList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, lang);
	_buf.putShort((short)paramList.size());
	for(int _i = 0; _i < paramList.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, paramList.get(_i));
	}
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
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

