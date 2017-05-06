if(-not (Test-Path "TooManyOrbits.version"))
{
    Write-Host "Incorrect working directory. Please run again from correct directory" -ForegroundColor Red
    exit
}

$avc = (Get-Content TooManyOrbits.version) | ConvertFrom-Json
$avcVersion = "{0}.{1}.{2}" -f $avc.VERSION.MAJOR, $avc.VERSION.MINOR, $avc.VERSION.PATCH
$kspVersion = "{0}.{1}.{2}" -f $avc.KSP_VERSION.MAJOR, $avc.KSP_VERSION.MINOR, $avc.KSP_VERSION.PATCH

$avcRemote = (Invoke-WebRequest -Uri "http://ksp-avc.cybutek.net/version.php?download&id=470").Content | ConvertFrom-Json
$avcRemoteVersion = "{0}.{1}.{2}" -f $avcRemote.VERSION.MAJOR, $avcRemote.VERSION.MINOR, $avcRemote.VERSION.PATCH
$kspRemoteVersion = "{0}.{1}.{2}" -f $avcRemote.KSP_VERSION.MAJOR, $avcRemote.KSP_VERSION.MINOR, $avcRemote.KSP_VERSION.PATCH

$sd =  (Invoke-WebRequest -Uri "https://spacedock.info/api/mod/1340/latest") | ConvertFrom-Json
$sdVersion = $sd.friendly_version
$sdKspVersion = $sd.game_version

Write-Host "Version info" -foregroundcolor green
Write-Host 
Write-Host "Version to publish:"
Write-Host "  TMO KSP-AVC: " $avcVersion
Write-Host "  KSP Target: " $kspVersion
Write-Host 
Write-Host "Released Version:"
Write-Host "  KSP-AVC TMO Version: " $avcVersion
Write-Host "  KSP-AVC KSP Target: " $kspVersion
Write-Host "  SpaceDock TMO Version: " $sdVersion
Write-Host "  SpaceDock KSP Target: " $sdKspVersion
Write-Host 


$doBuild = Read-Host "Create Release [Y/N]?"
if($doBuild -ne "y" -and $doBuild -ne "Y")
{
    exit
}

# build VS project
$msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
$project = "TooManyOrbits\TooManyOrbits.csproj"

Write-Host
Write-Host "Building TooManyOrbits..." -foregroundcolor green
& $msbuild $project /p:Configuration=Release


# gather files to package
$tmpDir = "Release\Temp"
$pluginDir = "$tmpDir\GameData\TooManyOrbits"
$buildDir = "TooManyOrbits\bin\Release"
if(Test-Path $tmpDir)
{
    Remove-Item -Recurse -Force $tmpDir 
}

New-Item -ItemType Directory -Force -Path $pluginDir
Copy-Item "$buildDir\TooManyOrbits.dll" $pluginDir
Copy-Item "$buildDir\*.png" $pluginDir
Copy-Item "TooManyOrbits.version" $pluginDir
Copy-Item "LICENCE" "$pluginDir\LICENCE.txt"

# create release archive
$releaseFile = "Release\TooManyOrbits-$avcVersion.zip"
if(Test-Path $releaseFile)
{
    Remove-Item -Force $releaseFile 
}

Add-Type -assembly "system.io.compression.filesystem"
[io.compression.zipfile]::CreateFromDirectory($pluginDir, $releaseFile) 


# cleanup
Remove-Item -Recurse -Force $tmpDir 





Write-Host
Write-Host
Write-Host "Sucessfully created release file: $releaseFile" -ForegroundColor Green

Write-Host
Write-Host "Next steps:"
Write-Host "  1) Upload to SpaceDock       https://spacedock.info/mod/1340/TooManyOrbits"
Write-Host "  2) Update KSP-AVC version    http://ksp-avc.cybutek.net"
Write-Host "  3) Update forum post         http://forum.kerbalspaceprogram.com/index.php?/topic/159930-122-toomanyorbits-v101/"

try
{
    $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}
catch
{
}
