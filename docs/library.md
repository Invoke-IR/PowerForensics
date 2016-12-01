# PowerForensics


## Artifacts.ApacheAccessLog
            

        
### Fields

#### RemoteHostname

#### RemoteLogname

#### RemoteUsername

#### Timestamp

#### HttpMethod

#### Request

#### Status

#### ResponseSize

#### Referer

#### UserAgent

### Methods


#### Get(System.String)

> ##### Parameters
> **entry:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **Path:** 

> ##### Return value
> 

## Artifacts.JavaCache
            

        
### Fields

#### LastModified

#### ExpirationDate

#### ValidationTime

#### Signed

#### Version

#### Url

#### Namespace

#### CodebaseIp

#### HttpHeaders

### Methods


#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### Get(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

## Artifacts.Prefetch
            

        
### Fields

#### PREFETCH_ENABLED.DISABLED

#### PREFETCH_ENABLED.APPLICATION

#### PREFETCH_ENABLED.BOOT

#### PREFETCH_ENABLED.APPLICATION_BOOT

#### PREFETCH_VERSION.WINDOWS_8

#### PREFETCH_VERSION.WINDOWS_7

#### PREFETCH_VERSION.WINDOWS_XP

#### Version

#### Name

#### Path

#### PathHash

#### DependencyCount

#### PrefetchAccessTime

#### DeviceCount

#### RunCount

#### DependencyFiles

### Methods


#### Get(System.String)

> ##### Parameters
> **filePath:** 

> ##### Return value
> 

#### Get(System.String,System.Boolean)

> ##### Parameters
> **filePath:** 

> **fast:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetInstances(System.String,System.Boolean)

> ##### Parameters
> **volume:** 

> **fast:** 

> ##### Return value
> 

#### CheckStatus(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### ToString

> ##### Return value
> 

## Artifacts.Prefetch.PREFETCH_ENABLED
            

        
### Fields

#### DISABLED

#### APPLICATION

#### BOOT

#### APPLICATION_BOOT


## Artifacts.Prefetch.PREFETCH_VERSION
            

        
### Fields

#### WINDOWS_8

#### WINDOWS_7

#### WINDOWS_XP


## Artifacts.Amcache
            
https://msdn.microsoft.com/en-us/library/cc248285.aspx
        
### Fields

#### SequenceNumber

#### RecordNumber

#### ProductName

#### CompanyName

#### FileSize

#### Description

#### CompileTime

#### ModifiedTimeUtc

#### BornTimeUtc

#### Path

#### ModifiedTime2Utc

#### Hash

### Methods


#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetInstancesByPath(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

## Artifacts.RecentFileCache
            

        
### Methods


#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetInstancesByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

## Artifacts.Shimcache
            

        
### Fields

#### Path

#### LastModifiedTime

#### FileSize

#### LastUpdateTime

### Methods


#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetInstancesByPath(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

## Artifacts.LastVisitedMRU
            

        
### Fields

#### User

#### ImagePath

### Methods


#### Get(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

## Artifacts.RecentDocs
            

        
### Fields

#### User

#### Path

#### LastWriteTime

### Methods


#### Get(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Artifacts.RunMRU
            

        
### Fields

#### User

#### ImagePath

### Methods


#### Get(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Artifacts.TypedPaths
            

        
### Fields

#### User

#### ImagePath

### Methods


#### Get(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Artifacts.TypedUrls
            

        
### Fields

#### User

#### UrlString

### Methods


#### Get(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Artifacts.UserAssist
            

        
### Fields

#### User

#### ImagePath

#### RunCount

#### FocusTime

#### LastExecutionTimeUtc

### Methods


#### Constructor

> ##### Parameters
> **user:** 

> **vk:** 

> **bytes:** 


#### Get(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### ToString

> ##### Return value
> 

## Artifacts.WordWheelQuery
            

        
### Fields

#### User

#### SearchString

### Methods


#### Get(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Artifacts.MicrosoftOffice.FileMRU
            

        
### Fields

#### User

#### Path

#### LastAccessedTime

### Methods


#### Get(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Artifacts.MicrosoftOffice.OutlookCatalog
            

        
### Fields

#### User

#### Path

### Methods


#### Get(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Artifacts.MicrosoftOffice.PlaceMRU
            

        
### Fields

#### User

#### Path

#### LastAccessedTime

### Methods


#### Get(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Artifacts.MicrosoftOffice.TrustRecord
            

        
### Fields

#### User

#### Path

#### TrustTime

### Methods


#### Get(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Artifacts.AlternateDataStream
            

        
### Fields

#### FullName

#### Name

#### StreamName

### Methods


#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetInstancesByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

## Artifacts.Persistence.RunKey
            

        
### Fields

#### AutoRunLocation

#### Name

#### ImagePath

### Methods


#### Constructor

> ##### Parameters
> **location:** 

> **vk:** 


#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### Get(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

## Artifacts.ScheduledJob
            
https://msdn.microsoft.com/en-us/library/cc248285.aspx
        
### Fields

#### PRODUCT_VERSION.WindowsNT4

#### PRODUCT_VERSION.Windows2000

#### PRODUCT_VERSION.WindowsXP

#### PRODUCT_VERSION.WindowsVista

#### PRODUCT_VERSION.Windows7

#### PRODUCT_VERSION.Windows8

#### PRODUCT_VERSION.Windows8_1

#### PRIORITY_CLASS.NORMAL

#### PRIORITY_CLASS.IDLE

#### PRIORITY_CLASS.HIGH

#### PRIORITY_CLASS.REALTIME

#### STATUS.SCHED_S_TASK_READY

#### STATUS.SCHED_S_TASK_RUNNING

#### STATUS.SCHED_S_TASK_NOT_SCHEDULED

#### TASK_FLAG.INTERACTIVE

#### TASK_FLAG.DELETE_WHEN_DONE

#### TASK_FLAG.DISABLED

#### TASK_FLAG.START_ONLY_IF_IDLE

#### TASK_FLAG.KILL_ON_IDLE_END

#### TASK_FLAG.DONT_START_IF_ON_BATTERIES

#### TASK_FLAG.KILL_IF_GOING_ON_BATTERIES

#### TASK_FLAG.RUN_ONLY_IF_DOCKED

#### TASK_FLAG.HIDDEN

#### TASK_FLAG.RUN_IF_CONNECTED_TO_INTERNET

#### TASK_FLAG.RESTART_ON_IDLE_RESUME

#### TASK_FLAG.SYSTEM_REQUIRED

#### TASK_FLAG.RUN_ONLY_IF_LOGGED_ON

#### TASK_FLAG.APPLICATION_NAME

#### ProductVersion

#### FileVersion

#### Uuid

#### ErrorRetryCount

#### ErrorRetryInterval

#### IdleDeadline

#### IdleWait

#### MaximumRuntime

#### ExitCode

#### Status

#### Flags

#### RunTime

#### RunningInstanceCount

#### ApplicationName

#### Parameters

#### WorkingDirectory

#### Author

#### Comment

#### StartTime

### Methods


#### Get(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### ToString

> ##### Return value
> 

## Artifacts.ScheduledJob.PRODUCT_VERSION
            

        
### Fields

#### WindowsNT4

#### Windows2000

#### WindowsXP

#### WindowsVista

#### Windows7

#### Windows8

#### Windows8_1


## Artifacts.ScheduledJob.PRIORITY_CLASS
            

        
### Fields

#### NORMAL

#### IDLE

#### HIGH

#### REALTIME


## Artifacts.ScheduledJob.STATUS
            

        
### Fields

#### SCHED_S_TASK_READY

#### SCHED_S_TASK_RUNNING

#### SCHED_S_TASK_NOT_SCHEDULED


## Artifacts.ScheduledJob.TASK_FLAG
            

        
### Fields

#### INTERACTIVE

#### DELETE_WHEN_DONE

#### DISABLED

#### START_ONLY_IF_IDLE

#### KILL_ON_IDLE_END

#### DONT_START_IF_ON_BATTERIES

#### KILL_IF_GOING_ON_BATTERIES

#### RUN_ONLY_IF_DOCKED

#### HIDDEN

#### RUN_IF_CONNECTED_TO_INTERNET

#### RESTART_ON_IDLE_RESUME

#### SYSTEM_REQUIRED

#### RUN_ONLY_IF_LOGGED_ON

#### APPLICATION_NAME


## Artifacts.ScheduledTask
            

        
### Fields

#### Path

#### Name

#### Author

#### Description

### Methods


#### Get(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Artifacts.Sid
            

        
### Methods


#### Get(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetByPath(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

## Artifacts.UserDetail
            

        
### Fields

#### LastLogon

#### PasswordLastSet

#### AccountExpires

#### LastIncorrectPassword

#### RelativeIdentifier

#### AccountActive

#### PasswordRequired

#### CountryCode

#### InvalidPasswordCount

#### LogonCount

### Methods


#### Get

> ##### Return value
> 

## Artifacts.ShellLink
            

        
### Fields

#### LINK_FLAGS.HasLinkTargetIdList

#### LINK_FLAGS.HasLinkInfo

#### LINK_FLAGS.HasName

#### LINK_FLAGS.HasRelativePath

#### LINK_FLAGS.HasWorkingDir

#### LINK_FLAGS.HasArguments

#### LINK_FLAGS.HasIconLocation

#### LINK_FLAGS.IsUnicode

#### LINK_FLAGS.ForceNoLinkInfo

#### LINK_FLAGS.HasExpString

#### LINK_FLAGS.RunInSeparateProcess

#### LINK_FLAGS.Unused1

#### LINK_FLAGS.HasDarwinId

#### LINK_FLAGS.RunAsUser

#### LINK_FLAGS.HasExpIcon

#### LINK_FLAGS.NoPidlAlias

#### LINK_FLAGS.Unused2

#### LINK_FLAGS.RunWithShimLayer

#### LINK_FLAGS.ForceNoLinkTrack

#### LINK_FLAGS.EnableTargetMetadata

#### LINK_FLAGS.DisableLinkPathTracking

#### LINK_FLAGS.DisableKnownFolderTracking

#### LINK_FLAGS.DisableKnownFolderAlias

#### LINK_FLAGS.AllowLinkToLink

#### LINK_FLAGS.UnaliasOnSave

#### LINK_FLAGS.PreferEnvironmentPath

#### LINK_FLAGS.KeepLocalIdListForUncTarget

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_READONLY

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_HIDDEN

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_SYSTEM

#### FILEATTRIBUTE_FLAGS.Reserved1

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_DIRECTORY

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_ARCHIVE

#### FILEATTRIBUTE_FLAGS.Reserved2

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_NORMAL

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_TEMPORARY

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_SPARSE_FILE

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_REPARSE_POINT

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_COMPRESSED

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_OFFLINE

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_NOT_CONTENT_INDEXED

#### FILEATTRIBUTE_FLAGS.FILE_ATTRIBUTE_ENCRYPTED

#### SHOWCOMMAND.SW_SHOWNORMAL

#### SHOWCOMMAND.SW_SHOWMAXIMIZED

#### SHOWCOMMAND.SW_SHOWMINNOACTIVE

#### HOTKEY_FLAGS.HOTKEYF_SHIFT

#### HOTKEY_FLAGS.HOTKEYF_CONTROL

#### HOTKEY_FLAGS.HOTKEYF_ALT

#### HOTKEY_FLAGS.K_0

#### HOTKEY_FLAGS.K_1

#### HOTKEY_FLAGS.K_2

#### HOTKEY_FLAGS.K_3

#### HOTKEY_FLAGS.K_4

#### HOTKEY_FLAGS.K_5

#### HOTKEY_FLAGS.K_6

#### HOTKEY_FLAGS.K_7

#### HOTKEY_FLAGS.K_8

#### HOTKEY_FLAGS.K_9

#### HOTKEY_FLAGS.K_A

#### HOTKEY_FLAGS.K_B

#### HOTKEY_FLAGS.K_C

#### HOTKEY_FLAGS.K_D

#### HOTKEY_FLAGS.K_E

#### HOTKEY_FLAGS.K_F

#### HOTKEY_FLAGS.K_G

#### HOTKEY_FLAGS.K_H

#### HOTKEY_FLAGS.K_I

#### HOTKEY_FLAGS.K_J

#### HOTKEY_FLAGS.K_K

#### HOTKEY_FLAGS.K_L

#### HOTKEY_FLAGS.K_M

#### HOTKEY_FLAGS.K_N

#### HOTKEY_FLAGS.K_O

#### HOTKEY_FLAGS.K_P

#### HOTKEY_FLAGS.K_Q

#### HOTKEY_FLAGS.K_R

#### HOTKEY_FLAGS.K_S

#### HOTKEY_FLAGS.K_T

#### HOTKEY_FLAGS.K_U

#### HOTKEY_FLAGS.K_V

#### HOTKEY_FLAGS.K_W

#### HOTKEY_FLAGS.K_X

#### HOTKEY_FLAGS.K_Y

#### HOTKEY_FLAGS.K_Z

#### HOTKEY_FLAGS.VK_F1

#### HOTKEY_FLAGS.VK_F2

#### HOTKEY_FLAGS.VK_F3

#### HOTKEY_FLAGS.VK_F4

#### HOTKEY_FLAGS.VK_F5

#### HOTKEY_FLAGS.VK_F6

#### HOTKEY_FLAGS.VK_F7

#### HOTKEY_FLAGS.VK_F8

#### HOTKEY_FLAGS.VK_F9

#### HOTKEY_FLAGS.VK_F10

#### HOTKEY_FLAGS.VK_F11

#### HOTKEY_FLAGS.VK_F12

#### HOTKEY_FLAGS.VK_F13

#### HOTKEY_FLAGS.VK_F14

#### HOTKEY_FLAGS.VK_F15

#### HOTKEY_FLAGS.VK_F16

#### HOTKEY_FLAGS.VK_F17

#### HOTKEY_FLAGS.VK_F18

#### HOTKEY_FLAGS.VK_F19

#### HOTKEY_FLAGS.VK_F20

#### HOTKEY_FLAGS.VK_F21

#### HOTKEY_FLAGS.VK_F22

#### HOTKEY_FLAGS.VK_F23

#### HOTKEY_FLAGS.VK_F24

#### HOTKEY_FLAGS.VK_NUMLOCK

#### HOTKEY_FLAGS.VK_SCROLL

#### LINKINFO_FLAGS.VolumeIDAndLocalBasePath

#### LINKINFO_FLAGS.CommonNetworkRelativeLinkAndPathSuffix

#### Path

#### LinkFlags

#### FileAttributes

#### CreationTime

#### AccessTime

#### WriteTime

#### FileSize

#### IconIndex

#### ShowCommand

#### HotKey

#### IdList

#### VolumeId

#### LocalBasePath

#### CommonNetworkRelativeLink

#### CommonPathSuffix

#### LocalBasePathUnicode

#### CommonPathSuffixUnicode

#### Name

#### RelativePath

#### WorkingDirectory

#### CommandLineArguments

#### IconLocation

#### ExtraData

### Methods


#### Get(System.String)

> ##### Parameters
> **filePath:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### ToString

> ##### Return value
> 

## Artifacts.ShellLink.LINK_FLAGS
            

        
### Fields

#### HasLinkTargetIdList

#### HasLinkInfo

#### HasName

#### HasRelativePath

#### HasWorkingDir

#### HasArguments

