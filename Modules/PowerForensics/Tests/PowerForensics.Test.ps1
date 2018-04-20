Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'ConvertTo-Gource' {

}

Describe 'Copy-ForensicFile' {

}

Describe 'Get-ForensicAlternateDataStream' {

}

Describe 'Get-ForensicAmcache' {

}

Describe 'Get-ForensicAttrDef' {

}

Describe 'Get-ForensicBitmap' {

}

Describe 'Get-ForensicBootSector' {  
    It 'should work with explicit parameters' {
        { Get-ForensicBootSector -Path \\.\PHYSICALDRIVE0 } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicBootSector \\.\PHYSICALDRIVE0 } | Should Not Throw
    }
    It 'should work with the -AsBytes parameter' {
        (Get-ForensicBootSector \\.\PHYSICALDRIVE0 -AsBytes).GetType().FullName | Should Be 'System.Object[]'
    }
}

Describe 'Get-ForensicChildItem' {

}

Describe 'Get-ForensicContent' {

}

Describe 'Get-ForensicEventLog' {

}

Describe 'Get-ForensicExplorerTypedPath' {

}

Describe 'Get-ForensicFileRecord' {

}

Describe 'Get-ForensicFileRecordIndex' {

}

Describe 'Get-ForensicFileSlack' {

}

Describe 'Get-ForensicGuidPartitionTable' {

}

Describe 'Get-ForensicMasterBootRecord' {

}

Describe 'Get-ForensicMftSlack' {

}

Describe 'Get-ForensicNetworkList' {

}

Describe 'Get-ForensicOfficeFileMru' {

}

Describe 'Get-ForensicOfficeOutlookCatalog' {

}

Describe 'Get-ForensicOfficeTrustRecord' {

}

Describe 'Get-ForensicPartitionTable' {

}

Describe 'Get-ForensicPrefetch' {

}

Describe 'Get-ForensicRecentFileCache' {

}

Describe 'Get-ForensicRegistryKey' {

}

Describe 'Get-ForensicRegistryValue' {

}

Describe 'Get-ForensicRunKey' {

}

Describe 'Get-ForensicRunMru' {

}

Describe 'Get-ForensicScheduledJob' {

}

Describe 'Get-ForensicShellLink' {

}

Describe 'Get-ForensicShimcache' {

}

Describe 'Get-ForensicSid' {

}

Describe 'Get-ForensicTimeline' {

}

Describe 'Get-ForensicTimezone' {

}

Describe 'Get-ForensicTypedUrl' {

}

Describe 'Get-ForensicUnallocatedSpace' {

}

Describe 'Get-ForensicUserAssist' {

}

Describe 'Get-ForensicUsnJrnl' {

}

Describe 'Get-ForensicUsnJrnlInformation' {

}

Describe 'Get-ForensicVolumeBootRecord' {

}

Describe 'Get-ForensicVolumeInformation' {

}

Describe 'Get-ForensicVolumeName' {

}

Describe 'Get-ForensicWindowsSearchHistory' {

}

Describe 'Invoke-ForensicDD' {

}