Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-ContentRaw' { 
    Write-Host "More tests needed"   
    Context 'Get-Content of C:\windows\system32\config\SAM' { 
        It 'should error' {
            { Copy-ForensicContentRaw C:\Windows\System32\config\SAM } | Should Throw
        }
    }
    Context 'Get-ContentRaw of C:\windows\system32\config\SAM' { 
        It 'should work' {
            { Get-ForensicContentRaw C:\Windows\System32\config\SAM } | Should Not Throw
        }
    }
}