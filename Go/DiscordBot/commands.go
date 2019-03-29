package main

import (
	"fmt"
	"strings"

	"github.com/bwmarrin/discordgo"
)

/*
PingCommand ...
Handles the ping command from a Discord user... which simply returns Pong! back
to the user.

discord: the Discord session struct
chID: the Discord channel ID to send the message through
*/
func PingCommand(discord *discordgo.Session, chID string) {
	msg, err := discord.ChannelMessageSend(chID, "Pong!")
	errCheck("Did not send ping message back successfully", err)

	fmt.Println("Responded to ping by user " + msg.Author.String())
}

/*
ClearCommand ...
Handles the clear command from a Discord user.
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
	errCheck("Oh no! Could not retrieve messages in channel.", err)

	for _, v := range msgs {
		fmt.Printf("Deleting message %s from user %s\n", v.Content, v.Author)
		err = discord.ChannelMessageDelete(ch.ID, v.ID)
		errCheck("Did not remove the message successfully.", err)
	}
}

/*
ListenCommand ...
Handles the listen command from a Discord user.

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
JoinCommand ...
Handles the join command from a Discord user.

This allows the bot to join the voice channel the calling user is currently in.
If the user is not in a voice channel... the bot will stay put.

NOTE: voiceConn is a pointer to a voiceConnection object in main.go. It's maintained to keep
track of where the bot is.

discord: The Discord session struct
message: the Message containing a lot of info (channel ID, author, content, etc)
*/
func JoinCommand(discord *discordgo.Session, message *discordgo.MessageCreate) {
	vs, err := findUserVoiceState(discord, message.Author.ID)
	errCheck("Could not find user in voice channel.", err)

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
RankCommand ...
Handles the rank command from a Discord user.

This allows the user to assign a role to themselves. Cool, right?

discord: The Discord session struct
message: the Message containing a lot of info (channel ID, author, content, etc)
args: The arguments for the command (in this case, the role the user wants)

TODO: check if user already has role.
TODO: if no args, list roles on the server and print them in calling channel
*/
func RankCommand(discord *discordgo.Session, message *discordgo.MessageCreate, args []string) {
	rolename := strings.Join(args, " ")
	gu, err := discord.Guild(message.GuildID)
	errCheck("Couldn't get guild ID", err)
	roles := gu.Roles

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
