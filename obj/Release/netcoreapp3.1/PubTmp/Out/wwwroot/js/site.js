// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function updateSource(source_track){
    var s = String(source_track);
    var audio = document.getElementById('music_source');
    audio.src = s;
    var audio_element = document.getElementById('music');
    audio_element.load();
}