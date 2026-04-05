package GC2GS.p003_FristTeamActivityOp;

import java.nio.ByteBuffer;
/*********
 * 游戏主体逻辑测试
 **/
public class GC2GS_003_001_ReqGameLogicTest implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long instanceId;
/** 测试参数1 */
private int param1;


public GC2GS_003_001_ReqGameLogicTest() {
	instanceId = (long)0;
	param1 = 0;
}

public GC2GS_003_001_ReqGameLogicTest(
	 long _instanceId
	, int _param1
) {	instanceId = _instanceId;
	param1 = _param1;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)1; }

/** 活动实例ID */
public long getInstanceId() { return instanceId; }
/** 活动实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 测试参数1 */
public int getParam1() { return param1; }
/** 测试参数1 */
public void setParam1(int _param1) { param1 = _param1; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) param1 = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(param1);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)1);
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

