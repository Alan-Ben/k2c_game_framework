package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_002_RetCrossServerGroupInfo implements ALBasicProtocolPack._IALProtocolStructure {
private Common.Common_CrossServerGroupInfo groupInfo;


public GS2GC_002_002_RetCrossServerGroupInfo() {
	groupInfo = new Common.Common_CrossServerGroupInfo();
}

public GS2GC_002_002_RetCrossServerGroupInfo(
	 Common.Common_CrossServerGroupInfo _groupInfo
) {	groupInfo = _groupInfo;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)2; }

public Common.Common_CrossServerGroupInfo getGroupInfo() { return groupInfo; }
public void setGroupInfo(Common.Common_CrossServerGroupInfo _groupInfo) { groupInfo = _groupInfo; }


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
	_buf.put((byte)2);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)2);
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

