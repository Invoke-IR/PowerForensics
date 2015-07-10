![alt text](https://2.bp.blogspot.com/-1LLYVd_quJU/VZVojHdUc-I/AAAAAAAAAy4/OOfTAKgq458/s1600/New_PowerForensics_Blue_xsmall_nontransparent.png "PowerForensics")

#PowerForensics

PowerForensics is a powershell digital forensics framework. It currently
supports NTFS and is in the process of adding support for the ext4 file system.

Developed by [@jaredcatkinson](https://twitter.com/jaredcatkinson)

## Boot Sector:
    Get-MBR                         -   parses the first sector of the hard drive and returns a MasterBootRecord object
    Get-GPT                         -   parses the first sector of the hard drive and returns a GuidPartitionTable object
    Get-BootSector                  -   parses the first sector of the hard drive and returns the appropriate boot sector (MBR or GPT)
    Get-PartitionTable              -   parses the first sector of the hard drive and returns the partition table

## New Technology File System (NTFS):
    Get-FileRecord                  -   returns Master File Table entries
    Get-FileRecordIndex             -   returns a file's MFT record index number
    Get-DeletedFile                 -   returns Master File Table entries of files that are marked as deleted
    Get-AttrDef                     -   parses the $AttrDef file to return definitions of MFT Attributes 
    Get-BadCluster                  -   parses the $BadClus file to check for damaged clusters
    Get-Bitmap                      -   parses the $Bitmap file to determine if a cluster is marked as in use
    Get-UsnJrnl                     -   parses the $UsnJrnl file's $J data attribute and returns USN Journal Entries
    Get-UsnJrnlInformation          -   parses the $UsnJrnl file's $MAX data attribute and returns USN Journal Metadata
    Get-VolumeBootRecord            -   parses the $Boot file located in the first sector of the volume and returns the VolumeBootRecord object
    Get-VolumeInformation           -   parses the $Volume file's $VOLUME_INFORMATION attribute and returns a VolumeInformation Object
    Get-VolumeName                  -   parses the $Volume file's $VOLUME_NAME attribute and returns the VolumeName

## Extended File System 4 (ext4):
    Get-Superblock                  -   returns the ext4 SuperBlock object
    Get-BlockGroupDescriptor        -   returns the Block Group Descriptor Table entries
    Get-Inode                       -   returns the Inode Table entries

## Windows Artifacts
    Get-VolumeShadowCopy            -   returns Win32_ShadowCopy objects
    Get-Prefetch                    -   parses the binary structure of Windows Prefetch files and returns a custom Prefetch object
    Get-ScheduledJobRaw             -   parses the binary structure of Scheduled Jobs (at jobs) and returns a custom ScheduledJob object

## Utilities:
    Invoke-DD                       -   provides a bit for bit copy of a specified device
    Copy-FileRaw                    -   runs all current escalation checks and returns a report
    Get-ChildItemRaw                -   finds remaining unattended installation files
    Get-ContentRaw                  -   checks for any encrypted web.config strings
    Get-Hash                        -   returns a cryptographic has for the specified file
    Get-Timezone                    -   returns the .NET Timezone object

## Formatters:
    Format-Hex                      -   Formats byte array output into a hexdump