#### HasIconLocation

#### IsUnicode

#### ForceNoLinkInfo

#### HasExpString

#### RunInSeparateProcess

#### Unused1

#### HasDarwinId

#### RunAsUser

#### HasExpIcon

#### NoPidlAlias

#### Unused2

#### RunWithShimLayer

#### ForceNoLinkTrack

#### EnableTargetMetadata

#### DisableLinkPathTracking

#### DisableKnownFolderTracking

#### DisableKnownFolderAlias

#### AllowLinkToLink

#### UnaliasOnSave

#### PreferEnvironmentPath

#### KeepLocalIdListForUncTarget


## Artifacts.ShellLink.FILEATTRIBUTE_FLAGS
            

        
### Fields

#### FILE_ATTRIBUTE_READONLY

#### FILE_ATTRIBUTE_HIDDEN

#### FILE_ATTRIBUTE_SYSTEM

#### Reserved1

#### FILE_ATTRIBUTE_DIRECTORY

#### FILE_ATTRIBUTE_ARCHIVE

#### Reserved2

#### FILE_ATTRIBUTE_NORMAL

#### FILE_ATTRIBUTE_TEMPORARY

#### FILE_ATTRIBUTE_SPARSE_FILE

#### FILE_ATTRIBUTE_REPARSE_POINT

#### FILE_ATTRIBUTE_COMPRESSED

#### FILE_ATTRIBUTE_OFFLINE

#### FILE_ATTRIBUTE_NOT_CONTENT_INDEXED

#### FILE_ATTRIBUTE_ENCRYPTED


## Artifacts.ShellLink.SHOWCOMMAND
            

        
### Fields

#### SW_SHOWNORMAL

#### SW_SHOWMAXIMIZED

#### SW_SHOWMINNOACTIVE


## Artifacts.ShellLink.HOTKEY_FLAGS
            

        
### Fields

#### HOTKEYF_SHIFT

#### HOTKEYF_CONTROL

#### HOTKEYF_ALT

#### K_0

#### K_1

#### K_2

#### K_3

#### K_4

#### K_5

#### K_6

#### K_7

#### K_8

#### K_9

#### K_A

#### K_B

#### K_C

#### K_D

#### K_E

#### K_F

#### K_G

#### K_H

#### K_I

#### K_J

#### K_K

#### K_L

#### K_M

#### K_N

#### K_O

#### K_P

#### K_Q

#### K_R

#### K_S

#### K_T

#### K_U

#### K_V

#### K_W

#### K_X

#### K_Y

#### K_Z

#### VK_F1

#### VK_F2

#### VK_F3

#### VK_F4

#### VK_F5

#### VK_F6

#### VK_F7

#### VK_F8

#### VK_F9

#### VK_F10

#### VK_F11

#### VK_F12

#### VK_F13

#### VK_F14

#### VK_F15

#### VK_F16

#### VK_F17

#### VK_F18

#### VK_F19

#### VK_F20

#### VK_F21

#### VK_F22

#### VK_F23

#### VK_F24

#### VK_NUMLOCK

#### VK_SCROLL


## Artifacts.ShellLink.LINKINFO_FLAGS
            

        
### Fields

#### VolumeIDAndLocalBasePath

#### CommonNetworkRelativeLinkAndPathSuffix


## Artifacts.IdList
            

        
### Fields

#### ItemIdList


## Artifacts.ItemId
            

        
### Fields

#### Data


## Artifacts.VolumeId
            

        
### Fields

#### DRIVE_TYPE.DRIVE_UNKNOWN

#### DRIVE_TYPE.DRIVE_NO_ROOT_DIR

#### DRIVE_TYPE.DRIVE_REMOVABLE

#### DRIVE_TYPE.DRIVE_FIXED

#### DRIVE_TYPE.DRIVE_REMOTE

#### DRIVE_TYPE.DRIVE_CDROM

#### DRIVE_TYPE.DRIVE_RAMDISK

#### DriveType

#### DriveSerialNumber

#### Data


## Artifacts.VolumeId.DRIVE_TYPE
            

        
### Fields

#### DRIVE_UNKNOWN

#### DRIVE_NO_ROOT_DIR

#### DRIVE_REMOVABLE

#### DRIVE_FIXED

#### DRIVE_REMOTE

#### DRIVE_CDROM

#### DRIVE_RAMDISK


## Artifacts.CommonNetworkRelativeLink
            

        
### Fields

#### COMMON_NETWORK_RELATIVE_LINK_FLAGS.ValidDevice

#### COMMON_NETWORK_RELATIVE_LINK_FLAGS.ValidNetType

#### NETWORK_PROVIDER_TYPE.WNNC_NET_AVID

#### NETWORK_PROVIDER_TYPE.WNNC_NET_DOCUSPACE

#### NETWORK_PROVIDER_TYPE.WNNC_NET_MANGOSOFT

#### NETWORK_PROVIDER_TYPE.WNNC_NET_SERNET

#### NETWORK_PROVIDER_TYPE.WNNC_NET_RIVERFRONT1

#### NETWORK_PROVIDER_TYPE.WNNC_NET_RIVERFRONT2

#### NETWORK_PROVIDER_TYPE.WNNC_NET_DECORB

#### NETWORK_PROVIDER_TYPE.WNNC_NET_PROTSTOR

#### NETWORK_PROVIDER_TYPE.WNNC_NET_FJ_REDIR

#### NETWORK_PROVIDER_TYPE.WNNC_NET_DISTINCT

#### NETWORK_PROVIDER_TYPE.WNNC_NET_TWINS

#### NETWORK_PROVIDER_TYPE.WNNC_NET_RDR2SAMPLE

#### NETWORK_PROVIDER_TYPE.WNNC_NET_CSC

#### NETWORK_PROVIDER_TYPE.WNNC_NET_3IN1

#### NETWORK_PROVIDER_TYPE.WNNC_NET_EXTENDNET

#### NETWORK_PROVIDER_TYPE.WNNC_NET_STAC

#### NETWORK_PROVIDER_TYPE.WNNC_NET_FOXBAT

#### NETWORK_PROVIDER_TYPE.WNNC_NET_YAHOO

#### NETWORK_PROVIDER_TYPE.WNNC_NET_EXIFS

#### NETWORK_PROVIDER_TYPE.WNNC_NET_DAV

#### NETWORK_PROVIDER_TYPE.WNNC_NET_KNOWARE

#### NETWORK_PROVIDER_TYPE.WNNC_NET_OBJECT_DIRE

#### NETWORK_PROVIDER_TYPE.WNNC_NET_MASFAX

#### NETWORK_PROVIDER_TYPE.WNNC_NET_HOB_NFS

#### NETWORK_PROVIDER_TYPE.WNNC_NET_SHIVA

#### NETWORK_PROVIDER_TYPE.WNNC_NET_IBMAL

#### NETWORK_PROVIDER_TYPE.WNNC_NET_LOCK

#### NETWORK_PROVIDER_TYPE.WNNC_NET_TERMSRV

#### NETWORK_PROVIDER_TYPE.WNNC_NET_SRT

#### NETWORK_PROVIDER_TYPE.WNNC_NET_QUINCY

#### NETWORK_PROVIDER_TYPE.WNNC_NET_OPENAFS

#### NETWORK_PROVIDER_TYPE.WNNC_NET_AVID1

#### NETWORK_PROVIDER_TYPE.WNNC_NET_DFS

#### NETWORK_PROVIDER_TYPE.WNNC_NET_KWNP

#### NETWORK_PROVIDER_TYPE.WNNC_NET_ZENWORKS

#### NETWORK_PROVIDER_TYPE.WNNC_NET_DRIVEONWEB

#### NETWORK_PROVIDER_TYPE.WNNC_NET_VMWARE

#### NETWORK_PROVIDER_TYPE.WNNC_NET_RSFX

#### NETWORK_PROVIDER_TYPE.WNNC_NET_MFILES

#### NETWORK_PROVIDER_TYPE.WNNC_NET_MS_NFS

#### NETWORK_PROVIDER_TYPE.WNNC_NET_GOOGLE

#### NetworkProviderType

#### NetName

#### DeviceName

#### NetNameUnicode

#### DeviceNameUnicode


## Artifacts.CommonNetworkRelativeLink.COMMON_NETWORK_RELATIVE_LINK_FLAGS
            

        
### Fields

#### ValidDevice

#### ValidNetType


## Artifacts.CommonNetworkRelativeLink.NETWORK_PROVIDER_TYPE
            

        
### Fields

#### WNNC_NET_AVID

#### WNNC_NET_DOCUSPACE

#### WNNC_NET_MANGOSOFT

#### WNNC_NET_SERNET

#### WNNC_NET_RIVERFRONT1

#### WNNC_NET_RIVERFRONT2

#### WNNC_NET_DECORB

#### WNNC_NET_PROTSTOR

#### WNNC_NET_FJ_REDIR

#### WNNC_NET_DISTINCT

#### WNNC_NET_TWINS

#### WNNC_NET_RDR2SAMPLE

#### WNNC_NET_CSC

#### WNNC_NET_3IN1

#### WNNC_NET_EXTENDNET

#### WNNC_NET_STAC

#### WNNC_NET_FOXBAT

#### WNNC_NET_YAHOO

#### WNNC_NET_EXIFS

#### WNNC_NET_DAV

#### WNNC_NET_KNOWARE

#### WNNC_NET_OBJECT_DIRE

#### WNNC_NET_MASFAX

#### WNNC_NET_HOB_NFS

#### WNNC_NET_SHIVA

#### WNNC_NET_IBMAL

#### WNNC_NET_LOCK

#### WNNC_NET_TERMSRV

#### WNNC_NET_SRT

#### WNNC_NET_QUINCY

#### WNNC_NET_OPENAFS

#### WNNC_NET_AVID1

#### WNNC_NET_DFS

#### WNNC_NET_KWNP

#### WNNC_NET_ZENWORKS

#### WNNC_NET_DRIVEONWEB

#### WNNC_NET_VMWARE

#### WNNC_NET_RSFX

#### WNNC_NET_MFILES

#### WNNC_NET_MS_NFS

#### WNNC_NET_GOOGLE


## Artifacts.ConsoleDataBlock
            

        
### Fields

#### FILL.FOREGROUND_BLUE

#### FILL.FOREGROUND_GREEN

#### FILL.FOREGROUND_RED

#### FILL.FOREGROUND_INTENSITY

#### FILL.BACKGROUND_BLUE

#### FILL.BACKGROUND_GREEN

#### FILL.BACKGROUND_RED

#### FILL.BACKGROUND_INTENSITY

#### FONT.FF_DONTCARE

#### FONT.FF_ROMAN

#### FONT.FF_SWISS

#### FONT.FF_MODERN

#### FONT.FF_SCRIPT

#### FONT.FF_DECORATIVE

#### FillAttribute

#### PopupFillAttribute

#### ScreenBufferSizeX

#### ScreenBufferSizeY

#### WindowSizeX

#### WindowSizeY

#### WindowOriginX

#### WindowOriginY

#### FontSize

#### FontFamily

#### FontWeight

#### FaceName

#### CursorSize

#### FullScreen

#### QuickEdit

#### InsertMode

#### AutoPosition

#### HistoryBufferSize

#### NumberOfHistroyBuffers

#### HistoryNoDup

#### ColorTable


## Artifacts.ConsoleDataBlock.FILL
            

        
### Fields

#### FOREGROUND_BLUE

#### FOREGROUND_GREEN

#### FOREGROUND_RED

#### FOREGROUND_INTENSITY

#### BACKGROUND_BLUE

#### BACKGROUND_GREEN

#### BACKGROUND_RED

#### BACKGROUND_INTENSITY


## Artifacts.ConsoleDataBlock.FONT
            

        
### Fields

#### FF_DONTCARE

#### FF_ROMAN

#### FF_SWISS

#### FF_MODERN

#### FF_SCRIPT

#### FF_DECORATIVE


## Artifacts.ConsoleFeDataBlock
            

        
### Fields

#### CodePage


## Artifacts.DarwinDataBlock
            

        
### Fields

#### DarwinDataAnsi

#### DarwinDataUnicode


## Artifacts.EnvironmentVariableDataBlock
            

        
### Fields

#### TargetAnsi

#### TargetUnicode


## Artifacts.ExtraData
            

        
### Fields

#### EXTRA_DATA_TYPE.EnvironmentVariableDataBlock

#### EXTRA_DATA_TYPE.ConsoleDataBlock

#### EXTRA_DATA_TYPE.TrackerDataBlock

#### EXTRA_DATA_TYPE.ConsoleFeDataBlock

#### EXTRA_DATA_TYPE.SpecialFolderDataBlock

#### EXTRA_DATA_TYPE.DarwinDataBlock

#### EXTRA_DATA_TYPE.IconEnvironmentDataBlock

#### EXTRA_DATA_TYPE.ShimDataBlock

#### EXTRA_DATA_TYPE.PropertyStoreDataBlock

#### EXTRA_DATA_TYPE.KnownFolderDataBlock

#### EXTRA_DATA_TYPE.VistaAndAboveIDListDataBlock

#### Name


## Artifacts.ExtraData.EXTRA_DATA_TYPE
            

        
### Fields

#### EnvironmentVariableDataBlock

#### ConsoleDataBlock

#### TrackerDataBlock

#### ConsoleFeDataBlock

#### SpecialFolderDataBlock

#### DarwinDataBlock

#### IconEnvironmentDataBlock

#### ShimDataBlock

#### PropertyStoreDataBlock

#### KnownFolderDataBlock

#### VistaAndAboveIDListDataBlock


## Artifacts.IconEnvironmentDataBlock
            

        
### Fields

#### TargetAnsi

#### TargetUnicode


## Artifacts.KnownFolderDataBlock
            

        
### Fields

#### KnownFolderId

#### Offset


## Artifacts.PropertyStoreDataBlock
            

        
### Fields

#### PropertyStore


## Artifacts.ShimDataBlock
            

        
### Fields

#### LayerName


## Artifacts.SpecialFolderDataBlock
            

        
### Fields

#### SpecialFolderId

#### Offset


## Artifacts.TrackerDataBlock
            

        
### Fields

#### Version

#### MachineId

#### Droid

#### DroidBirth


## Artifacts.VistaAndAboveIDListDataBlock
            

        
### Fields

#### IdList


## Artifacts.NetworkList
            

        
### Fields

#### WriteTimeUtc

#### ProfileGuid

#### Description

#### Source

#### DnsSuffix

#### FirstNetwork

#### DefaultGatewayMac

### Methods


#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetInstancesByPath(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

## Artifacts.WindowsVersion
            

        
### Fields

#### ProductName

#### CurrentMajorVersion

#### CurrentMinorVersion

#### CurrentVersion

#### InstallTime

#### RegisteredOwner

#### SystemRoot

### Methods


#### Get(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetByPath(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

## Artifacts.Timezone
            

        
### Fields

#### RegistryTimezone

### Methods


#### Get(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetByPath(System.String)

> ##### Parameters
> **hivePath:** 

> ##### Return value
> 

## GuidPartitionTable
            

        
### Fields

#### Revision

#### HeaderSize

#### MyLBA

#### AlternateLBA

#### FirstUsableLBA

#### LastUsableLBA

#### DiskGuid

#### PartitionEntryLBA

#### NumberOfPartitionEntries

#### SizeOfPartitionEntry

