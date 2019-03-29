package main

import (
	"fmt"
	"strings"

	"github.com/bwmarrin/discordgo"
)

/*
PingCommand handles the ping command from a Discord user... which simply returns Pong! back
to the user.

discord: the Discord session struct
message: the Message containing a lot of info (channel ID, author, content, etc)
*/
func PingCommand(discord *discordgo.Session, message *discordgo.MessageCreate) {
	_, err := discord.ChannelMessageSend(message.ChannelID, "Pong!")
	errCheck("Did not send ping message back successfully", err)

	fmt.Println("Responded to ping by user " + message.Author.String())
}

/*
ClearCommand handles the clear command from a Discord user.

This clears up to 100 messages in the channel it is called in.
!!REQUIRES MANAGE MESSAGES PERMISSION TO WORK!!

discord: The Discord session struct
message: the Message containing a lot of info (channel ID, author, content, etc)
*/
func ClearCommand(discord *discordgo.Session, message *discordgo.MessageCreate) {
	// Clears up to 100 messages less than 2 weeks old in the called channel.
	ch, err := discord.Channel(message.ChannelID)
	errCheck("Oh no! Could not get channel ID.", err)

	msgs, err := discord.ChannelMessages(ch.ID, 100, "", "", "")
	errCheck("Oh no! Could not get channel messages.", err)

	for _, v := range msgs {
		fmt.Printf("Deleting message %s from user %s\n", v.Content, v.Author)
		err = discord.ChannelMessageDelete(ch.ID, v.ID)
		errCheck("Did not remove the message successfully.", err)
	}
}

/*
ListenCommand handles the listen command from a Discord user.

This allows users to listen to songs through YouTube. (TODO)

discord: The Discord session struct
message: the Message containing a lot of info (channel ID, author, content, etc)
args: the arguments for the command
*/
func ListenCommand(discord *discordgo.Session, message *discordgo.MessageCreate, args []string) {
	// Listen to music! (TODO)
	if len(args) >= 1 {
		msg, err := discord.ChannelMessageSend(message.ChannelID, "Proper usage... watch this space.")
		errCheck("Did not send msg successfully", err)
		fmt.Println("Listen is a valid command. Requested by " + msg.Author.String())
	} else {
		msg, err := discord.ChannelMessageSend(message.ChannelID, "Usage: !listen <url>")
		errCheck("Did not send msg successfully", err)
		fmt.Println("Listen not invoked correctly. Requested by " + msg.Author.String())
	}
}

/*
JoinCommand handles the join command from a Discord user.

This allows the bot to join the voice channel the calling user is currently in.
If the user is not in a voice channel... the bot will stay put.

NOTE: voiceConn is a pointer to a voiceConnection object in main.go. It's maintained to keep
track of where the bot is.

discord: The Discord session struct
message: the Message containing a lot of info (channel ID, author, content, etc)
*/
func JoinCommand(discord *discordgo.Session, message *discordgo.MessageCreate) {
	vs, err := findUserVoiceState(discord, message.Author.ID)
	errCheckNonPanic(discord, message, "You need to be in a voice channel, first.", err)

	if err != nil {
		return
	}

	if voiceConn != nil {
		voiceConn.Disconnect()
		voiceConn.Close()
	}

	voiceConn, err = discord.ChannelVoiceJoin(vs.GuildID, vs.ChannelID, false, true)
	errCheck("Couldn't join voice channel.", err)
	voiceConn.AddHandler(func(vc *discordgo.VoiceConnection, vs *discordgo.VoiceSpeakingUpdate) {

	})
}

