function getPlaylistItemData {
    Param(
        [parameter(Mandatory=$true)]
        [String]
        $url,
        [parameter(Mandatory=$false)]
        [String]
        $range = "1", #The default range
        [Parameter(Mandatory=$false)]
        [String[]]
        $filter = ("--get-id")
    )

    #construct the params array
    [String[]]$params = $filter + ('--playlist-items', $range, $url)

    #fetch the video information and write it to the output
    & "F:\Music\ytdl\youtube-dl.exe" $params | Write-Output
}

function testFunc([String]$arg) {
    Write-Host "This argument was passed: $arg" -ForegroundColor Green
}