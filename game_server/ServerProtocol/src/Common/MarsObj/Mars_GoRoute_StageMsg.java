package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 前往火星-阶段留言
 **/
public class Mars_GoRoute_StageMsg implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家名称 */
private String playerName;
/** 留言内容 */
private String content;
/** 留言时间（毫秒） */
private long timeMs;


public Mars_GoRoute_StageMsg() {
	playerName = "";
	content = "";
	timeMs = (long)0;
}

public Mars_GoRoute_StageMsg(
	 String _playerName
	, String _content
	, long _timeMs
) {	playerName = _playerName;
	content = _content;
	timeMs = _timeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家名称 */
public String getPlayerName() { return playerName; }
/** 玩家名称 */
public void setPlayerName(String _playerName) { playerName = _playerName; }
/** 留言内容 */
public String getContent() { return content; }
/** 留言内容 */
public void setContent(String _content) { content = _content; }
/** 留言时间（毫秒） */
public long getTimeMs() { return timeMs; }
/** 留言时间（毫秒） */
public void setTimeMs(long _timeMs) { timeMs = _timeMs; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, playerName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
	_buf.putLong(timeMs);
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

