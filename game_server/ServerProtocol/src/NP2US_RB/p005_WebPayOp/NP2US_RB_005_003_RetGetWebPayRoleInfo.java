package NP2US_RB.p005_WebPayOp;

import java.nio.ByteBuffer;
/*********
 * 返回网页支付角色信息
 **/
public class NP2US_RB_005_003_RetGetWebPayRoleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 角色信息 */
private Common.ServerObj.ServerObj_WebPayRoleInfo roleInfo;


public NP2US_RB_005_003_RetGetWebPayRoleInfo() {
	roleInfo = new Common.ServerObj.ServerObj_WebPayRoleInfo();
}

public NP2US_RB_005_003_RetGetWebPayRoleInfo(
	 Common.ServerObj.ServerObj_WebPayRoleInfo _roleInfo
) {	roleInfo = _roleInfo;
}

public final byte getMainOrder() { return (byte)5; }

public final byte getSubOrder() { return (byte)3; }

/** 角色信息 */
public Common.ServerObj.ServerObj_WebPayRoleInfo getRoleInfo() { return roleInfo; }
/** 角色信息 */
public void setRoleInfo(Common.ServerObj.ServerObj_WebPayRoleInfo _roleInfo) { roleInfo = _roleInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + roleInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + roleInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _roleInfoCustLen = _buf.getInt();
	int _roleInfoCurPos = _buf.position();
	roleInfo.ReadUnzipBuf(_buf, _roleInfoCurPos + _roleInfoCustLen);
	_buf.position(_roleInfoCurPos + _roleInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(roleInfo.GetBufSize());
	roleInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)5);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)5);
	_recBuf.put((byte)3);
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

