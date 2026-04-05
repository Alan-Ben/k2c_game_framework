package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_CustomSlotGroup implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.WCGGS2GC_CustomRoomSlot> slots;


public WCGGS2GC_CustomSlotGroup() {
	slots = new java.util.ArrayList<Common.WCGGS2GC_CustomRoomSlot>();
}

public WCGGS2GC_CustomSlotGroup(
	 java.util.ArrayList<Common.WCGGS2GC_CustomRoomSlot> _slots
) {	slots = _slots;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public java.util.ArrayList<Common.WCGGS2GC_CustomRoomSlot> getSlots() { return slots; }
public void addSlots(Common.WCGGS2GC_CustomRoomSlot _slots) { slots.add(_slots); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < slots.size(); _i++) {
	_size += 4 + slots.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < slots.size(); _i++) {
	_size += 4 + slots.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _slotsCount = _buf.getShort();
	for(int _i = 0; _i < _slotsCount; _i++) { 
		Common.WCGGS2GC_CustomRoomSlot _slots = new Common.WCGGS2GC_CustomRoomSlot();
		if(_buf.remaining() <= 0) return;
	int __slotsCustLen = _buf.getInt();
	int __slotsCurPos = _buf.position();
	_slots.ReadUnzipBuf(_buf, __slotsCurPos + __slotsCustLen);
	_buf.position(__slotsCurPos + __slotsCustLen);

		slots.add(_slots);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)slots.size());
	for(int _i = 0; _i < slots.size(); _i++) { 
		_buf.putInt(slots.get(_i).GetBufSize());
	slots.get(_i).PutUnzipBuf(_buf);
	}
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

