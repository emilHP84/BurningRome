using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMusicSwitcher : MonoBehaviour
{
    [SerializeField] SO_Playlist menuplaylist,battleplaylist, suddendeathplaylist, endplaylist;

    void OnEnable()
    {
        EVENTS.OnMenu += SetMenuPlaylist;
        EVENTS.OnBattleStart += SetBattlePlaylist;
        EVENTS.OnSuddenDeathStart += SetSuddenDeathPlaylist;
        EVENTS.OnScoreDisplay += SetEndPlaylist;
    }

    private void OnDisable()
    {
        EVENTS.OnMenu -= SetMenuPlaylist;
        EVENTS.OnBattleStart -= SetBattlePlaylist;
        EVENTS.OnSuddenDeathStart -= SetSuddenDeathPlaylist;
        EVENTS.OnScoreDisplay -= SetEndPlaylist;
    }

    private void SetMenuPlaylist()
    {
        if (MUSIC.PLAYER) MUSIC.PLAYER.SetPlaylist(menuplaylist);
    }
    private void SetBattlePlaylist()
    {
        if (MUSIC.PLAYER) MUSIC.PLAYER.SetPlaylist(battleplaylist);
    }

    private void SetSuddenDeathPlaylist()
    {
        if (MUSIC.PLAYER) MUSIC.PLAYER.SetPlaylist(suddendeathplaylist);
    }
    void SetEndPlaylist()
    {
        if (MUSIC.PLAYER) MUSIC.PLAYER.SetPlaylist(endplaylist);
    }


} // SCRIPT END
