# TODO:
# Add tests for pipeline input

Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Copy-ForensicFile' {    
    Context 'ByPath ParameterSet' { 
        It 'should work with explicit parameters' {
            { Copy-ForensicFile -Path C:\Windows\System32\config\SOFTWARE -Destination C:\Windows\Temp\SOFTWAREcopy } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Copy-ForensicFile C:\Windows\System32\config\SAM C:\Windows\Temp\SAMcopy } | Should Not Throw
        }
        It 'should fail if file does not exist' {
        
        }
        It 'should fail if destination file already exists' {
        
        }
    }
    Context 'ByIndex ParameterSet' { 
        It 'should work with explicit parameters' {
            { Copy-ForensicFile -VolumeName \\.\C: -Index 7 -Destination C:\Windows\Temp\BOOT } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Copy-ForensicFile -Index 4 -Destination C:\Windows\Temp\ATTRDEF } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Copy-ForensicFile 0 C:\Windows\Temp\MFT } | Should Not Throw
        }
        It 'should fail if index is not valid' {
        
        }
        It 'should fail if destination file already exists' {
        
        }
    }
}

# Took forever on AppVeyor
Describe 'Get-ForensicAlternateDataStream' {    
    <#Context 'ByVolume ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicAlternateDataStream -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with volume letter only' {
            { Get-ForensicAlternateDataStream -VolumeName C } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicAlternateDataStream \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicAlternateDataStream } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicAlternateDataStream -VolumeName \\.\L: } | Should Throw
        }
    }#>
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicAlternateDataStream -Path 'C:\$Extend\$UsnJrnl' } | Should Not Throw
        }
        It 'should return two ADS named $J and $Max' {
            $ads = Get-ForensicAlternateDataStream -Path 'C:\$Extend\$UsnJrnl'
            $ads.Length | Should Be 2
            $ads[0].StreamName | Should Be '$J'
            $ads[1].StreamName | Should Be '$Max'
        }
        It 'should return nothing if file has no ADS' {
            $foo = Get-ForensicAlternateDataStream -Path C:\Windows\notepad.exe
            $foo | Should Be $null
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicAlternateDataStream -Path C:\This\file\should\not\exist.txt } | Should Throw
        }
    }
}

# Tests Commented Out
Describe 'Get-ForensicAmcache' {    
    Context 'ByVolume ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicAmcache -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with volume letter only' {
             { Get-ForensicAmcache -VolumeName C } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicAmcache \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicAmcache } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicAmcache -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicAmcache -HivePath C:\Windows\appcompat\Programs\Amcache.hve } | Should Not Throw
        }
        It 'should fail when the file is not an Amcache.hve' {
            { Get-ForensicAmcache -HivePath C:\Windows\System32\config\SAM } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicAmcache -HivePath C:\crazydirectory\thisshouldnotexist.hve } | Should Throw
        }
    }
}

Describe 'Get-ForensicAttrDef' {    
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            { Get-ForensicAttrDef -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with volume letter only' {
            { Get-ForensicAttrDef -VolumeName C } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicAttrDef \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicAttrDef } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicAttrDef -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicAttrDef -Path 'C:\$AttrDef' } | Should Not Throw
        }
        It 'should fail if the file is not an AttrDef file' {
            # Need to figure out how to determine if a file is an AttrDef file
        }
        It 'should fail if the file does not exist' {
            { Get-ForensicAttrDef -Path C:\this\file\should\not\exist.txt } | Should Throw
        }
    }
}

Describe 'Get-ForensicBitmap' {    
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            (Get-ForensicBitmap -VolumeName \\.\C: -Cluster 0).InUse | Should Be $true
        }
        It 'should work with volume letter only' {
            (Get-ForensicBitmap -VolumeName C -Cluster 0).Cluster | Should Be 0
        }
        It 'should work with positional parameters' {
            (Get-ForensicBitmap \\.\C: -Cluster 0).InUse | Should Be $true
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicBitmap -Cluster 0 } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicBitmap -VolumeName \\.\L: -Cluster 0 } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicBitmap -Path 'C:\$Bitmap' -Cluster 0 } | Should Not Throw
        }
        # I dont think there is a way to ensure that the file is truly a Bitmap file
        It 'should fail if the file does not exist' {
            { Get-ForensicBitmap -Path C:\this\file\should\not\exist.txt -Cluster 0 } | Should Throw
        }
    }
}

