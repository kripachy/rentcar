
$connStr = 'Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\App_Data\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;'
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
$conn.Open()

Write-Host "Cleaning database tables..."
$cmd = $conn.CreateCommand()

# 1. Clear CarImages
$cmd.CommandText = "DELETE FROM CarImages"
$cmd.ExecuteNonQuery()
Write-Host "CarImages cleared."

# 2. Reset MainImageFileId in CarTbl
$cmd.CommandText = "UPDATE CarTbl SET MainImageFileId = NULL"
$cmd.ExecuteNonQuery()
Write-Host "CarTbl MainImageFileId reset."

# 3. Clear FileStorage
$cmd.CommandText = "DELETE FROM FileStorage"
$cmd.ExecuteNonQuery()
Write-Host "FileStorage cleared."

$conn.Close()

Write-Host "Deleting physical files from uploads..."
$uploadsPath = "C:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\uploads"
if (Test-Path $uploadsPath) {
    Get-ChildItem -Path $uploadsPath -Recurse -File | Remove-Item -Force
    Write-Host "Uploads folder cleared."
} else {
    Write-Host "Uploads folder not found, skipping."
}

Write-Host "Cleanup complete."
