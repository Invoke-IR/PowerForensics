$r = binshred .\hello_world.bmp .\bitmap.bs
$rows = $r.dibHeader.bitmapHeight
$r.pixelData.rows[($rows - 1)..0] | % { -join ($_.Pixels | % { if( $_.Pixel[0] ) { ' ' } else { '*' } }) }