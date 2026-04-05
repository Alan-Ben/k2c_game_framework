package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟改名
 **/
public class GuildLog_GuildRename implements ALBasicProtocolPack._IALProtocolStructure {
/** 操作者信息 */
private Common.GuildObj.GuildLog_OperatorInfo operatorInfo;
private String newName;


public GuildLog_GuildRename() {
	operatorInfo = new Common.GuildObj.GuildLog_OperatorInfo();
	newName = "";
}

public GuildLog_GuildRename(
	 Common.GuildObj.GuildLog_OperatorInfo _operatorInfo
	, String _newName
) {	operatorInfo = _operatorInfo;
	newName = _newName;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 操作者信息 */
public Common.GuildObj.GuildLog_OperatorInfo getOperatorInfo() { return operatorInfo; }
/** 操作者信息 */
public void setOperatorInfo(Common.GuildObj.GuildLog_OperatorInfo _operatorInfo) { operatorInfo = _operatorInfo; }
public String getNewName() { return newName; }
public void setNewName(String _newName) { newName = _newName; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + operatorInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(newName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + operatorInfo.GetBufSize();
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(newName);

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
	if(_buf.remaining() > 0) newName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(operatorInfo.GetBufSize());
	operatorInfo.PutUnzipBuf(_buf);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, newName);
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

