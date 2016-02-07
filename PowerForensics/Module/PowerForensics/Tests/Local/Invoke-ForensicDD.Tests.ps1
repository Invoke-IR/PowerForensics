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