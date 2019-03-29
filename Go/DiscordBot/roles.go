package main

import (
	"github.com/bwmarrin/discordgo"
)

/*
AddRole ...
Adds the passed in role to the desired user. Essentially an error wrapper for GuildMemberRoleAdd.

discord: the Discord Session
gu: the Discord Guild instance (server)
role: the Discord Role struct
user: the Discord User wanting the role (struct)
*/
func AddRole(discord *discordgo.Session, gu *discordgo.Guild, role *discordgo.Role, user *discordgo.User) (err error) {
	err = discord.GuildMemberRoleAdd(gu.ID, user.ID, role.ID)

	if err == nil {
		return nil
	}

	return err
}

/*
RemoveRole ...
Removes the passed in role to the desired user. Essentially an error wrapper for GuildMemberRoleAdd.

discord: the Discord Session
gu: the Discord Guild instance (server)
role: the Discord Role struct
user: the Discord User wanting the role (struct)
*/
func RemoveRole(discord *discordgo.Session, gu *discordgo.Guild, role *discordgo.Role, user *discordgo.User) (err error) {
	err = discord.GuildMemberRoleRemove(gu.ID, user.ID, role.ID)

	if err == nil {
		return nil
	}

	return err
}