#### PartitionTable

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

### Methods


#### Get(System.String)

> ##### Parameters
> **devicePath:** 

> ##### Return value
> 

#### GetBytes(System.String)

> ##### Parameters
> **devicePath:** 

> ##### Return value
> 

#### GetPartitionTable

> ##### Return value
> 

## GuidPartitionTableEntry
            

        
### Fields

#### PARTITION_ATTRIBUTE.RequirePartition

#### PARTITION_ATTRIBUTE.NoBlockIOProtocol

#### PARTITION_ATTRIBUTE.LegacyBIOSBootable

#### PartitionType

#### UniquePartitionGuid

#### StartingLBA

#### EndingLBA

#### Attributes

#### PartitionName


## GuidPartitionTableEntry.PARTITION_ATTRIBUTE
            

        
### Fields

#### RequirePartition

#### NoBlockIOProtocol

#### LegacyBIOSBootable


## MasterBootRecord
            

        
### Fields

#### DiskSignature

#### CodeSection

#### MbrSignature

#### PartitionTable

### Methods


#### GetBytes(System.String)

> ##### Parameters
> **drivePath:** 

> ##### Return value
> 

#### Get(System.String)

> ##### Parameters
> **drivePath:** 

> ##### Return value
> 

#### GetPartitions(System.Byte[],System.UInt32,System.String)

> ##### Parameters
> **bytes:** 

> **startSector:** 

> **drivePath:** 

> ##### Return value
> 

#### GetExtended(PowerForensics.PartitionEntry,System.String)

> ##### Parameters
> **entry:** 

> **drivePath:** 

> ##### Return value
> 

#### GetPartitionTable

> ##### Return value
> 

## PartitionEntry
            

        
### Fields

#### Bootable

#### SystemId

#### StartSector

#### EndSector


## Ext.BlockGroupDescriptor
            

        
### Fields

#### FLAGS.EXT4_BG_INODE_UNINIT

#### FLAGS.EXT4_BG_BLOCK_UNINIT

#### FLAGS.EXT4_BG_INODE_ZEROED

#### BlockBitmap

#### InodeBitmap

#### InodeTable

#### FreeBlockCount

#### FreeInodeCount

#### DirectoryCount

#### SnapshotExclusionBitmap

#### BlockBitmapChecksum

#### InodeBitmapChecksum

#### UnusedInodeCount

#### Flags

#### Checksum

### Methods


#### Get(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

#### Get(System.String,System.UInt32)

> ##### Parameters
> **volumeName:** 

> **blockGroup:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volumeName:** 

> ##### Return value
> 

## Ext.BlockGroupDescriptor.FLAGS
            

        
### Fields

#### EXT4_BG_INODE_UNINIT

#### EXT4_BG_BLOCK_UNINIT

#### EXT4_BG_INODE_ZEROED


## Ext.Inode
            

        
### Fields

#### FILE_MODE.S_IXOTH

#### FILE_MODE.S_IWOTH

#### FILE_MODE.S_IROTH

#### FILE_MODE.S_IXGRP

#### FILE_MODE.S_IWGRP

#### FILE_MODE.S_IRGRP

#### FILE_MODE.S_IXUSR

#### FILE_MODE.S_IWUSR

#### FILE_MODE.S_IRUSR

#### FILE_MODE.S_ISVTX

#### FILE_MODE.S_ISGID

#### FILE_MODE.S_ISUID

#### FILE_MODE.S_IFIFO

#### FILE_MODE.S_IFCHR

#### FILE_MODE.S_IFDIR

#### FILE_MODE.S_IFBLK

#### FILE_MODE.S_IFREG

#### FILE_MODE.S_IFLNK

#### FILE_MODE.S_IFSOCK

#### FLAGS.EXT4_SECRM_FL

#### FLAGS.EXT4_UNRM_FL

#### FLAGS.EXT4_COMPR_FL

#### FLAGS.EXT4_SYNC_FL

#### FLAGS.EXT4_IMMUTABLE_FL

#### FLAGS.EXT4_APPEND_FL

#### FLAGS.EXT4_NODUMP_FL

#### FLAGS.EXT4_NOATIME_FL

#### FLAGS.EXT4_DIRTY_FL

#### FLAGS.EXT4_COMPRBLK_FL

#### FLAGS.EXT4_NOCOMPR_FL

#### FLAGS.EXT4_ENCRYPT_FL

#### FLAGS.EXT4_INDEX_FL

#### FLAGS.EXT4_IMAGIC_FL

#### FLAGS.EXT4_JOURNAL_DATA_FL

#### FLAGS.EXT4_NOTAIL_FL

#### FLAGS.EXT4_DIRSYNC_FL

#### FLAGS.EXT4_TOPDIR_FL

#### FLAGS.EXT4_HUGE_FILE_FL

#### FLAGS.EXT4_EXTENTS_FL

#### FLAGS.EXT4_EA_INODE_FL

#### FLAGS.EXT4_EOFBLOCKS_FL

#### FLAGS.EXT4_SNAPFILE_FL

#### FLAGS.EXT4_SNAPFILE_DELETED_FL

#### FLAGS.EXT4_SNAPFILE_SHRUNK_FL

#### FLAGS.EXT4_INLINE_DATA_FL

#### FLAGS.EXT4_PROJINHERIT_FL

#### FLAGS.EXT4_RESERVED_FL

#### FileMode

#### UserId

#### i_size_lo

#### AccessTime

#### ChangeTime

#### ModifiedTime

#### DeletionTime

#### GroupId

#### i_links_count

#### i_blocks_lo

#### Flags

#### osd1

#### i_block

#### FileVersion

#### i_file_acl_lo

#### i_size_high

#### i_obso_faddr

#### osd2

#### i_extra_isize

#### i_checksum_hi

#### ChangeTimeExtra

#### ModifiedTimeExtra

#### AccessTimeExtra

#### CreationTime

#### CreationTimeExtra

#### i_version_hi

#### ProjectId

### Methods


#### Get(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

## Ext.Inode.FILE_MODE
            

        
### Fields

#### S_IXOTH

#### S_IWOTH

#### S_IROTH

#### S_IXGRP

#### S_IWGRP

#### S_IRGRP

#### S_IXUSR

#### S_IWUSR

#### S_IRUSR

#### S_ISVTX

#### S_ISGID

#### S_ISUID

#### S_IFIFO

#### S_IFCHR

#### S_IFDIR

#### S_IFBLK

#### S_IFREG

#### S_IFLNK

#### S_IFSOCK


## Ext.Inode.FLAGS
            

        
### Fields

#### EXT4_SECRM_FL

#### EXT4_UNRM_FL

#### EXT4_COMPR_FL

#### EXT4_SYNC_FL

#### EXT4_IMMUTABLE_FL

#### EXT4_APPEND_FL

#### EXT4_NODUMP_FL

#### EXT4_NOATIME_FL

#### EXT4_DIRTY_FL

#### EXT4_COMPRBLK_FL

#### EXT4_NOCOMPR_FL

#### EXT4_ENCRYPT_FL

#### EXT4_INDEX_FL

#### EXT4_IMAGIC_FL

#### EXT4_JOURNAL_DATA_FL

#### EXT4_NOTAIL_FL

#### EXT4_DIRSYNC_FL

#### EXT4_TOPDIR_FL

#### EXT4_HUGE_FILE_FL

#### EXT4_EXTENTS_FL

#### EXT4_EA_INODE_FL

#### EXT4_EOFBLOCKS_FL

#### EXT4_SNAPFILE_FL

#### EXT4_SNAPFILE_DELETED_FL

#### EXT4_SNAPFILE_SHRUNK_FL

#### EXT4_INLINE_DATA_FL

#### EXT4_PROJINHERIT_FL

#### EXT4_RESERVED_FL


## Ext.Superblock
            

        
### Fields

#### STATE.CleanlyUmounted

#### STATE.ErrorsDetected

#### STATE.RecoveringOrphans

#### ERRORS.Continue

#### ERRORS.RemountReadOnly

#### ERRORS.Panic

#### CREATOR_OS.Linux

#### CREATOR_OS.Hurd

#### CREATOR_OS.Masix

#### CREATOR_OS.FreeBSD

#### CREATOR_OS.Lites

#### REVISION_LEVEL.Originalformat

#### REVISION_LEVEL.v2formatwithdynamicinodesizes

#### FEATURE_COMPAT.COMPAT_DIR_PREALLOC

#### FEATURE_COMPAT.COMPAT_IMAGIC_INODES

#### FEATURE_COMPAT.COMPAT_HAS_JOURNAL

#### FEATURE_COMPAT.COMPAT_EXT_ATTR

#### FEATURE_COMPAT.COMPAT_RESIZE_INODE

#### FEATURE_COMPAT.COMPAT_DIR_INDEX

#### FEATURE_COMPAT.COMPAT_LAZY_BG

#### FEATURE_COMPAT.COMPAT_EXCLUDE_INODE

#### FEATURE_COMPAT.COMPAT_EXCLUDE_BITMAP

#### FEATURE_COMPAT.COMPAT_SPARSE_SUPER2

#### FEATURE_INCOMPAT.INCOMPAT_COMPRESSION

#### FEATURE_INCOMPAT.INCOMPAT_FILETYPE

#### FEATURE_INCOMPAT.INCOMPAT_RECOVER

#### FEATURE_INCOMPAT.INCOMPAT_JOURNAL_DEV

#### FEATURE_INCOMPAT.INCOMPAT_META_BG

#### FEATURE_INCOMPAT.INCOMPAT_EXTENTS

#### FEATURE_INCOMPAT.INCOMPAT_64BIT

#### FEATURE_INCOMPAT.INCOMPAT_MMP

#### FEATURE_INCOMPAT.INCOMPAT_FLEX_BG

#### FEATURE_INCOMPAT.INCOMPAT_EA_INODE

#### FEATURE_INCOMPAT.INCOMPAT_DIRDATA

#### FEATURE_INCOMPAT.INCOMPAT_CSUM_SEED

#### FEATURE_INCOMPAT.INCOMPAT_LARGEDIR

#### FEATURE_INCOMPAT.INCOMPAT_INLINE_DATA

#### FEATURE_INCOMPAT.INCOMPAT_ENCRYPT

#### FEATURE_RO_COMPAT.RO_COMPAT_SPARSE_SUPER

#### FEATURE_RO_COMPAT.RO_COMPAT_LARGE_FILE

#### FEATURE_RO_COMPAT.RO_COMPAT_BTREE_DIR

#### FEATURE_RO_COMPAT.RO_COMPAT_HUGE_FILE

#### FEATURE_RO_COMPAT.RO_COMPAT_GDT_CSUM

#### FEATURE_RO_COMPAT.RO_COMPAT_DIR_NLINK

#### FEATURE_RO_COMPAT.RO_COMPAT_EXTRA_ISIZE

#### FEATURE_RO_COMPAT.RO_COMPAT_HAS_SNAPSHOT

#### FEATURE_RO_COMPAT.RO_COMPAT_QUOTA

#### FEATURE_RO_COMPAT.RO_COMPAT_BIGALLOC

#### FEATURE_RO_COMPAT.RO_COMPAT_METADATA_CSUM

#### FEATURE_RO_COMPAT.RO_COMPAT_REPLICA

#### FEATURE_RO_COMPAT.RO_COMPAT_READONLY

#### FEATURE_RO_COMPAT.RO_COMPAT_PROJECT

#### DEFAULT_HASH_VERSION.Legacy

#### DEFAULT_HASH_VERSION.HalfMD4

#### DEFAULT_HASH_VERSION.Tea

#### DEFAULT_HASH_VERSION.UnsignedLegacy

#### DEFAULT_HASH_VERSION.UnsignedHalfMD4

#### DEFAULT_HASH_VERSION.UnsignedTea

#### DEFAULT_MOUNT_OPTIONS.EXT4_DEFM_DEBUG

#### DEFAULT_MOUNT_OPTIONS.EXT4_DEFM_BSDGROUPS

#### DEFAULT_MOUNT_OPTIONS.EXT4_DEFM_XATTR_USER

#### DEFAULT_MOUNT_OPTIONS.EXT4_DEFM_ACL

#### DEFAULT_MOUNT_OPTIONS.EXT4_DEFM_UID16

#### DEFAULT_MOUNT_OPTIONS.EXT4_DEFM_JMODE_DATA

#### DEFAULT_MOUNT_OPTIONS.EXT4_DEFM_JMODE_ORDERED

#### DEFAULT_MOUNT_OPTIONS.EXT4_DEFM_JMODE_WBACK

#### DEFAULT_MOUNT_OPTIONS.EXT4_DEFM_NOBARRIER

#### DEFAULT_MOUNT_OPTIONS.EXT4_DEFM_BLOCK_VALIDITY

#### DEFAULT_MOUNT_OPTIONS.EXT4_DEFM_DISCARD

#### DEFAULT_MOUNT_OPTIONS.EXT4_DEFM_NODELALLOC

#### FLAGS.SignedDirectoryHash

#### FLAGS.UnsignedDirectoryHash

#### FLAGS.DevelopmentCode

#### CHECKSUM_TYPE.crc32c

#### ENCRYPTION_ALGORITHMS.ENCRYPTION_MODE_INVALID

#### ENCRYPTION_ALGORITHMS.ENCRYPTION_MODE_AES_256_XTS

#### ENCRYPTION_ALGORITHMS.ENCRYPTION_MODE_AES_256_GCM

#### ENCRYPTION_ALGORITHMS.ENCRYPTION_MODE_AES_256_CBC

#### InodeCount

#### BlockCount

#### RootBlockCount

#### FreeBlockCount

#### FreeInodeCount

#### FirstDataBlock

#### BlockSize

#### ClusterSize

#### BlocksPerGroup

#### ClustersPerGroup

#### InodesPerGroup

#### MountTime

#### WriteTime

#### MountCount

#### MaxMountCount

#### Magic

#### State

#### Errors

#### MinorRevisionLevel

#### LastCheck

#### CheckInterval

#### CreatorOs

#### RevisionLevel

#### DefaultUserId

#### DefaultGroupId

#### FirstInode

#### InodeSize

#### BlockGroupNumber

#### FeatureCompat

#### FeatureIncompat

#### FeatureRoCompat

#### Uuid

#### VolumeName

#### LastMountedDirectory

#### AlgorithmUsageBitmap

#### PreallocatedBlocks

#### PreallocatedDirectoryBlocks

#### ReservedGdtBlocks

#### JournalUuid

#### JournalInode

#### JournalDevice

#### LastOrphanedList

#### HashSeed

#### DefaultHashVersion

#### JournalBackupType

#### GroupDescriptorSize

#### DefaultMountOptions

#### FirstMetablockBlockGroup

#### MkfsTime

#### JournalBlocks

#### MinimumExtraInodeSize

#### DesiredInodeSize

#### Flags

#### RaidStride

#### MmpInterval

#### MmpBlock

#### RaidStipeWidth

#### FlexibleBlockGroupSize

#### ChecksumType

#### KBytesWritten

#### SnapshotInode

#### SnapshotId

#### ReservedSnapshotBlockCount

#### SnapshotListInode

#### ErrorCount

#### FirstErrorTime

#### FirstErrorInode

#### FirstErrorBlock

#### FirstErrorFunction

#### FirstErrorLine

#### LastErrorTime

#### LastErrorInode

