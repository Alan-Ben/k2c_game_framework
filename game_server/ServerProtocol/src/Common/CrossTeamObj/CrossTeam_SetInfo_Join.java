package Common.CrossTeamObj;

import java.nio.ByteBuffer;
/*********
 * 组队设置-加入条件
 **/
public class CrossTeam_SetInfo_Join implements ALBasicProtocolPack._IALProtocolStructure {
private Common.CrossTeamEnum.ENPCrossTeamJoinCond cond;
private long value;


public CrossTeam_SetInfo_Join() {
	cond = Common.CrossTeamEnum.ENPCrossTeamJoinCond.values()[0];
	value = (long)0;
}

public CrossTeam_SetInfo_Join(
	 Common.CrossTeamEnum.ENPCrossTeamJoinCond _cond
	, long _value
) {	cond = _cond;
	value = _value;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public Common.CrossTeamEnum.ENPCrossTeamJoinCond getCond() { return cond; }
public void setCond(Common.CrossTeamEnum.ENPCrossTeamJoinCond _cond) { cond = _cond; }
public long getValue() { return value; }
public void setValue(long _value) { value = _value; }


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
	if(_buf.remaining() > 0) cond = Common.CrossTeamEnum.ENPCrossTeamJoinCond.ENPCrossTeamJoinCond_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) value = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(cond.ordinal());

	_buf.putLong(value);
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

