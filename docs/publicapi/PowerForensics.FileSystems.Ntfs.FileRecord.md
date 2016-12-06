


# FileSystems.Ntfs.FileRecord

## Fields

### VolumePath
Path to Volume
### OffsetOfUS
Offset of Update Sequence
### SizeOfUS
Size in words of Update Sequence Number and Array
### LogFileSequenceNumber
$LogFile Sequence Number
### SequenceNumber
Sequence number
### Hardlinks

### OffsetOfAttribute

### Flags

### Deleted

### Directory

### RealSize

### AllocatedSize

### ReferenceToBase

### NextAttrId

### RecordNumber

### Attribute

### ModifiedTime

### AccessedTime

### ChangedTime

### BornTime

### Permission

### FullName

### Name

### ParentSequenceNumber

### ParentRecordNumber

### FNModifiedTime

### FNAccessedTime

### FNChangedTime

### FNBornTime

## Methods


### GetInstances(System.String)

> #### Parameters
> **volume:** 

> #### Return value
> 

### GetInstancesByPath(System.String)

> #### Parameters
> **path:** 

> #### Return value
> 

### Get(System.String)

> #### Parameters
> **path:** 

> #### Return value
> 

### Get(System.String,System.Int32)

> #### Parameters
> **volume:** 

> **index:** 

> #### Return value
> 

### GetRecordBytes(System.String)

> #### Parameters
> **path:** 

> #### Return value
> 

### GetRecordBytes(System.String,System.Int32)

> #### Parameters
> **volume:** 

> **index:** 

> #### Return value
> 

### GetContentBytes(System.String)

> #### Parameters
> **path:** 

> #### Return value
> 

### GetContentBytes(System.String,System.String)

> #### Parameters
> **path:** 

> **streamName:** 

> #### Return value
> 

### GetContent

> #### Return value
> 

### GetContent(System.String)

> #### Parameters
> **StreamName:** 

> #### Return value
> 

### CopyFile(System.String)

> #### Parameters
> **Destination:** 


### CopyFile(System.String,System.String)

> #### Parameters
> **Destination:** 

> **StreamName:** 


### GetChild

> #### Return value
> 

### GetParent

> #### Return value
> 

### GetUsnJrnl

> #### Return value
> 

### GetSlack

> #### Return value
> 

### GetMftSlack

> #### Return value
> 

### ToString

> #### Return value
> 