package main

import (
	"errors"

	"github.com/bwmarrin/discordgo"
)

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
