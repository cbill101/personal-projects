package main

import (
	"github.com/bwmarrin/discordgo"
)

func AddRole(discord *discordgo.Session, gu *discordgo.Guild, role *discordgo.Role, user *discordgo.User) (err error) {
	err = discord.GuildMemberRoleAdd(gu.ID, user.ID, role.ID)

	if err == nil {
		return nil
	}

	return err
}