# Not Done
Describe 'Get-ForensicBootSector' {    
    It 'should work with explicit parameters' {
        { Get-ForensicBootSector -Path \\.\PHYSICALDRIVE0 } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicBootSector \\.\PHYSICALDRIVE0 } | Should Not Throw
    }
    It 'should work with the -AsBytes parameter' {
        (Get-ForensicBootSector \\.\PHYSICALDRIVE0 -AsBytes).GetType().Name | Should Be 'Byte[]'
    }
    It 'should parse a Master Boot Record formatted drive' {
    
    }
    It 'should parse a Guid Partition Table formatted drive' {
    
    }
}

Describe 'Get-ForensicChildItem' {    
    It 'should work with explicit parameters' {
        { Get-ForensicChildItem -Path C:\ } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicChildItem C:\ } | Should Not Throw
    }
    It 'should work without nonmandatory parameters' {
        Set-Location \
        { Get-ForensicChildItem } | Should Not Throw
    }
    It 'should fail gracefully if the file does not exist' {
        { Get-ForensicChildItem -Path C:\windoxs } | Should Not Throw
    }
    It 'should work with listing system files' {
        (Get-ForensicChildItem -Path 'C:\$Volume').FullName | Should Be '$Volume'
    }
}

# Not Done
Describe 'Get-ForensicContent' {  
    Context 'ByIndex ParameterSet' { 
        It 'should work with explicit parameters' {
            { Get-ForensicContent -VolumeName \\.\C: -Index 0 } | Should Not Throw
        }
        It 'should work with volume letter only' {
            { Get-ForensicContent -VolumeName C -Index 0 } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicContent -Index 0 } | Should Not Throw
        }
        It 'should work with the Encoding parameter' {
            
        }
        It 'should work with the TotalCount parameter' {
            (Get-ForensicContent -VolumeName \\.\C: -Index 0 -TotalCount 10).Length | Should Be 10
        }
        It 'should work with the Tail parameter' {
            (Get-ForensicContent -VolumeName \\.\C: -Index 0 -Tail 10).Length | Should Be 10
        }
        It 'should fail if the file does not exist' {
            { Get-ForensicContent -VolumeName \\.\C: -Index 1000000 } | Should Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicContent -VolumeName \\.\L: -Index 0 } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' { 
        It 'should work with explicit parameters' {
            { Get-ForensicContent -Path C:\Windows\System32\config\SAM } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicContent C:\Windows\System32\config\SAM } | Should Not Throw
        }
        It 'should work with the Encoding parameter' {
            
        }
        It 'should work with the TotalCount parameter' {
            (Get-ForensicContent -Path C:\Windows\System32\config\SAM -TotalCount 10).Length | Should Be 10
        }
        It 'should work with the Tail parameter' {
            (Get-ForensicContent -Path C:\Windows\System32\config\SAM -Tail 10).Length | Should Be 10
        }
        It 'should fail if the file does not exist' {
            { Get-ForensicContent -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}

Describe 'Get-ForensicEventLog' {    
    Context 'ByVolume ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicEventLog -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with volume letter only' {
            { Get-ForensicEventLog -VolumeName C } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicEventLog \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicEventLog } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicEventLog -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicEventLog -Path C:\Windows\System32\winevt\Logs\Application.evtx } | Should Not Throw
        }
        It 'should fail when the file is not an EventLog' {
            { Get-ForensicEventLog -Path C:\Windows\system32\cmd.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicEventLog -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}

# MftPath parameter doesnt seem to work
Describe 'Get-ForensicFileRecord' {    
    Context 'ByIndex ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicFileRecord -VolumeName \\.\C: -Index 5 } | Should Not Throw
        }
        It 'should work with volume letter only' {
            { Get-ForensicFileRecord -VolumeName C -Index 5 } | Should Not Throw
        }
        It 'should work with positional "Index" parameter' {
            { Get-ForensicFileRecord -VolumeName \\.\C: 5 }
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicFileRecord -Index 5} | Should Not Throw
        }
        It 'should work without nonmandatory and positional parameters' {
            { Get-ForensicFileRecord 5 } | Should Not Throw
        }
        It 'should return 1 File Record' {
            $r = Get-ForensicFileRecord 5
            $r.Length | Should Be 1
        }
        It 'should return all File Records' {
            { $mft = Get-ForensicFileRecord } | Should Not Throw
            [GC]::Collect()
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            (Get-ForensicFileRecord -Path 'C:\$Volume').FullName | Should Be 'C:\$Volume'
        }
        It 'should work with positional parameters' {
            (Get-ForensicFileRecord 'C:\$Volume').FullName | Should Be 'C:\$Volume'
        }
        It 'should work fail if the file does not exist' {
            { Get-ForensicFileRecord -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
    Context 'ByMftPath ParameterSet' {
        It 'should work with explicit parameters' {
            #{ Get-ForensicFileRecord -MftPath 'C:\$MFT' } | Should Not Throw
        }
        It 'should fail if not a $MFT file' {
            #{ Get-ForensicFileRecord -MftPath C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail if the file does not exist' {
            #{ Get-ForensicFileRecord -MftPath C:\this\file\does\not\exist.txt } | Should Throw 
        }
    }
}

Describe 'Get-ForensicFileRecordIndex' {    
    It 'should work with explicit parameters' {
        { Get-ForensicFileRecordIndex -Path C:\Windows\notepad.exe } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicFileRecordIndex C:\Windows\system32\cmd.exe } | Should Not Throw
    }
    It 'should work with directories' {
        (Get-ForensicFileRecordIndex -Path C:\) | Should be 5
    }
    It 'should work with system files' {
        { Get-ForensicFileRecordIndex -Path 'C:\$Extend\$UsnJrnl' } | Should Not Throw
    }
    It 'should fail when the file does not exist' {
        { Get-ForensicFileRecordIndex -Path C:\this\file\does\not\exist.txt } | Should Throw
    }
}

