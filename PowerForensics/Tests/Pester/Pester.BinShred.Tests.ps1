Describe "Tests for the BinShred cmdlet" {

	It "Validates the basic path and template parameters" {
		
		$tempContentPath = [IO.Path]::GetTempFileName()
		$tempTemplatePath = [IO.Path]::GetTempFileName()
		
		try
		{
			Set-Content -Path $tempContentPath -Value ([byte[]] (3, 0x00, 0x00, 0x00, 65, 66, 67, 68, 69, 70)) -Encoding Byte
			Set-Content -Path $tempTemplatePath -Value '
                root :
                    pe;
                pe :
                    itemCount (4 bytes as Int32)
                    stuff (root.pe.itemCount items);
                stuff :
                    value (2 bytes as ASCII);
			'
			
			$results = ConvertFrom-ForensicBinaryData -Path $tempContentPath -TemplatePath $tempTemplatePath
			
			$results.pe.itemCount | Should be 3
			$results.pe.stuff.count | Should be 3
			$results.pe.stuff[0].value | Should be "AB"
			$results.pe.stuff[1].value | Should be "CD"
			$results.pe.stuff[2].value | Should be "EF"			
		}
		finally
		{
			Remove-Item $tempContentPath -Force
			Remove-Item $tempTemplatePath -Force
		}
	}
	
	It "Validates the basic content and template parameters" {
		
		$tempTemplatePath = [IO.Path]::GetTempFileName()
		
		try
		{
			$content = [byte[]] (3, 0x00, 0x00, 0x00, 65, 66, 67, 68, 69, 70)
			Set-Content -Path $tempTemplatePath -Value '
                root :
                    pe;
                pe :
                    itemCount (4 bytes as Int32)
                    stuff (root.pe.itemCount items);
                stuff :
                    value (2 bytes as ASCII);
			'
			
			$results = ConvertFrom-ForensicBinaryData -Content $content -TemplatePath $tempTemplatePath
			
			$results.pe.itemCount | Should be 3
			$results.pe.stuff.count | Should be 3
			$results.pe.stuff[0].value | Should be "AB"
			$results.pe.stuff[1].value | Should be "CD"
			$results.pe.stuff[2].value | Should be "EF"			
		}
		finally
		{
			Remove-Item $tempTemplatePath -Force
		}
	}	
}