#### LastErrorLine

#### LastErrorBlock

#### LastErrorFunction

#### MountOptions

#### UserQuotaInode

#### GroupQuotaInode

#### OverheadBlocks

#### SuperblockBackupBlockGroup

#### EncryptionAlgorithms

#### EncryptPasswordSalt

#### LostAndFoundInode

#### ProjectQuotaInode

#### ChecksumSeed

#### Checksum

### Methods


#### Get(System.String)

> ##### Parameters
> **volumenName:** 

> ##### Return value
> 

## Ext.Superblock.STATE
            

        
### Fields

#### CleanlyUmounted

#### ErrorsDetected

#### RecoveringOrphans


## Ext.Superblock.ERRORS
            

        
### Fields

#### Continue

#### RemountReadOnly

#### Panic


## Ext.Superblock.CREATOR_OS
            

        
### Fields

#### Linux

#### Hurd

#### Masix

#### FreeBSD

#### Lites


## Ext.Superblock.REVISION_LEVEL
            

        
### Fields

#### Originalformat

#### v2formatwithdynamicinodesizes


## Ext.Superblock.FEATURE_COMPAT
            

        
### Fields

#### COMPAT_DIR_PREALLOC

#### COMPAT_IMAGIC_INODES

#### COMPAT_HAS_JOURNAL

#### COMPAT_EXT_ATTR

#### COMPAT_RESIZE_INODE

#### COMPAT_DIR_INDEX

#### COMPAT_LAZY_BG

#### COMPAT_EXCLUDE_INODE

#### COMPAT_EXCLUDE_BITMAP

#### COMPAT_SPARSE_SUPER2


## Ext.Superblock.FEATURE_INCOMPAT
            

        
### Fields

#### INCOMPAT_COMPRESSION

#### INCOMPAT_FILETYPE

#### INCOMPAT_RECOVER

#### INCOMPAT_JOURNAL_DEV

#### INCOMPAT_META_BG

#### INCOMPAT_EXTENTS

#### INCOMPAT_64BIT

#### INCOMPAT_MMP

#### INCOMPAT_FLEX_BG

#### INCOMPAT_EA_INODE

#### INCOMPAT_DIRDATA

#### INCOMPAT_CSUM_SEED

#### INCOMPAT_LARGEDIR

#### INCOMPAT_INLINE_DATA

#### INCOMPAT_ENCRYPT


## Ext.Superblock.FEATURE_RO_COMPAT
            

        
### Fields

#### RO_COMPAT_SPARSE_SUPER

#### RO_COMPAT_LARGE_FILE

#### RO_COMPAT_BTREE_DIR

#### RO_COMPAT_HUGE_FILE

#### RO_COMPAT_GDT_CSUM

#### RO_COMPAT_DIR_NLINK

#### RO_COMPAT_EXTRA_ISIZE

#### RO_COMPAT_HAS_SNAPSHOT

#### RO_COMPAT_QUOTA

#### RO_COMPAT_BIGALLOC

#### RO_COMPAT_METADATA_CSUM

#### RO_COMPAT_REPLICA

#### RO_COMPAT_READONLY

#### RO_COMPAT_PROJECT


## Ext.Superblock.DEFAULT_HASH_VERSION
            

        
### Fields

#### Legacy

#### HalfMD4

#### Tea

#### UnsignedLegacy

#### UnsignedHalfMD4

#### UnsignedTea


## Ext.Superblock.DEFAULT_MOUNT_OPTIONS
            

        
### Fields

#### EXT4_DEFM_DEBUG

#### EXT4_DEFM_BSDGROUPS

#### EXT4_DEFM_XATTR_USER

#### EXT4_DEFM_ACL

#### EXT4_DEFM_UID16

#### EXT4_DEFM_JMODE_DATA

#### EXT4_DEFM_JMODE_ORDERED

#### EXT4_DEFM_JMODE_WBACK

#### EXT4_DEFM_NOBARRIER

#### EXT4_DEFM_BLOCK_VALIDITY

#### EXT4_DEFM_DISCARD

#### EXT4_DEFM_NODELALLOC


## Ext.Superblock.FLAGS
            

        
### Fields

#### SignedDirectoryHash

#### UnsignedDirectoryHash

#### DevelopmentCode


## Ext.Superblock.CHECKSUM_TYPE
            

        
### Fields

#### crc32c


## Ext.Superblock.ENCRYPTION_ALGORITHMS
            

        
### Fields

#### ENCRYPTION_MODE_INVALID

#### ENCRYPTION_MODE_AES_256_XTS

#### ENCRYPTION_MODE_AES_256_GCM

#### ENCRYPTION_MODE_AES_256_CBC


## Fat.DirectoryEntry
            

        
### Fields

#### FILE_ATTR.ATTR_READ_ONLY

#### FILE_ATTR.ATTR_HIDDEN

#### FILE_ATTR.ATTR_SYSTEM

#### FILE_ATTR.ATTR_VOLUME_ID

#### FILE_ATTR.ATTR_DIRECTORY

#### FILE_ATTR.ATTR_ARCHIVE

#### FILE_ATTR.ATTR_LONG_NAME

#### Volume

#### FileName

#### FullName

#### Deleted

#### DIR_Attribute

#### Directory

#### Hidden

#### DIR_CreationTimeTenth

#### DIR_CreationTime

#### DIR_CreationDate

#### CreationTime

#### DIR_LastAccessDate

#### AccessTime

#### DIR_FirstClusterHI

#### DIR_WriteTime

#### DIR_WriteDate

#### WriteTime

#### DIR_FirstClusterLO

#### FirstCluster

#### FileSize

### Methods


#### Constructor

> ##### Parameters
> **bytes:** 

> **index:** 

> **volume:** 

> **longNameList:** 

> **directoryName:** 


#### Get(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### Get(System.Byte[],System.Int32,System.String,System.Collections.Generic.List{PowerForensics.Fat.LongDirectoryEntry},System.String)

> ##### Parameters
> **bytes:** 

> **index:** 

> **volume:** 

> **list:** 

> **directoryName:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetInstances(System.Byte[],System.String,System.String)

> ##### Parameters
> **bytes:** 

> **volume:** 

> **directoryName:** 

> ##### Return value
> 

#### GetChildItem(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetRootDirectory(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetShortName(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **index:** 

> ##### Return value
> 

#### GetLongName(System.String,System.Collections.Generic.List{PowerForensics.Fat.LongDirectoryEntry},System.String)

> ##### Parameters
> **FileName:** 

> **list:** 

> **directoryName:** 

> ##### Return value
> 

#### TestIfFree(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **index:** 

> ##### Return value
> 

#### GetChildItem

> ##### Return value
> 

#### GetChildItem(System.Boolean)

> ##### Parameters
> **recurse:** 

> ##### Return value
> 

#### GetContent

> ##### Return value
> 

## Fat.DirectoryEntry.FILE_ATTR
            

        
### Fields

#### ATTR_READ_ONLY

#### ATTR_HIDDEN

#### ATTR_SYSTEM

#### ATTR_VOLUME_ID

#### ATTR_DIRECTORY

#### ATTR_ARCHIVE

#### ATTR_LONG_NAME


## Fat.FatVolumeBootRecord
            

        
### Fields

#### FatType

#### BS_OEMName

#### BPB_NumberOfFATs

#### BPB_RootEntryCount

#### BPB_TotalSector16

#### BPB_Media

#### BPB_FatSize16

#### BPB_TotalSector32

#### BPB_FatSize32

#### BPB_ExtFlags

#### BPB_FileSystemVersion

#### BPB_RootCluster

#### BPB_FileSytemInfo

#### BPB_BackupBootSector

#### BS_DriveNumber

#### BS_BootSignature

#### BS_VolumeId

#### BS_VolumeLabel

#### BS_FileSystemType

### Methods


#### GetFatType(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

## Fat.FileAllocationTable
            

        
### Fields

#### 

#### 

### Methods


#### GetBytes(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetBytes(System.String,PowerForensics.Fat.FatVolumeBootRecord)

> ##### Parameters
> **volume:** 

> **vbr:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **volume:** 

> **sector:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **bytes:** 

> **sector:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **bytes:** 

> **sector:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **bytes:** 

> **sector:** 

> ##### Return value
> 

## Fat.FileAllocationTableEntry
            

        
### Fields

#### StartSector

#### EndSector

### Methods


#### Get(System.String,System.Int32)

> ##### Parameters
> **volume:** 

> **sector:** 

> ##### Return value
> 

#### parseFat12(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **sector:** 

> ##### Return value
> 

#### parseFat16(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **sector:** 

> ##### Return value
> 

#### parseFat32(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **sector:** 

> ##### Return value
> 

## Fat.FileSystemInformation
            

        
### Fields

#### FSI_LeadSig

#### FSI_StrucSig

#### FSI_Free_Count

#### FSI_Nxt_Free

#### FSI_TrailSig

### Methods


#### Get(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Fat.LongDirectoryEntry
            

        
### Fields

#### LDIR_Ord

#### LDIR_Name1

#### LDIR_Attr

#### LDIR_Type

#### LDIR_Chksum

#### LDIR_Name2

#### LDIR_FstClusLO

#### LDIR_Name3

#### LDIR_NamePart

### Methods


#### Get(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **index:** 

> ##### Return value
> 

## Generic.VolumeBootRecord
            

        
### Fields

#### MEDIA_DESCRIPTOR.FloppyDisk

#### MEDIA_DESCRIPTOR.HardDriveDisk

#### MediaDescriptor

#### BytesPerSector

#### SectorsPerCluster

#### BytesPerCluster

#### ReservedSectors

#### SectorsPerTrack

#### NumberOfHeads

#### HiddenSectors

#### CodeSection

### Methods


#### checkFooter(System.Byte[])

> ##### Parameters
> **bytes:** 


#### Get(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### Get(System.IO.FileStream)

> ##### Parameters
> **streamToRead:** 

> ##### Return value
> 

#### Get(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

#### GetBytes(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetBytes(System.IO.FileStream)

> ##### Parameters
> **streamToRead:** 

> ##### Return value
> 

#### GetBytesByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

## Generic.VolumeBootRecord.MEDIA_DESCRIPTOR
            

        
### Fields

#### FloppyDisk

#### HardDriveDisk


## HFSPlus.AllocationFile
            

        
### Methods


#### GetContent(System.String)
Returns the Contents of the HFS+ Allocation File.
> ##### Parameters
> **volumeName:** 

> ##### Return value
> 

#### IsAllocationBlockUsed(System.String,System.UInt32)

> ##### Parameters
> **volumeName:** 

> **blockNumber:** 

> ##### Return value
> 

## HFSPlus.AttributesFile
            

        
### Methods


#### GetContent(System.String)
Returns the content of the HFS+ Attributes File.
> ##### Parameters
> **volumeName:** 

> ##### Return value
> 

#### GetHeaderNode(System.String)

> ##### Parameters
> **volumeName:** 

> ##### Return value
> 

#### GetNode(System.String,System.UInt32)

> ##### Parameters
> **volumeName:** 

> **nodeNumber:** 

> ##### Return value
> 

#### GetNodeBytes(System.String,System.UInt32)

> ##### Parameters
> **volumeName:** 

> **nodeNumber:** 

> ##### Return value
> 

## HFSPlus.BTree.Node
            

        
### Fields

#### VolumeName

#### FileName

#### NodeNumber

#### NodeDescriptor

#### Records

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

### Methods


#### Get(System.String,System.String,System.UInt32)

> ##### Parameters
> **volumeName:** 

> **fileName:** 

> **nodeNumber:** 

> ##### Return value
> 

#### Get(System.Byte[],System.String,System.String,System.UInt32)

> ##### Parameters
> **bytes:** 

> **volumeName:** 

> **fileName:** 

> **nodeNumber:** 

> ##### Return value
> 

#### GetHeaderNode(System.String,System.String)

> ##### Parameters
> **volumeName:** 

> **fileName:** 

> ##### Return value
> 

#### GetHeaderBytes(System.String,System.String)

> ##### Parameters
> **volumeName:** 

> **fileName:** 

> ##### Return value
> 

#### GetBytes(System.String,System.String,System.UInt32)

> ##### Parameters
> **volumeName:** 

> **fileName:** 

> **nodeNumber:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volumeName:** 

> **fileName:** 

> ##### Return value
> 

#### 

> ##### Return value
> 

#### 

> ##### Return value
> 

## HFSPlus.BTree.NodeDescriptor
            

        
### Fields

#### NODE_KIND.kBTLeafNode

#### NODE_KIND.kBTIndexNode

#### NODE_KIND.kBTHeaderNode

#### NODE_KIND.kBTMapNode

#### VolumeName

#### FileName

#### fLink

#### bLink

#### Kind

#### Height

#### NumRecords

### Methods


#### Get(System.Byte[],System.Int32,System.String,System.String)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volumeName:** 

> **fileName:** 

> ##### Return value
> 

#### GetfLink

> ##### Return value
> 

#### GetbLink

> ##### Return value
> 

## HFSPlus.BTree.NodeDescriptor.NODE_KIND
            

        
### Fields

#### kBTLeafNode

#### kBTIndexNode

#### kBTHeaderNode

#### kBTMapNode


## HFSPlus.BTree.Record
            

        

## HFSPlus.BTree.HeaderRecord
            

        
### Fields

#### BTREE_TYPE.kHFSBTreeType

#### BTREE_TYPE.kUserBTreeType

#### BTREE_TYPE.kReservedBTreeType

#### BTREE_KEYCOMPARE.kHFSBinaryCompare

#### BTREE_KEYCOMPARE.kHFSCaseFolding

#### BTREE_ATTRIBUTE.kBTBadCloseMask

#### BTREE_ATTRIBUTE.kBTBigKeysMask

#### BTREE_ATTRIBUTE.kBTVariableIndexKeysMask

#### VolumeName

#### FileName

#### TreeDepth

#### RootNode

#### LeafRecords

#### FirstLeafNode

#### LastLeafNode

#### NodeSize

#### MaxKeyLength

#### TotalNodes

#### FreeNodes

#### ClumpSize

#### BTreeType

#### KeyCompareType

#### Attributes

### Methods


#### Get(System.Byte[],System.Int32,System.String,System.String)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volumeName:** 

> **fileName:** 

> ##### Return value
> 

#### GetRootNode

> ##### Return value
> 

#### GetFirstLeafNode

> ##### Return value
> 

#### GetLastLeafNode

> ##### Return value
> 

## HFSPlus.BTree.HeaderRecord.BTREE_TYPE
            

        
### Fields

#### kHFSBTreeType

#### kUserBTreeType

#### kReservedBTreeType


## HFSPlus.BTree.HeaderRecord.BTREE_KEYCOMPARE
            

        
### Fields

#### kHFSBinaryCompare

#### kHFSCaseFolding


## HFSPlus.BTree.HeaderRecord.BTREE_ATTRIBUTE
            

        
### Fields

#### kBTBadCloseMask

#### kBTBigKeysMask

#### kBTVariableIndexKeysMask


## HFSPlus.BTree.UserDataRecord
            

        
### Fields

#### UserData

### Methods


#### Get(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> ##### Return value
> 

## HFSPlus.BTree.MapRecord
            

        

## HFSPlus.BTree.KeyedRecord
            

        
### Fields

#### VolumeName

#### FileName

#### KeyLength

#### ParentCatalogNodeId