Describe 'Get-ForensicFileSlack' {    
    Context 'ByIndex ParameterSet' {
        It 'should work with explicit parameters' {
            #{ Get-ForensicFileSlack -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with the Index Parameter' {
            { Get-ForensicFileSlack -VolumeName \\.\C: -Index 0 } | Should Not Throw
        }
        It 'should work with volume letter only' {
            { Get-ForensicFileSlack -VolumeName C -Index 0} | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicFileSlack 0 } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            #{ Get-ForensicFileSlack } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicFileSlack -VolumeName \\.\L: -Index 0 } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicFileSlack -Path C:\Windows\notepad.exe } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicFileSlack C:\Windows\notepad.exe } | Should Not Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicFileSlack -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}

# Not Done
Describe 'Get-ForensicGuidPartitionTable' {    
    It 'should work with explicit parameters' {
            
    }
    It 'should work with positional parameters' {
    
    }
    It 'should return a byte array from the -AsBytes parameter' {
    
    }
    It 'should return a MasterBootRecord object if the drive is not Guid Partition Table format' {
    
    }
}

# Not Done
Describe 'Get-ForensicMasterBootRecord' { 
    It 'should work with explicit parameters' {
        { Get-ForensicMasterBootRecord -Path \\.\PHYSICALDRIVE0 } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicMasterBootRecord \\.\PHYSICALDRIVE0 } | Should Not Throw
    }
    It 'should return a byte array from the -AsBytes parameter' {
        $mbr = Get-ForensicMasterBootRecord -Path \\.\PHYSICALDRIVE0 -AsBytes
        $mbr.Length | Should Be 512
        $mbr.GetType().Name | Should Be 'Byte[]'
    }
    It 'should return the protective MBR if the drive is Guid Partition Table format' {
    
    }
}

# Not Done
Describe 'Get-ForensicMftSlack' {    
    Context 'ByIndex ParameterSet' {
        It 'should' {
            
        }   
    }
    Context 'ByPath ParameterSet' {
        It 'should' {
            
        }   
    }
    Context 'ByMftPath ParameterSet' {
        It 'should' {
            
        }   
    }
}

Describe 'Get-ForensicNetworkList' {    
    It 'should work with explicit parameters' {
        { Get-ForensicNetworkList -HivePath C:\Windows\System32\config\SOFTWARE } | Should Not Throw 
    }
    It 'should work with positional parameters' {
        { Get-ForensicNetworkList C:\Windows\System32\config\SOFTWARE } | Should Not Throw
    }
    It 'should work with nonmandatory parameters' {
        { Get-ForensicNetworkList } | Should Not Throw
    }
    It 'should fail if the hive is not a SOFTWARE hive' {
        { Get-ForensicNetworkList -HivePath C:\Windows\notepad.exe } | Should Throw
    }
}

# Not Done
# Need to figure out how to test GPT
Describe 'Get-ForensicPartitionTable' {    

}

Describe 'Get-ForensicPrefetch' {      
    if(Test-Path C:\Windows\Prefetch){
        Context 'ByVolume ParameterSet' { 
            It 'should work with explicit parameters' {
                { Get-ForensicPrefetch -VolumeName \\.\C: } | Should Not Throw
            }
            It 'should work with volume letter only' {
                { Get-ForensicPrefetch -VolumeName C } | Should Not Throw
            }
            It 'should work with positional parameters' {
                { Get-ForensicPrefetch \\.\C: } | Should Not Throw
            }
            It 'should work without nonmandatory parameters' {
                { Get-ForensicPrefetch } | Should Not Throw
            }
            It 'should fail when the volume does not exist' {
                { Get-ForensicPrefetch -VolumeName \\.\L: } | Should Throw
            }
            It 'should work with the Fast parameter' {
                { Get-ForensicPrefetch -Fast } | Should Not Throw
            }
        }
        Context 'ByPath ParameterSet' {
            It 'should work with explicit parameters' {
                Write-Host 'Need to write something to detect the name of a prefetch file'
            }
            It 'should fail when the file does not exist' {
                { Get-ForensicPrefetch -Path C:\this\file\does\not\exist.txt } | Should Throw
            }
        }
    }
    else{
        Write-Host 'Prefetch is not enabled'
    }
}

Describe 'Get-ForensicRegistryKey' {    
    Context 'ByKey ParameterSet' {
        It 'should work with from Hive Root' {
            { Get-ForensicRegistryKey -HivePath C:\Windows\System32\config\SOFTWARE } | Should Not Throw
        }
        It 'should work with the Key parameter specified' {
            { Get-ForensicRegistryKey -HivePath C:\Windows\System32\config\SYSTEM -Key ControlSet001 } | Should Not Throw
        }
        It 'should fail when the file is not a registry hive' {
            { Get-ForensicRegistryKey -HivePath C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the specified Key does not exist' {
            { Get-ForensicRegistryKey -HivePath C:\Windows\System32\config\SAM -Key keydoesnotexist } | Should Throw
        }

    }
    Context 'Recursive ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicRegistryKey -HivePath C:\Windows\System32\config\SAM -Recurse } | Should Not Throw
        }
        It 'should fail when the file is not a registry hive' {
            { Get-ForensicRegistryKey -HivePath C:\Windows\notepad.exe -Recurse } | Should Throw
        }
    }
}

