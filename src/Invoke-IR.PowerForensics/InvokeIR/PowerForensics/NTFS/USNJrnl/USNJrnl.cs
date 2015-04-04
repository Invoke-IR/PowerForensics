using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS.USNJrnl
{
    class USNJrnl
    {
        private enum USN_REASON
        {
            //0x00000001 The default $DATA attribute was overwritten
            //0x00000002 The default $DATA attribute was extended
            //0x00000004 The default $DATA attribute was truncated
            //0x00000010 A named $DATA attribute was overwritten
            //0x00000020 A named $DATA attribute was extended
            //0x00000040 A named $DATA attribute was truncated
            //0x00000100 The file or directory was created
            //0x00000200 The file or directory was deleted
            //0x00000400 The extended attributes of the file were changed
            //0x00000800 The security descriptor was changed
            //0x00001000 The name changed—change journal entry has old name
            //0x00002000 The name changed—change journal entry has new name
            //0x00004000 Content indexed status changed
            //0x00008000 Changed basic file or directory attributes
            //0x00010000 A hard link was created or deleted
            //0x00020000 Compression status changed
            //0x00040000 Encryption status changed
            //0x00080000 Object ID changed
            //0x00100000 Reparse point value changed
            //0x00200000 A named $DATA attribute was created, deleted, or changed
            //0x80000000 The file or directory was closed
        }

        private struct USN_RECORD
        {
            uint RecordLength;
            ushort MajorVersion;
            ushort MinorVersion;
            ulong FileReferenceNumber;
            ulong ParentFileReferenceNumber;
            //USN Usn;
            DateTime TimeStamp;
            uint Reason;
            uint SourceInfo;
            uint SecurityId;
            uint FileAttributes;
            ushort FileNameLength;
            ushort FileNameOffset;
            byte[] FileName;
        }



    }
}