#### Name

### Methods


#### GetHfsString(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> ##### Return value
> 

## HFSPlus.BTree.PointerRecord
            

        
### Fields

#### NodeNumber

### Methods


#### Get(System.Byte[],System.Int32,System.String,System.String)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volumeName:** 

> **fileName:** 

> ##### Return value
> 

#### GetChildNodes

> ##### Return value
> 

## HFSPlus.BTree.DataRecord
            

        
### Fields

#### RECORD_TYPE.kHFSPlusFolderRecord

#### RECORD_TYPE.kHFSPlusFileRecord

#### RECORD_TYPE.kHFSPlusFolderThreadRecord

#### RECORD_TYPE.kHFSPlusFileThreadRecord

#### RECORD_FLAGS.kHFSFileLockedBit

#### RECORD_FLAGS.kHFSFileLockedMask

#### RECORD_FLAGS.kHFSThreadExistsBit

#### RECORD_FLAGS.kHFSThreadExistsMask

#### RecordType

### Methods


#### GetRecordType(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> ##### Return value
> 

## HFSPlus.BTree.DataRecord.RECORD_TYPE
            

        
### Fields

#### kHFSPlusFolderRecord

#### kHFSPlusFileRecord

#### kHFSPlusFolderThreadRecord

#### kHFSPlusFileThreadRecord


## HFSPlus.BTree.DataRecord.RECORD_FLAGS
            

        
### Fields

#### kHFSFileLockedBit

#### kHFSFileLockedMask

#### kHFSThreadExistsBit

#### kHFSThreadExistsMask


## HFSPlus.CatalogFile
            

        
### Fields

#### TEXT_ENCODING.MacRoman

#### TEXT_ENCODING.MacJapanese

#### TEXT_ENCODING.MacChineseTrad

#### TEXT_ENCODING.MacKorean

#### TEXT_ENCODING.MacArabic

#### TEXT_ENCODING.MacHebrew

#### TEXT_ENCODING.MacGreek

#### TEXT_ENCODING.MacCyrillic

#### TEXT_ENCODING.MacDevanagari

#### TEXT_ENCODING.MacGurmukhi

#### TEXT_ENCODING.MacGujarati

#### TEXT_ENCODING.MacOriya

#### TEXT_ENCODING.MacBengali

#### TEXT_ENCODING.MacTamil

#### TEXT_ENCODING.MacTelugu

#### TEXT_ENCODING.MacKannada

#### TEXT_ENCODING.MacMalayalam

#### TEXT_ENCODING.MacSinhales

#### TEXT_ENCODING.MacBurmese

#### TEXT_ENCODING.MacKhmer

#### TEXT_ENCODING.MacThai

#### TEXT_ENCODING.MacLaotian

#### TEXT_ENCODING.MacGeorgian

#### TEXT_ENCODING.MacArmenian

#### TEXT_ENCODING.MacChineseSimp

#### TEXT_ENCODING.MacTibetan

#### TEXT_ENCODING.MacMongolian

#### TEXT_ENCODING.MacEthiopic

#### TEXT_ENCODING.MacCentralEurRoman

#### TEXT_ENCODING.MacVietnamese

#### TEXT_ENCODING.MacExtArabic

#### TEXT_ENCODING.MacSymbol

#### TEXT_ENCODING.MacDingbats

#### TEXT_ENCODING.MacTurkish

#### TEXT_ENCODING.MacCroatian

#### TEXT_ENCODING.MacIcelandic

#### TEXT_ENCODING.MacRomanian

#### TEXT_ENCODING.MacUkrainian

#### TEXT_ENCODING.MacFarsi

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

### Methods


#### GetContent(System.String)
Returns the contents of the HFS+ Catalog File.
> ##### Parameters
> **volumeName:** 

> ##### Return value
> 

#### GetHeaderNode(System.String)

> ##### Parameters
> **volumeName:** 

> ##### Return value
> 

#### GetNode(System.String,System.UInt32)

> ##### Parameters
> **volumeName:** 

> **nodeNumber:** 

> ##### Return value
> 

#### GetNodeBytes(System.String,System.UInt32)

> ##### Parameters
> **volumeName:** 

> **nodeNumber:** 

> ##### Return value
> 

#### GetRecords(System.String)

> ##### Parameters
> **volumeName:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volumeName:** 

> **fileName:** 

> ##### Return value
> 

#### 

> ##### Return value
> 

#### 

> ##### Return value
> 

## HFSPlus.CatalogFile.TEXT_ENCODING
            

        
### Fields

#### MacRoman

#### MacJapanese

#### MacChineseTrad

#### MacKorean

#### MacArabic

#### MacHebrew

#### MacGreek

#### MacCyrillic

#### MacDevanagari

#### MacGurmukhi

#### MacGujarati

#### MacOriya

#### MacBengali

#### MacTamil

#### MacTelugu

#### MacKannada

#### MacMalayalam

#### MacSinhales

#### MacBurmese

#### MacKhmer

#### MacThai

#### MacLaotian

#### MacGeorgian

#### MacArmenian

#### MacChineseSimp

#### MacTibetan

#### MacMongolian

#### MacEthiopic

#### MacCentralEurRoman

#### MacVietnamese

#### MacExtArabic

#### MacSymbol

#### MacDingbats

#### MacTurkish

#### MacCroatian

#### MacIcelandic

#### MacRomanian

#### MacUkrainian

#### MacFarsi


## HFSPlus.CatalogFolderRecord
            

        
### Fields

#### Flags

#### Valence

#### CatalogNodeId

#### CreateDate

#### ContentModDate

#### AttributeModDate

#### AccessDate

#### BackupDate

#### Permissions

#### UserInfo

#### FinderInfo

#### TextEncoding

#### FolderCount

### Methods


#### Get(System.Byte[],System.Int32,System.String,System.String)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volumeName:** 

> **fileName:** 

> ##### Return value
> 

## HFSPlus.CatalogFileRecord
            

        
### Fields

#### FILE_FLAGS.kHFSFileLockedBit

#### FILE_FLAGS.kHFSFileLockedMask

#### FILE_FLAGS.kHFSThreadExistsBit

#### FILE_FLAGS.kHFSThreadExistsMask

#### Flags

#### CatalogNodeId

#### CreateDate

#### ContentModDate

#### AttributeModDate

#### AccessDate

#### BackupDate

#### Permissions

#### UserInfo

#### FinderInfo

#### TextEncoding

#### DataFork

#### ResourceFork

### Methods


#### Get(System.Byte[],System.Int32,System.String,System.String)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volumeName:** 

> **fileName:** 

> ##### Return value
> 

#### GetDataForkContent

> ##### Return value
> 

#### GetResourceForkContent

> ##### Return value
> 

## HFSPlus.CatalogFileRecord.FILE_FLAGS
            

        
### Fields

#### kHFSFileLockedBit

#### kHFSFileLockedMask

#### kHFSThreadExistsBit

#### kHFSThreadExistsMask


## HFSPlus.CatalogThread
            

        
### Fields

#### ParentId

#### NodeName

### Methods


#### Get(System.Byte[],System.Int32,System.String,System.String)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volumeName:** 

> **fileName:** 

> ##### Return value
> 

## HFSPlus.BSDInfo
            

        
### Fields

#### ADMIN_FLAGS.SF_ARCHIVED

#### ADMIN_FLAGS.SF_IMMUTABLE

#### ADMIN_FLAGS.SF_APPEND

#### OWNER_FLAGS.UF_NODUMP

#### OWNER_FLAGS.UF_IMMUTABLE

#### OWNER_FLAGS.UF_APPEND

#### OWNER_FLAGS.UF_OPAQUE

#### FILE_MODE.S_ISTXT

#### FILE_MODE.S_ISGID

#### FILE_MODE.S_ISUID

#### FILE_MODE.S_IXUSR

#### FILE_MODE.S_IWUSR

#### FILE_MODE.S_IRUSR

#### FILE_MODE.S_IRWXU

#### FILE_MODE.S_IXGRP

#### FILE_MODE.S_IWGRP

#### FILE_MODE.S_IRGRP

#### FILE_MODE.S_IRWXG

#### FILE_MODE.S_IXOTH

#### FILE_MODE.S_IWOTH

#### FILE_MODE.S_IROTH

#### FILE_MODE.S_IRWXO

#### FILE_MODE.S_IFIFO

#### FILE_MODE.S_IFCHR

#### FILE_MODE.S_IFDIR

#### FILE_MODE.S_IFBLK

#### FILE_MODE.S_IFREG

#### FILE_MODE.S_IFLNK

#### FILE_MODE.S_IFSOCK

#### FILE_MODE.S_IFWHT

#### FILE_MODE.S_IFMT

#### OwnerID

#### GroupID

#### AdminFlags

#### OwnerFlags

#### FileMode

#### Special

### Methods


#### Get(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> ##### Return value
> 

#### ToString

> ##### Return value
> 

## HFSPlus.BSDInfo.ADMIN_FLAGS
            

        
### Fields

#### SF_ARCHIVED

#### SF_IMMUTABLE

#### SF_APPEND


## HFSPlus.BSDInfo.OWNER_FLAGS
            

        
### Fields

#### UF_NODUMP

#### UF_IMMUTABLE

#### UF_APPEND

#### UF_OPAQUE


## HFSPlus.BSDInfo.FILE_MODE
            

        
### Fields

#### S_ISTXT

#### S_ISGID

#### S_ISUID

#### S_IXUSR

#### S_IWUSR

#### S_IRUSR

#### S_IRWXU

#### S_IXGRP

#### S_IWGRP

#### S_IRGRP

#### S_IRWXG

#### S_IXOTH

#### S_IWOTH

#### S_IROTH

#### S_IRWXO

#### S_IFIFO

#### S_IFCHR

#### S_IFDIR

#### S_IFBLK

#### S_IFREG

#### S_IFLNK

#### S_IFSOCK

#### S_IFWHT

#### S_IFMT


## HFSPlus.Point
            

        
### Fields

#### Vertical

#### Horizontal

### Methods


#### Get(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> ##### Return value
> 

## HFSPlus.Rect
            

        
### Fields

#### Top

#### Left

#### Bottom

#### Right

### Methods


#### Get(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> ##### Return value
> 

## HFSPlus.FileInfo
            

        
### Fields

#### FileType

#### FileCreator

#### FinderFlags

#### Location

#### ReservedField

### Methods


#### Get(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> ##### Return value
> 

## HFSPlus.ExtendedFileInfo
            

        
### Fields

#### ExtendedFinderFlags

#### PutAwayFolderID

### Methods


#### Get(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> ##### Return value
> 

## HFSPlus.FolderInfo
            

        
### Fields

#### WindowBounds

#### FinderFlags

#### Location

### Methods


#### Get(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> ##### Return value
> 

## HFSPlus.ExtendedFolderInfo
            

        
### Fields

#### ScrollPosition

#### ExtendedFinderFlags

#### PutAwayFolderID

### Methods


#### Get(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> ##### Return value
> 

## HFSPlus.ForkData
            

        
### Fields

#### LogicalSize

#### ClumpSize

#### TotalBlocks

#### Extents

### Methods


#### Get(System.Byte[],System.Int32,System.String,System.UInt32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volumeName:** 

> **blockSize:** 

> ##### Return value
> 

#### GetContent

> ##### Return value
> 

#### GetSlack

> ##### Return value
> 

## HFSPlus.ExtentDescriptor
            

        
### Fields

#### VolumeName

#### BlockSize

#### StartBlock

#### BlockCount

### Methods


#### GetInstances(System.Byte[],System.Int32,System.String,System.UInt32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volumeName:** 

> **blockSize:** 

> ##### Return value
> 

#### Get(System.Byte[],System.Int32,System.String,System.UInt32)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volumeName:** 

> **blockSize:** 

> ##### Return value
> 

#### GetContent

> ##### Return value
> 

## HFSPlus.ExtentsOverflowFile
            

        
### Methods


#### GetContent(System.String)
Returns the contents of the HFS+ Extents Overflow File.
> ##### Parameters
> **volumeName:** 

> ##### Return value
> 

#### GetHeaderNode(System.String)

> ##### Parameters
> **volumeName:** 

> ##### Return value
> 

#### GetNode(System.String,System.UInt32)

> ##### Parameters
> **volumeName:** 

> **nodeNumber:** 

> ##### Return value
> 

#### GetNodeBytes(System.String,System.UInt32)

> ##### Parameters
> **volumeName:** 

> **nodeNumber:** 

> ##### Return value
> 

## HFSPlus.ExtentsOverflowRecord
            

        
### Fields

#### FORK_TYPE.Data

#### FORK_TYPE.Resource

#### ForkType

#### CatalogNodeId

#### RelativeStartBlock

#### Extents

### Methods


#### Get(System.Byte[],System.Int32,System.String,System.String)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volumeName:** 

> **fileName:** 

> ##### Return value
> 

## HFSPlus.ExtentsOverflowRecord.FORK_TYPE
            

        
### Fields

#### Data

#### Resource


## HFSPlus.VolumeHeader
            

        
### Fields

#### HFS_VERSION.HFSPLUS

#### HFS_VERSION.HFSX

#### Signature

#### Version

#### Attributes

#### LastMountedVersion

#### JournalInfoBlock

#### CreateData

#### ModifyDate

#### BackupDate

#### CheckedDate

#### FileCount

#### FolderCount

#### BlockSize

#### TotalBlocks

#### FreeBlocks

#### NextAllocation

#### RsrcClumpSize

#### DataClumpSize

#### NextCatalogId

#### WriteCount

#### EncodingBitmap

#### FinderInfoArray0

#### FinderInfoArray1

#### FinderInfoArray2

#### FinderInfoArray3

#### FinderInfoArray4

#### FinderInfoArray5

#### FinderInfoArray6

#### FinderInfoArray7

#### AllocationFile

#### ExtentsOverflowFile

#### CatalogFile

#### AttributesFile

#### StartupFile

### Methods


#### Get(System.String)

> ##### Parameters
> **volumeName:** 

> ##### Return value
> 

#### GetAllocationFileBytes

> ##### Return value
> 

#### GetExtentsOverflowFileBytes

> ##### Return value
> 

#### GetCatalogFileBytes

> ##### Return value
> 

#### GetAttributesFileBytes

> ##### Return value
> 

#### GetStartupFileBytes

> ##### Return value
> 

## HFSPlus.VolumeHeader.HFS_VERSION
            

        
### Fields

#### HFSPLUS

#### HFSX


## Ntfs.MasterFileTable
            

        
### Methods


#### GetRecord(System.IO.FileStream,System.String)

> ##### Parameters
> **streamToRead:** 

> **volume:** 

> ##### Return value
> 

#### GetBytes(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetBytes(System.IO.FileStream,System.String)

> ##### Parameters
> **streamToRead:** 

> **volume:** 

> ##### Return value
> 

#### GetSlack(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetSlackByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetSlack(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

## Ntfs.Data
            

        
### Fields

#### RawData

#### 

#### 

#### 

#### 

### Methods


#### 

> ##### Parameters
> **bytes:** 

> **volume:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volume:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **bytes:** 

> **offset:** 

> **lengthByteCount:** 

> **offsetByteCount:** 

> **previousDR:** 

> **volume:** 

> ##### Return value
> 

#### 

> ##### Return value
> 

## Ntfs.EA
            

        

## Ntfs.EAInformation
            

        