Describe 'Get-ForensicRegistryValue' {    
    It 'should return one value if the Value parameter is used' {
        { Get-ForensicRegistryValue -HivePath C:\Windows\System32\config\SAM -Key SAM -Value C } | Should Not Throw
    }
    It 'should return all values if the Value parameter is not used' {
        { Get-ForensicRegistryValue -HivePath C:\Windows\System32\config\SAM -Key SAM } | Should Not Throw
    }
    It 'should fail if there are no values' {
        { Get-ForensicRegistryValue -HivePath C:\Windows\System32\config\SAM } | Should Throw
    }
    It 'should fail if the value does not exist' {
        { Get-ForensicRegistryValue -HivePath C:\Windows\System32\config\SAM -Key SAM -Value Jared } | Should Throw
    }
}

# Not Done
Describe 'Get-ForensicScheduledJob' {    
    Context 'ByVolume ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicScheduledJob -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with volume letter only' {
            { Get-ForensicScheduledJob -VolumeName C } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicScheduledJob \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicScheduledJob } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicScheduledJob -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            
        }
        It 'should fail when the file is not a ScheduledJob' {
            { Get-ForensicScheduledJob -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicScheduledJob -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}

# Not Done
Describe 'Get-ForensicShellLink' {    
    Context 'ByVolume ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicShellLink -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with volume letter only' {
            { Get-ForensicShellLink -VolumeName C } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicShellLink \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicShellLink } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicShellLink -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            # Need to have a path to a ShellLink file here        
        }
        It 'should fail when the file is not a ShellLink' {
            { Get-ForensicShellLink -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicShellLink -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}

Describe 'Get-ForensicSid' {    
    It 'should work with explicit parameters' {
        { Get-ForensicSid -HivePath C:\Windows\System32\config\SAM } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicSid C:\Windows\System32\config\SAM } | Should Not Throw
    }
    It 'should work with nonmandatory parameters' {
        { Get-ForensicSid } | Should Not Throw
    }
    It 'should fail if the hive is not a SAM hive' {
        Write-Host 'Need to make the cmdlet check the header of the hive'
        { Get-ForensicSid -HivePath C:\Windows\System32\config\SOFTWARE } | Should Throw
    }
}

Describe 'Get-ForensicTimezone' {    
    It 'should work with explicit parameters' {
        { Get-ForensicTimezone -HivePath C:\Windows\System32\config\SYSTEM } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicTimezone C:\Windows\System32\config\SYSTEM } | Should Not Throw
    }
    It 'should work with nonmandatory parameters' {
        { Get-ForensicTimezone } | Should Not Throw
    }
    It 'should fail if the hive is not a SYSTEM hive' {
        Write-Host 'Need to make the cmdlet check the header of the hive'
        { Get-ForensicTimezone -HivePath C:\Windows\System32\config\SAM } | Should Throw
    }
}

