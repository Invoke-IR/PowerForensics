#
# Module manifest for module 'PowerForensics'
#
# Generated by: Jared Atkinson
#
# Generated on: 4/3/2015
#

@{

# Script module or binary module file associated with this manifest.
ModuleToProcess = 'PowerForensics.dll'

# Version number of this module.
ModuleVersion = '1.0.0.0'

# ID used to uniquely identify this module
GUID = 'be6b06cb-2e5b-4ebe-a00a-8f6e407b68af'

# Author of this module
Author = 'Jared Atkinson'

# Company or vendor of this module
CompanyName = 'Invoke-IR'

# Copyright statement for this module
Copyright = '(c) 2015 Invoke-IR. All rights reserved.'

# Description of the functionality provided by this module
Description = 'A Digital Forensics framework for Windows PowerShell.'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '2.0'

# Name of the Windows PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
# PowerShellHostVersion = ''

# Minimum version of Microsoft .NET Framework required by this module
DotNetFrameworkVersion = '3.5'

# Minimum version of the common language runtime (CLR) required by this module
# CLRVersion = ''

# Processor architecture (None, X86, Amd64) required by this module
# ProcessorArchitecture = ''

# Modules that must be imported into the global environment prior to importing this module
# RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
# RequiredAssemblies = @()

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
# ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
TypesToProcess = @("PowerForensics_Types.ps1xml")

# Format files (.ps1xml) to be loaded when importing this module
FormatsToProcess = @("PowerForensics.ps1xml")

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
# NestedModules = @()

# Functions to export from this module
FunctionsToExport = '*'

# Cmdlets to export from this module
CmdletsToExport = @("Copy-FileRaw",
                    "Format-Hex",
                    "Format-MacTime",
                    "Get-AttrDef",
                    "Get-BadCluster",
                    "Get-Bitmap",
                    "Get-BootSector",
                    "Get-ChildItemRaw",
                    "Get-ContentRaw",
#                    "Get-DeletedExecutable",
                    "Get-DeletedFile",
                    "Get-FileRecord",
                    "Get-FileRecordAttribute",
                    "Get-FileRecordIndex",
                    "Get-GPT",
                    "Get-Hash",
                    "Get-MBR",
                    "Get-PartitionTable",
                    "Get-Prefetch",
                    "Get-Timezone",
                    "Get-UsnJrnl",
                    "Get-UsnJrnlInformation",
                    "Get-VolumeBootRecord",
                    "Get-VolumeInformation",
                    "Get-VolumeName",
                    "Get-VolumeShadowCopy",
                    "Invoke-DD")

# Variables to export from this module
VariablesToExport = '*'

# Aliases to export from this module
AliasesToExport = '*'

# List of all modules packaged with this module
# ModuleList = @()

# List of all files packaged with this module
# FileList = @()

# Private data to pass to the module specified in RootModule/ModuleToProcess
# PrivateData = ''

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''

}

