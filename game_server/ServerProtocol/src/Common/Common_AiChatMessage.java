package Common;

import java.nio.ByteBuffer;
/*********
 * AI聊天消息
 **/
public class Common_AiChatMessage implements ALBasicProtocolPack._IALProtocolStructure {
/** 角色类型 */
private CommonEnum.EAiChatRoleType roleType;
/** 聊天内容 */
private String msg;


public Common_AiChatMessage() {
	roleType = CommonEnum.EAiChatRoleType.values()[0];
	msg = "";
}

public Common_AiChatMessage(
	 CommonEnum.EAiChatRoleType _roleType
	, String _msg
) {	roleType = _roleType;
	msg = _msg;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 角色类型 */
public CommonEnum.EAiChatRoleType getRoleType() { return roleType; }
/** 角色类型 */
public void setRoleType(CommonEnum.EAiChatRoleType _roleType) { roleType = _roleType; }
/** 聊天内容 */
public String getMsg() { return msg; }
/** 聊天内容 */
public void setMsg(String _msg) { msg = _msg; }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(msg);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(msg);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roleType = CommonEnum.EAiChatRoleType.EAiChatRoleType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) msg = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(roleType.ordinal());

	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, msg);
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