# Need to dynamically find the current user's name
# Need to make it spcifically check the registry header for NTUSER.DAT signature
Describe 'Get-ForensicUserAssist' {    
    It 'should work with explicit parameters' {
        { Get-ForensicUserAssist -HivePath C:\Users\tester\NTUSER.DAT } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicUserAssist C:\Users\tester\NTUSER.DAT } | Should Not Throw
    }
    It 'should fail if the hive is not a NTUSER.DAT hive' {
        Write-Host 'Need to make the cmdlet check the header of the hive'
        { Get-ForensicUserAssist -HivePath C:\Windows\System32\config\SAM } | Should Throw
    }
}

Describe 'Get-ForensicUsnJrnl' {    
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            { $usn = Get-ForensicUsnJrnl -VolumeName \\.\C: } | Should Not Throw
            [GC]::Collect()
        }
        It 'should work with volume letter only' {
            { $usn = Get-ForensicUsnJrnl -VolumeName C } | Should Not Throw
            [GC]::Collect()
        }
        It 'should work with positional parameters' {
            { $usn = Get-ForensicUsnJrnl \\.\C: } | Should Not Throw
            [GC]::Collect()
        }
        It 'should work without nonmandatory parameters' {
            { $usn = Get-ForensicUsnJrnl } | Should Not Throw
            [GC]::Collect()
        }
        It 'should work with the Usn parameter' {
            
        }
        It 'should return just one entry with the Usn parameter' {
            
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicUsnJrnl -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { $usn = Get-ForensicUsnJrnl -Path 'C:\$Extend\$UsnJrnl' } | Should Not Throw
            [GC]::Collect()
        }
        It 'should work with the Usn parameter' {
        
        }
        It 'should return just one entry with the Usn parameter' {
        
        }
        It 'should fail when the file is not $UsnJrnl' {
            { Get-ForensicUsnJrnl -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicUsnJrnl -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}

Describe 'Get-ForensicUsnJrnlInformation' {    
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            { Get-ForensicUsnJrnlInformation -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with volume letter only' {
            { Get-ForensicUsnJrnlInformation -VolumeName C } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicUsnJrnlInformation \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicUsnJrnlInformation } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicUsnJrnlInformation -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicUsnJrnlInformation -Path 'C:\$Extend\$UsnJrnl' } | Should Not Throw
        } 
        It 'should fail when the file is not $UsnJrnl' {
            { Get-ForensicUsnJrnlInformation -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicUsnJrnlInformation -Path C:\this\file\does\not\exist.txt } | Should Throw
        }  
    }
}

