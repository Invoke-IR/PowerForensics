<p align="center">
  <img src="https://cdn.rawgit.com/Invoke-IR/PowerForensics/master/Images/powerforensics_square_blue.svg" width="300" height="300">
</p>

<h1 align="center">PowerForensics - PowerShell Digital Forensics</h1>

<h5 align="center">Developed by <a href="https://twitter.com/jaredcatkinson">@jaredcatkinson</a></h5>

## Overview
The purpose of PowerForensics is to provide an all inclusive framework for hard drive forensic analysis.
PowerForensics currently supports NTFS and FAT file systems, and work has begun on Extended File System and HFS+ support.

Detailed instructions for installing PowerForensics can be found <a href="http://www.invoke-ir.com/2016/02/installing-powerforensics.html">here</a>.

## Cmdlets
### Boot Sector
```
Get-ForensicMasterBootRecord - gets the MasterBootRecord from the first sector of the hard drive
Get-ForensicGuidPartitionTable - gets the GuidPartitionTable from the first sector of the hard drive
Get-ForensicBootSector - gets the appropriate boot sector (MBR or GPT) from the specified drive
Get-ForensicPartitionTable - gets the partition table for the specified drive
```

### Extended File System 4 (ext4)
```
Get-ForensicSuperblock - returns the ext4 SuperBlock object
Get-ForensicBlockGroupDescriptor - returns the Block Group Descriptor Table entries
Get-ForensicInode - returns the Inode Table entries
```

### New Technology File System (NTFS)
```
Get-ForensicAttrDef - gets definitions of MFT Attributes (parses $AttrDef)
Get-ForensicBitmap - determines if a cluster is marked as in use (parses $Bitmap)
Get-ForensicFileRecord - gets Master File Table entries (parses $MFT)
Get-ForensicFileRecordIndex - gets a file's MFT record index number
Get-ForensicUsnJrnl - getss Usn Journal Entries (parses $UsnJrnl:$J)
Get-ForensicUsnJrnlInformation - getss UsnJrnl Metadata (parses $UsnJrnl:$Max)
Get-ForensicVolumeBootRecord - gets the VolumeBootRecord from the first sector of the volume (parses $Boot)
Get-ForensicVolumeInformation - gets the $Volume file's $VOLUME_INFORMATION attribute
Get-ForensicVolumeName - gets the $Volume file's $VOLUME_NAME attribute
Get-ForensicFileSlack - gets the specified volume's slack space
Get-ForensicMftSlack - gets the Master File Table (MFT) slack space for the specified volume
Get-ForensicUnallocatedSpace - gets the unallocated space on the specified partition/volume (parses $Bitmap)
```

### Windows Artifacts
```
Get-AlternateDataStream - gets the NTFS Alternate Data Streams on the specified volume
Get-ForensicEventLog - gets the events in an event log or in all event logs
Get-ForensicExplorerTypedPath - gets the file paths that have been typed into the Windows Explorer application
Get-ForensicNetworkList - gets a list of networks that the system has previously been connected to 
Get-ForensicOfficeFileMru - gets a files that have been recently opened in Microsoft Office
Get-ForensicOfficeOutlookCatalog - gets a Outlook pst file paths
Get-ForensicOfficePlaceMru - gets a directories that have recently been opened in Microsoft Office
Get-ForensicOfficeTrustRecord - gets files that have been explicitly trusted within MicrosoftOffice
Get-ForensicPrefetch - gets Windows Prefetch artifacts by parsing the file's binary structure
Get-ForensicRunKey - gets the persistence mechanism stored in registry run keys
Get-ForensicRunMostRecentlyUsed - gets the commands that were issued by the user to the run dialog
Get-ForensicScheduledJob - gets Scheduled Jobs (at jobs) by parsing the file's binary structures
Get-ForensicShellLink - gets ShellLink (.lnk) artifacts by parsing the file's binary structure
Get-ForensicSid - gets the machine Security Identifier from the SAM hive
Get-ForensicTimezone - gets the system's timezone based on the registry setting
Get-ForensicTypedUrl - gets the Universal Resource Locators (URL) that have been typed into Internet Explorer
Get-ForensicUserAssist - gets the UserAssist entries from the specified volume
Get-ForensicWindowsSearchHistory - gets the terms that have been searched for using the Windows Search feature
```

#### Application Compatibility Cache
```
Get-ForensicAmcache - gets previously run commands from the Amcache.hve registry hive
Get-ForensicRecentFileCache - gets previously run commands from the RecentFileCache.bcf file
Get-ForensicShimcache - gets previously run commands from the AppCompatCache (AppCompatibility on XP) registry key
```


### Windows Registry
```
Get-ForensicRegistryKey - gets the keys of the specified registry hive
Get-ForensicRegistryValue - gets the values of the specified registry key
```

### Forensic Timeline
```
ConvertTo-ForensicTimeline - converts an object to a ForensicTimeline object
Get-ForensicTimeline - creates a forensic timeline
```

### Utilities
```
Copy-ForensicFile - creates a copy of a file from its raw bytes on disk 
Get-ForensicChildItem - returns a directory's contents by parsing the MFT structures
Get-ForensicContent - gets the content of a file from its raw bytes on disk
Invoke-ForensicDD - provides a bit for bit copy of a specified device
```

## Public API
PowerForensics is built on a C# Class Library (Assembly) that provides an public forensic API.
All of this module's cmdlets are built on this public API and tasks can easily be expanded upon to create new cmdlets.
API documentation can be found [here].

<p align="center">
  <img src="https://cdn.rawgit.com/Invoke-IR/PowerForensics/master/Images/powerforensics_square_blue.svg" width="300" height="300">
</p>