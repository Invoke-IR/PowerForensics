<p align="center">
  <img src="https://2.bp.blogspot.com/-1LLYVd_quJU/VZVojHdUc-I/AAAAAAAAAy4/OOfTAKgq458/s1600/New_PowerForensics_Blue_xsmall_nontransparent.png">
</p>

<h1 align="center">PowerForensics - PowerShell Digital Forensics</h1>

<h5 align="center">Developed by <a href="https://twitter.com/jaredcatkinson">@jaredcatkinson</a></h5>

<p align="center">
  <a href="https://gitter.im/Invoke-IR/PowerForensics">
    <img src="https://badges.gitter.im/Join%20Chat.svg">
  </a>
</p>
<p align="center">
  <a href="https://waffle.io/Invoke-IR/PowerForensics">
    <img src="https://badge.waffle.io/Invoke-IR/PowerForensics.png?label=ready&title=Ready">
  </a>
  <a href="https://waffle.io/Invoke-IR/PowerForensics">
    <img src="https://badge.waffle.io/Invoke-IR/PowerForensics.png?label=in%20progress&title=In%20Progress">
  </a>
</p>

## Overview
PowerForensics is a PowerShell digital forensics framework. It currently
supports NTFS and is in the process of adding support for the ext4 file system.

## Cmdlets
### Boot Sector:
```
Get-ForensicMasterBootRecord - gets the MasterBootRecord from the first sector of the hard drive
Get-ForensicGuidPartitionTable - gets the GuidPartitionTable from the first sector of the hard drive
Get-ForensicBootSector - gets the appropriate boot sector (MBR or GPT) from the specified drive
Get-ForensicPartitionTable - gets the partition table for the specified drive
```

### New Technology File System (NTFS):
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
Get-ForensicAmcache - gets previously run commands from the Amcache.hve registry hive
Get-ForensicEventLog - gets the events in an event log or in all event logs
Get-ForensicExplorerTypedPath - gets the file paths that have been typed into the Windows Explorer application
Get-ForensicNetworkList - gets a list of networks that the system has previously been connected to 
Get-ForensicPrefetch - gets Windows Prefetch artifacts by parsing the file's binary structure
Get-ForensicRunMostRecentlyUsed - gets the commands that were issued by the user to the run dialog
Get-ForensicScheduledJob - gets Scheduled Jobs (at jobs) by parsing the file's binary structures
Get-ForensicShellLink - gets ShellLink (.lnk) artifacts by parsing the file's binary structure
Get-ForensicSid - gets the machine Security Identifier from the SAM hive
Get-ForensicTimezone - gets the system's timezone based on the registry setting
Get-ForensicTypedUrl - gets the Universal Resource Locators (URL) that have been typed in the Internet Explorer browser
Get-ForensicUserAssist - gets the UserAssist entries from the specified volume
Get-ForensicWindowsSearchHistory - gets the search terms that have been searched for using the Windows Search feature
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

### Extended File System 4 (ext4):
```
Get-ForensicSuperblock - returns the ext4 SuperBlock object
Get-ForensicBlockGroupDescriptor - returns the Block Group Descriptor Table entries
Get-ForensicInode - returns the Inode Table entries
```

### Utilities
```
ConvertFrom-BinaryData - implements PowerForensics' BinShred API to parse binary data into an object
Copy-ForensicFile - creates a copy of a file from its raw bytes on disk 
Get-ForensicChildItem - returns a directory's contents by parsing the MFT structures
Get-ForensicContent - gets the content of a file from its raw bytes on disk
Invoke-ForensicDD - provides a bit for bit copy of a specified device
```

## [Module Installation](https://msdn.microsoft.com/en-us/library/dd878350(v=vs.85).aspx)
The easiest way to install PowerForensics is through the  ```Install-Module``` cmdlet. This is available by default in Windows 10, but can also be installed via the Windows Management Framework or the standalone MSI installer:

```
PS> Install-Module PowerForensics
```

For more information about installing modules from the PowerShell Gallery, see http://www.powershellgallery.com/.

If you wish to install directly from this repository, Jakub Jareš wrote an [excellent introduction](http://www.powershellmagazine.com/2014/03/12/get-started-with-pester-powershell-unit-testing-framework/) to module installation, so we've adapted those instructions here for PowerForensics. 

To begin open an internet browser and navigate to the main PowerForensics github [page](https://github.com/Invoke-IR/PowerForensics/releases). Once on this page you will need to find the latest release, download PowerForensics.zip, and extract the module into your modules directory.

<p align="center">
  <img src="https://1.bp.blogspot.com/-9iysGot_Irw/VlI8VYZef7I/AAAAAAAAA9Y/ud-z17k6I0s/s1600/Screenshot%2B2015-11-22%2B17.04.13.png">
</p>

If you used Internet Explorer to download the archive, you need to unblock the archive before extraction, otherwise PowerShell will complain when you import the module. If you are using PowerShell 3.0 or newer you can use the Unblock-File cmdlet to do that:
```powershell
Unblock-File -Path "$env:UserProfile\Downloads\PowerForensics-master.zip"
```

If you are using an older version of PowerShell you will have to unblock the file manually. Go to your Downloads folder and right-click PowerForensics.zip and select "Properties". On the general tab click Unblock and then click OK to close the dialog.

<p align="center">
  <img src="https://3.bp.blogspot.com/-9l3ETdnI_YE/VlI-grV7etI/AAAAAAAAA9s/IQjL_Zvfw64/s400/Screenshot%2B2015-11-22%2B17.15.00.png">
</p>

Open your Modules directory and create a new folder called PowerForensics. You can use this script to open the correct folder effortlessly:
```powershell
function Get-UserModulePath {
 
    $Path = $env:PSModulePath -split ";" -match $env:USERNAME
 
    if (-not (Test-Path -Path $Path))
    {
        New-Item -Path $Path -ItemType Container | Out-Null
    }
    
    $Path
}
 
Invoke-Item (Get-UserModulePath)
```

Extract the archive to the PowerForensics folder. When you are done you should have all these files in your PowerForensics directory:

<p align="center">
  <img src="https://4.bp.blogspot.com/-kP8N4QXDO2A/VlI-mvXCY8I/AAAAAAAAA9w/V2-MFg1Tg90/s1600/Screenshot%2B2015-11-22%2B17.13.19.png">
</p>

Start a new PowerShell session and import the PowerForensics module using the commands below:
```powershell
Get-Module -ListAvailable -Name PowerForensics
Import-Module PowerForensics
Get-Command -Module PowerForensics
```

You are now ready to use the PowerForensics PowerShell module!

<p align="center">
  <img src="https://2.bp.blogspot.com/-1LLYVd_quJU/VZVojHdUc-I/AAAAAAAAAy4/OOfTAKgq458/s1600/New_PowerForensics_Blue_xsmall_nontransparent.png">
</p>
