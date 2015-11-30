<p align="center">
  <img src="https://2.bp.blogspot.com/-1LLYVd_quJU/VZVojHdUc-I/AAAAAAAAAy4/OOfTAKgq458/s1600/New_PowerForensics_Blue_xsmall_nontransparent.png">
</p>

<h1 align="center">PowerForensics - PowerShell Digital Forensics</h1>

<h5 align="center">Developed by <a href="https://twitter.com/jaredcatkinson">@jaredcatkinson</a></h5>

<p align="center">
  <a href="https://ci.appveyor.com/project/Invoke-IR/powerforensics">
    <img src="https://ci.appveyor.com/api/projects/status/276f8iautyqlx3mk?svg=true" width="150">
  </a>
</p>
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
Get-MBR - parses the first sector of the hard drive and returns a MasterBootRecord object
Get-GPT - parses the first sector of the hard drive and returns a GuidPartitionTable object
Get-BootSector - parses the first sector of the hard drive and returns the appropriate boot sector (MBR or GPT)
Get-PartitionTable - parses the first sector of the hard drive and returns the partition table
```

### New Technology File System (NTFS):
```
Get-FileRecord - returns Master File Table entries
Get-FileRecordIndex - returns a file's MFT record index number
Get-DeletedFile - returns Master File Table entries of files that are marked as deleted
Get-AttrDef - parses the $AttrDef file to return definitions of MFT Attributes 
Get-BadCluster - parses the $BadClus file to check for damaged clusters
Get-Bitmap - parses the $Bitmap file to determine if a cluster is marked as in use
Get-UsnJrnl - parses the $UsnJrnl file's $J data attribute and returns USN Journal Entries
Get-UsnJrnlInformation - parses the $UsnJrnl file's $MAX data attribute and returns USN Journal Metadata
Get-VolumeBootRecord - parses the $Boot file located in the first sector of the volume and returns the VolumeBootRecord object
Get-VolumeInformation - parses the $Volume file's $VOLUME_INFORMATION attribute and returns a VolumeInformation Object
Get-VolumeName - parses the $Volume file's $VOLUME_NAME attribute and returns the VolumeName
```

### Extended File System 4 (ext4):
```
Get-Superblock - returns the ext4 SuperBlock object
Get-BlockGroupDescriptor - returns the Block Group Descriptor Table entries
Get-Inode - returns the Inode Table entries
```

### Windows Artifacts
```
Get-VolumeShadowCopy - returns Win32_ShadowCopy objects
Get-Prefetch - parses the binary structure of Windows Prefetch files and returns a custom Prefetch object
Get-ScheduledJob - parses the binary structure of Scheduled Jobs (at jobs) and returns a custom ScheduledJob object
```

### Utilities:
```
Invoke-DD - provides a bit for bit copy of a specified device
Copy-File - creates a copy of a file from its raw bytes on disk 
Get-ChildItem - returns a directory's contents by parsing the MFT structures
Get-Content - gets the content of a file from its raw bytes on disk
Get-Hash - returns a cryptographic hash for the specified file
Get-Timezone - determines a system's timezone based on the registry setting
```

### Formatters:
```
Format-Hex - Formats byte array output into a hexdump
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