Describe 'Get-ForensicVolumeBootRecord' {    
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            { Get-ForensicVolumeBootRecord -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with volume letter only' {
            { Get-ForensicVolumeBootRecord -VolumeName C } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicVolumeBootRecord \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicVolumeBootRecord } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicVolumeBootRecord -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicVolumeBootRecord -Path 'C:\$Boot' } | Should Not Throw
        }
        It 'should fail when the file is not $Boot' {
            { Get-ForensicVolumeBootRecord -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicVolumeBootRecord -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}

Describe 'Get-ForensicVolumeInformation' {    
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            (Get-ForensicVolumeInformation -VolumeName \\.\C:).Name | Should Be 'VOLUME_INFORMATION'
        }
        It 'should work with volume letter only' {
            (Get-ForensicVolumeInformation -VolumeName C).Name | Should Be 'VOLUME_INFORMATION'
        }
        It 'should work with positional parameters' {
            (Get-ForensicVolumeInformation \\.\C:).Name | Should Be 'VOLUME_INFORMATION'
        }
        It 'should work without nonmandatory parameters' {
            (Get-ForensicVolumeInformation).Name | Should Be 'VOLUME_INFORMATION'
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicVolumeInformation -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            (Get-ForensicVolumeInformation -Path 'C:\$Volume').Name | Should Be 'VOLUME_INFORMATION'
        }
        It 'should fail when the file is not $Volume' {
            { Get-ForensicVolumeInformation -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicVolumeInformation -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}

Describe 'Get-ForensicVolumeName' {    
    label.exe C: testdrive
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            (Get-ForensicVolumeName -VolumeName \\.\C:).VolumeNameString | Should Be 'testdrive'
        }
        It 'should work with volume letter only' {
            (Get-ForensicVolumeName -VolumeName C).VolumeNameString | Should Be 'testdrive'
        }
        It 'should work with positional parameters' {
            (Get-ForensicVolumeName \\.\C:).VolumeNameString | Should Be 'testdrive'
        }
        It 'should work without nonmandatory parameters' {
            (Get-ForensicVolumeName).VolumeNameString | Should Be 'testdrive'
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicVolumeName -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters'{
            (Get-ForensicVolumeName -Path 'C:\$Volume').VolumeNameString | Should Be 'testdrive'
        }
        It 'should fail when the file is not $Volume' {
            { Get-ForensicVolumeName -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicVolumeName -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}

# Not Done
Describe 'Invoke-ForensicDD' {    
    It 'should write to file' {
        #Invoke-ForensicDD -InFile \\.\PHYSICALDRIVE0 -OutFile C:\temp\mbr -Offset 0 -BlockSize 512 -Count 1
    }
    It 'should write to the Output Stream' {
        (Invoke-ForensicDD -InFile \\.\PHYSICALDRIVE0 -Offset 0 -BlockSize 1024 -Count 1).Length | Should Be 1024
    }
    It 'should work without nonmandatory parameters' {
        (Invoke-ForensicDD -InFile \\.\C: -Count 1).Length | Should Be 512
    }
}

Describe 'ConvertTo-ForensicTimeline' {
    It 'should work with explicit parameters' {
        { ConvertTo-ForensicTimeline -InputObject (Get-ForensicFileRecord -Index 0) } | Should Not Throw
    }
    It 'should work through the pipeline' {
        { Get-ForensicFileRecord -Index 0 | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    <#It 'should work with Prefetch objects' {
        { Get-ForensicPrefetch -VolumeName \\.\C: | ConvertTo-ForensicTimeline } | Should Not Throw
    }#>
    It 'should work with ScheduledJob objects' {
        { Get-ForensicScheduledJob -VolumeName \\.\C: | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should work with ShellLink objects' {
        { Get-ForensicShellLink -VolumeName \\.\C: | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should work with UserAssist objects' {
        { Get-ForensicUserAssist -HivePath C:\Users\tester\NTUSER.DAT | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should work with EventRecord objects' {
        { Get-ForensicEventLog -Path C:\Windows\System32\winevt\Logs\Application.evtx | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should work with FileRecord objects' {
        { Get-ForensicFileRecord -Index 0 | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should work with UsnJrnl objects' {
        { Get-ForensicUsnJrnl -VolumeName \\.\C: | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should work with NamedKey objects' {
        { Get-ForensicRegistryKey -HivePath C:\Windows\System32\config\SAM | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should fail for unsupported types' {
        { Get-Process -Name lsass | ConvertTo-ForensicTimeline } | Should Throw
    }

}

Describe 'Invoke-ForensicTimeline' {    
    It 'should work with explicit parameters' {
        { $timeline = Invoke-ForensicTimeline -VolumeName \\.\C: } | Should Not Throw
        [GC]::Collect()
    }
    It 'should work with volume letter only' {
        { $timeline = Invoke-ForensicTimeline -VolumeName C } | Should Not Throw
        [GC]::Collect()
    }
    It 'should work with positional parameters' {
        { $timeline = Invoke-ForensicTimeline \\.\C: } | Should Not Throw
        [GC]::Collect()
    }
    It 'should work without nonmandatory parameters' {
        { $timeline = Invoke-ForensicTimeline } | Should Not Throw
        [GC]::Collect()
    }
    It 'should fail when the file does not exist' {
        { $timeline = InvokeForensicTimeline -VolumeName \\.\L: } | Should Throw
    }
}