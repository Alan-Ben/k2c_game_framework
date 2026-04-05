package NP2US_R.p008_ScheduleOp;

import java.nio.ByteBuffer;
/*********
 * 变更跨服分组
 **/
public class NP2US_R_008_001_ChgCrossServerGroup implements ALBasicProtocolPack._IALProtocolStructure {
/** 分组信息 */
private Common.NpServerObj.NPServerObj_CrossServerGroupInfo groupInfo;


public NP2US_R_008_001_ChgCrossServerGroup() {
	groupInfo = new Common.NpServerObj.NPServerObj_CrossServerGroupInfo();
}

public NP2US_R_008_001_ChgCrossServerGroup(
	 Common.NpServerObj.NPServerObj_CrossServerGroupInfo _groupInfo
) {	groupInfo = _groupInfo;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)1; }

/** 分组信息 */
public Common.NpServerObj.NPServerObj_CrossServerGroupInfo getGroupInfo() { return groupInfo; }
/** 分组信息 */
public void setGroupInfo(Common.NpServerObj.NPServerObj_CrossServerGroupInfo _groupInfo) { groupInfo = _groupInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + groupInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + groupInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _groupInfoCustLen = _buf.getInt();
	int _groupInfoCurPos = _buf.position();
	groupInfo.ReadUnzipBuf(_buf, _groupInfoCurPos + _groupInfoCustLen);
	_buf.position(_groupInfoCurPos + _groupInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(groupInfo.GetBufSize());
	groupInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
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

