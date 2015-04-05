using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvokeIR.PowerForensics.NTFS
{

    public class IndexRoot : Attr
    {

        struct NODE_HEADER
        {
            internal uint StartEntryOffset;
            internal uint EndUsedOffset;
            internal uint EndAllocatedOffset;
            internal uint Flags;
        }
        
        struct ATTR_INDEX_ROOT
        {
            internal AttrHeader.ATTR_HEADER_RESIDENT header;
            internal uint AttributeType;
            internal uint CollationSortingRule;
            internal uint IndexSizeinBytes;
            internal byte IndexSizeinClusters;
            internal byte[] Padding;
            internal NODE_HEADER NodeHeader;

        }

    }

}
