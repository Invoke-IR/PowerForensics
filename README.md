<p align="center">
  <img src="https://2.bp.blogspot.com/-1LLYVd_quJU/VZVojHdUc-I/AAAAAAAAAy4/OOfTAKgq458/s1600/New_PowerForensics_Blue_xsmall_nontransparent.png">
</p>

#PowerForensics

PowerForensics is a PowerShell digital forensics framework. It currently
supports NTFS and is in the process of adding support for the ext4 file system.

Developed by [@jaredcatkinson](https://twitter.com/jaredcatkinson)

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
Get-ScheduledJobRaw - parses the binary structure of Scheduled Jobs (at jobs) and returns a custom ScheduledJob object
```

### Utilities:
```
Invoke-DD - provides a bit for bit copy of a specified device
Copy-FileRaw - runs all current escalation checks and returns a report
Get-ChildItemRaw - finds remaining unattended installation files
Get-ContentRaw - checks for any encrypted web.config strings
Get-Hash - returns a cryptographic has for the specified file
Get-Timezone - returns the .NET Timezone object
```

### Formatters:
```
Format-Hex - Formats byte array output into a hexdump
```

## [Module Installation](https://msdn.microsoft.com/en-us/library/dd878350(v=vs.85).aspx)
Jakub Jareš wrote an [excellent introduction](http://www.powershellmagazine.com/2014/03/12/get-started-with-pester-powershell-unit-testing-framework/) to module installation, so I decided to adapt his example for PowerForensics. 

To begin open an internet browser and navigate to the main PowerForensics github [page](https://github.com/Invoke-IR/PowerForensics). Once on this page you will need to download and extract the module into your modules directory.

<p align="center">
  <img src="http://3.bp.blogspot.com/-grhkJC70sRo/Vd_eHf1lejI/AAAAAAAAA4E/QaEnIZQREew/s640/Screenshot%2B2015-08-27%2B23.57.32.png">
</p>

If you used Internet Explorer to download the archive, you need to unblock the archive before extraction, otherwise PowerShell will complain when you import the module. If you are using PowerShell 3.0 or newer you can use the Unblock-File cmdlet to do that:
```powershell
Unblock-File -Path "$env:UserProfile\Downloads\PowerForensics-master.zip"
```

If you are using an older version of PowerShell you will have to unblock the file manually. Go to your Downloads folder and right-click PowerForensics-master.zip and select "Properties". On the general tab click Unblock and then click OK to close the dialog.

<p align="center">
  <img src="http://3.bp.blogspot.com/-A6C2p8swj50/Vd_eHSFhWVI/AAAAAAAAA4A/y7PMPb1XRCk/s640/Screenshot%2B2015-08-27%2B23.58.30.png">
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
  <img src="http://3.bp.blogspot.com/-1I65699uSJk/Vd_f3zNuFDI/AAAAAAAAA4U/NCO52iz3w84/s640/Screenshot%2B2015-08-28%2B00.13.00.png">
</p>

Start a new PowerShell session and import the PowerForensics module using the commands below:
```powershell
Get-Module -ListAvailable -Name PowerForensics
Import-Module PowerForensics
Get-Command -Module PowerForensics
```

You are now ready to use the PowerForensics PowerShell module!
