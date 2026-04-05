package GC2GS.p038_MarsOp;

import java.nio.ByteBuffer;
/*********
 * 前往火星-发送阶段留言
 **/
public class GC2GS_038_004_ReqSendStageMsg implements ALBasicProtocolPack._IALProtocolStructure {
/** 当前阶段 */
private int stage;
/** 留言内容 */
private String content;


public GC2GS_038_004_ReqSendStageMsg() {
	stage = 0;
	content = "";
}

public GC2GS_038_004_ReqSendStageMsg(
	 int _stage
	, String _content
) {	stage = _stage;
	content = _content;
}

public final byte getMainOrder() { return (byte)38; }

public final byte getSubOrder() { return (byte)4; }

/** 当前阶段 */
public int getStage() { return stage; }
/** 当前阶段 */
public void setStage(int _stage) { stage = _stage; }
/** 留言内容 */
public String getContent() { return content; }
/** 留言内容 */
public void setContent(String _content) { content = _content; }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stage = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(stage);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)38);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)38);
	_recBuf.put((byte)4);
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