/*
LeaveCommand handles the leave command from a Discord user.

This allows the bot to leave the voice channel the bot is currently in.
If the user is not in a voice channel... the bot will stay put.

NOTE: voiceConn is a pointer to a voiceConnection object in main.go. It's maintained to keep
track of where the bot is.

discord: The Discord session struct
message: the Message containing a lot of info (channel ID, author, content, etc)
*/
func LeaveCommand(discord *discordgo.Session, message *discordgo.MessageCreate) {
	vs, err := findUserVoiceState(discord, botID)
	errCheckNonPanic(discord, message, "I'm not in a voice channel!", err)

	if err != nil {
		return
	}

	vc := discord.VoiceConnections[vs.GuildID]

	leaveVoiceChannel(vc)
}

/*
RankCommand handles the rank command from a Discord user.

This allows the user to assign a role to themselves. Cool, right?
If no args, simply lists all the roles.
If the user has the role already, this will remove the role from them.

discord: The Discord session struct
message: the Message containing a lot of info (channel ID, author, content, etc)
args: The arguments for the command (in this case, the role the user wants)
*/
func RankCommand(discord *discordgo.Session, message *discordgo.MessageCreate, args []string) {
	rolename := strings.Join(args, " ")
	gu, err := discord.Guild(message.GuildID)
	errCheck("Couldn't get guild ID", err)
	roles := gu.Roles

	mem, err := discord.GuildMember(gu.ID, message.Author.ID)
	errCheck("Could not retrieve member from Guild.", err)

	memRoles := mem.Roles

	if strings.Compare("", rolename) == 0 {
		var roleStr strings.Builder

		roleStr.WriteString("```\nAvailable roles:\n")
		for _, role := range roles {
			if strings.Compare(role.Name, "@everyone") == 0 {
				continue
			}
			roleStr.WriteString(role.Name)
			roleStr.WriteString("\n")
		}

		roleStr.WriteString("```")

		discord.ChannelMessageSend(message.ChannelID, roleStr.String())
		return
	}
	exists := false

	for _, role := range roles {
		if strings.Compare(role.Name, rolename) == 0 {
			exists = true

			if Contains(memRoles, role.ID) {
				err = RemoveRole(discord, gu, role, message.Author)
				errCheck("Couldn't remove role successfully", err)
				discord.ChannelMessageSend(message.ChannelID, message.Author.Mention()+", you successfully left **"+role.Name+"**.")
				fmt.Println("Successfully removed " + role.Name + " role from user " + message.Author.String())
				break
			}

			err = AddRole(discord, gu, role, message.Author)
			if err != nil {
				discord.ChannelMessageSend(message.ChannelID, "Sorry "+message.Author.Mention()+", I could not assign the role **"+role.Name+"** to you.")
			}
			errCheck("Couldn't add role successfully", err)

			discord.ChannelMessageSend(message.ChannelID, message.Author.Mention()+", you successfully joined **"+role.Name+"**.")
			fmt.Println("Successfully added " + role.Name + " role to user " + message.Author.String())
			break
		}
	}

	if !exists {
		discord.ChannelMessageSend(message.ChannelID, "The role **"+rolename+"** does not exist on this server.")
	}
}

/*
HelpCommand handles the help command from a Discord user.

This prints out the available commands for the bot.

discord: The Discord session struct
message: the Message containing a lot of info (channel ID, author, content, etc)
*/
func HelpCommand(discord *discordgo.Session, message *discordgo.MessageCreate) {
	var str strings.Builder
	str.WriteString("```Available commands: \n")

	str.WriteString("!ping: Pong!\n")
	str.WriteString("!clear: Clears up to 100 messages in the called channel less than 2 weeks old.\n")
	str.WriteString("!listen <url>: WIP, eventually listen to music.\n")
	str.WriteString("!join: Bot joins your current voice channel. Does nothing if not in one.\n")
	str.WriteString("!leave: Bot leaves its voice channel. Does nothing if bot is not in voice channel.\n")
	str.WriteString("!rank <rank>: Set <rank> to yourself. If already set, leaves that rank. !rank lists all ranks.\n```")

	discord.ChannelMessageSend(message.ChannelID, str.String())
}