## Ntfs.FileName
            

        
### Fields

#### ATTR_FILENAME_FLAG.READONLY

#### ATTR_FILENAME_FLAG.HIDDEN

#### ATTR_FILENAME_FLAG.SYSTEM

#### ATTR_FILENAME_FLAG.ARCHIVE

#### ATTR_FILENAME_FLAG.DEVICE

#### ATTR_FILENAME_FLAG.NORMAL

#### ATTR_FILENAME_FLAG.TEMP

#### ATTR_FILENAME_FLAG.SPARSE

#### ATTR_FILENAME_FLAG.REPARSE

#### ATTR_FILENAME_FLAG.COMPRESSED

#### ATTR_FILENAME_FLAG.OFFLINE

#### ATTR_FILENAME_FLAG.NCI

#### ATTR_FILENAME_FLAG.ENCRYPTED

#### ATTR_FILENAME_FLAG.DIRECTORY

#### ATTR_FILENAME_FLAG.INDEXVIEW

#### Filename

#### ParentSequenceNumber

#### ParentRecordNumber

#### Namespace

#### AllocatedSize

#### RealSize

#### Flags

#### ER

#### NameLength

#### ModifiedTime

#### AccessedTime

#### ChangedTime

#### BornTime


## Ntfs.FileName.ATTR_FILENAME_FLAG
            

        
### Fields

#### READONLY

#### HIDDEN

#### SYSTEM

#### ARCHIVE

#### DEVICE

#### NORMAL

#### TEMP

#### SPARSE

#### REPARSE

#### COMPRESSED

#### OFFLINE

#### NCI

#### ENCRYPTED

#### DIRECTORY

#### INDEXVIEW


## Ntfs.FileRecordAttribute
            

        
### Fields

#### ATTR_TYPE.STANDARD_INFORMATION

#### ATTR_TYPE.ATTRIBUTE_LIST

#### ATTR_TYPE.FILE_NAME

#### ATTR_TYPE.OBJECT_ID

#### ATTR_TYPE.SECURITY_DESCRIPTOR

#### ATTR_TYPE.VOLUME_NAME

#### ATTR_TYPE.VOLUME_INFORMATION

#### ATTR_TYPE.DATA

#### ATTR_TYPE.INDEX_ROOT

#### ATTR_TYPE.INDEX_ALLOCATION

#### ATTR_TYPE.BITMAP

#### ATTR_TYPE.REPARSE_POINT

#### ATTR_TYPE.EA_INFORMATION

#### ATTR_TYPE.EA

#### ATTR_TYPE.LOGGED_UTILITY_STREAM

#### ATTR_TYPE.ATTR_FLAG_COMPRESSED

#### ATTR_TYPE.ATTR_FLAG_ENCRYPTED

#### ATTR_TYPE.ATTR_FLAG_SPARSE

#### Name

#### NameString

#### NonResident

#### AttributeId

#### AttributeSize

### Methods


#### GetInstances(System.Byte[],System.Int32,System.Int32,System.String)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **bytesPerFileRecord:** 

> **volume:** 

> ##### Return value
> 

#### Get(System.Byte[],System.Int32,System.String)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volume:** 

> ##### Return value
> 

## Ntfs.FileRecordAttribute.ATTR_TYPE
            

        
### Fields

#### STANDARD_INFORMATION

#### ATTRIBUTE_LIST

#### FILE_NAME

#### OBJECT_ID

#### SECURITY_DESCRIPTOR

#### VOLUME_NAME

#### VOLUME_INFORMATION

#### DATA

#### INDEX_ROOT

#### INDEX_ALLOCATION

#### BITMAP

#### REPARSE_POINT

#### EA_INFORMATION

#### EA

#### LOGGED_UTILITY_STREAM

#### ATTR_FLAG_COMPRESSED

#### ATTR_FLAG_ENCRYPTED

#### ATTR_FLAG_SPARSE


## Ntfs.IndexAllocationTest
            

        
### Fields

#### Entries


## Ntfs.IndexRoot
            

        
### Fields

#### INDEX_ROOT_FLAGS.INDEX_ROOT_ONLY

#### INDEX_ROOT_FLAGS.INDEX_ALLOCATION

#### AttributeType

#### CollationSortingRule

#### IndexSize

#### ClustersPerIndexRecord

#### StartOffset

#### TotalSize

#### AllocatedSize

#### Flags

#### EntryBytes

#### Entries


## Ntfs.IndexRoot.INDEX_ROOT_FLAGS
            

        
### Fields

#### INDEX_ROOT_ONLY

#### INDEX_ALLOCATION


## Ntfs.ObjectId
            

        
### Fields

#### ObjectIdGuid

#### BirthVolumeId

#### BirthObjectId

#### BirthDomainId


## Ntfs.StandardInformation
            

        
### Fields

#### ATTR_STDINFO_PERMISSION.READONLY

#### ATTR_STDINFO_PERMISSION.HIDDEN

#### ATTR_STDINFO_PERMISSION.SYSTEM

#### ATTR_STDINFO_PERMISSION.ARCHIVE

#### ATTR_STDINFO_PERMISSION.DEVICE

#### ATTR_STDINFO_PERMISSION.NORMAL

#### ATTR_STDINFO_PERMISSION.TEMP

#### ATTR_STDINFO_PERMISSION.SPARSE

#### ATTR_STDINFO_PERMISSION.REPARSE

#### ATTR_STDINFO_PERMISSION.COMPRESSED

#### ATTR_STDINFO_PERMISSION.OFFLINE

#### ATTR_STDINFO_PERMISSION.NCI

#### ATTR_STDINFO_PERMISSION.ENCRYPTED

#### BornTime

#### ModifiedTime

#### ChangedTime

#### AccessedTime

#### Permission

#### MaxVersionNumber

#### VersionNumber

#### ClassId

#### OwnerId

#### SecurityId

#### QuotaCharged

#### UpdateSequenceNumber


## Ntfs.StandardInformation.ATTR_STDINFO_PERMISSION
            

        
### Fields

#### READONLY

#### HIDDEN

#### SYSTEM

#### ARCHIVE

#### DEVICE

#### NORMAL

#### TEMP

#### SPARSE

#### REPARSE

#### COMPRESSED

#### OFFLINE

#### NCI

#### ENCRYPTED


## Ntfs.VolumeInformation
            

        
### Fields

#### ATTR_VOLINFO.FLAG_DIRTY
Dirty
#### ATTR_VOLINFO.FLAG_RLF
Resize logfile
#### ATTR_VOLINFO.FLAG_UOM
Upgrade on mount
#### ATTR_VOLINFO.FLAG_MONT
Mounted on NT4
#### ATTR_VOLINFO.FLAG_DUSN
Delete USN underway
#### ATTR_VOLINFO.FLAG_ROI
Repair object Ids
#### ATTR_VOLINFO.FLAG_MBC
Modified by chkdsk
#### Version

#### Flags

### Methods


#### Get(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### Get(PowerForensics.Ntfs.FileRecord)

> ##### Parameters
> **record:** 

> ##### Return value
> 

## Ntfs.VolumeInformation.ATTR_VOLINFO
            

        
### Fields

#### FLAG_DIRTY
Dirty
#### FLAG_RLF
Resize logfile
#### FLAG_UOM
Upgrade on mount
#### FLAG_MONT
Mounted on NT4
#### FLAG_DUSN
Delete USN underway
#### FLAG_ROI
Repair object Ids
#### FLAG_MBC
Modified by chkdsk

## Ntfs.VolumeName
            

        
### Fields

#### VolumeNameString

### Methods


#### Get(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### Get(PowerForensics.Ntfs.FileRecord)

> ##### Parameters
> **fileRecord:** 

> ##### Return value
> 

## Ntfs.AttributeList
            

        
### Fields

#### AttributeReference


## Ntfs.AttrRef
            

        
### Fields

#### Name

#### RecordLength

#### AttributeNameLength

#### AttributeNameOffset

#### LowestVCN

#### RecordNumber

#### SequenceNumber

#### NameString


## Ntfs.IndexAllocation
            

        
### Fields

#### 

#### Entries


## Ntfs.DataRun
            

        
### Fields

#### Volume

#### StartCluster

#### ClusterLength

#### Sparse

### Methods


#### GetInstances(System.Byte[],System.String)

> ##### Parameters
> **bytes:** 

> **volume:** 

> ##### Return value
> 

#### GetInstances(System.Byte[],System.Int32,System.String)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volume:** 

> ##### Return value
> 

#### Get(System.Byte[],System.Int32,System.Int32,System.Int32,PowerForensics.Ntfs.DataRun,System.String)

> ##### Parameters
> **bytes:** 

> **offset:** 

> **lengthByteCount:** 

> **offsetByteCount:** 

> **previousDR:** 

> **volume:** 

> ##### Return value
> 

#### GetBytes

> ##### Return value
> 

## Ntfs.NonResident
            

        
### Fields

#### Volume

#### commonHeader

#### StartVCN

#### LastVCN

#### DataRunOffset

#### CompUnitSize

#### AllocatedSize

#### RealSize

#### InitializedSize

#### DataRun

### Methods


#### GetBytes

> ##### Return value
> 

#### GetBytesTest

> ##### Return value
> 

#### GetBytes(PowerForensics.Generic.VolumeBootRecord)

> ##### Parameters
> **VBR:** 

> ##### Return value
> 

#### GetSlack

> ##### Return value
> 

## Ntfs.FileRecord
            

        
### Fields

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### FILE_RECORD_FLAG.INUSE
File record is in use
#### FILE_RECORD_FLAG.DIR
File record is a directory
#### VolumePath

#### OffsetOfUS

#### SizeOfUS

#### LogFileSequenceNumber

#### SequenceNumber

#### Hardlinks

#### OffsetOfAttribute

#### Flags

#### Deleted

#### Directory

#### RealSize

#### AllocatedSize

#### ReferenceToBase

#### NextAttrId

#### RecordNumber

#### Attribute

#### ModifiedTime

#### AccessedTime

#### ChangedTime

#### BornTime

#### Permission

#### FullName

#### Name

#### ParentSequenceNumber

#### ParentRecordNumber

#### FNModifiedTime

#### FNAccessedTime

#### FNChangedTime

#### FNBornTime

### Methods


#### 

> ##### Parameters
> **bytes:** 

> **offset:** 

> **bytesPerFileRecord:** 

> **volume:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **bytes:** 

> **offset:** 

> **volume:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetInstancesByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetInstances(System.String,System.Boolean)

> ##### Parameters
> **volume:** 

> **fast:** 

> ##### Return value
> 

#### GetInstances(System.Byte[],System.String,System.Boolean)

> ##### Parameters
> **bytes:** 

> **volume:** 

> **fast:** 

> ##### Return value
> 

#### Get(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### Get(System.String,System.Boolean)

> ##### Parameters
> **path:** 

> **fast:** 

> ##### Return value
> 

#### Get(System.String,System.Int32)

> ##### Parameters
> **volume:** 

> **index:** 

> ##### Return value
> 

#### Get(System.String,System.Int32,System.Boolean)

> ##### Parameters
> **volume:** 

> **index:** 

> **fast:** 

> ##### Return value
> 

#### Get(System.Byte[],System.String,System.Int32,System.Boolean)

> ##### Parameters
> **bytes:** 

> **volume:** 

> **bytesPerFileRecord:** 

> **fast:** 

> ##### Return value
> 

#### GetRecordBytes(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetRecordBytes(System.String,System.Int32)

> ##### Parameters
> **volume:** 

> **index:** 

> ##### Return value
> 

#### GetRecordBytesPrivate(System.String,System.Int32)

> ##### Parameters
> **volume:** 

> **index:** 

> ##### Return value
> 

#### GetContentBytes(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetContentBytes(System.String,System.String)

> ##### Parameters
> **path:** 

> **streamName:** 

> ##### Return value
> 

#### GetContent

> ##### Return value
> 

#### GetContent(System.String)

> ##### Parameters
> **StreamName:** 

> ##### Return value
> 

#### GetContent(PowerForensics.Ntfs.FileRecordAttribute)

> ##### Parameters
> **attribute:** 

> ##### Return value
> 

#### GetContent(PowerForensics.Ntfs.NtfsVolumeBootRecord)

> ##### Parameters
> **VBR:** 

> ##### Return value
> 

#### CopyFile(System.String)

> ##### Parameters
> **Destination:** 


#### CopyFile(System.String,System.String)

> ##### Parameters
> **Destination:** 

> **StreamName:** 


#### GetChild

> ##### Return value
> 

#### GetParent

> ##### Return value
> 

#### GetUsnJrnl

> ##### Return value
> 

#### GetSlack

> ##### Return value
> 

#### GetMftSlack

> ##### Return value
> 

#### ToString

> ##### Return value
> 

#### ApplyFixup(System.Byte[]@,System.Int32)

> ##### Parameters
> **bytes:** 

> **BytesPerFileRecord:** 


#### isDeleted(PowerForensics.Ntfs.FileRecord.FILE_RECORD_FLAG)

> ##### Parameters
> **flags:** 

> ##### Return value
> 

#### isDirectory(PowerForensics.Ntfs.FileRecord.FILE_RECORD_FLAG)

> ##### Parameters
> **flags:** 

> ##### Return value
> 

## Ntfs.FileRecord.FILE_RECORD_FLAG
            

        
### Fields

#### INUSE
File record is in use
#### DIR
File record is a directory

## Ntfs.IndexEntry
            

        
### Fields

#### RecordNumber

#### Directory

#### Size

#### StreamSize

#### Flags

#### Stream

#### Entry

#### Filename

#### FullName

### Methods


#### Get(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

## Ntfs.AttrDef
            

        
### Fields

#### ATTR_DEF_ENTRY.INDEX

#### ATTR_DEF_ENTRY.ALWAYS_RESIDENT

#### ATTR_DEF_ENTRY.ALWAYS_NONRESIDENT

#### Name

#### Type

#### DisplayRule

#### CollationRule

#### Flags

#### MinSize

#### MaxSize

### Methods


#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetInstancesByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetInstances(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

## Ntfs.AttrDef.ATTR_DEF_ENTRY
            

        
### Fields

#### INDEX

#### ALWAYS_RESIDENT

#### ALWAYS_NONRESIDENT


## Ntfs.BadClus
            

        
### Fields

#### Cluster

#### Bad

### Methods


#### GetFileRecord(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetBadStream(PowerForensics.Ntfs.FileRecord)

> ##### Parameters
> **fileRecord:** 

> ##### Return value
> 

## Ntfs.Bitmap
            

        
### Fields

#### Cluster

#### InUse

### Methods


#### Get(System.String,System.Int64)

> ##### Parameters
> **volume:** 

> **cluster:** 

> ##### Return value
> 

#### GetByPath(System.String,System.Int64)

> ##### Parameters
> **path:** 

> **cluster:** 

> ##### Return value
> 

#### Get(System.String,System.Int32,System.Int64)

> ##### Parameters
> **volume:** 

> **recordNumber:** 

> **cluster:** 

> ##### Return value
> 

#### Get(System.Byte[],System.Int64)

> ##### Parameters
> **bytes:** 

> **cluster:** 

> ##### Return value
> 

#### GetInstancesByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetInstances(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

#### GetDataStream(PowerForensics.Ntfs.FileRecord)

> ##### Parameters
> **fileRecord:** 

