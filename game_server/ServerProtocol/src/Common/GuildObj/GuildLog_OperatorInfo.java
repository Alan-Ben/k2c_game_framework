package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟操作者信息
 **/
public class GuildLog_OperatorInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 操作者名 */
private String operatorName;
/** 操作者职位 */
private Common.GuildEnum.EGuildPositionType operatorPosition;


public GuildLog_OperatorInfo() {
	operatorName = "";
	operatorPosition = Common.GuildEnum.EGuildPositionType.values()[0];
}

public GuildLog_OperatorInfo(
	 String _operatorName
	, Common.GuildEnum.EGuildPositionType _operatorPosition
) {	operatorName = _operatorName;
	operatorPosition = _operatorPosition;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 操作者名 */
public String getOperatorName() { return operatorName; }
/** 操作者名 */
public void setOperatorName(String _operatorName) { operatorName = _operatorName; }
/** 操作者职位 */
public Common.GuildEnum.EGuildPositionType getOperatorPosition() { return operatorPosition; }
/** 操作者职位 */
public void setOperatorPosition(Common.GuildEnum.EGuildPositionType _operatorPosition) { operatorPosition = _operatorPosition; }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(operatorName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(operatorName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) operatorName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) operatorPosition = Common.GuildEnum.EGuildPositionType.EGuildPositionType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, operatorName);
	_buf.putInt(operatorPosition.ordinal());

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

