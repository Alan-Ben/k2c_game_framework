package GC2GS.p032_GuildOp.GuildOpStructure;

import java.nio.ByteBuffer;
/*********
 * 添加火星求助信息
 **/
public class GuildOp_041_001_AddMarsHelp implements ALBasicProtocolPack._IALProtocolStructure {
private int dealLimit;
private int dealSecs;
private long usHelpDBId;
private byte[] helpExData;


public GuildOp_041_001_AddMarsHelp() {
	dealLimit = 0;
	dealSecs = 0;
	usHelpDBId = (long)0;
	helpExData = null;
}

public GuildOp_041_001_AddMarsHelp(
	 int _dealLimit
	, int _dealSecs
	, long _usHelpDBId
	, byte[] _helpExData
) {	dealLimit = _dealLimit;
	dealSecs = _dealSecs;
	usHelpDBId = _usHelpDBId;
	helpExData = _helpExData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getDealLimit() { return dealLimit; }
public void setDealLimit(int _dealLimit) { dealLimit = _dealLimit; }
public int getDealSecs() { return dealSecs; }
public void setDealSecs(int _dealSecs) { dealSecs = _dealSecs; }
public long getUsHelpDBId() { return usHelpDBId; }
public void setUsHelpDBId(long _usHelpDBId) { usHelpDBId = _usHelpDBId; }
public byte[] getHelpExData() { return helpExData; }
public java.nio.ByteBuffer get_buffer_HelpExData() { if(null == helpExData)return null; else return ByteBuffer.wrap(helpExData); }

public void setHelpExData(byte[] _helpExData) { helpExData = _helpExData; }
public void setHelpExData(java.nio.ByteBuffer _helpExData) 
{
	if(null == _helpExData){return;}
	int _oldPos = _helpExData.position();
	int _bufLength = _helpExData.remaining();
	helpExData = new byte[_bufLength];
	_helpExData.get(helpExData);
	_helpExData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 16;
	_size += 4 + (helpExData == null ? 0 : helpExData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 4 + (helpExData == null ? 0 : helpExData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealSecs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usHelpDBId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _helpExDataCount = _buf.getInt();
	if(0 < _helpExDataCount){
		helpExData = new byte[_helpExDataCount];
		_buf.get(helpExData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(dealLimit);
	_buf.putInt(dealSecs);
	_buf.putLong(usHelpDBId);
	_buf.putInt((helpExData == null ? 0 : helpExData.length));
	if(null != helpExData){_buf.put(helpExData);}

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

