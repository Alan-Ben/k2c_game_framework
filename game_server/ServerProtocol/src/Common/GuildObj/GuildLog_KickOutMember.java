package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟踢出成员
 **/
public class GuildLog_KickOutMember implements ALBasicProtocolPack._IALProtocolStructure {
/** 操作者信息 */
private Common.GuildObj.GuildLog_OperatorInfo operatorInfo;
/** 目标成员名 */
private String targetPlayerName;


public GuildLog_KickOutMember() {
	operatorInfo = new Common.GuildObj.GuildLog_OperatorInfo();
	targetPlayerName = "";
}

public GuildLog_KickOutMember(
	 Common.GuildObj.GuildLog_OperatorInfo _operatorInfo
	, String _targetPlayerName
) {	operatorInfo = _operatorInfo;
	targetPlayerName = _targetPlayerName;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 操作者信息 */
public Common.GuildObj.GuildLog_OperatorInfo getOperatorInfo() { return operatorInfo; }
/** 操作者信息 */
public void setOperatorInfo(Common.GuildObj.GuildLog_OperatorInfo _operatorInfo) { operatorInfo = _operatorInfo; }
/** 目标成员名 */
public String getTargetPlayerName() { return targetPlayerName; }
/** 目标成员名 */
public void setTargetPlayerName(String _targetPlayerName) { targetPlayerName = _targetPlayerName; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + operatorInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(targetPlayerName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + operatorInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(targetPlayerName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _operatorInfoCustLen = _buf.getInt();
	int _operatorInfoCurPos = _buf.position();
	operatorInfo.ReadUnzipBuf(_buf, _operatorInfoCurPos + _operatorInfoCustLen);
	_buf.position(_operatorInfoCurPos + _operatorInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) targetPlayerName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(operatorInfo.GetBufSize());
	operatorInfo.PutUnzipBuf(_buf);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, targetPlayerName);
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

