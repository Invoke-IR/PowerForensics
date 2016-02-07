Describe 'Get-ForensicAttrDef' {    
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            { Get-ForensicAttrDef -VolumeName \\.\C: } | Should Not Throw
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