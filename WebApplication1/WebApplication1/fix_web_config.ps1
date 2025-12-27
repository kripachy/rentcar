$path = "c:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\Web.config"
try {
    $content = Get-Content $path
    $startIndex = 0
    for ($i=0; $i -lt $content.Count; $i++) {
        if ($content[$i] -match "<configuration>") {
            $startIndex = $i
            break
        }
    }
    
    # If <configuration> tag is found, keep it and everything after
    if ($startIndex -ge 0) {
        $cleanContent = $content[$startIndex..($content.Count-1)]
        $header = '<?xml version="1.0" encoding="utf-8"?>'
        $finalContent = @($header) + $cleanContent
        $finalContent | Set-Content -Path $path -Encoding UTF8
        Write-Host "Web.config fixed successfully."
    } else {
        Write-Error "Could not find <configuration> tag in Web.config"
    }
} catch {
    Write-Error "Error fixing Web.config: $_"
}