> ##### Return value
> 

## Ntfs.LogFile
            

        
### Methods


#### GetFileRecord(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetDataAttr(PowerForensics.Ntfs.FileRecord)

> ##### Parameters
> **fileRecord:** 

> ##### Return value
> 

#### getBytes(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### Get(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Ntfs.RestartAreaHeader
            

        
### Fields

#### Signature

#### USOffset

#### USCount

#### CheckDiskLSN

#### SystemPageSize

#### LogPageSize

#### RestartOffset

#### MinorVersion

#### MajorVersion

#### USArray

#### CurrentLSN

#### LogClient

#### ClientList

#### Flags

### Methods


#### Get(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### Get(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

## Ntfs.Restart
            

        
### Fields

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### 

#### RestartHeader

### Methods


#### 

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

#### Constructor

> ##### Parameters
> **bytes:** 


#### Get(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

## Ntfs.PageHeader
            

        
### Fields

#### Signature

#### USOffset

#### USCount

#### LastLSN

#### Flags

#### PageCount

#### PagePosition

#### NextRecordOffset

#### LastEndLSN

#### USN

#### USArray

### Methods


#### Get(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

## Ntfs.OperationRecord
            

        
### Fields

#### OPERATION_CODE.Noop

#### OPERATION_CODE.CompensationlogRecord

#### OPERATION_CODE.InitializeFileRecordSegment

#### OPERATION_CODE.DeallocateFileRecordSegment

#### OPERATION_CODE.WriteEndofFileRecordSegement

#### OPERATION_CODE.CreateAttribute

#### OPERATION_CODE.DeleteAttribute

#### OPERATION_CODE.UpdateResidentValue

#### OPERATION_CODE.UpdataeNonResidentValue

#### OPERATION_CODE.UpdateMappingPairs

#### OPERATION_CODE.DeleteDirtyClusters

#### OPERATION_CODE.SetNewAttributeSizes

#### OPERATION_CODE.AddindexEntryRoot

#### OPERATION_CODE.DeleteindexEntryRoot

#### OPERATION_CODE.AddIndexEntryAllocation

#### OPERATION_CODE.SetIndexEntryVenAllocation

#### OPERATION_CODE.UpdateFileNameRoot

#### OPERATION_CODE.UpdateFileNameAllocation

#### OPERATION_CODE.SetBitsInNonresidentBitMap

#### OPERATION_CODE.ClearBitsInNonresidentBitMap

#### OPERATION_CODE.PrepareTransaction

#### OPERATION_CODE.CommitTransaction

#### OPERATION_CODE.ForgetTransaction

#### OPERATION_CODE.OpenNonresidentAttribute

#### OPERATION_CODE.DirtyPageTableDump

#### OPERATION_CODE.TransactionTableDump

#### OPERATION_CODE.UpdateRecordDataRoot

#### LSN

#### PreviousLSN

#### ClientUndoLSN

#### ClientDataLength

#### ClientID

#### RecordType

#### TransactionID

#### Flags

#### RedoOP

#### UndoOP

#### RedoOffset

#### RedoLength

#### UndoOffset

#### UndoLength

#### TargetAttribute

#### LCNtoFollow

#### RecordOffset

#### AttrOffset

#### MFTClusterIndex

#### TargetVCN

#### TargetLCN

### Methods


#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### Get(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

## Ntfs.OperationRecord.OPERATION_CODE
            

        
### Fields

#### Noop

#### CompensationlogRecord

#### InitializeFileRecordSegment

#### DeallocateFileRecordSegment

#### WriteEndofFileRecordSegement

#### CreateAttribute

#### DeleteAttribute

#### UpdateResidentValue

#### UpdataeNonResidentValue

#### UpdateMappingPairs

#### DeleteDirtyClusters

#### SetNewAttributeSizes

#### AddindexEntryRoot

#### DeleteindexEntryRoot

#### AddIndexEntryAllocation

#### SetIndexEntryVenAllocation

#### UpdateFileNameRoot

#### UpdateFileNameAllocation

#### SetBitsInNonresidentBitMap

#### ClearBitsInNonresidentBitMap

#### PrepareTransaction

#### CommitTransaction

#### ForgetTransaction

#### OpenNonresidentAttribute

#### DirtyPageTableDump

#### TransactionTableDump

#### UpdateRecordDataRoot


## Ntfs.NtfsVolumeBootRecord
            

        
### Fields

#### BytesPerFileRecord

#### BytesPerIndexBlock

#### TotalSectors

#### MftStartIndex

#### MftMirrStartIndex

#### VolumeSerialNumber

### Methods


#### GetBytes(System.IO.FileStream)

> ##### Parameters
> **streamToRead:** 

> ##### Return value
> 

#### getBytesPerFileRecord(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **bytesPerCluster:** 

> ##### Return value
> 

#### getBytesPerIndexBlock(System.Byte[],System.Int32)

> ##### Parameters
> **bytes:** 

> **bytesPerCluster:** 

> ##### Return value
> 

#### getVolumeSerialNumber(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

## Ntfs.UsnJrnl
            

        
### Fields

#### USN_REASON.DATA_OVERWRITE

#### USN_REASON.DATA_EXTEND

#### USN_REASON.DATA_TRUNCATION

#### USN_REASON.NAMED_DATA_OVERWRITE

#### USN_REASON.NAMED_DATA_EXTEND

#### USN_REASON.NAMED_DATA_TRUNCATION

#### USN_REASON.FILE_CREATE

#### USN_REASON.FILE_DELETE

#### USN_REASON.EA_CHANGE

#### USN_REASON.SECURITY_CHANGE

#### USN_REASON.RENAME_OLD_NAME

#### USN_REASON.RENAME_NEW_NAME

#### USN_REASON.INDEXABLE_CHANGE

#### USN_REASON.BASIC_INFO_CHANGE

#### USN_REASON.HARD_LINK_CHANGE

#### USN_REASON.COMPRESSION_CHANGE

#### USN_REASON.ENCRYPTION_CHANGE

#### USN_REASON.OBJECT_ID_CHANGE

#### USN_REASON.REPARSE_POINT_CHANGE

#### USN_REASON.STREAM_CHANGE

#### USN_REASON.CLOSE

#### USN_SOURCE.DATA_MANAGEMENT

#### USN_SOURCE.AUXILIARY_DATA

#### USN_SOURCE.REPLICATION_MANAGEMENT

#### USN40Version

#### VolumePath

#### Version

#### RecordNumber

#### FileSequenceNumber

#### ParentFileRecordNumber

#### ParentFileSequenceNumber

#### Usn

#### TimeStamp

#### Reason

#### SourceInfo

#### SecurityId

#### FileAttributes

#### FileName

#### FullName

#### 

#### 

#### 

#### 

### Methods


#### Get(System.String,System.Int64)

> ##### Parameters
> **volume:** 

> **usn:** 

> ##### Return value
> 

#### GetByPath(System.String,System.Int64)

> ##### Parameters
> **path:** 

> **usn:** 

> ##### Return value
> 

#### Get(System.String,System.Int32,System.Int64)

> ##### Parameters
> **volume:** 

> **recordnumber:** 

> **usn:** 

> ##### Return value
> 

#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetInstancesByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetTestInstances(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetInstances(System.String,System.Int32)

> ##### Parameters
> **volume:** 

> **recordnumber:** 

> ##### Return value
> 

#### GetJStream(PowerForensics.Ntfs.FileRecord)

> ##### Parameters
> **fileRecord:** 

> ##### Return value
> 

#### GetFileRecord

> ##### Return value
> 

#### GetParentFileRecord

> ##### Return value
> 

#### ToString

> ##### Return value
> 

#### 

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### 

> ##### Parameters
> **path:** 

> ##### Return value
> 

## Ntfs.UsnJrnl.USN_REASON
            

        
### Fields

#### DATA_OVERWRITE

#### DATA_EXTEND

#### DATA_TRUNCATION

#### NAMED_DATA_OVERWRITE

#### NAMED_DATA_EXTEND

#### NAMED_DATA_TRUNCATION

#### FILE_CREATE

#### FILE_DELETE

#### EA_CHANGE

#### SECURITY_CHANGE

#### RENAME_OLD_NAME

#### RENAME_NEW_NAME

#### INDEXABLE_CHANGE

#### BASIC_INFO_CHANGE

#### HARD_LINK_CHANGE

#### COMPRESSION_CHANGE

#### ENCRYPTION_CHANGE

#### OBJECT_ID_CHANGE

#### REPARSE_POINT_CHANGE

#### STREAM_CHANGE

#### CLOSE


## Ntfs.UsnJrnl.USN_SOURCE
            

        
### Fields

#### DATA_MANAGEMENT

#### AUXILIARY_DATA

#### REPLICATION_MANAGEMENT


## Ntfs.UsnJrnlInformation
            

        
### Fields

#### MaxSize

#### AllocationDelta

#### UsnId

#### LowestUsn

### Methods


#### Get(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetBytes(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetBytesByPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

## Formats.ForensicTimeline
            

        
### Fields

#### ACTIVITY_TYPE.m

#### ACTIVITY_TYPE.a

#### ACTIVITY_TYPE.c

#### ACTIVITY_TYPE.b

#### Date

#### ActivityType

#### Source

#### SourceType

#### User

#### FileName

#### Description

### Methods


#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### Get(PowerForensics.Artifacts.Amcache)

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### GetInstances(PowerForensics.Artifacts.Amcache[])

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### Get(PowerForensics.Fat.DirectoryEntry)

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### GetInstances(PowerForensics.Fat.DirectoryEntry[])

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### Get(PowerForensics.Artifacts.Prefetch)

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### GetInstances(PowerForensics.Artifacts.Prefetch[])

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### Get(PowerForensics.Artifacts.ScheduledJob)

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### GetInstances(PowerForensics.Artifacts.ScheduledJob[])

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### Get(PowerForensics.Artifacts.ShellLink)

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### GetInstances(PowerForensics.Artifacts.ShellLink[])

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### Get(PowerForensics.Artifacts.UserAssist)

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### GetInstances(PowerForensics.Artifacts.UserAssist[])

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### Get(PowerForensics.EventLog.EventRecord)

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### GetInstances(PowerForensics.EventLog.EventRecord[])

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### Get(PowerForensics.Ntfs.FileRecord)

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### GetInstances(PowerForensics.Ntfs.FileRecord[])

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### Get(PowerForensics.Ntfs.UsnJrnl)

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### GetInstances(PowerForensics.Ntfs.UsnJrnl[])

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### Get(PowerForensics.Registry.NamedKey)

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### GetInstances(PowerForensics.Registry.NamedKey[])

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### ToFriendlyString(PowerForensics.Formats.ForensicTimeline.ACTIVITY_TYPE)

> ##### Parameters
> **type:** 

> ##### Return value
> 

## Formats.ForensicTimeline.ACTIVITY_TYPE
            

        
### Fields

#### m

#### a

#### c

#### b


## Formats.Gource
            

        
### Methods


#### Get(PowerForensics.Formats.ForensicTimeline)

> ##### Parameters
> **input:** 

> ##### Return value
> 

#### GetInstances(PowerForensics.Formats.ForensicTimeline[])

> ##### Parameters
> **input:** 

> ##### Return value
> 

## Helper
            

        
### Fields

#### FILE_SYSTEM_TYPE.EXFAT

#### FILE_SYSTEM_TYPE.FAT

#### FILE_SYSTEM_TYPE.NTFS

#### FILE_SYSTEM_TYPE.SOMETHING_ELSE

### Methods


#### GetFileSystemType(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetFileSystemType(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

#### getVolumeName(System.String@)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetVolumeFromPath(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetVolumeLetter(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### GetSecurityIdentifier(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

#### getFileStream(System.String)

> ##### Parameters
> **fileName:** 

> ##### Return value
> 

#### readDrive(System.String,System.Int64,System.Int64)

> ##### Parameters
> **device:** 

> **offset:** 

> **sizeToRead:** 

> ##### Return value
> 

#### readSector(System.String,System.Int64,System.Int64)

> ##### Parameters
> **device:** 

> **sectorOffset:** 

> **sectorCount:** 

> ##### Return value
> 

#### readDrive(System.IO.FileStream,System.Int64,System.Int64)

> ##### Parameters
> **streamToRead:** 

> **offset:** 

> **sizeToRead:** 

> ##### Return value
> 

#### SwapEndianness(System.Int16)

> ##### Parameters
> **value:** 

> ##### Return value
> 

#### SwapEndianness(System.Int32)

> ##### Parameters
> **value:** 

> ##### Return value
> 

#### SwapEndianness(System.Int64)

> ##### Parameters
> **value:** 

> ##### Return value
> 

#### SwapEndianness(System.UInt16)

> ##### Parameters
> **value:** 

> ##### Return value
> 

#### SwapEndianness(System.UInt32)

> ##### Parameters
> **value:** 

> ##### Return value
> 

#### SwapEndianness(System.UInt64)

> ##### Parameters
> **value:** 

> ##### Return value
> 

#### FromOSXTime(System.UInt32)

> ##### Parameters
> **seconds:** 

> ##### Return value
> 

#### ToOSXTime(System.DateTime)

> ##### Parameters
> **dateTime:** 

> ##### Return value
> 

#### FromUnixTime(System.UInt32)

> ##### Parameters
> **unixTime:** 

> ##### Return value
> 

#### ToUnixTime(System.DateTime)

> ##### Parameters
> **dateTime:** 

> ##### Return value
> 

#### GetFATTime(System.UInt16)

> ##### Parameters
> **date:** 

> ##### Return value
> 

#### GetFATTime(System.UInt16,System.UInt16)

> ##### Parameters
> **date:** 

> **time:** 

> ##### Return value
> 

#### GetFATTime(System.UInt16,System.UInt16,System.Byte)

> ##### Parameters
> **date:** 

> **time:** 

> **tenth:** 

> ##### Return value
> 

#### GetSubArray(System.Byte[],System.Int64,System.Int64)

> ##### Parameters
> **InputBytes:** 

> **offset:** 

> **length:** 

> ##### Return value
> 

#### GetSubArray(System.Byte[],System.Int32,System.Int32)

> ##### Parameters
> **InputBytes:** 

> **offset:** 

> **length:** 

> ##### Return value
> 

#### ApplyFixup(System.Byte[]@,System.Int32)

> ##### Parameters
> **bytes:** 

> **offset:** 


#### FromRot13(System.String)

> ##### Parameters
> **value:** 

> ##### Return value
> 

## Helper.FILE_SYSTEM_TYPE
            

        
### Fields

#### EXFAT

#### FAT

#### NTFS

#### SOMETHING_ELSE


## Utilities.DD
            

        
### Methods


#### Get(System.String,System.String,System.Int64,System.UInt32,System.UInt32)

> ##### Parameters
> **inFile:** 

> **outFile:** 

> **offset:** 

> **blockSize:** 

> **count:** 


#### Get(System.String,System.Int64,System.UInt32,System.UInt32)

> ##### Parameters
> **inFile:** 

> **offset:** 

> **blockSize:** 

> **count:** 

> ##### Return value
> 

## Utilities.Compression.Xpress
            

        
### Methods


#### DecompressBuffer(System.Byte[],System.UInt32,System.UInt32)

> ##### Parameters
> **inputBuffer:** 

> **outputSize:** 

> **inputConsumed:** 

> ##### Return value
> 

