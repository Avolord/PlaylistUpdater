#& ".\youtube-dl.exe" -U > .\redirection.log

[System.Object]$global:settings = $null

function Main {
    #load the settings
    loadSettings
    if($null -eq $global:settings) {
        return
    }

    $playlist_data = loadPlaylistData $global:settings.locations.playlist_data

    processPlaylistData $playlist_data

    #savePlaylistData $playlist_data
}
function loadSettings() {
    try {
        $global:settings = Get-Content "settings.json" | ConvertFrom-Json
    }
    catch {
        Write-Host "Error while loading the config: `r`n $_.Exception.Message"
    }

    if(
        $null -eq $global:settings.locations.ffmpeg -or `
        $null -eq $global:settings.locations.youtube_dl -or `
        $null -eq $global:settings.locations.playlist_data -or `
        $null -eq $global:settings.locations.archives `
    ) {
        Write-Host "The settings.json is incomplete or corrupted!" -ForegroundColor Red
        $global:settings = $null
    }
    else {
        if((Test-Path $global:settings.locations.archives) -eq $false) {
            New-Item -Path $global:settings.locations.archives -ItemType Directory | Out-Null
        }
    }


}
function loadPlaylistData([String]$playlist_data_path) {
    [System.Object]$data = {}
    try {
        $data = Import-Csv $playlist_data_path -Delimiter ","
    }
    catch {
        Write-Host "Error while loading the playlist-update-data:  `r`n $_.Exception.Message"
    }

    return $data
}
function savePlaylistData([System.Object]$playlist_data) {
    $playlist_data | Export-Csv $global:settings.locations.playlist_data -Delimiter "," -NoType
}
function processPlaylistData([System.Object]$playlist_data) {
    [String]$date = (Get-Date -Format "yyyyMMdd")

    $playlist_data | ForEach-Object {
        #update the playlists
        if($_.SORTED -eq "Yes") {
            updatePlaylist_Sorted($_)
        } 
        else {
            updatePlaylist_Unsorted($_)
        }

        #If the playlist was updated correctly set the new date
        $_.LAST_UPDATED = $date
    }
}
function updatePlaylist_Unsorted([System.Object]$playlist) {
    $ARCHIVE = $global:settings.locations.archives + $playlist.CHANNEL + ".archive"
    #$COMMAND = "echo {} >> $("./tmp")"
    $COMMAND = "testFunc {}"
    $OUTPUT_FORMAT = "$($playlist.LOCATION)\%(title)s.%(ext)s"

    #Generate the archive if it is not already there
    if((Test-Path $ARCHIVE) -eq $False) {
        New-Item -Path $ARCHIVE -ItemType File | Out-Null
    }

    Write-Host "Updating $($playlist.CHANNEL)'s Playlist..." -ForegroundColor Yellow

    & $global:settings.locations.youtube_dl @(			`
            '-x', `
            '--audio-format', 'm4a', `
            '--exec', $COMMAND, `
            '--download-archive', $ARCHIVE,	`
            '--dateafter', $playlist.LAST_UPDATED,	`
            '--playlist-end', 20, `
            '--youtube-skip-dash-manifest', `
            '--console-title', `
            '--ffmpeg-location', $global:settings.locations.ffmpeg, `
            '--embed-thumbnail', `
            '--quiet', `
            '--restrict-filenames', `
            '--add-metadata', `
            '--metadata-from-title', '%(artist)s - %(title)s', `
            '--ignore-errors', `
            $playlist.URL, `
            '-o', $OUTPUT_FORMAT)			
}
function updatePlaylist_Sorted([System.Object]$playlist, [System.Object]$settings) {
    $ARCHIVE = $global:settings.locations.archives + $playlist.CHANNEL + ".archive"
    #$COMMAND = "echo {} >> $("./tmp")"
    #$OUTPUT_FORMAT = "$($playlist.LOCATION)\%(title)s.%(ext)s"



    #Generate the archive if it is not already there
    if((Test-Path $ARCHIVE) -eq $False) {
        New-Item -Path $ARCHIVE -ItemType File | Out-Null
    }
}
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
    & "..\youtube-dl.exe" $params | Write-Output
}

function testFunc([String]$arg) {
    Write-Host "This argument was passed: $arg" -ForegroundColor Green
}

#Start the script
Main

#getPlaylistItemData -url "PLSrMfDSgltgfvmLgVH1TE9vCq6rRmugOi" -range '1-2'

#TODO: Compare the Video-order with the order of the previous check (if available) and change the metadata of the items accordingly to follow the new order.
# The goal is to achieve this with as little metadata writes as possible.

#Better error handling