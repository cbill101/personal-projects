package main

import (
	"errors"

	"github.com/bwmarrin/discordgo"
)

/*
findUserVoiceState ...
Finds the voice state associated with the desired user, if it exists.

session: the Discord Session
userid: the Discord user ID

returns a VoiceState, error tuple.
*/
func findUserVoiceState(session *discordgo.Session, userid string) (*discordgo.VoiceState, error) {
	for _, guild := range session.State.Guilds {
		for _, vs := range guild.VoiceStates {
			if vs.UserID == userid {
				return vs, nil
			}
		}
	}
	return nil, errors.New("Could not find user's voice state")
}

/*
leaveVoiceChannel ...
The bot leaves the voice channel it is currently in.
Contract: This function assumes the vc struct is not null.

vc: the voice connection associated with the bot

returns a nil, error tuple.
*/
func leaveVoiceChannel(vc *discordgo.VoiceConnection) (*discordgo.VoiceConnection, error) {
	err := vc.Disconnect()

	if err != nil {
		vc.Close()
		return nil, err
	}

	vc.Close()
	vc = nil

	return vc, nil
}