#### DecompressBufferLZ77(System.Byte[],System.UInt32,System.UInt32)

> ##### Parameters
> **inputBuffer:** 

> **outputSize:** 

> **inputConsumed:** 

> ##### Return value
> 

## EventLog.BinaryXml
            

        
### Fields

#### TOKEN_TYPE.BinXmlTokenEOF

#### TOKEN_TYPE.BinXmlTokenOpenStartElementTag

#### TOKEN_TYPE.BinXmlTokenCloseStartElementTag

#### TOKEN_TYPE.BinXmlTokenCloseEmptyElementTag

#### TOKEN_TYPE.BinXmlTokenEndElementTag

#### TOKEN_TYPE.BinXmlTokenValue

#### TOKEN_TYPE.BinXmlTokenAttribute

#### TOKEN_TYPE.BinXmlTokenCDATASection

#### TOKEN_TYPE.BinXmlTokenCharRef

#### TOKEN_TYPE.BinXmlTokenEntityRef

#### TOKEN_TYPE.BinXmlTokenPITarget

#### TOKEN_TYPE.BinXmlTokenPIData

#### TOKEN_TYPE.BinXmlTokenTemplateInstance

#### TOKEN_TYPE.BinXmlTokenNormalSubstitution

#### TOKEN_TYPE.BinXmlTokenOptionalSubstitution

#### TOKEN_TYPE.BinXmlFragmentHeaderToken

#### TOKEN_TYPE.BinXmlTokenOpenStartElementTag_AttributeList

#### TOKEN_TYPE.BinXmlTokenAttribute_Additional

#### VALUE_TYPE.NullType

#### VALUE_TYPE.StringType

#### VALUE_TYPE.AnsiStringType

#### VALUE_TYPE.Int8Type

#### VALUE_TYPE.UInt8Type

#### VALUE_TYPE.Int16Type

#### VALUE_TYPE.UInt16Type

#### VALUE_TYPE.Int32Type

#### VALUE_TYPE.UInt32Type

#### VALUE_TYPE.Int64Type

#### VALUE_TYPE.UInt64Type

#### VALUE_TYPE.Real32Type

#### VALUE_TYPE.Real64Type

#### VALUE_TYPE.BoolType

#### VALUE_TYPE.BinaryType

#### VALUE_TYPE.GuidType

#### VALUE_TYPE.SizeTType

#### VALUE_TYPE.FileTimeType

#### VALUE_TYPE.SysTimeType

#### VALUE_TYPE.SidType

#### VALUE_TYPE.HexInt32Type

#### VALUE_TYPE.HexInt64Type

#### VALUE_TYPE.EvtHandle

#### VALUE_TYPE.BinXmlType

#### VALUE_TYPE.EvtXml

#### VALUE_TYPE.StringType_Array

#### VALUE_TYPE.AnsiStringType_Array

#### VALUE_TYPE.Int8Type_Array

#### VALUE_TYPE.UInt8Type_Array

#### VALUE_TYPE.Int16Type_Array

#### VALUE_TYPE.UInt16Type_Array

#### VALUE_TYPE.Int32Type_Array

#### VALUE_TYPE.UInt32Type_Array

#### VALUE_TYPE.Int64Type_Array

#### VALUE_TYPE.UInt64Type_Array

#### VALUE_TYPE.Real32Type_Array

#### VALUE_TYPE.Real64Type_Array

#### VALUE_TYPE.BoolType_Array

#### VALUE_TYPE.BinaryType_Array

#### VALUE_TYPE.GuidType_Array

#### VALUE_TYPE.SizeTType_Array

#### VALUE_TYPE.FileTimeType_Array

#### VALUE_TYPE.SysTimeType_Array

#### VALUE_TYPE.SidType_Array

#### VALUE_TYPE.HexInt32Type_Array

#### VALUE_TYPE.HexInt64Type_Array

#### TemplateInstance

#### ProviderName

#### Guid

#### EventId

#### Verson

#### Level

#### Task

#### Opcode

#### Keywords

#### TimeCreated

#### EventRecordId

#### ActivityId

#### ProcessId

#### ThreadId

#### Channel

#### UserId

### Methods


#### ToString

> ##### Return value
> 

## EventLog.BinaryXml.TOKEN_TYPE
            

        
### Fields

#### BinXmlTokenEOF

#### BinXmlTokenOpenStartElementTag

#### BinXmlTokenCloseStartElementTag

#### BinXmlTokenCloseEmptyElementTag

#### BinXmlTokenEndElementTag

#### BinXmlTokenValue

#### BinXmlTokenAttribute

#### BinXmlTokenCDATASection

#### BinXmlTokenCharRef

#### BinXmlTokenEntityRef

#### BinXmlTokenPITarget

#### BinXmlTokenPIData

#### BinXmlTokenTemplateInstance

#### BinXmlTokenNormalSubstitution

#### BinXmlTokenOptionalSubstitution

#### BinXmlFragmentHeaderToken

#### BinXmlTokenOpenStartElementTag_AttributeList

#### BinXmlTokenAttribute_Additional


## EventLog.BinaryXml.VALUE_TYPE
            

        
### Fields

#### NullType

#### StringType

#### AnsiStringType

#### Int8Type

#### UInt8Type

#### Int16Type

#### UInt16Type

#### Int32Type

#### UInt32Type

#### Int64Type

#### UInt64Type

#### Real32Type

#### Real64Type

#### BoolType

#### BinaryType

#### GuidType

#### SizeTType

#### FileTimeType

#### SysTimeType

#### SidType

#### HexInt32Type

#### HexInt64Type

#### EvtHandle

#### BinXmlType

#### EvtXml

#### StringType_Array

#### AnsiStringType_Array

#### Int8Type_Array

#### UInt8Type_Array

#### Int16Type_Array

#### UInt16Type_Array

#### Int32Type_Array

#### UInt32Type_Array

#### Int64Type_Array

#### UInt64Type_Array

#### Real32Type_Array

#### Real64Type_Array

#### BoolType_Array

#### BinaryType_Array

#### GuidType_Array

#### SizeTType_Array

#### FileTimeType_Array

#### SysTimeType_Array

#### SidType_Array

#### HexInt32Type_Array

#### HexInt64Type_Array


## EventLog.BinXmlAttributeList
            

        

## EventLog.BinXmlAttribute
            

        
### Fields

#### Token

#### AttributeNameOffset

#### Name


## EventLog.BinXmlName
            

        
### Fields

#### NameSize

#### Name

### Methods


#### ToString

> ##### Return value
> 

## EventLog.BinXmlValueText
            

        
### Fields

#### ValueToken

#### ValueType

#### ValueData


## EventLog.EventRecord
            

        
### Fields

#### LogPath

#### EventRecordId

#### WriteTime

#### EventData

### Methods


#### GetInstances(System.String)

> ##### Parameters
> **volume:** 

> ##### Return value
> 

#### Get(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### ToString

> ##### Return value
> 

## Registry.RegistryHeader
            

        
### Fields

#### Signature

#### PrimarySequenceNumber

#### SecondarySequenceNumber

#### ModificationTime

#### Version

#### FileType

#### RootKeyOffset

#### HiveBinsDataSize

#### HivePath

#### Checksum

### Methods


#### GetBytes(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### Get(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

## Registry.RegistryHelper
            

        
### Methods


#### GetHiveBytes(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetRootKey(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

## Registry.Cell
            

        
### Fields

#### Size

#### Allocated

#### Signature


## Registry.NamedKey
            

        
### Fields

#### NAMED_KEY_FLAGS.VolatileKey

#### NAMED_KEY_FLAGS.MountPoint

#### NAMED_KEY_FLAGS.RootKey

#### NAMED_KEY_FLAGS.Immutable

#### NAMED_KEY_FLAGS.SymbolicLink

#### NAMED_KEY_FLAGS.NameIsASCII

#### NAMED_KEY_FLAGS.PredefinedHandle

#### HivePath

#### Flags

#### WriteTime

#### ParentKeyOffset

#### NumberOfSubKeys

#### NumberOfVolatileSubKeys

#### SubKeysListOffset

#### VolatileSubKeysListOffset

#### NumberOfValues

#### ValuesListOffset

#### SecurityKeyOffset

#### ClassNameOffset

#### LargestSubKeyNameSize

#### LargestSubKeyClassNameSize

#### LargestValueNameSize

#### LargestValueDataSize

#### KeyNameSize

#### ClassNameSize

#### FullName

#### Name

### Methods


#### Get(System.String,System.String)

> ##### Parameters
> **path:** 

> **key:** 

> ##### Return value
> 

#### Get(System.Byte[],System.String,System.String)

> ##### Parameters
> **bytes:** 

> **path:** 

> **key:** 

> ##### Return value
> 

#### GetInstances(System.String,System.String)

> ##### Parameters
> **path:** 

> **key:** 

> ##### Return value
> 

#### GetInstances(System.Byte[],System.String)

> ##### Parameters
> **bytes:** 

> **path:** 

> ##### Return value
> 

#### GetInstances(System.Byte[],System.String,System.String)

> ##### Parameters
> **bytes:** 

> **path:** 

> **key:** 

> ##### Return value
> 

#### GetInstancesRecurse(System.String)

> ##### Parameters
> **path:** 

> ##### Return value
> 

#### GetInstances(System.Byte[],PowerForensics.Registry.NamedKey,System.Boolean)

> ##### Parameters
> **bytes:** 

> **nk:** 

> **recurse:** 

> ##### Return value
> 

#### isAllocated(System.Int32)

> ##### Parameters
> **size:** 

> ##### Return value
> 

#### GetValues

> ##### Return value
> 

#### GetValues(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

#### GetSubKeys

> ##### Return value
> 

#### GetSubKeys(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

#### GetSecurityKey

> ##### Return value
> 

#### GetSecurityKey(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

#### ToString

> ##### Return value
> 

## Registry.NamedKey.NAMED_KEY_FLAGS
            

        
### Fields

#### VolatileKey

#### MountPoint

#### RootKey

#### Immutable

#### SymbolicLink

#### NameIsASCII

#### PredefinedHandle


## Registry.SecurityDescriptor
            

        
### Fields

#### SECURITY_KEY_CONTROLS.SeOwnerDefaulted

#### SECURITY_KEY_CONTROLS.SeGroupDefaulted

#### SECURITY_KEY_CONTROLS.SeDaclPresent

#### SECURITY_KEY_CONTROLS.SeDaclDefaulted

#### SECURITY_KEY_CONTROLS.SeSaclPresent

#### SECURITY_KEY_CONTROLS.SeSaclDefaulted

#### SECURITY_KEY_CONTROLS.SeDaclAutoInheritReq

#### SECURITY_KEY_CONTROLS.SeSaclAutoInheritReq

#### SECURITY_KEY_CONTROLS.SeDaclAutoInherited

#### SECURITY_KEY_CONTROLS.SeSaclAutoInherited

#### SECURITY_KEY_CONTROLS.SeDaclProtected

#### SECURITY_KEY_CONTROLS.SeSaclProtected

#### SECURITY_KEY_CONTROLS.SeRmControlValid

#### SECURITY_KEY_CONTROLS.SeSelfRelative

#### Control

#### Owner

#### Group

#### SACL

#### DACL


## Registry.SecurityDescriptor.SECURITY_KEY_CONTROLS
            

        
### Fields

#### SeOwnerDefaulted

#### SeGroupDefaulted

#### SeDaclPresent

#### SeDaclDefaulted

#### SeSaclPresent

#### SeSaclDefaulted

#### SeDaclAutoInheritReq

#### SeSaclAutoInheritReq

#### SeDaclAutoInherited

#### SeSaclAutoInherited

#### SeDaclProtected

#### SeSaclProtected

#### SeRmControlValid

#### SeSelfRelative


## Registry.SecurityKey
            

        
### Fields

#### ReferenceCount

#### Descriptor


## Registry.ValueKey
            

        
### Fields

#### VALUE_KEY_DATA_TYPES.REG_NONE

#### VALUE_KEY_DATA_TYPES.REG_SZ

#### VALUE_KEY_DATA_TYPES.REG_EXPAND_SZ

#### VALUE_KEY_DATA_TYPES.REG_BINARY

#### VALUE_KEY_DATA_TYPES.REG_DWORD

#### VALUE_KEY_DATA_TYPES.REG_DWORD_BIG_ENDIAN

#### VALUE_KEY_DATA_TYPES.REG_LINK

#### VALUE_KEY_DATA_TYPES.REG_MULTI_SZ

#### VALUE_KEY_DATA_TYPES.REG_RESOURCE_LIST

#### VALUE_KEY_DATA_TYPES.REG_FULL_RESOURCE_DESCRIPTOR

#### VALUE_KEY_DATA_TYPES.REG_RESOURCE_REQUIREMENTS_LIST

#### VALUE_KEY_DATA_TYPES.REG_QWORD

#### VALUE_KEY_FLAGS.NameIsUnicode

#### VALUE_KEY_FLAGS.NameIsAscii

#### HivePath

#### Key

#### DataType

#### Name

#### Data

### Methods


#### Get(System.String,System.String,System.String)

> ##### Parameters
> **path:** 

> **key:** 

> **val:** 

> ##### Return value
> 

#### Get(System.Byte[],System.String,System.String,System.String)

> ##### Parameters
> **bytes:** 

> **path:** 

> **key:** 

> **val:** 

> ##### Return value
> 

#### GetInstances(System.String,System.String)

> ##### Parameters
> **path:** 

> **key:** 

> ##### Return value
> 

#### GetInstances(System.Byte[],System.String,System.String)

> ##### Parameters
> **bytes:** 

> **path:** 

> **key:** 

> ##### Return value
> 

#### GetData

> ##### Return value
> 

#### GetData(System.Byte[])

> ##### Parameters
> **bytes:** 

> ##### Return value
> 

## Registry.ValueKey.VALUE_KEY_DATA_TYPES
            

        
### Fields

#### REG_NONE

#### REG_SZ

#### REG_EXPAND_SZ

#### REG_BINARY

#### REG_DWORD

#### REG_DWORD_BIG_ENDIAN

#### REG_LINK

#### REG_MULTI_SZ

#### REG_RESOURCE_LIST

#### REG_FULL_RESOURCE_DESCRIPTOR

#### REG_RESOURCE_REQUIREMENTS_LIST

#### REG_QWORD


## Registry.ValueKey.VALUE_KEY_FLAGS
            

        
### Fields

#### NameIsUnicode

#### NameIsAscii


## Registry.BigData
            

        
### Methods


#### Get(System.Byte[],PowerForensics.Registry.ValueKey)

> ##### Parameters
> **bytes:** 

> **vk:** 

> ##### Return value
> 

## Registry.HashedLeaf
            

        
### Fields

#### HashValue


## Registry.Leaf
            

        
### Fields

#### HashValue


## Registry.LeafItem
            

        

## Registry.List
            

        
### Fields

#### Size

#### Signature

#### Allocated

#### Count

#### Offset

### Methods


#### Factory(System.Byte[],System.Byte[],System.String)

> ##### Parameters
> **bytes:** 

> **subKeyListBytes:** 

> **type:** 

> ##### Return value
> 

## Registry.OffsetRecord
            

        
### Fields

#### RelativeOffset

#### Hash


## Registry.ReferenceItem
            

        

## Registry.ValuesList
            

        
### Fields

#### Offset
