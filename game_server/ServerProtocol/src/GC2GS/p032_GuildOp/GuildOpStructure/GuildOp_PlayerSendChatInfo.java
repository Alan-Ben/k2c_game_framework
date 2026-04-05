package GC2GS.p032_GuildOp.GuildOpStructure;

import java.nio.ByteBuffer;
/*********
 * 附带玩家聊天Uid
 **/
public class GuildOp_PlayerSendChatInfo implements ALBasicProtocolPack._IALProtocolStructure {
private String chatUid;
private Common.NpChatObj.NPCommon_ChatPlayerContent playerContent;


public GuildOp_PlayerSendChatInfo() {
	chatUid = "";
	playerContent = new Common.NpChatObj.NPCommon_ChatPlayerContent();
}

public GuildOp_PlayerSendChatInfo(
	 String _chatUid
	, Common.NpChatObj.NPCommon_ChatPlayerContent _playerContent
) {	chatUid = _chatUid;
	playerContent = _playerContent;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public String getChatUid() { return chatUid; }
public void setChatUid(String _chatUid) { chatUid = _chatUid; }
public Common.NpChatObj.NPCommon_ChatPlayerContent getPlayerContent() { return playerContent; }
public void setPlayerContent(Common.NpChatObj.NPCommon_ChatPlayerContent _playerContent) { playerContent = _playerContent; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chatUid);
	_size += 4 + playerContent.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chatUid);
	_size += 4 + playerContent.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chatUid = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _playerContentCustLen = _buf.getInt();
	int _playerContentCurPos = _buf.position();
	playerContent.ReadUnzipBuf(_buf, _playerContentCurPos + _playerContentCustLen);
	_buf.position(_playerContentCurPos + _playerContentCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, chatUid);
	_buf.putInt(playerContent.GetBufSize());
	playerContent.PutUnzipBuf(_buf);
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

